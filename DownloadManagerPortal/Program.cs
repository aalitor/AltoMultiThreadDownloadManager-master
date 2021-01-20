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
            MSG = Receiver.ReadDownloadMessage();
            if (MSG != null)
            {
                var status = Properties.Settings.Default.ChromeIntegration ? "OK" : "NotOK";
                Sender.OpenStandardStreamOut(status);
            }


            App.Instance.Start(args, MSG);
        }

        public static DownloadMessage MSG;
    }
}
