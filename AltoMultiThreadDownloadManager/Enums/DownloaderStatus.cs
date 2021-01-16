using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltoMultiThreadDownloadManager.Enums
{
    public enum DownloaderStatus
    {
        Stopped = 0,
        Downloading = 1,
        Completed = 2,
        Starting = 3,
        InfoReceived = 4,
        MergingFiles = 5,
        Stopping = 6

    }
}
