// See https://aka.ms/new-console-template for more information

// /media/omegaleo/Development/Library Dev/HelperLib

using Microsoft.Extensions.Logging;
using OmegaLeo.HelperLib.Steamworks;

public class Program
{
    public static void Main()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .SetMinimumLevel(LogLevel.Information)
                .AddSimpleConsole(options =>
                {
                    options.SingleLine = true;
                    options.TimestampFormat = "HH:mm:ss ";
                });
        });

        var steamLogger = loggerFactory.CreateLogger<SteamManager>();
        var achievementLogger = loggerFactory.CreateLogger<AchievementManager>();

        SteamManager.ConfigureLogger(steamLogger);
        SteamManager.ConfigureAppId(480); // 480 is the AppId for Spacewar, a test app provided by Valve

        var steam = SteamManager.Instance;
        if (steam.IsSteamworksInitialized)
        {
            steamLogger.LogInformation("Welcome {SteamName}", steam.GetSteamName());
        }

        // Call this in your game loop (Unity Update, Godot _Process, etc.)
        steam.Update();

        var achievements = new AchievementManager(achievementLogger);
        achievements.UnlockAchievement("NEW_ACHIEVEMENT_0_4");

        steam.Shutdown();
    }
}