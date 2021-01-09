using System;
using System.Net;

namespace AltoMultiThreadDownloadManager.EventArguments
{
	/// <summary>
	/// Event arguments for response received event
	/// </summary>
	public class ResponseReceivedEventArgs : EventArgs
	{
        /// <summary>
        /// Constructor for event arguments
        /// </summary>
        /// <param name="response">Response received from server</param>
		public ResponseReceivedEventArgs(HttpWebResponse response)
		{
			Response = response;
		}
		/// <summary>
		/// Gets or sets the response
		/// </summary>
		public HttpWebResponse Response { get; set; }
	}
}
