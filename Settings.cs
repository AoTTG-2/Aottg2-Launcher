using System.IO;

namespace Aottg2Launcher
{
    class Settings
    {
        public static readonly string StartingFile = "Aottg2.exe";
        public static readonly string ServerURL = "https://aottg2.com/Patch/Windows";
        public static readonly string LauncherVersionURL = ServerURL + "/LauncherVersion";
        public static readonly string GameVersionURL = ServerURL + "/GameVersion";
        public static readonly string LauncherVersion = "7.28.2022";
        public static readonly string TempFileName = "Game.zip";
        public static readonly string GameVersionName = "GameVersion";
        public static string GameZipURL = string.Empty;
        public static string Platform = "Windows64";
        public static string InstallPath = string.Empty;
        public static string LocalGameVersion = string.Empty;
        public static string ServerGameVersion = string.Empty;
        public static bool Installed = false;

        public static void Init()
        {
            InstallPath = Properties.Settings.Default.InstallPath;
            Platform = Properties.Settings.Default.Platform;
            if (Platform == string.Empty)
                Platform = "Windows64";
            GameZipURL = ServerURL + "/" + Platform + ".zip";
            LocalGameVersion = string.Empty;
            ServerGameVersion = string.Empty;
            Installed = false;
            if (InstallPath != string.Empty && File.Exists(Updater.GetGameVersionPath()))
            {
                LocalGameVersion = File.ReadAllText(Updater.GetGameVersionPath());
                Installed = true;
            }
        }

        public static void Save()
        {
            Properties.Settings.Default.InstallPath = InstallPath;
            Properties.Settings.Default.Platform = Platform;
            Properties.Settings.Default.Save();
        }
    }
}