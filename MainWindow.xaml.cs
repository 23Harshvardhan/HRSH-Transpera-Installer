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
        WebClient client = new WebClient();

        public MainWindow()
        {
            InitializeComponent();

            // Events to be called while file is being downloaded and once the file is finished downloading.
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.DownloadFileCompleted += Client_DownloadFileCompleted;
        }

        private void btnInstall_Click(object sender, RoutedEventArgs e)
        {
            chkCleanInstall.Visibility = Visibility.Hidden;
            btnInstall.Visibility = Visibility.Hidden;
            progInstall.Visibility = Visibility.Visible;
            lblDownloading.Visibility = Visibility.Visible;
            lblPercentage.Visibility = Visibility.Visible;

            if (chkCleanInstall.IsChecked == true)
            {
                if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\HRSH-Transpera.exe"))
                {
                    System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\HRSH-Transpera.exe");
                }
                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\HRSH\Transpera"))
                {
                    Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\HRSH\Transpera", true);
                }
            }

            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\");
            }

            Uri uri = new Uri("https://an0maly.blob.core.windows.net/transpera/HRSH-Transpera.exe");

            client.DownloadFileAsync(uri, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HRSH\Transpera\HRSH-Transpera.exe");
        }

        #region ========== PROGESS BAR EVENTS FOR CLIENT ==========

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progInstall.Value = e.ProgressPercentage;
            lblPercentage.Content = e.ProgressPercentage.ToString() + "%";
        }

        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
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

        #endregion
    }
}
