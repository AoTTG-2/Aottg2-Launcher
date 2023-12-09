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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace Aottg2Launcher
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class InstallWindow : Window
    {
        public static bool InstallSuccess;
        public static bool InstallCancelled;

        public InstallWindow()
        {
            InitializeComponent();
            if (Settings.Platform == "Windows32")
            {
                Windows32Radio.IsChecked = true;
                Windows64Radio.IsChecked = false;
            }
            else
            {
                Windows64Radio.IsChecked = true;
                Windows32Radio.IsChecked = false;
            }
            DirectoryText.IsReadOnly = true;
            DirectoryText.Text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).TrimEnd(new char[] { '\\', '/' }) + "\\" + "Aottg2";
            InstallSuccess = false;
            InstallCancelled = false;
        }

        private void InstallButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Settings.InstallPath = DirectoryText.Text;
                if (Windows32Radio.IsChecked.HasValue && Windows32Radio.IsChecked.Value)
                    Settings.Platform = "Windows32";
                else
                    Settings.Platform = "Windows64";
                if (Updater.UninstallGame() && Updater.CreateInstallFolder())
                {
                    Settings.Save();
                    InstallSuccess = true;
                }
                else
                    InstallSuccess = false;
            }
            catch
            {
                InstallSuccess = false;
            }
            Close();
        }

        private void ChangeButtonClick(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (dialog.SelectedPath != string.Empty)
                    DirectoryText.Text = dialog.SelectedPath.TrimEnd(new char[] { '\\', '/' }) + "\\Aottg2";
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            InstallCancelled = true;
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
