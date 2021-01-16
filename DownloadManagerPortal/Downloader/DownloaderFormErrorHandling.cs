using AltoMultiThreadDownloadManager.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public void handleError(Exception ex)
        {
            WebException webex = null;
            if (ex is WebException)
                webex = (WebException)ex;

            if (ex is RemoteFilePropertiesChangedException)
            {
                
                if (!NewUrlRequested && !dorg.FlagStop)
                {
                    stopDownloader();
                    NewUrlRequested = true;

                    var rex = ex as RemoteFilePropertiesChangedException;
                    Clipboard.SetText(JsonConvert.SerializeObject(rex.OriginalInfo));
                    MessageBox.Show("handlealready");
                    Clipboard.SetText(JsonConvert.SerializeObject(rex.CurrentInfo));
                    MessageBox.Show(rex.OriginalInfo.Equals(rex.CurrentInfo).ToString());
                    MessageBox.Show("Remote file properties seems to be changed. Refresh the url");
                    RequestNewUrl();
                }
            }
            else
            {
                if (!(ex is WebException))
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
                            if ((dorg.Info == null || !dorg.LastInfo.Equals(dorg.getCurrentInformations()))
                            && !NewUrlRequested && !dorg.FlagStop)
                            {
                                NewUrlRequested = true;
                                MessageBox.Show("Remote file properties seems to be changed. Refresh the url");
                                RequestNewUrl();
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
