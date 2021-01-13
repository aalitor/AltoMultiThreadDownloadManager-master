namespace DownloadManagerPortal.Downloader
{
    partial class DownloaderForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblResumeability = new System.Windows.Forms.Label();
            this.btnPauseResume = new AltoControls.AltoButton();
            this.lblBytesReceived = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblServerFileName = new System.Windows.Forms.Label();
            this.lblContentSize = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.lblError = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.segmentedProgressBar1 = new DownloadManagerPortal.Downloader.UIControls.SegmentedProgressBar();
            this.lblActiveThreads = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblResumeability
            // 
            this.lblResumeability.AutoSize = true;
            this.lblResumeability.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblResumeability.ForeColor = System.Drawing.Color.Red;
            this.lblResumeability.Location = new System.Drawing.Point(314, 66);
            this.lblResumeability.Name = "lblResumeability";
            this.lblResumeability.Size = new System.Drawing.Size(21, 13);
            this.lblResumeability.TabIndex = 16;
            this.lblResumeability.Text = "No";
            // 
            // btnPauseResume
            // 
            this.btnPauseResume.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.btnPauseResume.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.btnPauseResume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPauseResume.BackColor = System.Drawing.Color.Transparent;
            this.btnPauseResume.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnPauseResume.Enabled = false;
            this.btnPauseResume.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnPauseResume.ForeColor = System.Drawing.Color.Black;
            this.btnPauseResume.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.btnPauseResume.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.btnPauseResume.Location = new System.Drawing.Point(292, 212);
            this.btnPauseResume.Name = "btnPauseResume";
            this.btnPauseResume.Radius = 10;
            this.btnPauseResume.Size = new System.Drawing.Size(86, 25);
            this.btnPauseResume.Stroke = false;
            this.btnPauseResume.StrokeColor = System.Drawing.Color.Gray;
            this.btnPauseResume.TabIndex = 21;
            this.btnPauseResume.Text = "Pause";
            this.btnPauseResume.Transparency = false;
            // 
            // lblBytesReceived
            // 
            this.lblBytesReceived.AutoSize = true;
            this.lblBytesReceived.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblBytesReceived.Location = new System.Drawing.Point(5, 90);
            this.lblBytesReceived.Name = "lblBytesReceived";
            this.lblBytesReceived.Size = new System.Drawing.Size(85, 13);
            this.lblBytesReceived.TabIndex = 19;
            this.lblBytesReceived.Text = "Bytes Received:";
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblSpeed.Location = new System.Drawing.Point(238, 90);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(41, 13);
            this.lblSpeed.TabIndex = 20;
            this.lblSpeed.Text = "Speed:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblStatus.Location = new System.Drawing.Point(5, 114);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(63, 13);
            this.lblStatus.TabIndex = 14;
            this.lblStatus.Text = "Last Status:";
            // 
            // lblServerFileName
            // 
            this.lblServerFileName.AutoSize = true;
            this.lblServerFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblServerFileName.Location = new System.Drawing.Point(5, 42);
            this.lblServerFileName.Name = "lblServerFileName";
            this.lblServerFileName.Size = new System.Drawing.Size(86, 13);
            this.lblServerFileName.TabIndex = 15;
            this.lblServerFileName.Text = "Server Filename:";
            // 
            // lblContentSize
            // 
            this.lblContentSize.AutoSize = true;
            this.lblContentSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblContentSize.Location = new System.Drawing.Point(5, 66);
            this.lblContentSize.Name = "lblContentSize";
            this.lblContentSize.Size = new System.Drawing.Size(70, 13);
            this.lblContentSize.TabIndex = 18;
            this.lblContentSize.Text = "Content Size:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label5.Location = new System.Drawing.Point(238, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Resumeability:";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(8, 168);
            this.progressBar1.Maximum = 10000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(370, 20);
            this.progressBar1.TabIndex = 24;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblProgress.Location = new System.Drawing.Point(238, 114);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(51, 13);
            this.lblProgress.TabIndex = 20;
            this.lblProgress.Text = "Progress:";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(8, 13);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.ReadOnly = true;
            this.txtUrl.Size = new System.Drawing.Size(370, 20);
            this.txtUrl.TabIndex = 25;
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblError.Location = new System.Drawing.Point(5, 139);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(55, 13);
            this.lblError.TabIndex = 14;
            this.lblError.Text = "Last Error:";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // segmentedProgressBar1
            // 
            this.segmentedProgressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.segmentedProgressBar1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.segmentedProgressBar1.Bars = new DownloadManagerPortal.Downloader.UIControls.Bar[0];
            this.segmentedProgressBar1.ContentLength = ((long)(100));
            this.segmentedProgressBar1.Location = new System.Drawing.Point(8, 191);
            this.segmentedProgressBar1.Name = "segmentedProgressBar1";
            this.segmentedProgressBar1.Size = new System.Drawing.Size(370, 18);
            this.segmentedProgressBar1.TabIndex = 13;
            // 
            // lblActiveThreads
            // 
            this.lblActiveThreads.AutoSize = true;
            this.lblActiveThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblActiveThreads.Location = new System.Drawing.Point(238, 139);
            this.lblActiveThreads.Name = "lblActiveThreads";
            this.lblActiveThreads.Size = new System.Drawing.Size(82, 13);
            this.lblActiveThreads.TabIndex = 20;
            this.lblActiveThreads.Text = "Active Threads:";
            // 
            // DownloaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 250);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblResumeability);
            this.Controls.Add(this.btnPauseResume);
            this.Controls.Add(this.lblBytesReceived);
            this.Controls.Add(this.lblActiveThreads);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblServerFileName);
            this.Controls.Add(this.lblContentSize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.segmentedProgressBar1);
            this.Name = "DownloaderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Downloader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblResumeability;
        private AltoControls.AltoButton btnPauseResume;
        private System.Windows.Forms.Label lblBytesReceived;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblServerFileName;
        private System.Windows.Forms.Label lblContentSize;
        private System.Windows.Forms.Label label5;
        private Downloader.UIControls.SegmentedProgressBar segmentedProgressBar1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblActiveThreads;
    }
}