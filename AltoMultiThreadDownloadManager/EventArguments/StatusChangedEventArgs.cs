using AltoMultiThreadDownloadManager.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltoMultiThreadDownloadManager.EventArguments
{
    public class StatusChangedEventArgs : EventArgs
    {
        public DownloaderStatus OldStatus { get; set; }
        public DownloaderStatus CurrentStatus { get; set; }

        public StatusChangedEventArgs(DownloaderStatus oldStatus, DownloaderStatus currentStatus)
        {
            this.OldStatus = oldStatus;
            this.CurrentStatus = currentStatus;
        }
    }
}
