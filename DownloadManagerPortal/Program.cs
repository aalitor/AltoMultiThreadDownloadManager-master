using AltoMultiThreadDownloadManager.NativeMessages;
using DownloadManagerPortal.SingleInstancing;
using System;

namespace DownloadManagerPortal
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            App.Instance.Start(args, Receiver.ReadDownloadMessage());
        }
    }
}
