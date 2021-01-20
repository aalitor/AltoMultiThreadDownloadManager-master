namespace AltoMultiThreadDownloadManager.Enums
{
    /// <summary>
    /// Defines possible download statuses
    /// </summary>
    public enum State
    {
        /// <summary>
        /// Sending Request
        /// </summary>
        SendRequest= 5,
        /// <summary>
        /// Starting getting response
        /// </summary>
        GetResponse = 6,
        /// <summary>
        /// Started getting response stream
        /// </summary>
        GetResponseStream = 7,
        /// <summary>
        /// Downloading
        /// </summary>
        Downloading = 8,
        /// <summary>
        /// Stopped
        /// </summary>
        Stopped = 2,
        /// <summary>
        /// Range completed
        /// </summary>
        Completed = 3,
        /// <summary>
        /// Range downloading failed 
        /// </summary>
        Failed = 1,
        /// <summary>
        /// Initial status when range created
        /// </summary>
        None = 4
    }
}
