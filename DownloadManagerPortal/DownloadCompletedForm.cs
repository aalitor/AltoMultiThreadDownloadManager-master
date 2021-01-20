using AltoMultiThreadDownloadManager;
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
using System.IO;
using DownloadManagerPortal.SingleInstancing;
namespace DownloadManagerPortal
{
    public partial class DownloadCompletedForm : Form
    {
        public DownloadCompletedForm()
        {
            InitializeComponent();
        }


        HttpMultiThreadDownloader mtdo;
        public DownloadCompletedForm(HttpMultiThreadDownloader downloader)
        {
            InitializeComponent();
            mtdo = downloader;
            this.Load += DownloadCompletedForm_Load;
        }

        void DownloadCompletedForm_Load(object sender, EventArgs e)
        {
            txtPath.Text = mtdo.FilePath;
            pictureBox1.Image = IconReader.GetFileIcon(mtdo.FilePath, IconReader.IconSize.Large, false).ToBitmap();
            this.Text = Path.GetFileName(mtdo.FilePath);
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {

        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(txtPath.Text);
            }
            catch
            {

            }
            finally
            {
                this.Close();
            }
        }

        private void DownloadCompletedForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            App.Instance.MainForm.Activate();
        }
    }
}
