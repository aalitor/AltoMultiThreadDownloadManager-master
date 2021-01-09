using System.Linq;
using System.Net;
using AltoMultiThreadDownloadManager.Helpers;

namespace AltoMultiThreadDownloadManager
{
    /// <summary>
    /// Contains the response headers info
    /// </summary>
    public class DownloadInfo
    {
        /// <summary>
        /// Gets the constructor for downloadinfo
        /// </summary>
        /// <param name="url">Url of the source</param>
        /// <param name="contentSize">Content size of the remote file</param>
        /// <param name="acceptRanges">The information for if server supports resumeability or not</param>
        /// <param name="serverfn">Filename of the remote file</param>
        public DownloadInfo(string url, long contentSize, bool acceptRanges, string serverfn)
        {
            Url = url;
            ContentSize = contentSize;
            AcceptRanges = acceptRanges;
            ServerFileName = serverfn;

        }
        /// <summary>
        /// Gets the download url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Gets the Content-Size
        /// </summary>
        public long ContentSize { get; set; }
        /// <summary>
        /// Gets if download supports resumeability
        /// </summary>
        public bool AcceptRanges { get; set; }
        /// <summary>
        /// Gets the filename that server sent
        /// </summary>
        public string ServerFileName { get; set; }
        /// <summary>
        /// Gets the download info using url only
        /// </summary>
        /// <param name="url">Source url</param>
        /// <returns></returns>
        public static DownloadInfo Get(string url)
        {
            using (var response = (HttpWebResponse)RequestHelper.CreateHttpRequest(url, null, true).GetResponse())
            {
                return GetFromResponse(response, url);
            }

        }
        /// <summary>
        /// Gets the download info both url and response
        /// </summary>
        /// <param name="response">HttpWebResponse received from server</param>
        /// <param name="url">Source url</param>
        /// <returns></returns>
        public static DownloadInfo GetFromResponse(HttpWebResponse response, string url)
        {
            var headers = response.Headers;

            var serverFileName = FileNameHelper.GetFileName(response);
            var contentSize = response.ContentLength;
            var contentRange = response.Headers[HttpResponseHeader.ContentRange];
            if (contentSize < 1 && !string.IsNullOrEmpty(contentRange))
            {
                var parts = contentRange.Split('/');

                if (parts.Length > 1)
                    long.TryParse(parts[1], out contentSize);
            }

            var acceptRanges = headers.AllKeys.Any(x => x.ToLower().Contains("range") && headers[x].Contains("bytes"));
            acceptRanges &= contentSize > 0;
            return new DownloadInfo(url, contentSize, acceptRanges, serverFileName);
        }

        public override bool Equals(object obj)
        {
            var d = (DownloadInfo)obj;
            if (d != null)
            {
                return d.AcceptRanges == this.AcceptRanges &&
                    d.ContentSize == this.ContentSize &&
                    d.ServerFileName == this.ServerFileName;
            }
            else
                return false;
        }
        public override int GetHashCode()
        {
            return AcceptRanges.GetHashCode() ^
                    ContentSize.GetHashCode() ^
                    ServerFileName.GetHashCode();
        }
    }
}
