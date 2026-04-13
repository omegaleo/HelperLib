using System;

namespace OmegaLeo.HelperLib.Steamworks
{
    public class SteamManager
    {
        private static SteamManager _instance;
        private static uint? _configuredAppId;
        public static SteamManager Instance => _instance ??= new SteamManager();

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

        public bool IsSteamworksInitialized { get; private set; }

        private SteamManager()
        {
            try
            {
                // Attempt to initialize Steamworks
                if (SteamNativeApi.Initialize(_configuredAppId))
                {
                    IsSteamworksInitialized = true;
                    Console.WriteLine("Steamworks Initialized Successfully!");
                }
                else
                {
                    Console.WriteLine("Steamworks Initialization Failed. Is Steam running?");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An exception occurred during Steamworks initialization: {e.Message}");
            }
        }

        // This must be called from your game's main loop (e.g., Update in Unity/Godot)
        public void Update()
        {
            if (IsSteamworksInitialized)
            {
                SteamNativeApi.RunCallbacks();
            }
        }

        public void Shutdown()
        {
            if (IsSteamworksInitialized)
            {
                SteamNativeApi.Shutdown();
                IsSteamworksInitialized = false;
            }
        }

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
