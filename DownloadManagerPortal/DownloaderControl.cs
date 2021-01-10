using System;
using System.Linq;
using System.Windows.Forms;
using AltoMultiThreadDownloadManager;
using System.IO;
using AltoMultiThreadDownloadManager.Helpers;
using DownloadManagerPortal.DownloadHandler.UIControls;
using System.Diagnostics;
using AltoMultiThreadDownloadManager.Exceptions;
using System.Net;
using AltoMultiThreadDownloadManager.NativeMessages;
using Newtonsoft.Json;
using System.Collections.Generic;
using DownloadManagerPortal.SingleInstancing;
namespace DownloadManagerPortal
{
    public partial class DownloaderControl : UserControl
    {
        public MultiThreadDownloadOrganizer dorg { get; set; }
        public bool NewUrlRequested { get; set; }
        bool directStart;
        string rootRangeDir;
        WaitingNewUrl waiterForm;

        public DownloaderControl(MultiThreadDownloadOrganizer mtdo, bool directStart = true)
        {
            InitializeComponent();
            this.dorg = mtdo;
            setMTDOComponents();
            btnDelete.Click += btnDelete_Click;
            btnPauseResume.Click += btnPauseOrResume_Click;
            this.Load += DownloaderControl_Load;
            this.directStart = directStart;
            rootRangeDir = mtdo.RangeDir;
            lblStatus.Text = dorg.Status.ToString();
        }
        void setMTDOComponents()
        {
            dorg.Stopped += dorg_Stopped;
            dorg.Resumed += dorg_Resumed;
            dorg.DownloadInfoReceived += dorg_DownloadInfoReceived;
            dorg.Completed += dorg_Completed;
            dorg.ProgressChanged += dorg_ProgressChanged;
            dorg.MergingProgressChanged += dorg_MergingProgressChanged;
            dorg.ErrorOccured += dorg_ErrorOccured;
            dorg.StatusChanged += dorg_StatusChanged;
            updateUI();
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
                list.Add(dorg);
            }


