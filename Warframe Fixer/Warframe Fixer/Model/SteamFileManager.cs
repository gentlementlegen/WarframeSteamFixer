using Microsoft.Win32;

namespace Warframe_Fixer.Model
{
    public partial class SteamFileModel
    {
        public string SteamId64;
    }

    public class SteamFileManager
    {
        private const string FileName = "appmanifest_230410.acf";
        private string FilePath = "";
        private readonly SteamFileModel _fileModel = new SteamFileModel();

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
            //System.IO.File.WriteAllText(FilePath, text);
        }
    }
}