using Microsoft.Win32;
using System.Diagnostics;

namespace Warframe_Fixer.Model
{
    public partial class SteamFileModel
    {
        public string SteamId64;
    }

    /// <summary>
    /// Manages the file fixing of the Steam app.
    /// </summary>
    public class SteamFileManager
    {
        private const string FileName = "appmanifest_230410.acf";
        private string FilePath = "";
        private readonly SteamFileModel _fileModel = new SteamFileModel();
        private string _steamExePath;

        public SteamFileManager()
        {
            FilePath = GetSteamInstallPath(Registry.LocalMachine.OpenSubKey(@"Software\WOW6432Node\Valve"), "Steam") + FileName;
        }

        private string GetSteamInstallPath(RegistryKey parentKey, string name)
        {
            string[] nameList = parentKey.GetSubKeyNames();
            for (int i = 0; i < nameList.Length; i++)
            {
                RegistryKey regKey = parentKey.OpenSubKey(nameList[i]);
                try
                {
                    return regKey.GetValue("InstallPath").ToString() + @"\steamapps\";
                }
                catch { }
            }
            return "";
        }

        public void FixFile(string steamId)
        {
            _fileModel.SteamId64 = steamId;
            var text = _fileModel.TransformText();
            ShutdownSteamInstance();
            System.IO.File.WriteAllText(FilePath, text);
            StartSteamInstance();
        }

        /// <summary>
        /// Closes any running Steam instance.
        /// </summary>
        private void ShutdownSteamInstance()
        {
            var processes = Process.GetProcessesByName("Steam");
            if (processes.Length > 0)
            {
                foreach (var process in processes)
                {
                    _steamExePath = process.MainModule.FileName;
                    process.Kill();
                    process.WaitForExit();
                    process.Close();
                }
            }
        }

        private void StartSteamInstance()
        {
            if (!string.IsNullOrEmpty(_steamExePath))
                Process.Start(_steamExePath);
        }
    }
}