            json = JsonConvert.SerializeObject(list);
            Properties.Settings.Default.DownloadList = json;
            Properties.Settings.Default.Save();
        }
        void dorg_StatusChanged(object sender, AltoMultiThreadDownloadManager.EventArguments.StatusChangedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lblStatus.Text = e.CurrentStatus.ToString();
                saveMTDO();
                switch (e.CurrentStatus)
                {
                    case DownloaderStatus.Completed:
                        btnPauseResume.Text = "Download again";
                        btnPauseResume.Enabled = true;
                        btnDelete.Enabled = true;
                        btnOpenFile.Enabled = true;
                        App.Instance.MainForm.ControlBox = true;
                        break;
                    case DownloaderStatus.Downloading:
                        btnPauseResume.Text = "Pause";
                        btnPauseResume.Enabled = true;
                        btnDelete.Enabled = false;
                        btnOpenFile.Enabled = false;
                        App.Instance.MainForm.ControlBox = true;
                        break;
                    case DownloaderStatus.MergingFiles:
                        btnPauseResume.Enabled = false;
                        btnDelete.Enabled = false;
                        btnOpenFile.Enabled = false;
                        App.Instance.MainForm.ControlBox = false;
                        break;
                    case DownloaderStatus.Stopped:
                        btnPauseResume.Text = "Resume";
                        btnPauseResume.Enabled = true;
                        btnDelete.Enabled = true;
                        App.Instance.MainForm.ControlBox = true;
                        break;
                }
            });
        }
        void DownloaderControl_Load(object sender, EventArgs e)
        {
            DoubleBuffering.SetDoubleBuffered(this);
            if (!directStart)
            {
                btnDelete.Enabled = true;
                btnPauseResume.Text = dorg.Progress == 100 ? "Download again" : "Resume";
                btnPauseResume.Enabled = true;
                btnOpenFile.Enabled = dorg.Progress == 100;
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
                else
                {
                    MessageBox.Show("Remote file properties seems to be changed. Refresh the url");
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
                        timer1.Start();
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
            dorg = new MultiThreadDownloadOrganizer(dorg.Url, Path.GetDirectoryName(dorg.FilePath), dorg.RangeDir, dorg.NofThread)
            {
                DownloadRequestMessage = dorg.DownloadRequestMessage,
            };
            setMTDOComponents();
            dorg.Start();
        }
        public void RefreshUrl(DownloadMessage msg)
        {
            this.Invoke((MethodInvoker)delegate
            {
                NewUrlRequested = false;
                waiterForm.Close();
                dorg.Url = msg.Url;
                dorg.Info.Url = msg.Url;
                dorg.DownloadRequestMessage = msg;
                if (dorg.Status == DownloaderStatus.Completed)
                {
                    DownloadAgain();
                }
                else if (dorg.Status == DownloaderStatus.Stopped)
                    dorg.Resume();
            });
        }
        private void dorg_ErrorOccured(object sender, System.IO.ErrorEventArgs e)
        {
            var ex = e.GetException();


            if (ex is RemoteFilePropertiesChangedException)
            {
                if (dorg.IsActive)
                    dorg.Stop();
                MessageBox.Show("Remote file properties seems to be changed. Refresh the url");

                requestNewUrl();
            }
            else
            {
                if (!(ex is WebException))
                {
                    MessageBox.Show(ex.Message + " " + ex.StackTrace);
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
                        if (status == (HttpStatusCode)403 && (dorg.Info == null || !dorg.LastInfo.Equals(dorg.getCurrentInformations())))
                        {
                            if (dorg.IsActive)
                                dorg.Stop();
                            MessageBox.Show("Remote file properties seems to be changed. Refresh the url");
                            requestNewUrl();
                        }
                    }
                    else
                    {
                        lblStatus.Text = ex.Message;
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
            waiterForm.ShowDialog();
        }

        private void dorg_MergingProgressChanged(object sender, AltoMultiThreadDownloadManager.EventArguments.MergingProgressChangedEventArgs e)
        {
            timer1.Enabled = false;
            progressBar1.Value = (int)e.Progress;
        }

        private void dorg_ProgressChanged(object sender, AltoMultiThreadDownloadManager.EventArguments.ProgressChangedEventArgs e)
        {

        }

        private void dorg_Completed(object sender, EventArgs e)
        {
            btnOpenFile.Enabled = true;
            btnPauseResume.Text = "Download again";
            if (Directory.Exists(dorg.RangeDir))
                Directory.Delete(dorg.RangeDir, true);
            updateUI();
        }

        private void dorg_DownloadInfoReceived(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                var info = dorg.Info;
                dorg.LastInfo = info.Clone();
                //Set the filename after we have ServerFileName determined
                dorg.FilePath = Path.Combine(dorg.FilePath, info.ServerFileName);
                dorg.RangeDir = Path.Combine(rootRangeDir, info.ServerFileName);
                Directory.CreateDirectory(dorg.RangeDir);
                foreach (var item in dorg.Ranges)
                {
                    item.SaveDir = dorg.RangeDir;
                }
                //Set progress bar totallength
                segmentedProgressBar1.ContentLength = info.ContentSize;

                timer1.Start();
                btnPauseResume.Text = "Pause";
                btnPauseResume.Enabled = info.AcceptRanges;
                lblContentSize.Text = string.Format(lblContentSize.Text, info.ContentSize.ToHumanReadableSize());
                lblServerFileName.Text = string.Format(lblServerFileName.Text, info.ServerFileName);
                lblResumeability.Text = string.Format(lblResumeability.Text, info.AcceptRanges);
            });
        }

        private void dorg_Resumed(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void dorg_Stopped(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            updateUI();

        }
        void updateUI()
        {
            try
            {
                segmentedProgressBar1.ContentLength = dorg.Info.ContentSize;
                segmentedProgressBar1.Bars =
                    dorg.Ranges.ToList().Select(x => new Bar(x.TotalBytesReceived, x.Start, x.Status)).ToArray();
                progressBar1.Value = (int)dorg.Progress;
                lblSpeed.Text = string.Format("Speed: {0}", dorg.Speed.ToHumanReadableSize() + "/s");
                lblBytesReceived.Text = string.Format("Bytes Received: {0} / {1}", dorg.TotalBytesReceived.ToHumanReadableSize(), dorg.Info.ContentSize.ToHumanReadableSize());
                lblContentSize.Text = string.Format("Content Size: {0}", dorg.Info.ContentSize.ToHumanReadableSize());
                lblServerFileName.Text = string.Format("Server Filename: {0}", dorg.Info.ServerFileName);
                lblResumeability.Text = string.Format("Resumeability: {0}", dorg.Info.AcceptRanges ? "Yes" : "No");
            }
            catch
            {

            }
        }
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(dorg.FilePath);
            }
            catch
            {

            }
        }






    }
}
