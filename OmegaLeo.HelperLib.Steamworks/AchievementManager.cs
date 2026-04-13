using System;

namespace OmegaLeo.HelperLib.Steamworks
{
    public class AchievementManager
    {
        public void UnlockAchievement(string achievementId)
        {
            if (SteamManager.Instance.IsSteamworksInitialized)
            {
                SteamNativeApi.SetAchievement(achievementId);
                SteamNativeApi.StoreStats(); // Don't forget to store the stats
                Console.WriteLine($"Attempting to unlock achievement: {achievementId}");
            }
        }

        public bool IsAchievementUnlocked(string achievementId, out bool unlocked)
        {
            unlocked = false;
            if (SteamManager.Instance.IsSteamworksInitialized)
            {
                return SteamNativeApi.GetAchievement(achievementId, out unlocked);
            }
            return false;
        }
    }
}
