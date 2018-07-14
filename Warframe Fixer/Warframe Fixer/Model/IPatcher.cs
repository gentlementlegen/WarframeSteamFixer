namespace Warframe_Fixer.Model
{
    /// <summary>
    /// Interface for patching files.
    /// </summary>
    public interface IPatcher
    {
        /// <summary>
        /// Starts the patcher.
        /// </summary>
        /// <returns></returns>
        bool Patch();

        /// <summary>
        /// The id of the current user being patched.
        /// </summary>
        string SteamId { get; set; }

        /// <summary>
        /// The 64 version of the Steam Id.
        /// </summary>
        string SteamId64 { get; }
    }
}