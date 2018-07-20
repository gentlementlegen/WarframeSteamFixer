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
        private string _filePath = "";
        private readonly SteamFileModel _fileModel = new SteamFileModel();
        private string _steamExePath;

        public SteamFileManager()
        {
            _filePath = GetSteamInstallPath(Registry.LocalMachine.OpenSubKey(@"Software\WOW6432Node\Valve"), "Steam") + FileName;
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
            Logger.Log("Patching Steam file at [" + _filePath + "]");
            System.IO.File.WriteAllText(_filePath, text);
            StartSteamInstance();
        }

        /// <summary>
        /// Closes any running Steam instance.
        /// </summary>
        private void ShutdownSteamInstance()
        {
            var processes = Process.GetProcessesByName("Steam");
            Logger.Log("Shutting down any Steam running instances");
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
            Logger.Log("Trying to restart Steam client");
            if (!string.IsNullOrEmpty(_steamExePath))
                Process.Start(_steamExePath);
        }
    }
}