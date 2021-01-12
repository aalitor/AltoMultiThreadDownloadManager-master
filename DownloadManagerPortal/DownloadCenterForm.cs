using AltoMultiThreadDownloadManager;
using AltoMultiThreadDownloadManager.NativeMessages;
using DownloadManagerPortal.Downloader;
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
        List<DownloaderForm> formlist = new List<DownloaderForm>();
        public DownloadCenterForm()
        {
            InitializeComponent();
            this.Load += DownloadCenterForm_Load;
            this.FormClosing += DownloadCenterForm_FormClosing;
            DoubleBuffering.SetDoubleBuffered(listView1);
            timer1.Tick += timer1_Tick;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
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
            AddRows();
            foreach (var item in formlist)
            {
                WriteItem(item.dorg);
            }
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
                        var a = new DownloaderForm(item, false);
                        a.FormClosed += a_FormClosed;
                        formlist.Add(a);
                    }
            }
            catch
            {

            }
        }
        #endregion
        bool checkDownloadCompleted(DownloaderForm f)
        {
            return f != null && f.dorg != null && f.dorg.Info != null && f.dorg.Progress == 100;
        }

        void showForm(DownloaderForm f, DownloadMessage req, bool directStart)
        {

            if (f != null && f.Visible)
            {
                return;
            }
            else
            {

                removeForm(req.FileName, req.Url);
                var a = new DownloaderForm(f.dorg, directStart);
                a.FormClosed += a_FormClosed;
                formlist.Add(a);
                a.Show(null);
            }
        }

        void a_FormClosed(object sender, FormClosedEventArgs e)
        {
            var a = (DownloaderForm)sender;
            a.dorg.Stop();
            removeForm(a.dorg.DownloadRequestMessage.FileName, a.dorg.DownloadRequestMessage.Url);
            a = new DownloaderForm(a.dorg, false);
            a.FormClosed += a_FormClosed;
            formlist.Add(a);
            this.Activate();
        }
        public void OnMessageReceived(MessageEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.Activate();
                var msg = e.Message.ToString();
                var downloadRequest = JsonConvert.DeserializeObject<DownloadMessage>(msg);

                var f = findDownloader(downloadRequest.FileName, downloadRequest.Url);
                var completed = checkDownloadCompleted(f);

                if (f == null)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        var mtdo = createMTDO(downloadRequest);
                        var downloader = new DownloaderForm(mtdo, true);
                        showForm(downloader, downloadRequest, true);
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
                    MessageBox.Show("Download already exists but stopped, it will be resumed",
                        downloadRequest.FileName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    f.dorg.Resume();
                    if (!f.Visible)
                        f.Show(null);
                }
                else if (f.dorg.IsActive)
                {
                    MessageBox.Show("Download already running!", f.dorg.Info.ServerFileName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (!f.Visible)
                        f.Show(null);
                }
            });
        }




        #region Select or remove download in list


        internal DownloaderForm findDownloaderFromFilename(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return null;
            var t = MTDOList.Where(x => x.dorg != null && x.dorg.Info != null &&
                            x.dorg.Info.ServerFileName == filename);

            return t.Any() ? t.OrderByDescending(x => x.dorg.TotalBytesReceived)
                .ThenByDescending(x => x.dorg.LastTry).First() : null;
        }
        internal DownloaderForm findDownloaderFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;
            var t = MTDOList.Where(x => x.dorg != null && x.dorg.Info != null &&
                            x.dorg.Info.Url == url);

            return t.Any() ? t.OrderByDescending(x => x.dorg.TotalBytesReceived)
                .ThenByDescending(x => x.dorg.LastTry).First() : null;
        }
        internal DownloaderForm findDownloader(string filename, string url)
        {
            var u = findDownloaderFromUrl(url);
            var f = findDownloaderFromFilename(filename);
            return u != null ? u : f != null ? f : null;
        }

        internal void removeForm(string filename, string url)
        {
            formlist = formlist.Where(x => x != null && x.dorg.Info != null && x.dorg.Info.ServerFileName != filename).ToList();
            formlist = formlist.Where(x => x != null && x.dorg.Info != null && x.dorg.Info.Url != url).ToList();
        }
        #endregion

        internal MultiThreadDownloadOrganizer createMTDO(DownloadMessage MSG)
        {
            var finalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            tempFolder = Path.Combine(tempFolder, "AltoDownloadAccelerator");
            var nofThreads = 8;
            var dorg = new MultiThreadDownloadOrganizer(MSG.Url, finalFolder, "", tempFolder, nofThreads);
            dorg.DownloadRequestMessage = MSG;
            return dorg;
        }

        internal IEnumerable<DownloaderForm> MTDOList
        {
            get
            {
                return this.formlist.Cast<Control>()
                    .Where(x => x is DownloaderForm)
                    .Select(x => (DownloaderForm)x);

            }
        }


        public void OnNewInstanceCreated(EventArgs e)
        {
            throw new NotImplementedException();
        }


        private void btnAddDownload_Click(object sender, EventArgs e)
        {
            new EnterUrlForm().ShowDialog();
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                return;
            var item = listView1.SelectedItems[0];
            var f = findDownloader(item.Text, item.SubItems[7].Text);
            var msg = JsonConvert.SerializeObject(f.dorg.DownloadRequestMessage);
            OnMessageReceived(new MessageEventArgs(msg));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                return;
            if (MessageHelper.AskYes("Are you sure to delete the selected items?")) ;
            foreach (var item in listView1.SelectedItems)
            {
                var lvi = (ListViewItem)item;
                var f = findDownloader(lvi.Text, "");
                var folder = f.dorg.RangeDir;
                removeForm(f.dorg.DownloadRequestMessage.FileName, f.dorg.DownloadRequestMessage.Url);
                listView1.SmallImageList.Images.RemoveAt(lvi.Index);
                listView1.Items.Remove(lvi);
                if (Directory.Exists(folder))
                    Directory.Delete(folder, true);

            }
            saveDownloadList();
        }


    }
}