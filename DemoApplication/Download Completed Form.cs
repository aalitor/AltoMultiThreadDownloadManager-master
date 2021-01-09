using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AltoMultiThreadDownloadManager.AssociatedIcons;
using System.Diagnostics;
namespace DemoApplication
{
    public partial class Download_Completed_Form : Form
    {
        public Download_Completed_Form(string filepath)
        {
            InitializeComponent();
            this.Load += Download_Completed_Form_Load;
            txtPath.Text = filepath;
        }

        void Download_Completed_Form_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = IconReader.GetFileIcon(txtPath.Text, IconReader.IconSize.Large, false).ToBitmap();

            btnOpenFile.Click += btnOpenFile_Click;
            btnOpenFolder.Click += btnOpenFolder_Click;
        }

        void btnOpenFolder_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void btnOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                Process.Start(txtPath.Text);
            }
            catch
            {

            }
            this.Close();
        }
    }
}
