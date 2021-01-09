using System;
using System.IO;
using System.Security.Cryptography;

namespace AltoMultiThreadDownloadManager.FileVerification
{
    /// <summary>
    /// Provides method to calculate Md5 checksum for files
    /// </summary>
    public static class Checksum
    {
        /// <summary>
        /// Calculates Md5 value for given filename
        /// </summary>
        /// <param name="filename">Filename to calculate Md5</param>
        /// <returns></returns>
        public static string CalculateMD5(string filename)
        {
            if (!File.Exists(filename))
                return null;
            using (var md5 = MD5.Create())
            {
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 1024))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
