using AltoMultiThreadDownloadManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AltoMultiThreadDownloadManager.Helpers;
using AltoMultiThreadDownloadManager.AssociatedIcons;
using DownloadManagerPortal.Downloader;
using System.IO;
using AltoMultiThreadDownloadManager.Enums;
namespace DownloadManagerPortal
{
    public partial class DownloadCenterForm
    {
        void AddRows()
        {

        }
        DownloaderForm getSelectedItem()
        {
            if (listView1.SelectedItems.Count < 1)
                return null;

            var filename = listView1.SelectedItems[0].Text;
            var url = listView1.SelectedItems[0].SubItems[7].Text;

            return findDownloader(filename, url);
        }

        void disableButtonsIfActive()
        {
            if (listView1.SelectedItems.Count < 1)
            {
                btnResume.Enabled = false;
                btnDelete.Enabled = false;
                return;
            }
            if (listView1.SelectedItems.Count > 1)
            {
                btnResume.Enabled = false;
                btnDelete.Enabled = true;
                return;
            }

            var f = getSelectedItem();


            if (f == null || f.dorg == null)
            {
                btnResume.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                btnResume.Enabled = !f.dorg.IsActive;
                btnDelete.Enabled = !f.dorg.IsActive;

                btnResume.Text = f.dorg.Status == HttpDownloaderStatus.Completed ? "Download again" :
                    f.dorg.Status == HttpDownloaderStatus.Stopped ? "Resume" : "Pause";

            }
        }
        void WriteItem(HttpMultiThreadDownloader mtdo)
        {
            if (mtdo == null || mtdo.Info == null)
                return;
            var fullList = listView1.Items.Cast<ListViewItem>();
            var itemlist = fullList.Where(x => x.Text == mtdo.Info.ServerFileName ||
                                               x.SubItems[7].Text == mtdo.Info.Url);
            var i = -1;
            if (listView1.SmallImageList == null)
                listView1.SmallImageList = new ImageList();
            var subitems = new string[]
            {
                mtdo.Info.ServerFileName,
                mtdo.ProgressString,
                mtdo.TotalBytesReceived.ToHumanReadableSize(),

                mtdo.Info.ContentSize > 0 ? mtdo.Info.ContentSize.ToHumanReadableSize() :
                mtdo.Status == HttpDownloaderStatus.Completed ? 
                mtdo.TotalBytesReceived.ToHumanReadableSize() : "Unknown",

                mtdo.Status.ToString(),
                mtdo.Speed.ToHumanReadableSize() + "/s",
                mtdo.Info.ResumeCapability.ToString(),
                mtdo.Url
            };

            if (itemlist.Any())
            {
                var it = itemlist.First();
                i = it.Index;
                for (int j = 0; j < it.SubItems.Count; j++)
                {
                    var a = (System.Windows.Forms.ListViewItem.ListViewSubItem)it.SubItems[j];
                    a.Text = subitems[j];
                }
                listView1.SmallImageList.Images[i] = IconReader.GetFileIcon(mtdo.FilePath, IconReader.IconSize.Small, false).ToBitmap();
            }
            else
            {
                listView1.SmallImageList.Images.Add(IconReader.GetFileIcon(mtdo.FilePath, IconReader.IconSize.Small, false));

                var lvi = new ListViewItem(subitems, listView1.Items.Count);
                listView1.Items.Add(lvi);
            }


        }


        void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var item in MTDOList.Select(x => x.dorg))
            {
                WriteItem(item);
            }

            disableButtonsIfActive();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
                return;
            if (MessageHelper.AskYes("Are you sure to delete the selected items?"))
                foreach (var item in listView1.SelectedItems)
                {
                    var lvi = (ListViewItem)item;
                    var f = findDownloader(lvi.Text, lvi.SubItems[7].Text);
                    if (f != null && f.dorg != null)
                    {
                        if (f.dorg.IsActive)
                            continue;
                    }
                    else
                    {
                        continue;

                    }
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
