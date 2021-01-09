using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltoMultiThreadDownloadManager
{
    public static class Logger
    {
        static string path = @"C:\users\PCDFN\desktop\log.txt";
        static object logLocker = new object();
        public static void Write(string text)
        {
            lock (logLocker)
            {
                File.AppendAllLines(path, new string[] { text });
            }
        }
    }
}
