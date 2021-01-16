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

                btnResume.Text = f.dorg.Status == DownloaderStatus.Completed ? "Download again" :
                    f.dorg.Status == DownloaderStatus.Stopped ? "Resume" : "Pause";

            }
        }
        void WriteItem(MultiThreadDownloadOrganizer mtdo)
        {
            if (mtdo == null || mtdo.Info == null)
                return;
            var fullList = listView1.Items.Cast<ListViewItem>();
            var itemlist = fullList.Where(x => x.Text == mtdo.Info.ServerFileName);
            var i = -1;
            var s = 1;
            if (listView1.SmallImageList == null)
                listView1.SmallImageList = new ImageList();
            if (itemlist.Any())
            {
                i = itemlist.First().Index;
                listView1.Items[i].Text = mtdo.Info.ServerFileName;
                listView1.Items[i].SubItems[s++].Text = mtdo.ProgressString;
                listView1.Items[i].SubItems[s++].Text = mtdo.TotalBytesReceived.ToHumanReadableSize();
                listView1.Items[i].SubItems[s++].Text = mtdo.Info.ContentSize.ToHumanReadableSize();
                listView1.Items[i].SubItems[s++].Text = mtdo.Status.ToString();
                listView1.Items[i].SubItems[s++].Text = mtdo.Speed.ToHumanReadableSize() + "/s";
                listView1.Items[i].SubItems[s++].Text = mtdo.Info.ResumeCapability.ToString();
                listView1.Items[i].SubItems[s++].Text = mtdo.Url;
                listView1.SmallImageList.Images[i] = IconReader.GetFileIcon(mtdo.FilePath, IconReader.IconSize.Small, false).ToBitmap();
                listView1.Items[i].ImageIndex = i;
            }
            else
            {
                var lvi = new ListViewItem(mtdo.Info.ServerFileName);
                lvi.SubItems.Add(mtdo.ProgressString);
                lvi.SubItems.Add(mtdo.TotalBytesReceived.ToHumanReadableSize());
                lvi.SubItems.Add(mtdo.Info.ContentSize.ToHumanReadableSize());
                lvi.SubItems.Add(mtdo.Status.ToString());
                lvi.SubItems.Add(mtdo.Speed.ToHumanReadableSize() + "/s");
                lvi.SubItems.Add(mtdo.Info.ResumeCapability.ToString());
                lvi.SubItems.Add(mtdo.Url);
                lvi.ImageIndex = listView1.Items.Count;
                listView1.SmallImageList.Images.Add(IconReader.GetFileIcon(mtdo.FilePath, IconReader.IconSize.Small, false));
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
