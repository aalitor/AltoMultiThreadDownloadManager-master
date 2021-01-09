using AltoMultiThreadDownloadManager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AltoMultiThreadDownloadManager.Helpers;
using AltoMultiThreadDownloadManager.NativeMessages;
using AltoMultiThreadDownloadManager.AssociatedIcons;
using System.IO;
using DownloadManagerPortal.ChromeIntegrator;
using System.Reflection;
using System.Diagnostics;
using DownloadManagerPortal.SingleInstancing;
using DownloadManagerPortal.DownloadHandler;
namespace DownloadManagerPortal
{
    partial class MainForm : Form, ISingleInstanceEnforcer
    {
        List<DownloadHandlerForm> formlist = new List<DownloadHandlerForm>();
        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
            this.FormClosing += MainForm_FormClosing;
            listView1.SmallImageList = new ImageList();
        }

        public void OnNewInstanceCreated(EventArgs e)
        {

        }

        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            //Taxes: Remote Desktop Connection and painting
            //http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo aProp =
                  typeof(System.Windows.Forms.Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }

        #region Save or read saved download list
        void saveDownloadList()
        {
            var list = formlist.Select(x => x.dorg);
            var json = JsonConvert.SerializeObject(list);
            Properties.Settings.Default.DownloadList = json;
            Properties.Settings.Default.Save();
        }


        void readSettings()
        {
            try
            {
                formlist = new List<DownloadHandlerForm>();
                var json = Properties.Settings.Default.DownloadList;
                var list = JsonConvert.DeserializeObject<List<MultiThreadDownloadOrganizer>>(json);
                if (list != null && list.Any())
                    foreach (var item in list)
                    {
                        var f = new DownloadHandlerForm(item);
                        formlist.Add(f);
                    }
            }
            catch
            {

            }
        }
        #endregion

        #region Listview add update operations
        private void timer1_Tick(object sender, EventArgs e)
        {
            fillTable();
        }
        List<DownloadHandlerForm> getUpdateList()
        {
            return formlist.Where(x => x != null && x.dorg != null && x.dorg.Info != null).ToList();
        }
        void fillTable()
        {
            var ulist = getUpdateList();

            foreach (var item in ulist)
            {
                addOrUpdateListItem(item);
            }

        }

        void addOrUpdateListItem(DownloadHandlerForm item)
        {

            var mtdo = item.dorg; ;
            var key = mtdo.Info != null ? mtdo.Info.ServerFileName : "";

            if (string.IsNullOrEmpty(key))
                return;
            var list = listView1.Items.Cast<ListViewItem>();
            if (list.Any(x => x.Text == mtdo.Info.ServerFileName))
            {
                var lvi = list.First(x => x.Text == mtdo.Info.ServerFileName);
                updateListItem(lvi, mtdo);
            }
            else
            {
                addListItem(mtdo);
            }

        }
        void addListItem(MultiThreadDownloadOrganizer mtdo)
        {
            if (mtdo == null || mtdo.Info == null || string.IsNullOrEmpty(mtdo.Info.ServerFileName))
                return;

            var lvi = new ListViewItem(mtdo.Info.ServerFileName);
            var img = IconReader.GetFileIcon(mtdo.Info.ServerFileName, IconReader.IconSize.Small, false);
            lvi.ImageIndex = listView1.Items.Count;

            listView1.SmallImageList.Images.Add(img);

            lvi.SubItems.Add(mtdo.Info.AcceptRanges ? "Yes" : "No");
            lvi.SubItems.Add(mtdo.Info.ContentSize.ToHumanReadableSize());
            lvi.SubItems.Add(mtdo.TotalBytesReceived.ToHumanReadableSize());
            lvi.SubItems.Add("%" + mtdo.Progress.ToString());
            lvi.SubItems.Add(mtdo.Speed.ToHumanReadableSize() + "/s");
            lvi.SubItems.Add(mtdo.LastTry.ToShortDateString() + " " + mtdo.LastTry.ToShortTimeString());
            listView1.Items.Add(lvi);
        }
        void updateListItem(ListViewItem lvi, MultiThreadDownloadOrganizer mtdo)
        {
            var i = 1;
            lvi.SubItems[i++].Text = (mtdo.Info.AcceptRanges ? "Yes" : "No");
            lvi.SubItems[i++].Text = (mtdo.Info.ContentSize.ToHumanReadableSize());
            lvi.SubItems[i++].Text = (mtdo.TotalBytesReceived.ToHumanReadableSize());
            lvi.SubItems[i++].Text = ("%" + mtdo.Progress.ToString());
            lvi.SubItems[i++].Text = (mtdo.Speed.ToHumanReadableSize() + "/s");
            lvi.SubItems[i++].Text = (mtdo.LastTry.ToShortDateString() + " " + mtdo.LastTry.ToShortTimeString());
        }
        #endregion

