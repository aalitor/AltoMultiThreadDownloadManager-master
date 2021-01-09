namespace AltoMultiThreadDownloadManager
{
    /// <summary>
    /// Defines possible download statuses
    /// </summary>
    public enum State
    {
        SendRequest= 5,
        GetResponse = 6,
        GetResponseStream = 7,
        Downloading = 8,
        Stopped = 2,
        Completed = 3,
        Failed = 1,
        None = 4
    }
}
