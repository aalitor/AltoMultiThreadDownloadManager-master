using System;
using System.Linq;
using System.Net;
using AltoMultiThreadDownloadManager.EventArguments;

namespace AltoMultiThreadDownloadManager.Helpers
{
    /// <summary>
    /// Description of RequestHelper.
    /// </summary>
    public static class RequestHelper
    {
        public static HttpWebRequest CreateHttpRequest(DownloadInfo info, long start, long end, EventHandler<BeforeSendingRequestEventArgs> before)
        {
            if (start < 0 || (info.ContentSize > 0 && start >= info.ContentSize))
                throw new Exception("Range start index is out of the bounds");

            if (start > 0 && !info.AcceptRanges)
                throw new Exception("Remote file doesn't support ranges");
            
            var request = CreateHttpRequest(info.Url, before, false);

            if (info.ContentSize > 0)
                request.AddRange(start, end);

            return request;
        }

        public static HttpWebRequest CreateHttpRequest(string url, EventHandler<BeforeSendingRequestEventArgs> before, bool initial)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var whc = new WebHeaderCollection();
            request.SetHeaderValue("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36 Edge/18.18362");
            request.AllowAutoRedirect = true;
            request.Method = "GET";
            request.Timeout = 30000;
            request.KeepAlive = false;
            request.ReadWriteTimeout = 3000;
            if (initial)
                request.AddRange(0);
            before.Raise(null, new BeforeSendingRequestEventArgs(request));
            return request;
        }

        public static HttpWebResponse GetRangedResponse(DownloadInfo info, long start, long end, WebRequest request, EventHandler<AfterGettingResponseEventArgs> after)
        {
            var response = (HttpWebResponse)request.GetResponse();

            after.Raise(null, new AfterGettingResponseEventArgs(response));
            if (response.ContentLength != end - start + 1)
                throw new Exception(string.Format(info.ContentSize + " Returned content size is wrong start={0}, end={1}, returned = {2}, shouldbe = {3}",
                        start, end, response.ContentLength, end - start + 1));

            return response;
        }

        public static void SetHeaders(this HttpWebRequest request, WebHeaderCollection whc)
        {
            foreach (var header in whc.AllKeys)
                request.SetHeaderValue(header, whc[header]);
        }
        public static void SetHeaderValue(this HttpWebRequest request, string header, object value)
        {
            var restrictedHeaders = new[]{
                "Accept",
                "Connection",
                "Content-Length",
                "Content-Type",
                "Date",
                "Expect",
                "Host",
                "If-Modified-Since",
                "Range",
                "Referer",
                "Transfer-Encoding",
                "User-Agent",
                "Proxy-Connection"
            };

            if (restrictedHeaders.Contains(header))
            {
                switch (header)
                {
                    case "Accept":
                        request.Accept = (string)value;
                        break;
                    case "Connection":
                        request.Connection = (string)value;
                        break;
                    case "Content-Length":
                        request.ContentLength = (long)value;
                        break;
                    case "Content-Type":
                        request.ContentType = (string)value;
                        break;
                    case "Date":
                        request.Date = (DateTime)value;
                        break;
                    case "Expect":
                        request.Expect = (string)value;
                        break;
                    case "Host":
                        request.Host = (string)value;
                        break;
                    case "If-Modified-Since":
                        request.IfModifiedSince = (DateTime)value;
                        break;
                    //case "Range":
                    //    request.AddRange
                    //    break;
                    case "Referer":
                        request.Referer = (string)value;
                        break;
                    case "Transfer-Encoding":
                        request.TransferEncoding = (string)value;
                        break;
                    case "User-Agent":
                        request.UserAgent = (string)value;
                        break;
                    case "Proxy-Connection":
                        request.Proxy = (IWebProxy)value;
                        break;
                }
            }
            else
                request.Headers[header] = (string)value;

        }
    }
}
