namespace DownloadManagerPortal
{
    partial class DownloadCompletedForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadCompletedForm));
            this.btnOpenFolder = new AltoControls.AltoButton();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new AltoControls.AltoButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.btnOpenFolder.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.btnOpenFolder.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenFolder.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOpenFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnOpenFolder.ForeColor = System.Drawing.Color.Black;
            this.btnOpenFolder.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.btnOpenFolder.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.btnOpenFolder.Location = new System.Drawing.Point(238, 39);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Radius = 10;
            this.btnOpenFolder.Size = new System.Drawing.Size(79, 25);
            this.btnOpenFolder.Stroke = false;
            this.btnOpenFolder.StrokeColor = System.Drawing.Color.Gray;
            this.btnOpenFolder.TabIndex = 13;
            this.btnOpenFolder.Text = "Open Folder";
            this.btnOpenFolder.Transparency = false;
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(12, 13);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(388, 20);
            this.txtPath.TabIndex = 14;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.btnOpenFile.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.btnOpenFile.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenFile.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOpenFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnOpenFile.ForeColor = System.Drawing.Color.Black;
            this.btnOpenFile.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.btnOpenFile.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.btnOpenFile.Location = new System.Drawing.Point(323, 39);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Radius = 10;
            this.btnOpenFile.Size = new System.Drawing.Size(77, 25);
            this.btnOpenFile.Stroke = false;
            this.btnOpenFile.StrokeColor = System.Drawing.Color.Gray;
            this.btnOpenFile.TabIndex = 13;
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.Transparency = false;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 39);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(42, 40);
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // DownloadCompletedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 91);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.btnOpenFolder);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DownloadCompletedForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DownloadCompletedForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DownloadCompletedForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AltoControls.AltoButton btnOpenFolder;
        private System.Windows.Forms.TextBox txtPath;
        private AltoControls.AltoButton btnOpenFile;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}