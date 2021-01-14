namespace DownloadManagerPortal
{
    partial class DownloadCenterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadCenterForm));
            this.btnAddDownload = new AltoControls.AltoButton();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnResume = new AltoControls.AltoButton();
            this.btnDelete = new AltoControls.AltoButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnIntegrateChrome = new AltoControls.AltoButton();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnSettings = new AltoControls.AltoButton();
            this.SuspendLayout();
            // 
            // btnAddDownload
            // 
            this.btnAddDownload.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.btnAddDownload.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.btnAddDownload.BackColor = System.Drawing.Color.Transparent;
            this.btnAddDownload.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAddDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnAddDownload.ForeColor = System.Drawing.Color.Black;
            this.btnAddDownload.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.btnAddDownload.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.btnAddDownload.Location = new System.Drawing.Point(13, 12);
            this.btnAddDownload.Name = "btnAddDownload";
            this.btnAddDownload.Radius = 10;
            this.btnAddDownload.Size = new System.Drawing.Size(98, 25);
            this.btnAddDownload.Stroke = false;
            this.btnAddDownload.StrokeColor = System.Drawing.Color.Gray;
            this.btnAddDownload.TabIndex = 12;
            this.btnAddDownload.Text = "Add Download";
            this.btnAddDownload.Transparency = false;
            this.btnAddDownload.Click += new System.EventHandler(this.btnAddDownload_Click);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader7,
            this.columnHeader3,
            this.columnHeader8,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(13, 58);
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(750, 221);
            this.listView1.TabIndex = 13;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 155;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Progress";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Total Bytes Received";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "ContentSize";
            this.columnHeader3.Width = 110;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Status";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Speed";
            this.columnHeader4.Width = 103;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Resumeability";
            this.columnHeader5.Width = 146;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Url";
            this.columnHeader6.Width = 156;
            // 
            // btnResume
            // 
            this.btnResume.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.btnResume.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.btnResume.BackColor = System.Drawing.Color.Transparent;
            this.btnResume.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnResume.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnResume.ForeColor = System.Drawing.Color.Black;
            this.btnResume.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.btnResume.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.btnResume.Location = new System.Drawing.Point(117, 12);
            this.btnResume.Name = "btnResume";
            this.btnResume.Radius = 10;
            this.btnResume.Size = new System.Drawing.Size(98, 25);
            this.btnResume.Stroke = false;
            this.btnResume.StrokeColor = System.Drawing.Color.Gray;
            this.btnResume.TabIndex = 12;
            this.btnResume.Text = "Resume";
            this.btnResume.Transparency = false;
            this.btnResume.Click += new System.EventHandler(this.btnResume_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.btnDelete.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.btnDelete.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.btnDelete.Location = new System.Drawing.Point(221, 12);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Radius = 10;
            this.btnDelete.Size = new System.Drawing.Size(98, 25);
            this.btnDelete.Stroke = false;
            this.btnDelete.StrokeColor = System.Drawing.Color.Gray;
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Transparency = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            // 
            // btnIntegrateChrome
            // 
            this.btnIntegrateChrome.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnIntegrateChrome.Active2 = System.Drawing.Color.Lime;
            this.btnIntegrateChrome.BackColor = System.Drawing.Color.Transparent;
            this.btnIntegrateChrome.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnIntegrateChrome.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnIntegrateChrome.ForeColor = System.Drawing.Color.Black;
            this.btnIntegrateChrome.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnIntegrateChrome.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnIntegrateChrome.Location = new System.Drawing.Point(648, 12);
            this.btnIntegrateChrome.Name = "btnIntegrateChrome";
            this.btnIntegrateChrome.Radius = 10;
            this.btnIntegrateChrome.Size = new System.Drawing.Size(115, 25);
            this.btnIntegrateChrome.Stroke = false;
            this.btnIntegrateChrome.StrokeColor = System.Drawing.Color.Gray;
            this.btnIntegrateChrome.TabIndex = 12;
            this.btnIntegrateChrome.Text = "Integrate Chrome";
            this.btnIntegrateChrome.Transparency = false;
            this.btnIntegrateChrome.Click += new System.EventHandler(this.btnIntegrateChrome_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipText = "Alto Download Center";
            this.notifyIcon1.BalloonTipTitle = "Alto Download Center";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Open Download Center";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // btnSettings
            // 
            this.btnSettings.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.btnSettings.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.btnSettings.BackColor = System.Drawing.Color.Transparent;
            this.btnSettings.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSettings.ForeColor = System.Drawing.Color.Black;
            this.btnSettings.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.btnSettings.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.btnSettings.Location = new System.Drawing.Point(544, 12);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Radius = 10;
            this.btnSettings.Size = new System.Drawing.Size(98, 25);
            this.btnSettings.Stroke = false;
            this.btnSettings.StrokeColor = System.Drawing.Color.Gray;
            this.btnSettings.TabIndex = 12;
            this.btnSettings.Text = "Settings";
            this.btnSettings.Transparency = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // DownloadCenterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 291);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btnIntegrateChrome);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnResume);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnAddDownload);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(791, 330);
            this.Name = "DownloadCenterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Download Center";
            this.Resize += new System.EventHandler(this.DownloadCenterForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private AltoControls.AltoButton btnAddDownload;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private AltoControls.AltoButton btnResume;
        private AltoControls.AltoButton btnDelete;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private AltoControls.AltoButton btnIntegrateChrome;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private AltoControls.AltoButton btnSettings;
    }
}