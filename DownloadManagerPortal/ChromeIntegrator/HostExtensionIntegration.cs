using Newtonsoft.Json;
using System.IO;

namespace DownloadManagerPortal.ChromeIntegrator
{
    class HostExtensionIntegrator
    {
        public string ExtensionId
        {
            get
            {
                return Properties.Settings.Default.ChromeExtensionId;
            }
        }
        public string[] AllowedOrigins
        {
            get
            {
                return new[] { string.Format("chrome-extension://{0}/", ExtensionId) };
            }
        }
        public string ExePath { get; set; }
        public string HostPath { get; set; }

        public NativeMessageHost GetHost()
        {
            return new NativeMessageHost()
            {
                name = Properties.Settings.Default.ChromeExtensionName,
                description = "Download manager extension helper",
                type = "stdio",
                allowed_origins = AllowedOrigins,
                path = ExePath
            };
        }
        public string GetHostJsonString()
        {
            return JsonConvert.SerializeObject(GetHost(), Formatting.Indented);
        }
        public void CreateHostFile()
        {
            if (File.Exists(HostPath))
                File.Delete(HostPath);

            File.WriteAllText(HostPath, GetHostJsonString());
        }
    }

    
}
