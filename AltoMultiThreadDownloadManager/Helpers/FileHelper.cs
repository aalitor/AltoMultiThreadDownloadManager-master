using AltoMultiThreadDownloadManager.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
namespace AltoMultiThreadDownloadManager.Helpers
{
    /// <summary>
    /// Description of FileHelper.
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Checks and creates filestream according to the file if exists or not to append or not
        /// </summary>
        /// <param name="filePath">Filepath to check</param>
        /// <param name="append">Check file to append bytes or not</param>
        /// <returns></returns>
        public static FileStream CheckFile(string filePath, bool append)
        {
            var exists = File.Exists(filePath);
            if (append)
            {
                if (!exists) throw new Exception("File not exists to resume");

                else return new FileStream(filePath, FileMode.Append, FileAccess.Write);
            }
            else
            {
                if (!exists) return new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
                else return new FileStream(filePath, FileMode.Truncate, FileAccess.ReadWrite);
            }
        }

    }
}
