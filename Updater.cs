using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace Aottg2Launcher
{
    class Updater
    {
        public static bool CheckLauncher()
        {
            string serverVersion = DownloadText(Settings.LauncherVersionURL);
            return serverVersion == string.Empty || serverVersion == Settings.LauncherVersion;
        }

        public static bool FetchGameVersion()
        {
            Settings.ServerGameVersion = DownloadText(Settings.GameVersionURL);
            return Settings.ServerGameVersion != string.Empty;
        }

        public static bool CheckGameVersion()
        {
            return Settings.ServerGameVersion == string.Empty || Settings.ServerGameVersion == Settings.LocalGameVersion;
        }

        public static bool CreateInstallFolder()
        {
            try
            {
                Directory.CreateDirectory(Settings.InstallPath);
                File.WriteAllText(GetGameVersionPath(), string.Empty);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool UninstallGame()
        {
            try
            {
                if (Directory.Exists(Settings.InstallPath))
                    Directory.Delete(Settings.InstallPath, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void DownloadGame(MainWindow main)
        {
            Directory.CreateDirectory(Settings.InstallPath);
            WebClient client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(main.OnDownloadProgress);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(main.OnDownloadCompleted);
            client.DownloadFileAsync(new Uri(Settings.GameZipURL), GetTempPath());
        }

        public static bool ExtractGame()
        {
            try
            {
                ZipFile.ExtractToDirectory(GetTempPath(), Settings.InstallPath);
                File.Delete(GetTempPath());
                File.WriteAllText(GetGameVersionPath(), Settings.ServerGameVersion);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string DownloadText(string url)
        {
            try
            {
                using (WebClient web = new WebClient())
                {
                    return web.DownloadString(url);
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetTempPath()
        {
            return Settings.InstallPath + "/" + Settings.TempFileName;
        }

        public static string GetGameVersionPath()
        {
            return Settings.InstallPath + "/" + Settings.GameVersionName;
        }
    }
}