using AltoMultiThreadDownloadManager;
using AltoMultiThreadDownloadManager.Enums;
using AltoMultiThreadDownloadManager.NativeMessages;
using DownloadManagerPortal.ChromeIntegrator;
using DownloadManagerPortal.Downloader;
using DownloadManagerPortal.SingleInstancing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
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
            var saveDir = Properties.Settings.Default.SaveFolder;
            var nofThread = Properties.Settings.Default.NofThread;

            saveDir = string.IsNullOrEmpty(saveDir) ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : saveDir;
            nofThread = nofThread < 1 ? 8 : nofThread;

            Properties.Settings.Default.SaveFolder = saveDir;
            Properties.Settings.Default.NofThread = nofThread;
            Properties.Settings.Default.Save();
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
            {
                saveDownloadList();
                notifyIcon1.Visible = false;
                notifyIcon1.Icon = null;
                notifyIcon1.Dispose();
                notifyIcon1 = null;
            }
        }
        bool flagCloseAfterStop = true;
        void dorg_Stopped(object sender, EventArgs e)
        {
            if (flagCloseAfterStop && !MTDOList.Any(x => x.dorg.IsActive))
            {
                flagCloseAfterStop = false;
                saveDownloadList();
                notifyIcon1.Visible = false;
                notifyIcon1.Icon = null;
                notifyIcon1.Dispose();
                
                Application.Exit();
            }
        }

        void DownloadCenterForm_Load(object sender, EventArgs e)
        {
            var cm = new ContextMenu();
            var m = new MenuItem("Exit");
            m.Click += (a, n) =>
            {
                this.Close();
                Application.Exit();
            };
            cm.MenuItems.Add(m);
            notifyIcon1.ContextMenu = cm;
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
            return f != null && f.dorg != null && f.dorg.Status == DownloaderStatus.Completed;
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
                a.Shown += a_Shown;
                a.Show();
            }
        }

        void a_Shown(object sender, EventArgs e)
        {
            var f = (DownloaderForm)sender;
            f.Shown += f_Shown;
            f.TopMost = true;
        }

        void a_FormClosed(object sender, FormClosedEventArgs e)
        {
            var a = (DownloaderForm)sender;
            a.dorg.Stop();
            removeForm(a.dorg.DownloadRequestMessage.FileName, a.dorg.DownloadRequestMessage.Url);
            a = new DownloaderForm(a.dorg, false)
            {
                NewUrlRequested = a.NewUrlRequested
            };
            a.FormClosed += a_FormClosed;
            formlist.Add(a);
            


        }
        public void OnMessageReceived(MessageEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                var msg = e.Message.ToString();
                var downloadRequest = JsonConvert.DeserializeObject<DownloadMessage>(msg);

                var f = findDownloader(downloadRequest.FileName, downloadRequest.Url);
                if (f != null)
                {
                    f.TopMost = true;
                }
                var completed = checkDownloadCompleted(f);

                if (f == null)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        var mtdo = createMTDO(downloadRequest);
                        var downloader = new DownloaderForm(mtdo, true);
                        showForm(downloader, downloadRequest, true);
                    });
                }
                else if (f.NewUrlRequested)
                {
                    f.RefreshUrl(downloadRequest);
                }
                else if (completed)
                {
                    this.Activate();
                    f.dorg.Url = downloadRequest.Url;
                    f.dorg.Info.Url = downloadRequest.Url;
                    f.dorg.DownloadRequestMessage = downloadRequest;
                    f.Show();
                    var result = MessageBox.Show("Download already completed! Do you want to download again?",
                        downloadRequest.FileName, MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        f.HandleAlreadyCompleted();
                    }
                    else
                        f.Close();
                }
                else if (!f.dorg.IsActive)
                {
                    Directory.CreateDirectory(f.dorg.RangeDir);
                    foreach (var item in f.dorg.Ranges)
                    {
                        if (!File.Exists(item.FilePath))
                        {
                            item.TotalBytesReceived = 0;
                        }
                    }
                    f.dorg.Resume();
                    if (!f.Visible)
                        f.Show();
                }
                else if (f.dorg.IsActive)
                {
                    this.Activate();
                    if (!f.Visible)
                        f.Show();
                }
            });
        }

        void f_Shown(object sender, EventArgs e)
        {
            var f = sender as DownloaderForm;
            
            f.Activate();
            f.Focus();
            f.ShowInTaskbar = false;
        }





        #region Select or remove download in list


        internal DownloaderForm findDownloaderFromFilename(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return null;
            var t = MTDOList.Where(x => x.dorg != null && x.dorg.Info != null &&
                            x.dorg.Info.ServerFileName == filename);
            if (!t.Any())
            {
                t = MTDOList.Where(x => x.dorg != null && x.dorg.DownloadRequestMessage != null &&
                            x.dorg.DownloadRequestMessage.FileName == filename);
            }
            return t.FirstOrDefault();
        }
        internal DownloaderForm findDownloaderFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;
            var t = MTDOList.Where(x => x.dorg != null && x.dorg.Info != null &&
                            x.dorg.Info.Url == url);

            return t.FirstOrDefault();
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
            var finalFolder = Properties.Settings.Default.SaveFolder;
            var tempFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            tempFolder = Path.Combine(tempFolder, "AltoDownloadAccelerator");
            var nofThreads = 8;
            var dorg = new MultiThreadDownloadOrganizer(MSG.Url, finalFolder, "", tempFolder, nofThreads);
            dorg.Id = Guid.NewGuid().ToString("N");
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



        private void btnIntegrateChrome_Click(object sender, EventArgs e)
        {

            if (!isAdmin())
            {
                MessageBox.Show("You must start the program as admin to integrate. Because registry operations are necessary for integration.", "Integration failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var extname = Properties.Settings.Default.ChromeExtensionName;
            var extid = Properties.Settings.Default.ChromeExtensionId;
            var extensionUrl = Properties.Settings.Default.ExtensionUrl;

            var currentDir = Directory.GetCurrentDirectory();
            var hostPath = Path.Combine(currentDir, extname + ".json");
            var exePath = Assembly.GetExecutingAssembly().Location;

            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appDataChromeExtPath = appData + @"\Google\Chrome\User Data\Default\Extensions\" + extid;

            var h = new HostExtensionIntegrator();
            h.ExePath = exePath;
            h.HostPath = hostPath;
            h.CreateHostFile();

            RegistryExtensionIntegrator.Complete(hostPath, extname);

            var extExists = Directory.Exists(appDataChromeExtPath) && Directory.GetDirectories(appDataChromeExtPath).Any();

            if (RegistryExtensionIntegrator.CheckHost(extname, hostPath) && extExists)
            {
                MessageBox.Show("Completed");
            }
            else
            {
                MessageBox.Show("You must install the chrome extension in browser to complete integration if it is not installed",
                    "Integration almost ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process.Start(extensionUrl);
            }
        }

        bool isAdmin()
        {
            bool isElevated;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            return isElevated;
        }

        private void DownloadCenterForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            new DownloadSettingsForm().ShowDialog();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                notifyIcon1.ContextMenu.Show(this, Cursor.Position);
            }
        }



    }
}