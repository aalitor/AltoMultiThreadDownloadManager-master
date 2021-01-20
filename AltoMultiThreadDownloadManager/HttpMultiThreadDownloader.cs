
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AltoMultiThreadDownloadManager.EventArguments;
using AltoMultiThreadDownloadManager.Helpers;
using AltoMultiThreadDownloadManager.NativeMessages;
using AltoMultiThreadDownloadManager.Exceptions;
using SC = System.ComponentModel;
using Newtonsoft.Json;
using System.Threading;
using System.Windows.Forms;
using AltoMultiThreadDownloadManager.Enums;

// ReSharper disable All
namespace AltoMultiThreadDownloadManager
{
    /// <summary>
    /// Multi thread downloader object class
    /// </summary>
    public class HttpMultiThreadDownloader
    {
        /// <summary>
        /// Raises when download progress changed
        /// </summary>
        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;
        /// <summary>
        /// Raised when download completed but this doesn't mean partial files merged
        /// </summary>
        public event EventHandler<EventArgs> Completed;
        /// <summary>
        /// Raises when all download threads completely stopped
        /// </summary>
        public event EventHandler<EventArgs> Stopped;
        /// <summary>
        /// Raises when all download threads resumed
        /// </summary>
        public event EventHandler<EventArgs> Resumed;
        /// <summary>
        /// Raised when response headers received for the first initial request
        /// </summary>
        public event EventHandler<EventArgs> DownloadInfoReceived;
        /// <summary>
        /// Raised when any of the download thread failed
        /// </summary>
        public event EventHandler<ErrorEventArgs> ErrorOccured;
        /// <summary>
        /// Raised when before sending any of the download request
        /// </summary>
        public event EventHandler<BeforeSendingRequestEventArgs> BeforeSendingRequest;
        /// <summary>
        /// Raised when merging progress changed
        /// </summary>
        public event EventHandler<MergingProgressChangedEventArgs> MergingProgressChanged;
        /// <summary>
        /// Raised when download completed and partial files merged
        /// </summary>
        public event EventHandler<EventArgs> MergeCompleted;

        public event EventHandler<ChecksumValidationProgressChangedEventArgs> ChecksumValidationProgressChanged;
        public event EventHandler<StatusChangedEventArgs> StatusChanged;
        private HttpDownloaderStatus status;
        private Stopwatch stp = new Stopwatch();
        private List<HttpRangeDownloader> cdList = new List<HttpRangeDownloader>();
        private long totalBytesMerged;
        private bool flagMerged = false;
        private System.Windows.Forms.Timer stopTimer = new System.Windows.Forms.Timer();
        private long speedBytesOffset;
        private SC.AsyncOperation aop;
        /// <summary>
        /// Calls constructor for the downloader object
        /// </summary>
        /// <param name="url">URL of the source</param>
        /// <param name="finalPath">Final save path to merge all partial files downloaded</param>
        /// <param name="rangeDir">Temporary save path for partial downloads</param>
        /// <param name="nofThread">Number of async threads to download with</param>
        public HttpMultiThreadDownloader(string url, string saveDir, string saveFileName, string rangeDir, int nofThread)
        {
            new GlobalLock();
            HttpGlobalSettings.Set();
            Url = url;
            RangeDir = rangeDir;
            SaveDir = saveDir;
            NofThread = nofThread;
            SaveFileName = saveFileName;
            stopTimer.Tick += stopTimer_Tick;
            aop = SC.AsyncOperationManager.CreateOperation(null);
            speedBytesOffset = 0;
        }

        #region Events

