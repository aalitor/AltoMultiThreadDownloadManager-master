using System;
using System.IO;
using AltoMultiThreadDownloadManager.FileVerification;
using Newtonsoft.Json;
using AltoMultiThreadDownloadManager.Helpers;
using System.Collections.Generic;
using AltoMultiThreadDownloadManager.Enums;

namespace AltoMultiThreadDownloadManager
{
    /// <summary>
    /// Defines partial download range
    /// </summary>
    public class HttpRange
    {
        /// <summary>
        /// Constructor for partial download range
        /// </summary>
        /// <param name="start">Byte offset for the partial download</param>
        /// <param name="end">End of the range in bytes</param>
        /// <param name="saveDir">Temp directory to save the partial download</param>
        public HttpRange(long start, long end, string saveDir, string fileId)
        {
            Start = start;
            End = end;
            FileId = fileId;
            SaveDir = saveDir;
            Status = State.None;
        }
        /// <summary>
        /// Gets the start of the range
        /// </summary>
        public long Start { get; set; }
        /// <summary>
        /// Gets the end of the range
        /// </summary>
        public long End { get; set; }
        /// <summary>
        /// Gets the total bytes received for download
        /// </summary>
        public long TotalBytesReceived { get; set; }
        /// <summary>
        /// Checks if range completely downloaded
        /// </summary>
        public bool IsDownloaded
        {
            get
            {
                return TotalBytesReceived == Size;
            }
        }
        [JsonIgnore]
        /// <summary>
        /// Gets the part of the range not downloaded
        /// </summary>
        public HttpRange Remaining
        {
            get
            {
                return new HttpRange(Start + TotalBytesReceived, End, SaveDir, Guid.NewGuid().ToString("N"));
            }
        }
        /// <summary>
        /// Gets the unique file id for the range using Guid
        /// </summary>
        public string FileId { get; set; }
        /// <summary>
        /// Gets the range size in bytes
        /// </summary>
        public long Size
        {
            get
            {
                return End - Start + 1;
            }
        }
        /// <summary>
        /// Gets the partial range status when downloading or not
        /// </summary>
        public State Status
        {
            get;
            set;
        }
        
        /// <summary>
        /// Checks if range is stopped, failed or completed
        /// </summary>
        public bool IsIdle
        {
            get
            {
                return Status == State.Failed ||
                       Status == State.Completed ||
                       Status == State.Stopped;
            }
        }
        /// <summary>
        /// Gets the temp directory to save
        /// </summary>
        public string SaveDir { get; set; }
        /// <summary>
        /// Gets the full save path
        /// </summary>
        public string FilePath
        {
            get
            {
                return Path.Combine(SaveDir, FileId);
            }
        }
        /// <summary>
        /// Gets the last action datetime
        /// </summary>
        public DateTime LastTry { get; set; }
        /// <summary>
        /// Gets or sets the last checksum value for the partially downloaded file
        /// </summary>
        public string LastChecksum { get; set; }
        /// <summary>
        /// Check if two object are equal
        /// </summary>
        /// <param name="obj">Other object to check equality</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if(obj is HttpRange)
            {
                var r = (HttpRange)obj;
                return r.FileId == this.FileId;
            }
            return false;
        }
        /// <summary>
        /// Gets the hash code of the object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.FileId.GetHashCode();
        }
    }
}
