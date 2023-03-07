using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.IO;
using System.Net;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IWshRuntimeLibrary;
using System.Runtime.InteropServices;

namespace HRSH_Transpera_Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnInstall_Click(object sender, RoutedEventArgs e)
        {
            if(chkCleanInstall.IsChecked == true)
            {
                if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\HRSH-Transpera.exe"))
                {
                    System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\HRSH-Transpera.exe");
                }
                if(Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\HRSH\Transpera"))
                {
                    Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\HRSH\Transpera", true);
                }
            }

            WebClient client = new WebClient();

            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\");
            }

            client.DownloadFile("https://an0maly.blob.core.windows.net/transpera/HRSH-Transpera.exe", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\HRSH-Transpera.exe");

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string exePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\HRSH-Transpera.exe";

            dynamic wshShell = Activator.CreateInstance(Type.GetTypeFromProgID("WScript.Shell"));
            dynamic shortcut = wshShell.CreateShortcut(System.IO.Path.Combine(desktopPath, "HRSH Transpera.lnk"));

            shortcut.TargetPath = exePath;
            shortcut.WorkingDirectory = System.IO.Path.GetDirectoryName(exePath);
            shortcut.WindowStyle = 1; // Normal window
            shortcut.Description = "Unified mod manager for CSGO.";
            shortcut.Save();

            Marshal.FinalReleaseComObject(shortcut);
            Marshal.FinalReleaseComObject(wshShell);

            MessageBox.Show("Installation Successful!", "Happy Hacking", MessageBoxButton.OK);
            this.Close();
        }
    }
}