        private async void cd_Completed(object sender, EventArgs e)
        {
            if (Info.ContentSize < 1)
            {
                Status = HttpDownloaderStatus.MergingFiles;
                Directory.CreateDirectory(SaveDir);
                File.Delete(this.FilePath);
                File.Move(Ranges[0].FilePath, this.FilePath);
                Progress = 100;
                Status = HttpDownloaderStatus.Completed;
                Completed.Raise(this, EventArgs.Empty, aop);
            }
            else if (Ranges.All(x => x.IsDownloaded) &&
                Ranges.All(x => x.IsIdle) &&
                      TotalBytesReceived == Info.ContentSize &&
                      !flagMerged)
            {
                Directory.CreateDirectory(SaveDir);
                flagMerged = true;
                Progress = 100;
                Status = HttpDownloaderStatus.MergingFiles;
                await MergePartialFiles();
                ProgressChanged.Raise(this, new ProgressChangedEventArgs(Progress), aop);
                Status = HttpDownloaderStatus.Completed;
                Completed.Raise(this, EventArgs.Empty, aop);
            }
            else
                createNewThreadIfRequired();


        }

        private void cd_Stopped(object sender, EventArgs e)
        {
            if (Ranges.All(x => x.IsIdle))
            {
                LastTry = DateTime.Now;
                stopTimer.Stop();
                Status = HttpDownloaderStatus.Stopped;
                Stopped.Raise(this, EventArgs.Empty, aop);
            }

        }

        private void cd_Failed(object sender, ErrorEventArgs e)
        {
            if (FlagStop)
                return;
            var ex = e.GetException();
            if (ex is ReturnedContentSizeWrongException)
            {
                if (Ranges.Count == 2)
                {
                    foreach (var cd in cdList)
                    {
                        Info.AcceptRanges = false;
                        cd.Info.AcceptRanges = false;
                        cd.UseChunk = false;
                    }
                    Ranges.RemoveAt(1);
                    Ranges[0].End = Info.ContentSize - 1;
                    foreach (var item in cdList.Where(x => x.Wait))
                    {
                        item.Wait = false;
                    }
                    return;
                }
            }
            ErrorOccured.Raise(sender, e, aop);
        }
        private void cd_BeforeSendingRequest(object sender, BeforeSendingRequestEventArgs e)
        {
            e.Request.ServicePoint.ConnectionLimit = 1000;
            if (DownloadRequestMessage != null)
            {
                var headers = DownloadRequestMessage.GetWebHeaders();
                e.Request.SetHeaders(headers);
            }
            var acceptEncoding = e.Request.Headers[HttpRequestHeader.AcceptEncoding];
            if (!string.IsNullOrEmpty(acceptEncoding))
            {
                e.Request.Headers[HttpRequestHeader.AcceptEncoding] = string.Empty;
            }
            //If VPN connections active on desktop it effects badly for request, remove it
            e.Request.Proxy = new WebProxy();

            BeforeSendingRequest.Raise(this, e, aop);
        }

        private void cd_ResponseReceived(object sender, ResponseReceivedEventArgs e)
        {
            var cd = (HttpRangeDownloader)sender;
            if (Info == null)
            {
                Info = HttpDownloadInfo.GetFromResponse(e.Response, Url);
                LastInfo = Info.Clone();
                cd.Info = Info.Clone();
                cd.Wait = Info.AcceptRanges;
                var chunkedHeader = e.Response.Headers[HttpResponseHeader.TransferEncoding];
                UseChunk = Info.AcceptRanges && chunkedHeader != null && chunkedHeader.ToLower() == "chunked";
                Ranges.First().End = Info.ContentSize - 1;
                //NofThread = Info.AcceptRanges ? NofThread : 1;
                DownloadInfoReceived.Raise(this, EventArgs.Empty);
            }
            if (NofActiveThreads > 1 && Info.AcceptRanges && Info.ResumeCapability != Resumeability.Yes)
            {
                Info.ResumeCapability = Resumeability.Yes;
                foreach (var a in cdList)
                {
                    Info.AcceptRanges = true;
                    a.Info.AcceptRanges = true;
                }
                foreach (var item in cdList.Where(x => x.Wait))
                {
                    item.Wait = false;
                }
            }
            if(Info.ContentSize < 10 * 1024 + 1)
            {
                foreach (var item in cdList.Where(x => x.Wait))
                {
                    item.Wait = false;
                }
            }
            createNewThreadIfRequired();
        }

