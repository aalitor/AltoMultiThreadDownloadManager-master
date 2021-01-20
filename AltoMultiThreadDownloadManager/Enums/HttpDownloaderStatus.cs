using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltoMultiThreadDownloadManager.Enums
{
    /// <summary>
    /// Provides statuses for http download
    /// </summary>
    public enum HttpDownloaderStatus
    {
        /// <summary>
        /// Assigned when user calls Stop and all threads are idle
        /// </summary>
        Stopped = 0,
        /// <summary>
        /// At least one thread is downloading
        /// </summary>
        Downloading = 1,
        /// <summary>
        /// All threads completed downloading completely and partial files merged
        /// </summary>
        Completed = 2,
        /// <summary>
        /// Starting initial downloading
        /// </summary>
        Starting = 3,
        /// <summary>
        /// Assigned right after the download informations received
        /// </summary>
        InfoReceived = 4,
        /// <summary>
        /// Assigned when partial files are started merging
        /// </summary>
        MergingFiles = 5

    }
}
