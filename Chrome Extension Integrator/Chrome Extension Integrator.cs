using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Chrome_Extension_Integrator
{
    public partial class Chrome_Extension_Integrator : Form
    {
        public Chrome_Extension_Integrator()
        {
            InitializeComponent();
            btnOpenFile.Click += btnOpenFile_Click;
            btnIntegrate.Click += btnIntegrate_Click;
        }

        void btnIntegrate_Click(object sender, System.EventArgs e)
        {
            if(string.IsNullOrEmpty(txtExtId.Text) || string.IsNullOrEmpty(txtAppExe.Text))
            {
                MessageBox.Show("Please fill the required informations");
                return;
            }
            Integrator.Complete(txtExtId.Text, txtAppExe.Text, txtExtname.Text);
            MessageBox.Show("Integration Completed");
        }

        void btnOpenFile_Click(object sender, System.EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtAppExe.Text = openFileDialog1.FileName;
            }

        }
    }

    class Integrator
    {
        public static void Complete(string extid, string exepath, string extname)
        {
            exepath = Regex.Replace(exepath,  @"\\", "\\");
            //Create native message key in registry
            var regNatMsgPath = @"SOFTWARE\Google\Chrome\NativeMessagingHosts\";
            var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            var natmsgkey = hklm.OpenSubKey(regNatMsgPath, true);
            var key = natmsgkey.CreateSubKey(extname);
            //get exe path

            //Host details for native messaging
            Host extHost = new Host
            {
                name = extname,
                description = "Download manager extension helper",
                type = "stdio",
                allowed_origins = new[] { string.Format("chrome-extension://{0}/", extid) },
                path = exepath
            };

            //Convert host to json to write in host file
            var json = JsonConvert.SerializeObject(extHost, Formatting.Indented);

            //Host path to set as registry key value
            var hostPath = Path.Combine(Path.GetDirectoryName(exepath), extname + ".json");

            File.WriteAllText(hostPath, json);

            key.SetValue("", hostPath);

        }
        struct Host
        {
            public string name { get; set; }
            public string description { get; set; }
            public string type { get; set; }
            public string[] allowed_origins { get; set; }
            public string path { get; set; }
        }
    }
}
