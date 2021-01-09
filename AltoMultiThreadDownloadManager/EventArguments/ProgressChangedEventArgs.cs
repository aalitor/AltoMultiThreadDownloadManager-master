
using System;

namespace AltoMultiThreadDownloadManager.EventArguments
{
	/// <summary>
	/// Event arguments for file downloading progress changed event
	/// </summary>
	public class ProgressChangedEventArgs : EventArgs
	{
        /// <summary>
        /// Constructor for event arguments
        /// </summary>
        /// <param name="progress">Progress value for download</param>
		public ProgressChangedEventArgs(double progress)
		{
			Progress = progress;
		}
		/// <summary>
		/// Gets or sets the progress value
		/// </summary>
		public double Progress { get; set; }
	}
}
