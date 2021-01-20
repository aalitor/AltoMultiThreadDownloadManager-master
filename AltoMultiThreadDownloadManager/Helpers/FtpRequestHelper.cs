//using System;
//using System.Linq;
//using System.Net;
//using AltoMultiThreadDownloadManager.EventArguments;
//using AltoMultiThreadDownloadManager.Exceptions;

//namespace AltoMultiThreadDownloadManager.Helpers
//{
//    /// <summary>
//    /// Description of RequestHelper.
//    /// </summary>
//    public static class FtpRequestHelper
//    {
//        public static FtpWebRequest CreateHttpRequest(HttpDownloadInfo info, long start, long end, EventHandler<BeforeSendingRequestEventArgs> before)
//        {
//            if (start < 0 || (info.ContentSize > 0 && start >= info.ContentSize))
//                throw new Exception("Range start index is out of the bounds");

//            if (start > 0 && !info.AcceptRanges)
//                throw new Exception("Remote file doesn't support ranges");

//            var request = CreateHttpRequest(info.Url, before, false);

//            if (info.ContentSize > 0)
//                request.AddRange(start, end);

//            return request;
//        }

//        public static FtpWebRequest CreateHttpRequest(string url, EventHandler<BeforeSendingRequestEventArgs> before, bool initial)
//        {
//            var request = (HttpWebRequest)WebRequest.Create(url);
//            var whc = new WebHeaderCollection();
//            request.SetHeaderValue("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36 Edge/18.18362");
//            request.AllowAutoRedirect = true;
//            request.Method = "GET";
//            request.Timeout = 30000;
//            request.KeepAlive = false;
//            request.ReadWriteTimeout = 3000;
//            if (initial)
//                request.AddRange(0);
//            before.Raise(null, new BeforeSendingRequestEventArgs(request));

//            return request;
//        }

//        public static FtpWebResponse GetRangedResponse(HttpDownloadInfo info, long start, long end, WebRequest request, EventHandler<AfterGettingResponseEventArgs> after)
//        {
//            var response = (FtpWebResponse)request.GetResponse();

//            after.Raise(null, new AfterGettingResponseEventArgs(response));

//            if (response.ContentLength != end - start + 1)
//                throw new ReturnedContentSizeWrongException(start, end, response.ContentLength, end - start + 1);

//            return response;
//        }

       
//    }
//}
