/*
 * Created by SharpDevelop.
 * User: kafeinaltor
 * Date: 21.08.2020
 * Time: 22:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace DemoApplication
{
    partial class DownloadHandlerForm
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPauseResume;
        private System.Windows.Forms.Label lblContentSize;
        private System.Windows.Forms.Label lblResumeability;
        private System.Windows.Forms.Label lblServerFileName;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label label4;
        private DemoApplication.SegmentedProgressBar segmentedProgressBar1;
        private System.Windows.Forms.ProgressBar progressBar1;

        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPauseResume = new System.Windows.Forms.Button();
            this.lblContentSize = new System.Windows.Forms.Label();
            this.lblResumeability = new System.Windows.Forms.Label();
            this.lblServerFileName = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblTotalBytes = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblThreads = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.segmentedProgressBar1 = new DemoApplication.SegmentedProgressBar();
            this.SuspendLayout();
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(48, 12);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(373, 20);
            this.txtUrl.TabIndex = 0;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(48, 38);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "URL";
            // 
            // btnPauseResume
            // 
            this.btnPauseResume.Location = new System.Drawing.Point(129, 38);
            this.btnPauseResume.Name = "btnPauseResume";
            this.btnPauseResume.Size = new System.Drawing.Size(75, 23);
            this.btnPauseResume.TabIndex = 1;
            this.btnPauseResume.Text = "Pause";
            this.btnPauseResume.UseVisualStyleBackColor = true;
            // 
            // lblContentSize
            // 
            this.lblContentSize.AutoSize = true;
            this.lblContentSize.Location = new System.Drawing.Point(48, 101);
            this.lblContentSize.Name = "lblContentSize";
            this.lblContentSize.Size = new System.Drawing.Size(87, 13);
            this.lblContentSize.TabIndex = 3;
            this.lblContentSize.Text = "Content Size: {0}";
            // 
            // lblResumeability
            // 
            this.lblResumeability.AutoSize = true;
            this.lblResumeability.Location = new System.Drawing.Point(48, 131);
            this.lblResumeability.Name = "lblResumeability";
            this.lblResumeability.Size = new System.Drawing.Size(92, 13);
            this.lblResumeability.TabIndex = 3;
            this.lblResumeability.Text = "Resumeability: {0}";
            // 
            // lblServerFileName
            // 
            this.lblServerFileName.AutoSize = true;
            this.lblServerFileName.Location = new System.Drawing.Point(48, 75);
            this.lblServerFileName.Name = "lblServerFileName";
            this.lblServerFileName.Size = new System.Drawing.Size(103, 13);
            this.lblServerFileName.TabIndex = 3;
            this.lblServerFileName.Text = "Server Filename: {0}";
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(251, 101);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 13);
            this.lblProgress.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Progress:";
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(241, 131);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(0, 13);
            this.lblSpeed.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(194, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Speed:";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(48, 179);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(373, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // lblTotalBytes
            // 
            this.lblTotalBytes.AutoSize = true;
            this.lblTotalBytes.Location = new System.Drawing.Point(286, 157);
            this.lblTotalBytes.Name = "lblTotalBytes";
            this.lblTotalBytes.Size = new System.Drawing.Size(0, 13);
            this.lblTotalBytes.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(194, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Bytes Received:";
            // 
            // timer1
            // 
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblThreads
            // 
            this.lblThreads.AutoSize = true;
            this.lblThreads.Location = new System.Drawing.Point(158, 157);
            this.lblThreads.Name = "lblThreads";
            this.lblThreads.Size = new System.Drawing.Size(0, 13);
            this.lblThreads.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(49, 157);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Nof Active Threads:";
            // 
            // segmentedProgressBar1
            // 
            this.segmentedProgressBar1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.segmentedProgressBar1.Bars = new DemoApplication.Bar[0];
            this.segmentedProgressBar1.ContentLength = ((long)(100));
            this.segmentedProgressBar1.Location = new System.Drawing.Point(48, 212);
            this.segmentedProgressBar1.Name = "segmentedProgressBar1";
            this.segmentedProgressBar1.Size = new System.Drawing.Size(373, 20);
            this.segmentedProgressBar1.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 241);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.segmentedProgressBar1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblTotalBytes);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblThreads);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.lblServerFileName);
            this.Controls.Add(this.lblResumeability);
            this.Controls.Add(this.lblContentSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPauseResume);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtUrl);
            this.Name = "MainForm";
            this.Text = "Alto Download Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblTotalBytes;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblThreads;
        private System.Windows.Forms.Label label6;
    }
}
