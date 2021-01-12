using System;
using System.IO;
using System.Net;
using System.Threading;
using AltoMultiThreadDownloadManager.EventArguments;
using AltoMultiThreadDownloadManager.Helpers;

namespace AltoMultiThreadDownloadManager
{
    internal class RangeDownloader
    {
        public event EventHandler<EventArgs> Completed;
        public event EventHandler<EventArgs> Stopped;
        public event EventHandler<ErrorEventArgs> Failed;
        public event EventHandler<EventArgs> ProgressChanged;
        public event EventHandler<ResponseReceivedEventArgs> ResponseReceived;
        public event EventHandler<BeforeSendingRequestEventArgs> BeforeSendingRequest;
        public event EventHandler<AfterGettingResponseEventArgs> AfterGettingResponse;
        private Thread th = null;
        private HttpWebRequest request = null;
        private FileStream fileStream;
        public RangeDownloader(Range range, DownloadInfo info = null)
        {
            Range = range;
            Info = info;
            UseChunk = true;
        }



        public void Download()
        {
            stopFlag = false;
            th = new Thread(() =>
                {

                    DownloadProcedure();

                    while (!Range.IsDownloaded && !stopFlag && Info != null && Info.AcceptRanges)
                    {
                        DownloadProcedure();
                    }

                });
            th.Start();

        }
        void DownloadProcedure()
        {
            try
            {
                var endOfStream = false;

                var stOffset = Range.Remaining.Start;
                var chunkEndOffset = stOffset + 1 * 1024 * 1000 - 1;
                var endOffset = UseChunk ? Math.Min(chunkEndOffset, Range.End) : Range.End;
                Range.Status = Status = State.SendRequest;
                Range.LastTry = DateTime.Now;
                request = Info != null ?
                    RequestHelper.CreateHttpRequest(Info, stOffset, endOffset, BeforeSendingRequest) :
                    RequestHelper.CreateHttpRequest(Url, BeforeSendingRequest, true);
                Range.Status = Status = State.GetResponse;

                using (var response = Info != null ?
                       RequestHelper.GetRangedResponse(Info, stOffset, endOffset, request, AfterGettingResponse) :
                       request.GetResponse() as HttpWebResponse)
                {
                    Range.Status = Status = State.GetResponseStream;

                    ResponseReceived.Raise(this, new ResponseReceivedEventArgs(response));
                    using (fileStream = FileHelper.CheckFile(Range.FilePath, Range.TotalBytesReceived > 0))
                    {
                        using (var str = response.GetResponseStream())
                        {
                            var buffer = new byte[2048];

                            var bytesRead = 0;
                            while (true)
                            {


                                if (str != null)
                                    bytesRead = str.Read(buffer, 0, buffer.Length);
                                if (Info.ContentSize > 0)
                                    bytesRead = (int)Math.Min(Range.Size - Range.TotalBytesReceived, bytesRead);
                                if (bytesRead <= 0)
                                {
                                    endOfStream = true;
                                    break;

                                }
                                if (stopFlag)
                                {
                                    break;

                                }
                                if (Info != null && Info.ContentSize > 0 && Range.IsDownloaded)
                                {
                                    break;
                                }
                                Range.Status = Status = State.Downloading;

                                fileStream.Write(buffer, 0, bytesRead);

                                Range.TotalBytesReceived += bytesRead;
                                ProgressChanged.Raise(this, EventArgs.Empty);

                            }
                        }
                    }
                }
                lock (GlobalLock.Locker)
                {
                    Range.LastTry = DateTime.Now;
                    if ((!stopFlag && endOfStream && Info.ContentSize < 1) ||
                        (Info.ContentSize > 0 && Range.IsDownloaded))
                    {
                        Range.Status = Status = State.Completed;
                        Completed.Raise(this, EventArgs.Empty);
                    }
                    else if (stopFlag)
                    {
                        Range.Status = Status = State.Stopped;
                        Stopped.Raise(this, EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                lock (GlobalLock.Locker)
                {
                    Range.LastTry = DateTime.Now;
                    if (Info != null && Info.ContentSize > 0 && Range.IsDownloaded)
                    {
                        Range.Status = Status = State.Completed;
                        Completed.Raise(this, EventArgs.Empty);
                    }
                    else if (stopFlag)
                    {
                        Range.Status = Status = State.Stopped;
                        Stopped.Raise(this, EventArgs.Empty);
                    }
                    else
                    {
                        Range.Status = Status = State.Failed;
                        Failed.Raise(this, new ErrorEventArgs(ex));
                    }
                }
            }
            finally
            {
                if (request != null)
                    request.Abort();
            }

        }

        public void Reset()
        {
            stopFlag = false;
        }
        public void Stop()
        {
            stopFlag = true;
            if (request != null)
                request.Abort();
        }
        private volatile bool stopFlag;
        public int TryCount { get; set; }
        public bool UseChunk { get; set; }
        public State Status
        {
            get;
            set;
        }
        public Range Range { get; set; }
        public DownloadInfo Info { get; set; }
        public string Url { get; set; }
    }
}
