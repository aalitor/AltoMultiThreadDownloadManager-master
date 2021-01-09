/*
 * Created by SharpDevelop.
 * User: kafeinaltor
 * Date: 21.08.2020
 * Time: 22:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AltoMultiThreadDownloadManager;
using AltoMultiThreadDownloadManager.Helpers;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using AltoMultiThreadDownloadManager.EventArguments;
using AltoMultiThreadDownloadManager.NativeMessages;
namespace DemoApplication
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>

    public partial class DownloadHandlerForm : Form
    {
        public MultiThreadDownloadOrganizer dorg = null;
        public DownloadHandlerForm()
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
        }

        public DownloadHandlerForm(DownloadMessage message, SingleInstanceTracker)
        {
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
            btnStart.Click += btnStart_Click;
            btnPauseResume.Click += btnPauseResume_Click;
            this.Load += MainForm_Load;
            Program.MSG = message;
        }
        void MainForm_Load(object sender, EventArgs e)
        {
            var finalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var nofThreads = 8;
            dorg = new MultiThreadDownloadOrganizer("", finalFolder, tempFolder, nofThreads);

            if (Program.MTDO != null)
            {
                dorg = Program.MTDO;
                txtUrl.Text = dorg.Url;
                dorg.Stopped += dorg_Stopped;
                dorg.Resumed += dorg_Resumed;
                dorg.DownloadInfoReceived += dorg_DownloadInfoReceived;
                dorg.Completed += dorg_Completed;
                dorg.ErrorOccured += dorg_ErrorOccured;
                dorg.ProgressChanged += dorg_ProgressChanged;
                dorg.MergingProgressChanged += dorg_MergingProgressChanged;
                timer1.Start();

                dorg.LastTry = DateTime.Now;

                btnStart.Enabled = false;
                btnPauseResume.Enabled = false;
                dorg.Resume();
            }
            else if (Program.MSG != null)
            {
                dorg = new MultiThreadDownloadOrganizer(Program.MSG.Url, finalFolder, tempFolder, nofThreads);
                dorg.DownloadRequestMessage = Program.MSG;
                txtUrl.Text = Program.MSG.Url;

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
                dorg.Start();
            }
        }



        void dorg_ErrorOccured(object sender, ErrorEventArgs e)
        {
            MessageBox.Show(e.GetException().Message + " " + e.GetException().StackTrace);
        }
        void btnStart_Click(object sender, EventArgs e)
        {
            if (Program.MSG == null)
            {
                dorg = new MultiThreadDownloadOrganizer(txtUrl.Text, dorg.FilePath, dorg.RangeDir, dorg.NofThread);

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
                dorg.Start();
            }
        }

        void dorg_MergingProgressChanged(object sender, AltoMultiThreadDownloadManager.EventArguments.MergingProgressChangedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                //Create bars to represent downloaded segments
                //Need totalbytesreceived and start offset to calculate scale and drawing
                timer1.Enabled = false;
                progressBar1.Value = (int)e.Progress;
            });
        }

        void dorg_ProgressChanged(object sender, AltoMultiThreadDownloadManager.EventArguments.ProgressChangedEventArgs e)
        {
            //this.Invoke((MethodInvoker)delegate
            //{
            //    //Create bars to represent downloaded segments
            //    //Need totalbytesreceived and start offset to calculate scale and drawing
            //    segmentedProgressBar1.Bars =
            //        dorg.Ranges.Select(x => new Bar(x.TotalBytesReceived, x.Start, x.Status)).ToArray();
            //    progressBar1.Value = (int)dorg.Progress;
            //    lblProgress.Text = dorg.Progress + "%";
            //    lblSpeed.Text = dorg.Speed.ToHumanReadableSize() + "/s";
            //    lblTotalBytes.Text = dorg.TotalBytesReceived.ToHumanReadableSize();
            //});
        }
        void dorg_Completed(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                var f = new Download_Completed_Form(dorg.FilePath);
                f.FormClosing += (a, b) => this.Close();
                f.Show();
                this.Hide();
            }));
        }
        void dorg_DownloadInfoReceived(object sender, EventArgs e)
        {
            var info = dorg.Info;
            //Set the filename after we have ServerFileName determined
            dorg.FilePath = Path.Combine(dorg.FilePath, info.ServerFileName);
            //Set progress bar totallength
            segmentedProgressBar1.ContentLength = info.ContentSize;
            this.Invoke((MethodInvoker)delegate
            {
                timer1.Start();
                btnPauseResume.Enabled = info.AcceptRanges;
                lblContentSize.Text = string.Format(lblContentSize.Text, info.ContentSize.ToHumanReadableSize());
                lblServerFileName.Text = string.Format(lblServerFileName.Text, info.ServerFileName);
                lblResumeability.Text = string.Format(lblResumeability.Text, info.AcceptRanges);
            });
        }
        void dorg_Resumed(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {

                dorg.LastTry = DateTime.Now;
                btnPauseResume.Enabled = true;
                btnPauseResume.Text = "Pause";
            });
        }
        void dorg_Stopped(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                dorg.LastTry = DateTime.Now;
                btnPauseResume.Enabled = true;
                btnPauseResume.Text = "Resume";
            });
        }
        void btnPauseResume_Click(object sender, EventArgs e)
        {
            btnPauseResume.Enabled = false;
            if (btnPauseResume.Text == "Pause")
                dorg.Stop();
            else
                dorg.Resume();

            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Create bars to represent downloaded segments
            //Need totalbytesreceived and start offset to calculate scale and drawing
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
    }
}
