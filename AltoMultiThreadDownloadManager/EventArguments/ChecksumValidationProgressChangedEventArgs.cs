using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltoMultiThreadDownloadManager.EventArguments
{
    public class ChecksumValidationProgressChangedEventArgs : EventArgs
    {
        public int Progress { get; set; }
        public ChecksumValidationProgressChangedEventArgs(int progress)
        {
            this.Progress = progress;
        }
    }
}
