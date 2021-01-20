using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltoMultiThreadDownloadManager.Exceptions
{
    public class RemoteFilePropertiesChangedException : Exception
    {
        public HttpDownloadInfo OriginalInfo { get; set; }
        public HttpDownloadInfo CurrentInfo { get; set; }
        public RemoteFilePropertiesChangedException(HttpDownloadInfo originalInfo, HttpDownloadInfo currentInfo)
        {
            this.OriginalInfo = originalInfo;
            this.CurrentInfo = currentInfo;
        }

        public override string Message
        {
            get
            {
                return "Remote file properties seems to be changed, refreshing the download url may be solve this problem";
            }
        }
    }
}
