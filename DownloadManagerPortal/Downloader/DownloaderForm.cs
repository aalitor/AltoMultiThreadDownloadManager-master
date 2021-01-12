using AltoMultiThreadDownloadManager;
using AltoMultiThreadDownloadManager.Exceptions;
using AltoMultiThreadDownloadManager.NativeMessages;
using DownloadManagerPortal.Downloader.UIControls;
using DownloadManagerPortal.SingleInstancing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using AltoMultiThreadDownloadManager.Helpers;
namespace DownloadManagerPortal.Downloader
{
    public partial class DownloaderForm : Form
    {
        public DownloaderForm()
        {
            InitializeComponent();

        }

        public MultiThreadDownloadOrganizer dorg { get; set; }
        public bool NewUrlRequested { get; set; }
        bool directStart;
        string rootRangeDir;
        WaitingNewUrl waiterForm;

        public DownloaderForm(MultiThreadDownloadOrganizer mtdo, bool directStart = true)
        {
            InitializeComponent();
            this.FormClosing += DownloaderForm_FormClosing;
            this.dorg = mtdo;
            setDownloaderEvents();
            btnPauseResume.Click += btnPauseOrResume_Click;
            this.Load += DownloaderControl_Load;
            this.directStart = directStart;
            rootRangeDir = mtdo.RangeDir;
            lblStatus.Text = "Last Status: " + dorg.Status.ToString();
            this.Shown += DownloaderForm_Shown;
        }
        bool flagCloseAfterStop = false;
        void DownloaderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dorg != null && dorg.IsActive)
            {
                flagCloseAfterStop = true;
                dorg.Stop();
            }
        }
        void closeWaiterFormIfOpen()
        {
            var f = findWaiterForm();
            if (f == null)
                return;
            if (f.Visible)
                f.Close();
        }
        WaitingNewUrl findWaiterForm()
        {
            var f = Application.OpenForms.Cast<Form>()
                .Where(x => x is DownloaderForm)
                .Select(x => (DownloaderForm)x)
                .FirstOrDefault(x => x != null && x.dorg != null && x.dorg.Id == this.dorg.Id);
            return f == null ? null : f.waiterForm;
        }
        bool checkWaiterOpen()
        {
            var f = findWaiterForm();
            return f != null && f.Visible;
        }
        void setDownloaderEvents()
        {
            dorg.Stopped += dorg_Stopped;
            dorg.Resumed += dorg_Resumed;
            dorg.DownloadInfoReceived += dorg_DownloadInfoReceived;
            dorg.Completed += dorg_Completed;
            dorg.ProgressChanged += dorg_ProgressChanged;
            dorg.MergingProgressChanged += dorg_MergingProgressChanged;
            dorg.ErrorOccured += dorg_ErrorOccured;
            dorg.StatusChanged += dorg_StatusChanged;
        }
        void saveMTDO()
        {
            if (dorg == null || dorg.Info == null)
                return;
            var json = Properties.Settings.Default.DownloadList;
            var list = JsonConvert.DeserializeObject<List<MultiThreadDownloadOrganizer>>(json);
            if (list != null && list.Any())
            {
                var i = list.FindIndex(x => x != null && x.Info != null && x.Info.ServerFileName == dorg.Info.ServerFileName);
                if (i > -1)
                    list[i] = dorg;
                else
                    list.Add(dorg);
            }
            else
            {
                if (list == null)
                    list = new List<MultiThreadDownloadOrganizer>();
                list.Add(dorg);
            }


            json = JsonConvert.SerializeObject(list);
            Properties.Settings.Default.DownloadList = json;
            Properties.Settings.Default.Save();
        }
        void setButtonStatus(DownloaderStatus status)
        {
            lblStatus.Text = "Last Status: " + status.ToString();
            switch (status)
            {
                case DownloaderStatus.Completed:
                    btnPauseResume.Text = "Download again";
                    btnPauseResume.Enabled = true;
                    this.ControlBox = true;
                    this.Enabled = true;
                    saveMTDO();
                    break;
                case DownloaderStatus.Downloading:
                    btnPauseResume.Text = "Pause";
                    btnPauseResume.Enabled = true;
                    this.ControlBox = true;
                    this.Enabled = true;
                    closeWaiterFormIfOpen();
                    break;
                case DownloaderStatus.MergingFiles:
                    btnPauseResume.Enabled = false;
                    this.ControlBox = false;
                    this.Enabled = false;
                    saveMTDO();
                    break;
                case DownloaderStatus.Stopped:
                    btnPauseResume.Text = "Resume";
                    btnPauseResume.Enabled = true;
                    this.ControlBox = true;
                    this.Enabled = true;
                    saveMTDO();
                    break;
            }
        }
        void dorg_StatusChanged(object sender, AltoMultiThreadDownloadManager.EventArguments.StatusChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    setButtonStatus(e.CurrentStatus);
                });
            }
            else
            {
                setButtonStatus(e.CurrentStatus);
            }


        }
        void DownloaderControl_Load(object sender, EventArgs e)
        {
            DoubleBuffering.SetDoubleBuffered(this);
            if (!directStart)
            {
                btnPauseResume.Text = dorg.Status == DownloaderStatus.Completed ? "Download again" : "Resume";
                btnPauseResume.Enabled = true;
            }
            else
            {
                if (dorg.Ranges != null && dorg.Ranges.Any())
                {
                    dorg.Resume();
                }
                else
                {
                    dorg.Start();
                }
            }
        }
        void btnPauseOrResume_Click(object sender, EventArgs e)
        {
            btnPauseResume.Enabled = false;

            if (btnPauseResume.Text == "Pause")
            {
                if (dorg.Info != null && !dorg.Info.AcceptRanges)
                {
                    var pauseYes = MessageHelper.AskYes("Download doesn't have resumeability. Once it paused, it cannot be resumed from where it left. Do you still want to pause?");
                    if (pauseYes)
                    {
                        dorg.Stop();
                    }
                }
                else
                    dorg.Stop();

            }
            else if (dorg.Status == DownloaderStatus.Completed)
            {
                var currentInfo = dorg.getCurrentInformations();
                if (dorg.LastInfo != null && dorg.LastInfo.Equals(currentInfo))
                {
                    DownloadAgain();
                }
                else if(!NewUrlRequested)
                {
                    NewUrlRequested = true;
                    MessageBox.Show("Remote file properties seems to be changed. Refresh the url", "Url expired", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    requestNewUrl();
                }
            }
            else
            {
                if (dorg.Info != null && !dorg.Info.AcceptRanges)
                {
                    var resumeYes = MessageHelper.AskYes("Download doesn't have resumeability. It will be downloaded from beginning. Do you agree?");
                    if (resumeYes)
                    {
                        DownloadAgain();
                    }
                }
                else
                    dorg.Resume();
            }
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (!MessageHelper.AskYes("Are you sure to delete the download"))
                return;
            var p = (FlowLayoutPanel)this.Parent;
            p.Controls.Remove(this);
        }
        public void DownloadAgain()
        {
            if (!this.Visible)
                this.Show(null);

            var tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            tempFolder = Path.Combine(tempFolder, "AltoDownloadAccelerator");
            dorg = new MultiThreadDownloadOrganizer(dorg.Url, dorg.SaveDir, "", tempFolder, dorg.NofThread)
            {
                DownloadRequestMessage = dorg.DownloadRequestMessage,
                Id = dorg.Id
            };
            setDownloaderEvents();
            dorg.Start();
        }
        public void RefreshUrl(DownloadMessage msg)
        {
            if (!this.Visible)
                this.Show(null);
            this.Invoke((MethodInvoker)delegate
            {
                dorg.Url = msg.Url;
                dorg.Info.Url = msg.Url;
                this.Shown += DownloaderForm_Shown;
                dorg.DownloadRequestMessage = msg;
                if (dorg.Status == DownloaderStatus.Completed)
                {
                    DownloadAgain();
                }
                else if (dorg.Status == DownloaderStatus.Stopped)
                    dorg.Resume();
                NewUrlRequested = false;
            });
        }
        private void dorg_ErrorOccured(object sender, System.IO.ErrorEventArgs e)
        {
            var ex = e.GetException();

            var wform = findWaiterForm();
            if (ex is RemoteFilePropertiesChangedException && !NewUrlRequested && !checkWaiterOpen())
            {
                NewUrlRequested = true;
                if (dorg.IsActive)
                    dorg.Stop();
                MessageBox.Show("Remote file properties seems to be changed. Refresh the url");

                requestNewUrl();
            }
            else
            {
                if (!(ex is WebException))
                {
                    dorg.Stop();
                    lblError.Text = "Last Error: " + ex.Message;
                }
                else
                {
                    var webex = (WebException)ex;
                    if (webex == null)
                        return;
                    var response = (HttpWebResponse)webex.Response;
                    if (response != null)
                    {
                        var status = response.StatusCode;
                        if (!checkWaiterOpen() && status == (HttpStatusCode)403 && (dorg.Info == null || !dorg.LastInfo.Equals(dorg.getCurrentInformations()))
                            && !NewUrlRequested)
                        {
                            NewUrlRequested = true;
                            if (dorg.IsActive)
                                dorg.Stop();
                            MessageBox.Show("Remote file properties seems to be changed. Refresh the url");
                            requestNewUrl();
                        }
                    }
                    else
                    {
                        lblError.Text = "Last Error: " + ex.Message;
                    }
                }
            }
        }
        void requestNewUrl()
        {

            NewUrlRequested = true;
            waiterForm = new WaitingNewUrl();
            waiterForm.FormClosed += (m, n) => NewUrlRequested = true;
            waiterForm.Shown += (m, n) => waiterForm.Activate();
            Process.Start(dorg.DownloadRequestMessage.TabUrl);
            waiterForm.TopMost = true;
            this.Hide();
            waiterForm.ShowDialog();
        }
        int mergprogress = 0;
        void DownloaderForm_Shown(object sender, EventArgs e)
        {
            this.Activate();
        }

        private void dorg_MergingProgressChanged(object sender, AltoMultiThreadDownloadManager.EventArguments.MergingProgressChangedEventArgs e)
        {
            progressBar1.Value = (int)(100 * e.Progress);
        }

        private void dorg_ProgressChanged(object sender, AltoMultiThreadDownloadManager.EventArguments.ProgressChangedEventArgs e)
        {
            segmentedProgressBar1.ContentLength = dorg.Info.ContentSize;
            segmentedProgressBar1.Bars =
                dorg.Ranges.ToList().Select(x => new Bar(x.TotalBytesReceived, x.Start, x.Status)).ToArray();
            progressBar1.Value = (int)(dorg.Progress * 100);
            lblSpeed.Text = string.Format("Speed: {0}", dorg.Speed.ToHumanReadableSize() + "/s");
            lblProgress.Text = "Progress: " + dorg.ProgressString;
            lblBytesReceived.Text = string.Format("Bytes Received: {0} / {1}", dorg.TotalBytesReceived.ToHumanReadableSize(), dorg.Info.ContentSize.ToHumanReadableSize());
            lblContentSize.Text = string.Format("Content Size: {0}", dorg.Info.ContentSize.ToHumanReadableSize());
            lblServerFileName.Text = string.Format("Server Filename: {0}", dorg.Info.ServerFileName);
            lblResumeability.Text = string.Format("{0}", dorg.Info.AcceptRanges ? "Yes" : "No");
            lblResumeability.ForeColor = lblResumeability.Text == "Yes" ? Color.Green : Color.Red;
            this.Text = dorg.Info.ServerFileName;
            txtUrl.Text = dorg.Url;
        }

        private void dorg_Completed(object sender, EventArgs e)
        {
            btnPauseResume.Text = "Download again";
            if (Directory.Exists(dorg.RangeDir))
                Directory.Delete(dorg.RangeDir, true);
            this.FormClosed += DownloaderForm_FormClosed;
            this.Close();
        }

        void DownloaderForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (dorg != null && dorg.Status == DownloaderStatus.Completed)
            {
                new DownloadCompletedForm(dorg).Show(null);
            }
        }

        private void dorg_DownloadInfoReceived(object sender, EventArgs e)
        {
            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    var info = dorg.Info;
                    //Set the filename after we have ServerFileName determined
                    dorg.SaveFileName = info.ServerFileName;
                    var tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    tempFolder = Path.Combine(tempFolder, "AltoDownloadAccelerator");
                    dorg.RangeDir = Path.Combine(tempFolder, info.ServerFileName);
                    Directory.CreateDirectory(dorg.RangeDir);
                    foreach (var item in dorg.Ranges)
                    {
                        item.SaveDir = dorg.RangeDir;
                    }
                    //Set progress bar totallength
                    segmentedProgressBar1.ContentLength = info.ContentSize;

                    btnPauseResume.Text = "Pause";
                    btnPauseResume.Enabled = info.AcceptRanges;
                    lblContentSize.Text = string.Format(lblContentSize.Text, info.ContentSize.ToHumanReadableSize());
                    lblServerFileName.Text = string.Format(lblServerFileName.Text, info.ServerFileName);
                    lblResumeability.Text = string.Format(lblResumeability.Text, info.AcceptRanges);
                });
            }
            catch (Exception ex)
            {

            }
        }

        private void dorg_Resumed(object sender, EventArgs e)
        {
           
        }

        private void dorg_Stopped(object sender, EventArgs e)
        {

            if (flagCloseAfterStop)
            {
                flagCloseAfterStop = false;
                saveMTDO();
                this.Close();
            }
        }

    }
}
