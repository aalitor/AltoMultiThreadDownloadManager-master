
using System;
using System.Windows.Forms;
using AltoMultiThreadDownloadManager;
using AltoMultiThreadDownloadManager.Helpers;
using System.IO;
using System.Linq;
using AltoMultiThreadDownloadManager.NativeMessages;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Diagnostics;
using DownloadManagerPortal.DownloadHandler.UIControls;
using AltoMultiThreadDownloadManager.Exceptions;
using System.Net;
namespace DownloadManagerPortal.DownloadHandler
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>

    public partial class DownloadHandlerForm : Form
    {
        DownloadMessage MSG;
        MultiThreadDownloadOrganizer MTDO;
        public MultiThreadDownloadOrganizer dorg = null;
        AsyncOperation aop;
        public bool waitingNewUrl;
        public WaitingNewUrl WaiterForm { get; set; }

        public DownloadHandlerForm(MultiThreadDownloadOrganizer mtdobj)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            btnStart.Click += btnStart_Click;
            btnPauseResume.Click += btnPauseResume_Click;
            this.Load += MainForm_Load;
            this.MTDO = mtdobj;

            loadMTDO();
        }
        public string NameId
        {
            get
            {
                return dorg.Info != null ? dorg.Info.ServerFileName : "NULL.NULL";
            }
        }
        bool flagCloseAfterStop = false;
        void DownloadHandlerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dorg != null && dorg.IsActive)
            {
                this.ControlBox = false;
                this.Enabled = false;
                e.Cancel = true;
                flagCloseAfterStop = true;
                dorg.Stop();
            }
        }

        public DownloadHandlerForm(DownloadMessage message)
        {
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            btnStart.Click += btnStart_Click;
            btnPauseResume.Click += btnPauseResume_Click;
            this.Load += MainForm_Load;
            MSG = message;
            loadMTDO();
        }
        void loadMTDO()
        {
            txtUrl.Enabled = false;
            var finalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            tempFolder = Path.Combine(tempFolder, "AltoDownloadAccelerator");
            Directory.CreateDirectory(tempFolder);
            var nofThreads = 8;
            dorg = new MultiThreadDownloadOrganizer("", finalFolder, tempFolder, nofThreads);
            Logger.Write("loadMTDO " + DateTime.Now);

            if (MTDO != null)
            {
                var json = JsonConvert.SerializeObject(MTDO);

                dorg = JsonConvert.DeserializeObject<MultiThreadDownloadOrganizer>(json);

                txtUrl.Text = dorg.Url;
                dorg.Stopped += dorg_Stopped;
                dorg.Resumed += dorg_Resumed;
                dorg.DownloadInfoReceived += dorg_DownloadInfoReceived;
                dorg.Completed += dorg_Completed;
                dorg.ProgressChanged += dorg_ProgressChanged;
                dorg.MergingProgressChanged += dorg_MergingProgressChanged;
                dorg.ErrorOccured += dorg_ErrorOccured;
                dorg.LastTry = DateTime.Now;
                this.Text = dorg.Info.ServerFileName;
                btnStart.Enabled = false;
                btnPauseResume.Enabled = false;
                Logger.Write("loadMTDO if 1" + DateTime.Now);

            }
            else if (MSG != null)
            {
                dorg = new MultiThreadDownloadOrganizer(MSG.Url, finalFolder, tempFolder, nofThreads);
                dorg.DownloadRequestMessage = MSG;
                txtUrl.Text = MSG.Url;

                dorg.Stopped += dorg_Stopped;
                dorg.Resumed += dorg_Resumed;
                dorg.DownloadInfoReceived += dorg_DownloadInfoReceived;
                dorg.Completed += dorg_Completed;
                dorg.ErrorOccured += dorg_ErrorOccured;
                dorg.ProgressChanged += dorg_ProgressChanged;
                dorg.MergingProgressChanged += dorg_MergingProgressChanged;

                dorg.LastTry = DateTime.Now;

                btnStart.Enabled = false;
                btnPauseResume.Enabled = false;
            }
        }
        void MainForm_Load(object sender, EventArgs e)
        {
            this.FormClosing += DownloadHandlerForm_FormClosing;
            if (MTDO != null)
            {
                Logger.Write("Resume a girdi loaddan " + DateTime.Now);
                dorg.Resume();
            }
            else if (MSG != null)
            {
                dorg.Start();
            }

        }


        void dorg_ErrorOccured(object sender, ErrorEventArgs e)
        {
            var ex = e.GetException();
            if (ex is RemoteFilePropertiesChangedException)
            {
                var dialogResult = MessageBox.Show("Url was expired. A new tab will be opened in browser to refresh the link. AltoDownloadManager will capture the link automatically"
                    , "Refresh download link", MessageBoxButtons.YesNo);
                if (dialogResult == System.Windows.Forms.DialogResult.No)
                {
                    this.Close();
                    return;
                }
                waitingNewUrl = true;
                this.Hide();
                WaiterForm = new WaitingNewUrl(dorg.Info != null ? dorg.Info.ServerFileName : "");
                WaiterForm.FormClosed += WaiterForm_FormClosed;
                
                Process.Start(dorg.DownloadRequestMessage.TabUrl);
                WaiterForm.Shown += (m, n) => WaiterForm.Activate();
                WaiterForm.ShowDialog(null);
            }
            else
            {
                dorg.Stop();
                var webex = (WebException)ex;
                if (webex != null)
                {
                    MessageBox.Show(webex.Response.Headers[HttpResponseHeader.RetryAfter]);
                }
                MessageBox.Show(ex.Message + " " + ex.StackTrace);
            }
        }

        void WaiterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            waitingNewUrl = false;
            
            this.Close();
        }
        void btnStart_Click(object sender, EventArgs e)
        {

        }
        void dorg_MergingProgressChanged(object sender, AltoMultiThreadDownloadManager.EventArguments.MergingProgressChangedEventArgs e)
        {
            try
            {
                aop = AsyncOperationManager.CreateOperation(null);
                //Create bars to represent downloaded segments
                //Need totalbytesreceived and start offset to calculate scale and drawing
                timer1.Enabled = false;

                progressBar1.Value = (int)e.Progress;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace);
            }

        }

        void dorg_ProgressChanged(object sender, AltoMultiThreadDownloadManager.EventArguments.ProgressChangedEventArgs e)
        {
            try
            {
                segmentedProgressBar1.ContentLength = dorg.Info.ContentSize;
                segmentedProgressBar1.Bars =
                    dorg.Ranges.ToList().Select(x => new Bar(x.TotalBytesReceived, x.Start, x.Status)).ToArray();
                progressBar1.Value = (int)dorg.Progress;
                lblProgress.Text = dorg.Progress + "%";
                lblSpeed.Text = dorg.Speed.ToHumanReadableSize() + "/s";
                lblTotalBytes.Text = dorg.TotalBytesReceived.ToHumanReadableSize();
                lblThreads.Text = dorg.Ranges.ToList().Count(x => x.Status == State.Downloading).ToString();
                lblContentSize.Text = string.Format(lblContentSize.Text, dorg.Info.ContentSize.ToHumanReadableSize());
                lblServerFileName.Text = string.Format(lblServerFileName.Text, dorg.Info.ServerFileName);
                lblResumeability.Text = string.Format(lblResumeability.Text, dorg.Info.AcceptRanges);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace);
            }
        }
        void dorg_Completed(object sender, EventArgs e)
        {
            try
            {
                var f = new Download_Completed_Form(dorg.FilePath);
                f.Shown += (m, n) => f.Activate();
                f.FormClosed += (m, n) => this.Close();
                this.Hide();
                f.ShowDialog(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace);
            }
        }
        void dorg_DownloadInfoReceived(object sender, EventArgs e)
        {
            try
            {
                var info = dorg.Info;
                //Set the filename after we have ServerFileName determined
                dorg.FilePath = Path.Combine(dorg.FilePath, info.ServerFileName);
                dorg.RangeDir = Path.Combine(dorg.RangeDir, info.ServerFileName);
                Directory.CreateDirectory(dorg.RangeDir);
                foreach (var item in dorg.Ranges)
                {
                    item.SaveDir = dorg.RangeDir;
                }
                //Set progress bar totallength
                segmentedProgressBar1.ContentLength = info.ContentSize;

                timer1.Start();
                btnPauseResume.Enabled = info.AcceptRanges;
                lblContentSize.Text = string.Format(lblContentSize.Text, info.ContentSize.ToHumanReadableSize());
                lblServerFileName.Text = string.Format(lblServerFileName.Text, info.ServerFileName);
                lblResumeability.Text = string.Format(lblResumeability.Text, info.AcceptRanges);
            }
            catch (Exception ex)
            {

            }
        }
        void dorg_Resumed(object sender, EventArgs e)
        {
            try
            {
                dorg.LastTry = DateTime.Now;
                btnPauseResume.Enabled = true;
                btnPauseResume.Text = "Pause";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace);
            }
        }
        void dorg_Stopped(object sender, EventArgs e)
        {
            try
            {
                dorg.LastTry = DateTime.Now;
                btnPauseResume.Enabled = true;
                btnPauseResume.Text = "Resume";
                if (flagCloseAfterStop)
                {
                    flagCloseAfterStop = false;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.StackTrace);
            }
        }
        void btnPauseResume_Click(object sender, EventArgs e)
        {

            btnPauseResume.Enabled = false;
            if (btnPauseResume.Text == "Pause")
                dorg.Stop();
            else
            {
                Logger.Write("Resume a girdi butondan");

                dorg.Resume();
                timer1.Start();
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //Create bars to represent downloaded segments
                //Need totalbytesreceived and start offset to calculate scale and drawing
            }
            catch
            {

            }
        }
    }
}
