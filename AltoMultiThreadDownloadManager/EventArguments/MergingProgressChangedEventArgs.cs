
using System;

namespace AltoMultiThreadDownloadManager.EventArguments
{
	/// <summary>
	/// Event arguments for file merging event
	/// </summary>
	public class MergingProgressChangedEventArgs: EventArgs
	{
        /// <summary>
        /// Constructor for progress changed arguments
        /// </summary>
        /// <param name="progress">Progress for file merging process; max = 100</param>
		public MergingProgressChangedEventArgs(double progress)
		{
			Progress = progress;
		}
		/// <summary>
		/// Gets or sets the progress value
		/// </summary>
		public double Progress { get; set; }
	}
}