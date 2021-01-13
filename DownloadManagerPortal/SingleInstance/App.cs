using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DownloadManagerPortal;
using AltoMultiThreadDownloadManager.NativeMessages;
using AltoMultiThreadDownloadManager;
using Newtonsoft.Json;
using DownloadManagerPortal.Downloader;



namespace DownloadManagerPortal.SingleInstancing
{
    [Serializable]
    class App
    {
        #region Singleton

        private static App instance = new App();

        public static App Instance
        {
            get
            {
                return instance;
            }
        }

        private App()
        {
            AppManager.Instance.Initialize(this);
        }

        #endregion

        #region Fields

        private SingleInstanceTracker tracker = null;
        private bool disposed = false;

        #endregion

        #region Properties

        public Form MainForm
        {
            get
            {
                return (DownloadCenterForm)tracker.Enforcer;
            }
        }


        #endregion

        #region Methods

        private ISingleInstanceEnforcer GetSingleInstanceEnforcer()
        {
            return new DownloadCenterForm();
        }

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
            }
        }
        public void SendMessage(DownloadMessage msg)
        {
            if (tracker == null)
                // Attempt to create a tracker
                tracker = new SingleInstanceTracker("SingleInstanceSample", new SingleInstanceEnforcerRetriever(GetSingleInstanceEnforcer));
            if (msg != null)
            {
                var json = JsonConvert.SerializeObject(msg);
                tracker.SendMessageToFirstInstance(json);
            }
        }
        public void Start(string[] args, DownloadMessage msg)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {

                // Attempt to create a tracker
                tracker = new SingleInstanceTracker("SingleInstanceSample", new SingleInstanceEnforcerRetriever(GetSingleInstanceEnforcer));

                // If this is the first instance of the application, run the main form
                if (tracker.IsFirstInstance)
                {

                    if (msg != null)
                        return;
                    try
                    {
                        DownloadCenterForm form = (DownloadCenterForm)tracker.Enforcer;

                        //form.downloadList1.AddDownloadURLs(ResourceLocation.FromURLArray(args), 1, null, 0);

                        //if (Array.IndexOf<string>(args, "/as") >= 0)
                        //{
                        //    form.WindowState = FormWindowState.Minimized;
                        //}

                        //form.Load += delegate(object sender, EventArgs e)
                        //    {

                        //        if (form.WindowState == FormWindowState.Minimized)
                        //        {
                        //        }

                        //        if (args.Length > 0)
                        //        {
                        //        }
                        //    };

                        form.FormClosing += delegate(object sender, FormClosingEventArgs e)
                            {
                                Dispose();
                            };
                        Application.Run(form);
                    }
                    finally
                    {
                        Dispose();
                    }
                }
                else
                {
                    if (msg != null)
                    {
                        var json = JsonConvert.SerializeObject(msg);
                        tracker.SendMessageToFirstInstance(json);
                    }
                }
            }
            catch
            {
                return;
            }
            finally
            {
                if (tracker != null)
                    tracker.Dispose();
            }
        }

        #endregion
    }
}
