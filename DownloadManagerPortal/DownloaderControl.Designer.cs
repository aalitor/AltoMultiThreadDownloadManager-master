namespace DownloadManagerPortal
{
    partial class DownloaderControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblResumeability = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnOpenFile = new AltoControls.AltoButton();
            this.btnDelete = new AltoControls.AltoButton();
            this.btnPauseResume = new AltoControls.AltoButton();
            this.progressBar1 = new AltoControls.AltoPB();
            this.label5 = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblContentSize = new System.Windows.Forms.Label();
            this.lblServerFileName = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblBytesReceived = new System.Windows.Forms.Label();
            this.segmentedProgressBar1 = new DownloadManagerPortal.DownloadHandler.UIControls.SegmentedProgressBar();
            this.SuspendLayout();
            // 
            // lblResumeability
            // 
            this.lblResumeability.AutoSize = true;
            this.lblResumeability.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblResumeability.ForeColor = System.Drawing.Color.Red;
            this.lblResumeability.Location = new System.Drawing.Point(312, 23);
            this.lblResumeability.Name = "lblResumeability";
            this.lblResumeability.Size = new System.Drawing.Size(21, 13);
            this.lblResumeability.TabIndex = 6;
            this.lblResumeability.Text = "No";
            // 
            // timer1
            // 
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.btnOpenFile.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.btnOpenFile.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenFile.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOpenFile.Enabled = false;
            this.btnOpenFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnOpenFile.ForeColor = System.Drawing.Color.Black;
            this.btnOpenFile.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.btnOpenFile.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.btnOpenFile.Location = new System.Drawing.Point(396, 47);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Radius = 10;
            this.btnOpenFile.Size = new System.Drawing.Size(86, 25);
            this.btnOpenFile.Stroke = false;
            this.btnOpenFile.StrokeColor = System.Drawing.Color.Gray;
            this.btnOpenFile.TabIndex = 11;
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.Transparency = false;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.btnDelete.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.btnDelete.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.btnDelete.Location = new System.Drawing.Point(552, 48);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Radius = 10;
            this.btnDelete.Size = new System.Drawing.Size(86, 25);
            this.btnDelete.Stroke = false;
            this.btnDelete.StrokeColor = System.Drawing.Color.Gray;
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Remove";
            this.btnDelete.Transparency = false;
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
            this.btnPauseResume.Location = new System.Drawing.Point(646, 48);
            this.btnPauseResume.Name = "btnPauseResume";
            this.btnPauseResume.Radius = 10;
            this.btnPauseResume.Size = new System.Drawing.Size(86, 25);
            this.btnPauseResume.Stroke = false;
            this.btnPauseResume.StrokeColor = System.Drawing.Color.Gray;
            this.btnPauseResume.TabIndex = 11;
            this.btnPauseResume.Text = "Pause";
            this.btnPauseResume.Transparency = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(396, 4);
            this.progressBar1.MaxValue = 100;
            this.progressBar1.MinValue = 0;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.progressBar1.Size = new System.Drawing.Size(336, 18);
            this.progressBar1.TabIndex = 12;
            this.progressBar1.Text = "altoPB1";
            this.progressBar1.Value = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label5.Location = new System.Drawing.Point(236, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Resumeability:";
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblSpeed.Location = new System.Drawing.Point(236, 41);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(41, 13);
            this.lblSpeed.TabIndex = 10;
            this.lblSpeed.Text = "Speed:";
            // 
            // lblContentSize
            // 
            this.lblContentSize.AutoSize = true;
            this.lblContentSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblContentSize.Location = new System.Drawing.Point(3, 22);
            this.lblContentSize.Name = "lblContentSize";
            this.lblContentSize.Size = new System.Drawing.Size(70, 13);
            this.lblContentSize.TabIndex = 7;
            this.lblContentSize.Text = "Content Size:";
            // 
            // lblServerFileName
            // 
            this.lblServerFileName.AutoSize = true;
            this.lblServerFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblServerFileName.Location = new System.Drawing.Point(3, 6);
            this.lblServerFileName.Name = "lblServerFileName";
            this.lblServerFileName.Size = new System.Drawing.Size(86, 13);
            this.lblServerFileName.TabIndex = 5;
            this.lblServerFileName.Text = "Server Filename:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblStatus.Location = new System.Drawing.Point(3, 54);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(63, 13);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Last Status:";
            // 
            // lblBytesReceived
            // 
            this.lblBytesReceived.AutoSize = true;
            this.lblBytesReceived.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblBytesReceived.Location = new System.Drawing.Point(3, 38);
            this.lblBytesReceived.Name = "lblBytesReceived";
            this.lblBytesReceived.Size = new System.Drawing.Size(85, 13);
            this.lblBytesReceived.TabIndex = 8;
            this.lblBytesReceived.Text = "Bytes Received:";
            // 
            // segmentedProgressBar1
            // 
            this.segmentedProgressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.segmentedProgressBar1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.segmentedProgressBar1.Bars = new DownloadManagerPortal.DownloadHandler.UIControls.Bar[0];
            this.segmentedProgressBar1.ContentLength = ((long)(100));
            this.segmentedProgressBar1.Location = new System.Drawing.Point(396, 26);
            this.segmentedProgressBar1.Name = "segmentedProgressBar1";
            this.segmentedProgressBar1.Size = new System.Drawing.Size(336, 18);
            this.segmentedProgressBar1.TabIndex = 0;
            // 
            // DownloaderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnPauseResume);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.lblBytesReceived);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblServerFileName);
            this.Controls.Add(this.lblContentSize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblResumeability);
            this.Controls.Add(this.segmentedProgressBar1);
            this.MinimumSize = new System.Drawing.Size(677, 50);
            this.Name = "DownloaderControl";
            this.Size = new System.Drawing.Size(742, 75);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DownloadHandler.UIControls.SegmentedProgressBar segmentedProgressBar1;
        private System.Windows.Forms.Label lblResumeability;
        private System.Windows.Forms.Timer timer1;
        private AltoControls.AltoButton btnOpenFile;
        private AltoControls.AltoButton btnDelete;
        private AltoControls.AltoButton btnPauseResume;
        private AltoControls.AltoPB progressBar1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label lblContentSize;
        private System.Windows.Forms.Label lblServerFileName;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblBytesReceived;
    }
}
