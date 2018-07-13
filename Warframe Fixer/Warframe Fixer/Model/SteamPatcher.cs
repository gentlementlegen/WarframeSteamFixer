using System.Net.Http;
using System.Threading.Tasks;

namespace Warframe_Fixer.Model
{
    public class SteamPatcher : IPatcher
    {
        public string SteamId { get; set; } = "ferfer957";

        public string SteamId64 { get; private set; } = "";

        private SteamFileManager _fileManager = new SteamFileManager();
        private readonly HttpClient _client = new HttpClient();

        public bool Patch()
        {
            Task.Run(() => FetchSteamId64()).Wait();
            _fileManager.FixFile(SteamId64);
            return true;
        }

        public async Task<bool> FetchSteamId64()
        {
            // Request to steam API to get id 64
            // this is an ugly way to do it but I don't wanna publish my Steam API key.
            var content = await _client.GetStringAsync($"https://steamidfinder.com/lookup/{ SteamId }/");
            var toFind = "steamID64 <code>";
            var startPos = content.LastIndexOf(toFind) + toFind.Length;
            var endPos = content.IndexOf('<', startPos);
            SteamId64 = content.Substring(startPos, endPos - startPos);
            return !string.IsNullOrEmpty(SteamId64);
        }
    }
}