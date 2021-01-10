using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownloadManagerPortal
{
    static class MessageHelper
    {
        public static bool AskYes(string message)
        {
            var result = MessageBox.Show(message, "Alto Download Accelerator", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }
    }
}
