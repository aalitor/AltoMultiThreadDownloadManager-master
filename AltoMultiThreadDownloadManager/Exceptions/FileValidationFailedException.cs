using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltoMultiThreadDownloadManager.Exceptions
{
    class FileValidationFailedException : Exception
    {
        public string LastChecksum { get; set; }
        public string CurrentChecksum { get; set; }
        public string FilePath { get; set; }
        public FileValidationFailedException(string filePath, string lastChecksum, string currentChecksum)
        {
            this.FilePath = filePath;
            this.LastChecksum = lastChecksum;
            this.CurrentChecksum = currentChecksum;
        }
    }
}
