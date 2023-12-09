using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Diagnostics;

namespace Aottg2Launcher
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetProgress(0);
            if (!Updater.CheckLauncher())
                ThrowError("New launcher version available at aottgrc.com.", true);
            Reset();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (!Settings.Installed)
                Install();
        }

        public void Install()
        {
            InstallWindow install = new InstallWindow();
            install.Owner = this;
            install.ShowDialog();
            if (InstallWindow.InstallCancelled)
                return;
            if (InstallWindow.InstallSuccess)
                Reset();
            else
                ThrowError("Error while installing game. Make sure no game files are open and the install directory is valid.");
        }

        public void Reset()
        {
            SetProgress(0);
            Settings.Init();
            StatusLabel.Content = string.Empty;
            LaunchButton.Content = "Launch";
            if (Settings.Installed)
            {
                if (!Updater.FetchGameVersion())
                {
                    StatusLabel.Content = "Failed to connect to update server.";
                    LaunchButton.Content = "Play Offline";
                }
                else if (!Updater.CheckGameVersion())
                {
                    StatusLabel.Content = "Patching game files...";
                    if (Updater.UninstallGame() && Updater.CreateInstallFolder())
                        Updater.DownloadGame(this);
                    else
                        ThrowUpdateError();
                }
            }
            else
                LaunchButton.Content = "Install";
        }

        public void OnDownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            SetProgress(e.ProgressPercentage);
        }

        public void OnDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
                ThrowUpdateError();
            else
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    if (!Updater.ExtractGame())
                        ThrowUpdateError();
                    SetProgress(100);
                    StatusLabel.Content = "Game is up to date.";
                }));
            }
        }

        private void LaunchButtonClick(object sender, RoutedEventArgs e)
        {
            if (Settings.Installed)
            {
                try
                {
                    Process.Start(Settings.InstallPath + "/" + Settings.StartingFile);
                    Close();
                }
                catch
                {
                    ThrowError("Error while launching game. Try uninstalling and reinstalling at the top right.");
                }
            }
            else
            {
                Install();
            }
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinButtonClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void SettingsButtonClick(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            settings.Owner = this;
            settings.ShowDialog();
            if (SettingsWindow.UninstallCancelled)
                return;
            if (SettingsWindow.UninstallSuccess)
                Reset();
            else
                ThrowError("Error while uninstalling game. Make sure no game files are open.");
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ThrowError(string msg, bool exit = false)
        {
            ErrorWindow error = new ErrorWindow(msg);
            error.Owner = this;
            error.ShowDialog();
            if (exit)
                Environment.Exit(1);
            else
                Reset();
        }

        private void ThrowUpdateError()
        {
            ThrowError("Error while updating game. Make sure no game files are open and try again.");
        }

        private void SetProgress(int progress)
        {
            DownloadProgressBar.Value = progress / 100;
            ProgressLabel.Content = progress.ToString() + "%";
        }
    }
}
