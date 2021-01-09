namespace AltoMultiThreadDownloadManager
{
    /// <summary>
    /// Provides lock object to make downloads thread safe
    /// </summary>
    internal class GlobalLock
    {
        /// <summary>
        /// Creates a new lock object
        /// </summary>
        public GlobalLock()
        {
            Locker = new object();
        }
        /// <summary>
        /// Gets the lock object
        /// </summary>
		public static object Locker{ get; private set; }
    }
}
