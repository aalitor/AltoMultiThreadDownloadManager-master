using AltoMultiThreadDownloadManager.NativeMessages;
using DownloadManagerPortal.SingleInstancing;
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
    public partial class EnterUrlForm : Form
    {
        public EnterUrlForm()
        {
            InitializeComponent();
            this.FormClosed += EnterUrlForm_FormClosed;
        }
        DownloadMessage dmsg;
        void EnterUrlForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Task.Run(() =>
            {
                App.Instance.SendMessage(dmsg);
            });
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(txtUrl.Text, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!result)
            {
                MessageBox.Show("Please enter a valid url to start download!", "Invalid url", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            dmsg = new DownloadMessage()
                {
                    Url = txtUrl.Text,
                    TabUrl = txtUrl.Text,
                    Headers = new Header[0]
                };
            this.Close();
        }
    }
}
