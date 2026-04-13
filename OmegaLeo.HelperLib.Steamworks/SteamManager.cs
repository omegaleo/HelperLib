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
