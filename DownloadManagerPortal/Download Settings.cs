using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownloadManagerPortal
{
    public partial class DownloadSettingsForm : Form
    {
        public DownloadSettingsForm()
        {
            InitializeComponent();
            this.Load += DownloadSettingsForm_Load;
        }

        void DownloadSettingsForm_Load(object sender, EventArgs e)
        {
            txtSaveFolder.Text = Properties.Settings.Default.SaveFolder;
            numericUpDown1.Value = Properties.Settings.Default.NofThread;
            txtExtId.Text = Properties.Settings.Default.ChromeExtensionId;
            chkChrome.Checked = Properties.Settings.Default.ChromeIntegration;
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            var folder = txtSaveFolder.Text;
            var nofThread = (int)numericUpDown1.Value;

            Properties.Settings.Default.SaveFolder = folder;
            Properties.Settings.Default.NofThread = nofThread;
            Properties.Settings.Default.ChromeExtensionId = txtExtId.Text;
            Properties.Settings.Default.ChromeIntegration = chkChrome.Checked;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void btnOpenFBD_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSaveFolder.Text = folderBrowserDialog1.SelectedPath;
            }

        }
    }
}
