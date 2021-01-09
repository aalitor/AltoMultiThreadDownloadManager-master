
using System.Net;

namespace AltoMultiThreadDownloadManager
{
	/// <summary>
	/// Provides global settings to make a correct web request
	/// </summary>
	internal static class GlobalSettings
	{
        /// <summary>
        /// Sets settings
        /// </summary>
		public static void Set()
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
			| SecurityProtocolType.Tls11
			| SecurityProtocolType.Tls12
			| SecurityProtocolType.Ssl3;
			ServicePointManager.DefaultConnectionLimit = 10000;
			
		}
	}
}
