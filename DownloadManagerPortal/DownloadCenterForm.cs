using AltoMultiThreadDownloadManager;
using AltoMultiThreadDownloadManager.NativeMessages;
using DownloadManagerPortal.SingleInstancing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownloadManagerPortal
{
    public partial class DownloadCenterForm : Form, ISingleInstanceEnforcer
    {
        List<DownloaderControl> formlist = new List<DownloaderControl>();
        public DownloadCenterForm()
        {
            InitializeComponent();
            this.Load += DownloadCenterForm_Load;
            this.FormClosing += DownloadCenterForm_FormClosing;
        }

        void DownloadCenterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MTDOList.Any(x => x.dorg.IsActive))
            {
                flagCloseAfterStop = true;
                e.Cancel = true;
                foreach (var item in MTDOList)
                {
                    item.dorg.Stop();
                    item.dorg.Stopped += dorg_Stopped;
                }
            }
            else
                saveDownloadList();
        }
        bool flagCloseAfterStop = true;
        void dorg_Stopped(object sender, EventArgs e)
        {
            if (flagCloseAfterStop && !MTDOList.Any(x => x.dorg.IsActive))
            {
                flagCloseAfterStop = false;
                saveDownloadList();
                Application.Exit();
            }
        }

        void DownloadCenterForm_Load(object sender, EventArgs e)
        {
            readSettings();
        }



        #region Save or read saved download list
        void saveDownloadList()
        {
            var list = MTDOList.Select(x => x.dorg);
            var json = JsonConvert.SerializeObject(list);
            Properties.Settings.Default.DownloadList = json;
            Properties.Settings.Default.Save();
        }


        void readSettings()
        {
            try
            {
                var json = Properties.Settings.Default.DownloadList;
                var list = JsonConvert.DeserializeObject<List<MultiThreadDownloadOrganizer>>(json);
                if (list != null && list.Any())
                    foreach (var item in list)
                    {
                        var f = new DownloaderControl(item, false);
                        flowLayoutPanel1.Controls.Add(f);
                    }
            }
            catch
            {

            }
        }
        #endregion
        bool checkDownloadCompleted(DownloaderControl f)
        {
            return f != null && f.dorg != null && f.dorg.Info != null && f.dorg.Progress == 100;
        }


        public void OnMessageReceived(MessageEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.Activate();
                var msg = e.Message.ToString();
                var downloadRequest = JsonConvert.DeserializeObject<DownloadMessage>(msg);

                var f = findDownloader(downloadRequest.FileName);

                var completed = checkDownloadCompleted(f);

                if (f == null)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        var mtdo = createMTDO(downloadRequest);
                        var downloader = new DownloaderControl(mtdo);
                        flowLayoutPanel1.Controls.Add(downloader);
                        this.Activate();
                    });
                }
                else if (f.NewUrlRequested)
                {
                    f.RefreshUrl(downloadRequest);
                }
                else if (completed)
                {
                    var result = MessageBox.Show("Download already completed! Do you want to download again?",
                        downloadRequest.FileName, MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        f.dorg.Url = downloadRequest.Url;
                        f.dorg.Info.Url = downloadRequest.Url;
                        f.dorg.DownloadRequestMessage = downloadRequest;
                        f.DownloadAgain();
                    }
                }
                else if (!f.dorg.IsActive)
                {
                    f.dorg.Resume();
                }
            });
        }




        #region Select or remove download in list


        DownloaderControl findDownloader(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return null;
            var t = MTDOList.Where(x => x.dorg != null && x.dorg.Info != null &&
                            x.dorg.Info.ServerFileName == filename);

            return t.Any() ? t.OrderByDescending(x => x.dorg.TotalBytesReceived)
                .ThenByDescending(x => x.dorg.LastTry).First() : null;
        }


        #endregion

        MultiThreadDownloadOrganizer createMTDO(DownloadMessage MSG)
        {
            var finalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            tempFolder = Path.Combine(tempFolder, "AltoDownloadAccelerator");
            var nofThreads = 8;
            var dorg = new MultiThreadDownloadOrganizer(MSG.Url, finalFolder, tempFolder, nofThreads);
            dorg.DownloadRequestMessage = MSG;
            return dorg;
        }

        IEnumerable<DownloaderControl> MTDOList
        {
            get
            {
                return this.flowLayoutPanel1.Controls.Cast<Control>()
                    .Where(x => x is DownloaderControl)
                    .Select(x => (DownloaderControl)x);

            }
        }


        public void OnNewInstanceCreated(EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}