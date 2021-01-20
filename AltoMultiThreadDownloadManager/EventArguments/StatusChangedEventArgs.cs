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
        public HttpDownloaderStatus OldStatus { get; set; }
        public HttpDownloaderStatus CurrentStatus { get; set; }

        public StatusChangedEventArgs(HttpDownloaderStatus oldStatus, HttpDownloaderStatus currentStatus)
        {
            this.OldStatus = oldStatus;
            this.CurrentStatus = currentStatus;
        }
    }
}
