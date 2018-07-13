using System;

namespace Warframe_Fixer.Model
{
    public class SteamPatcher : IPatcher
    {
        public string SteamId { get; set; } = "";

        public string SteamId64 { get; private set; } = "";

        private SteamFileManager _fileManager = new SteamFileManager();

        public bool Patch()
        {
            return _fileManager.FixFile(SteamId);
        }

        public bool FetchSteamId64()
        {
            // Request to steam API to get id 64
            throw new NotImplementedException();
        }
    }
}