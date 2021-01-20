using AltoMultiThreadDownloadManager;
using AltoMultiThreadDownloadManager.NativeMessages;
using DownloadManagerPortal.Downloader.UIControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AltoMultiThreadDownloadManager.Helpers;
using AltoMultiThreadDownloadManager.Enums;
namespace DownloadManagerPortal.Downloader
{
    public partial class DownloaderForm : Form
    {
        public DownloaderForm()
        {
            InitializeComponent();

        }

        public HttpMultiThreadDownloader dorg { get; set; }
        public bool NewUrlRequested { get; set; }
        bool directStart;
        string rootRangeDir;

        public DownloaderForm(HttpMultiThreadDownloader mtdo, bool directStart = true)
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
            txtUrl.Text = mtdo.Url;
            this.Shown += DownloaderForm_Shown;
        }
        bool flagCloseAfterStop = false;
        void DownloaderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dorg != null && dorg.IsActive)
            {
                flagCloseAfterStop = true;
                dorg.Stop();
                e.Cancel = true;
            }
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
            var list = JsonConvert.DeserializeObject<List<HttpMultiThreadDownloader>>(json);
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
                    list = new List<HttpMultiThreadDownloader>();
                list.Add(dorg);
            }


            json = JsonConvert.SerializeObject(list);
            Properties.Settings.Default.DownloadList = json;
            Properties.Settings.Default.Save();
        }
        void setButtonStatus(HttpDownloaderStatus status)
        {
            lblStatus.Text = "Last Status: " + status.ToString();
            this.TopMost = false;
            switch (status)
            {
                case HttpDownloaderStatus.Completed:
                    btnPauseResume.Text = "Download again";
                    btnPauseResume.Enabled = true;
                    this.ControlBox = true;
                    this.Enabled = true;
                    saveMTDO();
                    break;
                case HttpDownloaderStatus.Downloading:
                    btnPauseResume.Text = "Pause";
                    btnPauseResume.Enabled = dorg != null && dorg.Info != null && dorg.Info.ResumeCapability == Resumeability.Yes;
                    btnCancel.Enabled = true;
                    this.ControlBox = true;
                    this.Enabled = true;
                    break;
                case HttpDownloaderStatus.MergingFiles:
                    this.ControlBox = false;
                    this.Enabled = false;
                    saveMTDO();
                    break;
                case HttpDownloaderStatus.Stopped:
                    btnPauseResume.Text = "Resume";
                    btnPauseResume.Enabled = true;
                    btnCancel.Enabled = true;
                    this.ControlBox = true;
                    this.Enabled = true;
                    saveMTDO();
                    if (flagCloseAfterStop)
                    {
                        this.Close();
                    }
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
                btnPauseResume.Text = dorg.Status == HttpDownloaderStatus.Completed ? "Download again" : "Resume";
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
                if (dorg.Info != null && dorg.Info.ResumeCapability != Resumeability.Yes)
                {
                    var pauseYes = MessageHelper.AskYes("Download doesn't have resumeability. Once it paused, it cannot be resumed from where it left. Do you still want to pause?");
                    if (pauseYes)
                    {
                        dorg.Stop();
                    }
                    else
                        btnPauseResume.Enabled = true;
                }
                else
                    dorg.Stop();

            }
            else if (dorg.Status == HttpDownloaderStatus.Completed)
            {
                HandleAlreadyCompleted();
            }
            else
            {
                Resume();
            }
        }
        public void Resume()
        {
            if (dorg.Info != null)
            {
                if (dorg.Info.ResumeCapability != Resumeability.Yes)
                {
                    this.Show();
                    var resumeYes = MessageHelper.AskYes("Download doesn't have resumeability. It will be downloaded from beginning. Do you agree?");
                    if (resumeYes)
                    {
                        DownloadAgain();
                    }
                    else
                    {
                        this.Close();
                        btnPauseResume.Enabled = true;

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
        public void HandleAlreadyCompleted()
        {
            var currentInfo = dorg.GetCurrentInformations();
            if (dorg.LastInfo != null && dorg.LastInfo.Equals(currentInfo))
            {

                DownloadAgain();
            }
            else if (!RequestAvailable)
            {
                RequestAvailable = true;
                var yesno = MessageBox.Show("Remote file properties seems to be changed. Do you want to renew url and auth data?", "Url expired", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (yesno == System.Windows.Forms.DialogResult.Yes)
                    RequestNewUrl();
                else
                {
                    NewUrlRequested = false;
                    RequestAvailable = false;
                    this.Close();
                }
            }
        }
        public void DownloadAgain()
        {
            var tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            tempFolder = Path.Combine(tempFolder, "AltoDownloadAccelerator");
            dorg = new HttpMultiThreadDownloader(dorg.Url, dorg.SaveDir, "", tempFolder, Properties.Settings.Default.NofThread)
            {
                DownloadRequestMessage = dorg.DownloadRequestMessage,
                Id = dorg.Id
            };
            setDownloaderEvents();
            dorg.Start();
            this.Activate();
        }
        public void RefreshUrl(DownloadMessage msg)
        {
            dorg.Url = msg.Url;
            dorg.Info.Url = msg.Url;
            dorg.DownloadRequestMessage = msg;
            this.Activate();
            if (dorg.Status == HttpDownloaderStatus.Completed)
            {
                DownloadAgain();
            }
            else if (dorg.Status == HttpDownloaderStatus.Stopped)
                dorg.Resume();

            btnPauseResume.Enabled = dorg != null && dorg.Info != null && dorg.Info.ResumeCapability == Resumeability.Yes;
            btnCancel.Enabled = false;
            NewUrlRequested = false;
            RequestAvailable = false;
        }
        private void dorg_ErrorOccured(object sender, System.IO.ErrorEventArgs e)
        {

            handleError(e.GetException());

        }

        public bool RequestAvailable = false;
        public void RequestNewUrl()
        {
            btnPauseResume.Enabled = false;
            btnCancel.Enabled = false;
            RequestAvailable = true;
            NewUrlRequested = true;
            Process.Start(dorg.DownloadRequestMessage.TabUrl);
        }
        void DownloaderForm_Shown(object sender, EventArgs e)
        {
            this.Activate();
        }

        private void dorg_MergingProgressChanged(object sender, AltoMultiThreadDownloadManager.EventArguments.MergingProgressChangedEventArgs e)
        {
            dorg.Progress = e.Progress;
        }

        private void dorg_ProgressChanged(object sender, AltoMultiThreadDownloadManager.EventArguments.ProgressChangedEventArgs e)
        {

        }

        private void dorg_Completed(object sender, EventArgs e)
        {
            btnPauseResume.Text = "Download again";
            //if (Directory.Exists(dorg.RangeDir))
            //    Directory.Delete(dorg.RangeDir, true);
            this.FormClosed += DownloaderForm_FormClosed;
            timer1.Stop();
            this.Close();

        }

        void DownloaderForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (dorg != null && dorg.Status == HttpDownloaderStatus.Completed)
            {
                var c = new DownloadCompletedForm(dorg);
                c.TopMost = true;
                c.Shown += c_Shown;
                c.Show();
            }
        }

        void c_Shown(object sender, EventArgs e)
        {
            var c = sender as DownloadCompletedForm;
            c.TopMost = false;
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
                    btnPauseResume.Enabled = info.ResumeCapability == Resumeability.Yes;
                    lblContentSize.Text = string.Format(lblContentSize.Text, info.ContentSize.ToHumanReadableSize());
                    lblServerFileName.Text = string.Format(lblServerFileName.Text, info.ServerFileName);
                    lblResumeability.Text = string.Format(lblResumeability.Text, info.ResumeCapability == Resumeability.Yes);
                });
            }
            catch
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            updateUI();
        }

        void updateUI()
        {
            if (dorg == null || dorg.Info == null)
                return;
            segmentedProgressBar1.ContentLength = dorg.Info.ContentSize;
            segmentedProgressBar1.Bars =
                dorg.Ranges.ToList().Select(x => new Bar(x.TotalBytesReceived, x.Start, x.Status)).ToArray();
            progressBar1.Value = (int)(dorg.Progress * 100);
            lblSpeed.Text = string.Format("Speed: {0}", dorg.Speed.ToHumanReadableSize() + "/s");
            lblProgress.Text = "Progress: " + dorg.ProgressString;
            lblBytesReceived.Text = string.Format("Bytes Received: {0} / {1}", dorg.TotalBytesReceived.ToHumanReadableSize(), dorg.Info.ContentSize.ToHumanReadableSize());
            lblContentSize.Text = string.Format("Content Size: {0}", dorg.Info.ContentSize.ToHumanReadableSize());
            lblServerFileName.Text = string.Format("Server Filename: {0}", dorg.Info.ServerFileName);
            lblResumeability.Text = string.Format("{0}", dorg.Info.ResumeCapability.ToString());
            lblResumeability.ForeColor = lblResumeability.Text == "Yes" ? Color.Green : Color.Red;
            lblActiveThreads.Text = "Active Threads: " + dorg.NofActiveThreads.ToString();
            this.Text = dorg.Info.ServerFileName;
            txtUrl.Text = dorg.Url;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