        private void cd_ProgressChanged(object sender, EventArgs e)
        {
            double prnew = 0;
            if (Info != null && Info.ContentSize > 0)
                prnew = double.Parse((TotalBytesReceived * 100d / Info.ContentSize).ToString("0.00"));
            else
                prnew = 0;
            Speed = (long)((TotalBytesReceived - speedBytesOffset) / stp.Elapsed.TotalSeconds);

            if (Progress == 0 || prnew != Progress)
            {
                Status = HttpDownloaderStatus.Downloading;
                Progress = prnew;
                ProgressChanged.Raise(this, new ProgressChangedEventArgs(Progress), aop);
            }
        }
        #endregion


        #region User methods

        /// <summary>
        /// Start the download with initial request, do not call this to resume
        /// </summary>
        public void Start()
        {
            stp.Start();
            Ranges = new List<HttpRange> { new HttpRange(0, long.MaxValue - 1, RangeDir, Guid.NewGuid().ToString("N")) };
            var cd = CreateNewRangeDownloader(Ranges[0]);
            cd.Url = Url;
            cd.Wait = true;
            cd.Download();
        }
        /// <summary>
        /// Stops all threads, to ensure stopping; wait for Stopped event
        /// </summary>
        public void Stop()
        {
            FlagStop = true;
            stp.Stop();
            if (Ranges.All(x => x.IsIdle))
            {
                Stopped.Raise(this, EventArgs.Empty, aop);

            }
            else
                stopTimer.Start();

        }
        void stopTimer_Tick(object sender, EventArgs e)
        {
            if (!FlagStop)
            {
                stopTimer.Stop();
                return;
            }
            foreach (var cd in cdList.Where(x => !x.Range.IsIdle))
            {
                cd.Stop();
            }

        }
        /// <summary>
        /// Resumes all threads after stopping
        /// <exception cref="AltoMultiThreadDownloader.Exceptions.RemoteFilePropertiesChangedException">Thrown when download informations changed, in case of expired url or changed authentication data</exception>
        /// </summary>
        public void Resume()
        {
            var currentInfo = GetCurrentInformations();

            if (currentInfo == null || !currentInfo.Equals(this.Info))
            {
                ErrorOccured.Raise(this,
                    new ErrorEventArgs(
                        new RemoteFilePropertiesChangedException(this.Info, currentInfo)), aop);
                return;
            }
            speedBytesOffset = TotalBytesReceived;
            FlagStop = false;
            stp.Reset();
            stp.Start();
            createNewThreadIfRequired();
            Resumed.Raise(this, EventArgs.Empty, aop);

        }
        /// <summary>
        /// Creates a clone of the downloader
        /// </summary>
        /// <returns></returns>
        public HttpMultiThreadDownloader Clone()
        {
            var json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<HttpMultiThreadDownloader>(json);
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Gets the newest download informations in case of url expired or remote file was modified
        /// </summary>
        /// <returns></returns>
        public HttpDownloadInfo GetCurrentInformations()
        {
            try
            {
                var req = HttpRequestHelper.CreateHttpRequest(this.Url, null, true);
                cd_BeforeSendingRequest(this, new BeforeSendingRequestEventArgs(req));
                using (var resp = (HttpWebResponse)req.GetResponse())
                {
                    return HttpDownloadInfo.GetFromResponse(resp, this.Url);
                }
            }
            catch
            {
                return null;
            }
        }
        private void createNewThreadIfRequired()
        {

            lock (GlobalLock.Locker)
            {
                if (FlagStop)
                    return;
                if (Info == null || !Info.AcceptRanges)
                    return;
                var realNofMaxThread = Info.AcceptRanges ? NofThread : 1;

                var nofCurrentThreads = (Ranges.Count(x => !x.IsIdle));
                var reqThreads = realNofMaxThread - nofCurrentThreads;
                if (reqThreads > 0)
                {
                    var avRanges = Ranges.Where(x => !x.IsDownloaded);


                    if (avRanges.Any(x => x.IsIdle))
                    {
                        var first = avRanges.First(x => x.IsIdle);
                        if (first.TotalBytesReceived > 0)
                        {
                            var fstart = first.Remaining.Start;
                            var fend = first.Remaining.End;
                            first.End = fstart - 1;
                            var next = new HttpRange(fstart, fend, RangeDir, Guid.NewGuid().ToString("N"));
                            Ranges.Add(next);
                            var cd = CreateNewRangeDownloader(next);
                            cd.Download();
                        }
                        else
                        {
                            if (!first.IsIdle)
                                return;

                            var rdlist = cdList.Where(x => x.Range.FileId == first.FileId);
                            if (rdlist.Any())
                            {
                                var cd = rdlist.First();
                                if (!cd.Range.IsIdle)
                                    return;

                                cd.Reset();
                                cd.UseChunk = this.UseChunk;

                                cd.Download();
                            }
                            else
                            {
                                var cd = CreateNewRangeDownloader(first);
                                cd.Download();
                            }

                        }

                    }
                    //10kb is for secure downloading without no corruption
                    //If a range < 10kb there is no dividing
                    else if (avRanges.Any(x => x.Remaining.Size > 10 * 1024))
                    {
                        var temp = avRanges.Where(x => x.Remaining.Size > 10 * 1024);
                        var first = avRanges.OrderByDescending(x => x.Remaining.Size).First();
                        var fstart = first.End - first.Remaining.Size / 2;
                        var fend = first.End;
                        first.End = fstart - 1;
                        var next = new HttpRange(fstart, fend, RangeDir, Guid.NewGuid().ToString("N"));
                        Ranges.Add(next);
                        var cd = CreateNewRangeDownloader(next);
                        cd.Download();
                    }
                }
                else if (reqThreads < 0)
                {
                    var temp = cdList.Where(x => !x.Range.IsDownloaded && !x.Range.IsIdle);
                    if (temp.Any())
                    {
                        var first = temp.OrderByDescending(x => x.Range.Remaining.Size).First();
                        first.Stop();
                    }
                }
            }
        }
        private HttpRangeDownloader CreateNewRangeDownloader(HttpRange c)
        {
            var cd = new HttpRangeDownloader(c, Info);
            cd.UseChunk = this.UseChunk;
            cd.Completed += cd_Completed;
            cd.Stopped += cd_Stopped;
            cd.Failed += cd_Failed;
            cd.ProgressChanged += cd_ProgressChanged;
            cd.ResponseReceived += cd_ResponseReceived;
            cd.BeforeSendingRequest += cd_BeforeSendingRequest;
            cdList.Add(cd);

            return cd;
        }
        private Task MergePartialFiles()
        {
            return Task.Run(() =>
                {

                    using (var fs = File.Create(FilePath))
                    {
                        foreach (var fileChunk in Ranges.Where(x => x.TotalBytesReceived > 0).OrderBy(x => x.Start)
                    .Select(x => x.FilePath))
                        {
                            var buffer = new byte[5 * 1024];
                            FileStream chunkStrem;
                            while (!TryOpen(fileChunk, out chunkStrem))
                            {
                                Thread.Sleep(500);
                            }
                            WriteFile(chunkStrem, fs);
                        }
                        MergeCompleted.Raise(this, EventArgs.Empty, aop);
                    }


                });
        }
        private bool TryOpen(string fileChunk, out FileStream fs)
        {

            fs = File.Open(fileChunk, FileMode.Open, FileAccess.Read, FileShare.Read);
            return true;

        }
        private bool WriteFile(FileStream chunkStream, FileStream fs)
        {

            var bytesRead = 0;
            var buffer = new byte[1200000];
            using (chunkStream)
                while ((bytesRead = chunkStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    totalBytesMerged += bytesRead;
                    double mergeProgress = totalBytesMerged * 100d / TotalBytesReceived;
                    mergeProgress = double.Parse(mergeProgress.ToString("0.00"));

                    MergingProgressChanged.Raise(this, new MergingProgressChangedEventArgs(mergeProgress), aop);

                    fs.Write(buffer, 0, bytesRead);
                    fs.Flush();
                }
            return true;

        }
        #endregion

        #region User properties
        public bool UseChunk { get; set; }
        /// <summary>
        /// Total bytes calculated by summing all bytes received from async threads
        /// </summary>
        public long TotalBytesReceived
        {
            get
            {
                return Ranges != null && Ranges.Any() ?
                    Ranges.ToList().Sum(x => x.TotalBytesReceived) : 0;
            }
        }
        /// <summary>
        /// Unique downloader id
        /// </summary>
        public string Id { get; set; }


        /// <summary>
        /// Gets the download speed
        /// </summary>
        public long Speed { get; set; }
        /// <summary>
        /// Gets the download progress as two precision percentage value
        /// </summary>
        public double Progress { get; set; }
        /// <summary>
        /// Gets progress percentage string as #.##%
        /// </summary>
        public string ProgressString
        {
            get
            {
                return Progress.ToString("0.00") + "%";
            }
        }
        /// <summary>
        /// Gets or sets the downloader status
        /// </summary>
        public HttpDownloaderStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                if (value != status)
                {
                    var old = status;
                    status = value;
                    StatusChanged.Raise(this, new StatusChangedEventArgs(old, status), aop);
                }
            }
        }
        /// <summary>
        /// Gets the number of active threads
        /// </summary>
        public int NofActiveThreads
        {
            get
            {
                lock (GlobalLock.Locker)
                    return Ranges != null && Ranges.Any() ?
                        Ranges.ToList().Count(x => !x.IsIdle) : 0;
            }
        }
        /// <summary>
        /// Gets if at least one thread is active for downloading
        /// </summary>
        public bool IsActive
        {
            get
            {
                return NofActiveThreads > 0;
            }
        }
        /// <summary>
        /// Gets or sets final save path
        /// </summary>
        public string FilePath
        {
            get
            {
                return Path.Combine(SaveDir, string.IsNullOrEmpty(SaveFileName) ? "defaultfilename.unknown" : SaveFileName);

            }
        }
        /// <summary>
        /// Gets the temporary save path for partial downloads
        /// </summary>
        public string RangeDir { get; set; }
        /// <summary>
        /// Gets the download url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Gets the ranges for partial downloads
        /// </summary>
        public List<HttpRange> Ranges { get; set; }
        /// <summary>
        /// Gets the download informations: Content-Length, Resumeability, ServerFileName
        /// </summary>
        public HttpDownloadInfo Info { get; set; }
        /// <summary>
        /// Gets or sets the final save directory for download
        /// </summary>
        public string SaveDir { get; set; }
        /// <summary>
        /// Gets the stop flag, if true it forces the download for stopping
        /// </summary>
        public bool FlagStop
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets or sets the number of max async threads
        /// </summary>
        public int NofThread { get; set; }
        /// <summary>
        /// Gets or sets the final save file name for download
        /// </summary>
        public string SaveFileName { get; set; }
        /// <summary>
        /// Gets or sets the download request message that was received via NativeMessaging from an external source or created manually
        /// e.g Chrome extension
        /// </summary>
        public DownloadMessage DownloadRequestMessage { get; set; }
        /// <summary>
        /// Stored information that was last received successfully
        /// </summary>
        public HttpDownloadInfo LastInfo { get; set; }
        /// <summary>
        /// Last action datetime for download
        /// </summary>
        public DateTime LastTry { get; set; }
        #endregion
    }
}
