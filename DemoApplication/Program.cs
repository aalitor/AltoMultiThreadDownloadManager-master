/*
 * Created by SharpDevelop.
 * User: kafeinaltor
 * Date: 21.08.2020
 * Time: 22:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using AltoMultiThreadDownloadManager.NativeMessages;


using Newtonsoft.Json;
using AltoMultiThreadDownloadManager;

namespace DemoApplication
{
    /// <summary>
    /// Class with program entry point.
    /// </summary>
    internal sealed class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                MSG = JsonConvert.DeserializeObject<DownloadMessage>(Clipboard.GetText());
                //MSG = Receiver.ReadDownloadMessage();
                if (MSG == null)
                {
                    var parameter = Environment.CommandLine;
                    var jsons = parameter.Split(new string[] { ".exe\" " }, StringSplitOptions.None);
                    if (jsons.Length > 1)
                    {
                        var json = jsons[1];
                        MTDO = !string.IsNullOrEmpty(json) ?
                            JsonConvert.DeserializeObject<MultiThreadDownloadOrganizer>(json) : null;
                    }
                }
                
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new DownloadHandlerForm());
            }

        }
        public static MultiThreadDownloadOrganizer MTDO = null;
        public static DownloadMessage MSG = null;
    }
}
