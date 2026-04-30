using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using OmegaLeo.HelperLib.Shared.Attributes;

namespace OmegaLeo.HelperLib.Steamworks
{
    [Documentation(nameof(SteamManager), "Manages Steamworks initialization, callback polling, and shutdown for applications using the Steam API.", null,
        @"```csharp
SteamManager.ConfigureLogger(logger);
SteamManager.ConfigureAppId(480);
var steam = SteamManager.Instance;
steam.Update();
```")]
    public class SteamManager
    {
        private static SteamManager _instance;
        private static uint? _configuredAppId;
        private static ILogger _logger = NullLogger.Instance;
        private static readonly SteamUgcManager UgcManager = new SteamUgcManager();
        private static readonly SteamRemotePlayManager RemotePlayManager = new SteamRemotePlayManager();
        private static readonly SteamControllerManager ControllerManager = new SteamControllerManager();
        private static readonly SteamFriendsManager FriendsManager = new SteamFriendsManager();
        private static readonly SteamGameCoordinatorManager GameCoordinatorManager = new SteamGameCoordinatorManager();
        private static readonly SteamGameServerManager GameServerManager = new SteamGameServerManager();
        private static readonly SteamGameServerStatsManager GameServerStatsManager = new SteamGameServerStatsManager();
        private static readonly SteamHtmlSurfaceManager HtmlSurfaceManager = new SteamHtmlSurfaceManager();
        private static readonly SteamInputManager InputManager = new SteamInputManager();
        private static readonly SteamInventoryManager InventoryManager = new SteamInventoryManager();
        private static readonly SteamMatchmakingManager MatchmakingManager = new SteamMatchmakingManager();
        private static readonly SteamMatchmakingServersManager MatchmakingServersManager = new SteamMatchmakingServersManager();
        private static readonly SteamMusicManager MusicManager = new SteamMusicManager();
        private static readonly SteamPartiesManager PartiesManager = new SteamPartiesManager();
        private static readonly SteamRemoteStorageManager RemoteStorageManager = new SteamRemoteStorageManager();
        private static readonly SteamScreenshotsManager ScreenshotsManager = new SteamScreenshotsManager();
        private static readonly SteamUserStatsManager UserStatsManager = new SteamUserStatsManager();
        private static readonly SteamUtilsManager UtilsManager = new SteamUtilsManager();
        private static readonly SteamVideoManager VideoManager = new SteamVideoManager();
        [Documentation(nameof(Instance), "Singleton access to the Steam manager.", null, null)]
        public static SteamManager Instance => _instance ??= new SteamManager();

        internal static ILogger Logger => _logger;

        [Documentation(nameof(ConfigureLogger), "Configures the logger used by Steamworks helper classes.", new[] { "logger: ILogger instance to receive Steamworks logs." }, null)]
        public static void ConfigureLogger(ILogger logger)
        {
            _logger = logger ?? NullLogger.Instance;
        }

        [Documentation(nameof(ConfigureAppId), "Configures the Steam AppId used during initialization. Must be called before accessing the singleton instance.", new[] { "appId: Steam application identifier." }, null)]
        public static bool ConfigureAppId(uint appId)
        {
            if (appId == 0)
            {
                return false;
            }

            // AppId must be configured before the singleton is created.
            if (_instance != null)
            {
                return false;
            }

            _configuredAppId = appId;
            SteamNativeApi.ConfigureAppId(appId);
            return true;
        }

        [Documentation(nameof(IsSteamworksInitialized), "Indicates whether Steamworks is initialized and ready for API calls.", null, null)]
        public bool IsSteamworksInitialized { get; private set; }

        [Documentation(nameof(Ugc), "Provides access to Steam Workshop (UGC) helper operations.", null, null)]
        public SteamUgcManager Ugc => UgcManager;

        [Documentation(nameof(RemotePlay), "Provides access to Steam Remote Play helper operations.", null, null)]
        public SteamRemotePlayManager RemotePlay => RemotePlayManager;

        [Documentation(nameof(Controller), "Provides access to ISteamController bindings.", null, null)]
        public SteamControllerManager Controller => ControllerManager;

        [Documentation(nameof(Friends), "Provides access to ISteamFriends bindings.", null, null)]
        public SteamFriendsManager Friends => FriendsManager;

        [Documentation(nameof(GameCoordinator), "Provides access to ISteamGameCoordinator bindings.", null, null)]
        public SteamGameCoordinatorManager GameCoordinator => GameCoordinatorManager;

        [Documentation(nameof(GameServer), "Provides access to ISteamGameServer bindings.", null, null)]
        public SteamGameServerManager GameServer => GameServerManager;

        [Documentation(nameof(GameServerStats), "Provides access to ISteamGameServerStats bindings.", null, null)]
        public SteamGameServerStatsManager GameServerStats => GameServerStatsManager;

        [Documentation(nameof(HtmlSurface), "Provides access to ISteamHTMLSurface bindings.", null, null)]
        public SteamHtmlSurfaceManager HtmlSurface => HtmlSurfaceManager;

        [Documentation(nameof(Input), "Provides access to ISteamInput bindings.", null, null)]
        public SteamInputManager Input => InputManager;

        [Documentation(nameof(Inventory), "Provides access to ISteamInventory bindings.", null, null)]
        public SteamInventoryManager Inventory => InventoryManager;

        [Documentation(nameof(Matchmaking), "Provides access to ISteamMatchmaking bindings.", null, null)]
        public SteamMatchmakingManager Matchmaking => MatchmakingManager;

        [Documentation(nameof(MatchmakingServers), "Provides access to ISteamMatchmakingServers bindings.", null, null)]
        public SteamMatchmakingServersManager MatchmakingServers => MatchmakingServersManager;

        [Documentation(nameof(Music), "Provides access to ISteamMusic bindings.", null, null)]
        public SteamMusicManager Music => MusicManager;

        [Documentation(nameof(Parties), "Provides access to ISteamParties bindings.", null, null)]
        public SteamPartiesManager Parties => PartiesManager;

        [Documentation(nameof(RemoteStorage), "Provides access to ISteamRemoteStorage bindings.", null, null)]
        public SteamRemoteStorageManager RemoteStorage => RemoteStorageManager;

        [Documentation(nameof(Screenshots), "Provides access to ISteamScreenshots bindings.", null, null)]
        public SteamScreenshotsManager Screenshots => ScreenshotsManager;

        [Documentation(nameof(UserStats), "Provides access to ISteamUserStats bindings.", null, null)]
        public SteamUserStatsManager UserStats => UserStatsManager;

        [Documentation(nameof(Utils), "Provides access to ISteamUtils bindings.", null, null)]
        public SteamUtilsManager Utils => UtilsManager;

        [Documentation(nameof(Video), "Provides access to ISteamVideo bindings.", null, null)]
        public SteamVideoManager Video => VideoManager;

        private SteamManager()
        {
            try
            {
                // Attempt to initialize Steamworks
                if (SteamNativeApi.Initialize(_configuredAppId))
                {
                    IsSteamworksInitialized = true;
                    _logger.LogInformation("Steamworks initialized successfully.");
                }
                else
                {
                    _logger.LogWarning("Steamworks initialization failed. Ensure Steam is running and AppId context is valid.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An exception occurred during Steamworks initialization.");
            }
        }

        // This must be called from your game's main loop (e.g., Update in Unity/Godot)
        [Documentation(nameof(Update), "Executes Steamworks callbacks. Call every frame from your game loop.", null, null)]
        public void Update()
        {
            if (IsSteamworksInitialized)
            {
                SteamNativeApi.RunCallbacks();
            }
        }

        [Documentation(nameof(Shutdown), "Shuts down Steamworks and clears the initialized state.", null, null)]
        public void Shutdown()
        {
            if (IsSteamworksInitialized)
            {
                SteamNativeApi.Shutdown();
                IsSteamworksInitialized = false;
            }
        }

        [Documentation(nameof(GetSteamName), "Gets the current Steam persona name. Returns \"Player\" when Steamworks is unavailable.", null, null)]
        public string GetSteamName()
        {
            if (IsSteamworksInitialized)
            {
                return SteamNativeApi.GetPersonaName();
            }
            return "Player";
        }
    }
}
