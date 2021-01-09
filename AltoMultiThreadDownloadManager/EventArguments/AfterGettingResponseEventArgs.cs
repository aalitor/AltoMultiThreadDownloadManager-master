using System;
using System.Net;

namespace AltoMultiThreadDownloadManager.EventArguments
{
	/// <summary>
	/// Event arguments for after getting response
	/// </summary>
	public class AfterGettingResponseEventArgs : EventArgs
	{
        /// <summary>
        /// Constructor for event arguments
        /// </summary>
        /// <param name="response">Response received after request</param>
		public AfterGettingResponseEventArgs(HttpWebResponse response)
		{
			Response = response;
		}
        /// <summary>
        /// Gets or sets the HttpWebResponse received
        /// </summary>
		public HttpWebResponse Response{ get; set; }
	}
}
