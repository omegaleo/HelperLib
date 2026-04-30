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

        steamLogger.LogInformation("--- Steam wrapper smoke tests ---");

        // Utils (read-only)
        steamLogger.LogInformation("Utils available: {Available}", steam.Utils.IsAvailable);
        if (steam.Utils.IsAvailable)
        {
            steamLogger.LogInformation("AppId: {AppId}, Country: {Country}, Overlay: {Overlay}",
                steam.Utils.GetAppId(),
                steam.Utils.GetIpCountry(),
                steam.Utils.IsOverlayEnabled());
        }

        // Friends (read-only)
        steamLogger.LogInformation("Friends available: {Available}", steam.Friends.IsAvailable);
        if (steam.Friends.IsAvailable)
        {
            // 65535 tests all friend relationship flags for quick smoke coverage.
            var friendCount = steam.Friends.GetFriendCount(65535);
            steamLogger.LogInformation("Friend count (all flags): {FriendCount}", friendCount);
        }

        // Remote storage (read-only)
        steamLogger.LogInformation("RemoteStorage available: {Available}", steam.RemoteStorage.IsAvailable);
        if (steam.RemoteStorage.IsAvailable)
        {
            steamLogger.LogInformation("Cloud enabled: {Enabled}, file count: {Count}",
                steam.RemoteStorage.IsCloudEnabledForApp(),
                steam.RemoteStorage.GetFileCount());
        }

        // User stats (safe read/request)
        steamLogger.LogInformation("UserStats available: {Available}", steam.UserStats.IsAvailable);
        if (steam.UserStats.IsAvailable)
        {
            var globalStatsCall = steam.UserStats.RequestGlobalStats(7);
            steamLogger.LogInformation("RequestGlobalStats call handle: {Handle}", globalStatsCall);
        }

        // UGC (safe query/subscription read)
        steamLogger.LogInformation("UGC subscribed count: {Count}", steam.Ugc.GetNumSubscribedItems());
        var ugcQuery = steam.Ugc.CreateQueryAllUgcRequest(queryType: 0, matchingType: 0, creatorAppId: 480, consumerAppId: 480, page: 1);
        if (ugcQuery != 0)
        {
            var ugcCall = steam.Ugc.SendQueryUgcRequest(ugcQuery);
            steamLogger.LogInformation("UGC query handle: {QueryHandle}, call handle: {CallHandle}", ugcQuery, ugcCall);
            steam.Ugc.ReleaseQueryUgcRequest(ugcQuery);
        }
        else
        {
            steamLogger.LogWarning("UGC query handle returned 0 (unsupported on this runtime or invalid params).");
        }

        // Matchmaking (safe request)
        steamLogger.LogInformation("Matchmaking available: {Available}", steam.Matchmaking.IsAvailable);
        if (steam.Matchmaking.IsAvailable)
        {
            steam.Matchmaking.AddRequestLobbyListResultCountFilter(10);
            var lobbyListCall = steam.Matchmaking.RequestLobbyList();
            steamLogger.LogInformation("RequestLobbyList call handle: {Handle}", lobbyListCall);
        }

        // Remote Play
        steamLogger.LogInformation("RemotePlay sessions: {SessionCount}", steam.RemotePlay.GetSessionCount());

        // Other read-only status checks
        steamLogger.LogInformation("Screenshots hooked: {Hooked}", steam.Screenshots.IsScreenshotsHooked());
        steamLogger.LogInformation("Music enabled: {Enabled}, playing: {Playing}, volume: {Volume}",
            steam.Music.IsEnabled(), steam.Music.IsPlaying(), steam.Music.GetVolume());
        if (steam.Video.IsBroadcasting(out var viewers))
        {
            steamLogger.LogInformation("Video broadcasting viewers: {Viewers}", viewers);
        }
        else
        {
            steamLogger.LogInformation("Video broadcasting is not active.");
        }

        steamLogger.LogInformation("--- End smoke tests ---");

        // Call this in your game loop (Unity Update, Godot _Process, etc.)
        steam.Update();

        var achievements = new AchievementManager(achievementLogger);
        achievements.UnlockAchievement("NEW_ACHIEVEMENT_0_4");

        steam.Shutdown();
    }
}