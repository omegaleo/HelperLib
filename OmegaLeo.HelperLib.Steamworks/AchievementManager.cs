using Microsoft.Extensions.Logging;
using OmegaLeo.HelperLib.Shared.Attributes;

namespace OmegaLeo.HelperLib.Steamworks
{
    [Documentation(nameof(AchievementManager), "Provides helper methods for unlocking and querying Steam achievements.", null,
        @"```csharp
var achievements = new AchievementManager(logger);
achievements.UnlockAchievement(""ACH_WIN_ONE_GAME"");
```")]
    public class AchievementManager
    {
        private readonly ILogger _logger;

        public AchievementManager(ILogger logger = null)
        {
            _logger = logger ?? SteamManager.Logger;
        }

        [Documentation(nameof(UnlockAchievement), "Attempts to unlock the provided achievement and store user stats.", new[] { "achievementId: Steam achievement identifier to unlock." }, null)]
        public void UnlockAchievement(string achievementId)
        {
            if (SteamManager.Instance.IsSteamworksInitialized)
            {
                SteamNativeApi.SetAchievement(achievementId);
                SteamNativeApi.StoreStats(); // Don't forget to store the stats
                _logger.LogInformation("Attempting to unlock achievement: {AchievementId}", achievementId);
            }
        }

        [Documentation(nameof(IsAchievementUnlocked), "Checks whether a Steam achievement is currently unlocked.", new[] { "achievementId: Steam achievement identifier to check.", "unlocked: True when the achievement is unlocked." }, null)]
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
