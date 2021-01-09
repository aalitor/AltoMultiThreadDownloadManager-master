
using System;
using System.Collections.Generic;
using System.Text;

namespace DownloadManagerPortal.SingleInstancing
{
    class AppManager
    {
        private AppManager()
        {
        }

        private static AppManager instance = new AppManager();

        public static AppManager Instance
        {
            get { return instance; }
        }

        private App application;

        public App Application
        {
            get { return application; }
        }

        public void Initialize(App app)
        {
            this.application = app;
        }
    }
}
