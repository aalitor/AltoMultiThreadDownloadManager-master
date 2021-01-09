using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownloadManagerPortal.DownloadHandler
{
    public partial class WaitingNewUrl : Form
    {
        string labelDesc;
        public WaitingNewUrl()
        {
            InitializeComponent();
        }
        public WaitingNewUrl(string description)
        {
            InitializeComponent();
            labelDesc = description;
            lblDescription.Text = "Waiting new url for the file " + labelDesc;
            this.Text = "Refresh url " + labelDesc;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
