namespace Chrome_Extension_Integrator
{
    partial class Chrome_Extension_Integrator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Chrome_Extension_Integrator));
            this.btnIntegrate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtExtId = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.txtAppExe = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtExtname = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnIntegrate
            // 
            this.btnIntegrate.Location = new System.Drawing.Point(347, 92);
            this.btnIntegrate.Name = "btnIntegrate";
            this.btnIntegrate.Size = new System.Drawing.Size(87, 35);
            this.btnIntegrate.TabIndex = 0;
            this.btnIntegrate.Text = "Integrate";
            this.btnIntegrate.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Extension Id:";
            // 
            // txtExtId
            // 
            this.txtExtId.Location = new System.Drawing.Point(92, 9);
            this.txtExtId.Name = "txtExtId";
            this.txtExtId.Size = new System.Drawing.Size(342, 20);
            this.txtExtId.TabIndex = 2;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(404, 36);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(30, 23);
            this.btnOpenFile.TabIndex = 3;
            this.btnOpenFile.Text = "...";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            // 
            // txtAppExe
            // 
            this.txtAppExe.Location = new System.Drawing.Point(92, 38);
            this.txtAppExe.Name = "txtAppExe";
            this.txtAppExe.Size = new System.Drawing.Size(306, 20);
            this.txtAppExe.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Application Exe:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Extension Name:";
            // 
            // txtExtname
            // 
            this.txtExtname.Enabled = false;
            this.txtExtname.Location = new System.Drawing.Point(92, 66);
            this.txtExtname.Name = "txtExtname";
            this.txtExtname.Size = new System.Drawing.Size(342, 20);
            this.txtExtname.TabIndex = 2;
            this.txtExtname.Text = "com.alto.multithreaddownloadmanager";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Executable Files (*.exe) | *.exe";
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Enabled = false;
            this.richTextBox1.Location = new System.Drawing.Point(4, 92);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ShowSelectionMargin = true;
            this.richTextBox1.Size = new System.Drawing.Size(337, 126);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // Chrome_Extension_Integrator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 230);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.txtExtname);
            this.Controls.Add(this.txtAppExe);
            this.Controls.Add(this.txtExtId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnIntegrate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Chrome_Extension_Integrator";
            this.Text = "Chrome Extension Integrator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnIntegrate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtExtId;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.TextBox txtAppExe;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtExtname;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}