        #region MainForm events

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveDownloadList();
        }
        void MainForm_Load(object sender, EventArgs e)
        {
            var exeDir = Directory.GetCurrentDirectory();
            var exePath = Assembly.GetExecutingAssembly().Location;
            var hostName = Properties.Settings.Default.ChromeExtensionName;
            var extId = Properties.Settings.Default.ChromeExtensionId; ;
            var hostPath = Path.Combine(exeDir, hostName + ".json");

            var hei = new HostExtensionIntegrator();
            hei.HostPath = hostPath;
            hei.ExePath = exePath;
            hei.CheckHostFile(hostPath);

            chkChrome.Checked = RegistryExtensionIntegrator.CheckIntegration(hostName, hostPath, extId);
            readSettings();
            SetDoubleBuffered(listView1);
            timer1.Start();
        }
        #endregion

        #region Download resume or stop operations
        private void btnAddDownload_Click(object sender, EventArgs e)
        {
            new EnterUrlForm().ShowDialog();
        }
        bool checkDownloadCompleted(DownloadHandlerForm f)
        {
            return f != null && f.dorg != null && f.dorg.Info != null && f.dorg.Progress == 100;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                return;

            var key = listView1.SelectedItems[0].Text;
            var f = findForm(key);
            if (checkDownloadCompleted(f))
            {
                var result = MessageBox.Show("Download already completed! Do you want to download again?",
                    "Download completed already", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    var form = new DownloadHandlerForm(f.dorg.DownloadRequestMessage);
                    removeForm(f);
                    formlist.Add(form);
                    form.ShowDialog(null);
                }
            }
            else if (!f.dorg.Info.AcceptRanges)
            {
                var result = MessageBox.Show("Download has no resume capability. The file will be downloaded again from beginning. Do you want to continue?", "No resume ability", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    var form = new DownloadHandlerForm(f.dorg.DownloadRequestMessage);
                    removeForm(f);
                    formlist.Add(form);
                    form.ShowDialog(null);
                }
            }
            else
                resume(key);
        }

        void resume(string key)
        {
            this.Invoke((MethodInvoker)delegate
            {
                var obj = findForm(key);
                if (obj == null)
                    return;
                var mtdoClone = obj.dorg.Clone();
                obj.Close();
                var f = new DownloadHandlerForm(mtdoClone);
                removeForm(f);
                formlist.Add(f);
                f.ShowDialog(null);
            });
        }
        void resumeFull(MultiThreadDownloadOrganizer mtdo)
        {
            this.Invoke((MethodInvoker)delegate
            {
                var mtdoClone = mtdo.Clone();
                var a = new DownloadHandlerForm(mtdoClone);
                formlist.Add(a);
                a.ShowDialog(null);
            });
        }
        public void OnMessageReceived(MessageEventArgs e)
        {
            var msg = e.Message.ToString();
            var downloadRequest = JsonConvert.DeserializeObject<DownloadMessage>(msg);

            var f = findForm(downloadRequest.FileName);

            var completed = checkDownloadCompleted(f);
            var formnull = f != null;

            if (completed)
            {
                var result = MessageBox.Show("Download already completed! Do you want to download again?",
                    "Download completed already", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    var form = new DownloadHandlerForm(downloadRequest);
                    removeForm(f);
                    formlist.Add(form);
                    form.ShowDialog(null);
                }
            }
            else if (formnull)
            {

                if (f.waitingNewUrl)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        f.waitingNewUrl = false;
                        f.dorg.DownloadRequestMessage = downloadRequest;
                        f.FormClosed += f_FormClosed;
                        f.WaiterForm.Close();
                    });
                }
                //MessageBox.Show("There is already the same download! " + downloadRequest.FileName);
                //return;
            }
            else
            {
                var form = new DownloadHandlerForm(downloadRequest);
                formlist.Add(form);
                form.Shown += (m, n) => form.Activate();
                form.ShowDialog(null);
            }
        }

        void f_FormClosed(object sender, FormClosedEventArgs e)
        {
            var f = sender as DownloadHandlerForm;
            var downloadRequest = f.dorg.DownloadRequestMessage;
            f.dorg.Url = downloadRequest.Url;
            if (f.dorg != null)
                f.dorg.Info.Url = downloadRequest.Url;
            f.dorg.DownloadRequestMessage = downloadRequest;
            removeForm(f);
            resumeFull(f.dorg);
        }
        #endregion

        #region Select or remove download in list
        void removeForm(DownloadHandlerForm h)
        {
            formlist = formlist.Where(x => x != null && x.dorg != null
                && x.dorg.Info.ServerFileName != h.dorg.Info.ServerFileName).ToList();
        }

        DownloadHandlerForm findForm(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return null;
            var t = formlist.Where(x => x.dorg != null && x.dorg.Info != null &&
                            x.dorg.Info.ServerFileName == filename);

            return t.Any() ? t.OrderByDescending(x => x.dorg.TotalBytesReceived)
                .ThenByDescending(x => x.dorg.LastTry).First() : null;
        }

        void removeForm(string filename)
        {
            formlist.RemoveAll(x => x != null && x.dorg != null && x.dorg.Info != null && x.dorg.Info.ServerFileName == filename);
        }
        #endregion

        #region Chrome Extension Integration
        private void btnChromeIntegrate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!RegistryExtensionIntegrator.HasAdminRights())
                {
                    MessageBox.Show("You must start as admin to integrate with Chrome", "Admin privilege required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var extensionUrl = "https://chrome.google.com/webstore/detail/comaltomultithreaddownloa/kimajdeajmdbbldmjdckapkabikgkkee?hl=tr&authuser=0";
                var exeDir = Directory.GetCurrentDirectory();
                var exePath = Assembly.GetExecutingAssembly().Location;
                var hostName = Properties.Settings.Default.ChromeExtensionName;
                var hostPath = Path.Combine(exeDir, hostName + ".json");

                var hei = new HostExtensionIntegrator();
                hei.HostPath = hostPath;
                hei.ExePath = exePath;
                hei.CheckHostFile(hostPath);

                RegistryExtensionIntegrator.Complete(hostPath, hostName);

                MessageBox.Show("Integration succesfull! Chrome web store will be opened to install the extension. Add it to chrome and it will be done!");
                Process.Start(extensionUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message + "\r\n\r\n" + ex.StackTrace);
            }
        }

        private void btnRemoveIntegration_Click(object sender, EventArgs e)
        {
            try
            {
                if (!RegistryExtensionIntegrator.HasAdminRights())
                {
                    MessageBox.Show("You must start as admin to remove Chrome Integration", "Admin privilege required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var hostName = Properties.Settings.Default.ChromeExtensionName;
                RegistryExtensionIntegrator.Remove(hostName);

                MessageBox.Show("Integration removed succesfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message + "\r\n\r\n" + ex.StackTrace);
            }
        }
        #endregion

        #region Delete Listview items

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var result = MessageBox.Show("Are you sure you want to delete selected items?", "Delete items", MessageBoxButtons.YesNo);
                if (result != System.Windows.Forms.DialogResult.Yes)
                    return;
                foreach (var item in listView1.SelectedItems)
                {
                    var lvi = (ListViewItem)item;
                    var form = findForm(lvi.Text);
                    foreach (var rng in form.dorg.Ranges)
                    {
                        if (File.Exists(rng.FilePath))
                            File.Delete(rng.FilePath);
                    }
                    if (Directory.Exists(form.dorg.RangeDir))
                        Directory.Delete(form.dorg.RangeDir);
                    removeForm(lvi.Text);
                    listView1.Items.Remove(lvi);

                }

                saveDownloadList();
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {

            var result = MessageBox.Show("Are you sure you want to delete all items?", "Delete items", MessageBoxButtons.YesNo);
            if (result != System.Windows.Forms.DialogResult.Yes)
                return;
            foreach (var item in listView1.Items)
            {
                var lvi = (ListViewItem)item;
                var form = findForm(lvi.Text);
                if (form != null && form.dorg != null)
                    foreach (var rng in form.dorg.Ranges)
                    {
                        if (File.Exists(rng.FilePath))
                            File.Delete(rng.FilePath);
                    }
                if (Directory.Exists(form.dorg.RangeDir))
                    Directory.Delete(form.dorg.RangeDir);
                removeForm(lvi.Text);
                listView1.Items.Remove(lvi);
            }

            saveDownloadList();

        }

        #endregion

    }
}
