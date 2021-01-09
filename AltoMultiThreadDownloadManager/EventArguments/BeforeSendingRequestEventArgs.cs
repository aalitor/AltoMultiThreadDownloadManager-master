using System;
using System.Net;

namespace AltoMultiThreadDownloadManager.EventArguments
{
    /// <summary>
    /// Event arguments for before request sent
    /// </summary>
	public class BeforeSendingRequestEventArgs : EventArgs
	{
        /// <summary>
        /// Constructor for event arguments
        /// </summary>
        /// <param name="req">Request that will be sent</param>
		public BeforeSendingRequestEventArgs(HttpWebRequest req)
		{
			Request = req;
		}
        /// <summary>
        /// Gets or sets the HttpWebRequest
        /// </summary>
		public HttpWebRequest Request{ get; set; }
	}
}
