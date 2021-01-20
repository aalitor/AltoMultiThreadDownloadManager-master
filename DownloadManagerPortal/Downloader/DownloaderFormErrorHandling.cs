using AltoMultiThreadDownloadManager.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownloadManagerPortal.Downloader
{
    public partial class DownloaderForm : Form
    {
        void stopDownloader()
        {
            if (dorg != null && dorg.IsActive)
            {
                dorg.Stop();
            }
        }
        object locker = new object();
        void WriteLog(string text)
        {
            if (dorg == null)
                return;
            lock (locker)
            {
                var path = Path.Combine(dorg.RangeDir, "errorlog.txt");
                File.AppendAllText(path, text);
            }
        }


        public void handleError(Exception ex)
        {
            WriteLog(ex.Message + "\r\n" + ex.StackTrace);
            WebException webex = null;
            if (ex is WebException)
                webex = (WebException)ex;
            
            if (ex is RemoteFilePropertiesChangedException)
            {
                
                if (!NewUrlRequested && !dorg.FlagStop)
                {
                    stopDownloader();
                    RequestAvailable = true;

                    var rex = ex as RemoteFilePropertiesChangedException;
                    var yesno = MessageBox.Show("Remote file properties seems to be changed. Do you want to renew url and auth data?", "Url expired", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (yesno == System.Windows.Forms.DialogResult.Yes)
                        RequestNewUrl();
                    else
                    {
                        RequestAvailable = false;
                        NewUrlRequested = false;
                        this.Close();
                    }
                }
            }
            else
            {
                if(ex is ReturnedContentSizeWrongException)
                {
                    lblError.Text = "Last Error: " + ex.Message;
                }
                else if (!(ex is WebException))
                {
                    stopDownloader();
                    lblError.Text = "Last Error: " + ex.Message;
                }
                else
                {

                    var response = (HttpWebResponse)webex.Response;
                    if (webex.Status == WebExceptionStatus.Timeout)
                    {

                    }
                    else if (response != null)
                    {
                        stopDownloader();
                        var status = response.StatusCode;
                        if (status == (HttpStatusCode)403)
                            if ((dorg.Info == null || !dorg.LastInfo.Equals(dorg.GetCurrentInformations()))
                            && !RequestAvailable && !dorg.FlagStop)
                            {
                                RequestAvailable = true;
                                var yesno = MessageBox.Show("Remote file properties seems to be changed. Do you want to renew url and auth data?", "Url expired", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (yesno == System.Windows.Forms.DialogResult.Yes)
                                    RequestNewUrl();
                                else
                                {
                                    RequestAvailable = false;
                                    NewUrlRequested = false;
                                    this.Close();
                                }
                            }
                            else
                            {
                                stopDownloader();
                                lblError.Text = "Last Error: " + ex.Message;
                            }
                        else
                        {
                            stopDownloader();
                            lblError.Text = "Last Error: " + ex.Message;
                        }
                    }
                    else
                    {
                        stopDownloader();
                        lblError.Text = "Last Error: " + ex.Message;
                    }
                }
            }
        }


    }
}
