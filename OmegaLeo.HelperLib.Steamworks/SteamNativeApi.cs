using System;
using System.IO;
using System.Runtime.InteropServices;

namespace OmegaLeo.HelperLib.Steamworks
{
    internal static class SteamNativeApi
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamApiInitDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SteamApiVoidDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SteamFriendsAccessorDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SteamUserStatsAccessorDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SteamFriendsGetPersonaNameDelegate(IntPtr steamFriends);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamUserStatsSetAchievementDelegate(IntPtr steamUserStats, [MarshalAs(UnmanagedType.LPStr)] string achievementId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamUserStatsStoreStatsDelegate(IntPtr steamUserStats);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamUserStatsGetAchievementDelegate(IntPtr steamUserStats, [MarshalAs(UnmanagedType.LPStr)] string achievementId, [MarshalAs(UnmanagedType.I1)] out bool achieved);

        private static readonly object SyncRoot = new object();

        private static IntPtr _libraryHandle;
        private static bool _libraryLoaded;
        private static bool _initialized;

        private static SteamApiInitDelegate _steamApiInit;
        private static SteamApiVoidDelegate _steamApiShutdown;
        private static SteamApiVoidDelegate _steamApiRunCallbacks;
        private static SteamFriendsAccessorDelegate _steamFriendsAccessor;
        private static SteamUserStatsAccessorDelegate _steamUserStatsAccessor;
        private static SteamFriendsGetPersonaNameDelegate _steamFriendsGetPersonaName;
        private static SteamUserStatsSetAchievementDelegate _steamUserStatsSetAchievement;
        private static SteamUserStatsStoreStatsDelegate _steamUserStatsStoreStats;
        private static SteamUserStatsGetAchievementDelegate _steamUserStatsGetAchievement;

        private static IntPtr _steamFriends;
        private static IntPtr _steamUserStats;

        public static bool IsAvailable => EnsureLoaded();

        public static bool Initialize(uint? appId = null)
        {
            lock (SyncRoot)
            {
                if (appId.HasValue && appId.Value > 0)
                {
                    ConfigureAppId(appId.Value);
                }

                if (_initialized)
                {
                    return true;
                }

                if (!EnsureLoaded())
                {
                    return false;
                }

                if (_steamApiInit == null)
                {
                    return false;
                }

                _initialized = _steamApiInit();
                if (_initialized)
                {
                    CacheInterfaces();
                }

                return _initialized;
            }
        }

        public static void ConfigureAppId(uint appId)
        {
            if (appId == 0)
            {
                return;
            }

            lock (SyncRoot)
            {
                ApplyAppIdEnvironment(appId);
                TryWriteSteamAppIdFile(appId);
            }
        }

        public static void Shutdown()
        {
            lock (SyncRoot)
            {
                if (!_initialized)
                {
                    return;
                }

                _steamApiShutdown?.Invoke();
                _initialized = false;
                _steamFriends = IntPtr.Zero;
                _steamUserStats = IntPtr.Zero;
            }
        }

        public static void RunCallbacks()
        {
            if (!_initialized)
            {
                return;
            }

            _steamApiRunCallbacks?.Invoke();
        }

        public static string GetPersonaName()
        {
            if (!EnsureReady())
            {
                return string.Empty;
            }

            var namePtr = _steamFriendsGetPersonaName?.Invoke(_steamFriends) ?? IntPtr.Zero;
            return namePtr == IntPtr.Zero ? string.Empty : Marshal.PtrToStringAnsi(namePtr) ?? string.Empty;
        }

        public static bool SetAchievement(string achievementId)
        {
            if (!EnsureReady())
            {
                return false;
            }

            return _steamUserStatsSetAchievement?.Invoke(_steamUserStats, achievementId) ?? false;
        }

        public static bool StoreStats()
        {
            if (!EnsureReady())
            {
                return false;
            }

            return _steamUserStatsStoreStats?.Invoke(_steamUserStats) ?? false;
        }

        public static bool GetAchievement(string achievementId, out bool achieved)
        {
            achieved = false;

            if (!EnsureReady())
            {
                return false;
            }

            return _steamUserStatsGetAchievement?.Invoke(_steamUserStats, achievementId, out achieved) ?? false;
        }

        private static bool EnsureReady()
        {
            if (!EnsureLoaded())
            {
                return false;
            }

            if (!_initialized)
            {
                return false;
            }

            if (_steamFriends == IntPtr.Zero || _steamUserStats == IntPtr.Zero)
            {
                CacheInterfaces();
            }

            return _steamFriends != IntPtr.Zero && _steamUserStats != IntPtr.Zero;
        }

        private static bool EnsureLoaded()
        {
            lock (SyncRoot)
            {
                if (_libraryLoaded)
                {
                    return true;
                }

                foreach (var candidate in GetLibraryCandidates())
                {
                    try
                    {
                        _libraryHandle = LoadUnmanagedLibrary(candidate);
                        _libraryLoaded = _libraryHandle != IntPtr.Zero;
                        if (_libraryLoaded)
                        {
                            BindDelegates();
                            return true;
                        }
                    }
                    catch
                    {
                        // Try the next candidate.
                    }
                }

                return false;
            }
        }

        private static void BindDelegates()
        {
            _steamApiInit =
                Bind<SteamApiInitDelegate>("SteamAPI_Init")
                ?? Bind<SteamApiInitDelegate>("SteamAPI_InitSafe");
            _steamApiShutdown = Bind<SteamApiVoidDelegate>("SteamAPI_Shutdown");
            _steamApiRunCallbacks = Bind<SteamApiVoidDelegate>("SteamAPI_RunCallbacks");
            _steamFriendsAccessor = BindInterfaceAccessor<SteamFriendsAccessorDelegate>("SteamFriends", "ISteamFriends");
            _steamUserStatsAccessor = BindInterfaceAccessor<SteamUserStatsAccessorDelegate>("SteamUserStats", "ISteamUserStats");
            _steamFriendsGetPersonaName = Bind<SteamFriendsGetPersonaNameDelegate>("SteamAPI_ISteamFriends_GetPersonaName");
            _steamUserStatsSetAchievement = Bind<SteamUserStatsSetAchievementDelegate>("SteamAPI_ISteamUserStats_SetAchievement");
            _steamUserStatsStoreStats = Bind<SteamUserStatsStoreStatsDelegate>("SteamAPI_ISteamUserStats_StoreStats");
            _steamUserStatsGetAchievement = Bind<SteamUserStatsGetAchievementDelegate>("SteamAPI_ISteamUserStats_GetAchievement");
        }

        private static T Bind<T>(string exportName) where T : class
        {
            return TryGetExport(_libraryHandle, exportName, out var export)
                ? Marshal.GetDelegateForFunctionPointer(export, typeof(T)) as T
                : null;
        }

        private static T BindInterfaceAccessor<T>(string friendlyName, string interfaceName) where T : class
        {
            for (var version = 1; version <= 64; version++)
            {
                var exportName = $"SteamAPI_{friendlyName}_v{version:000}";
                if (TryGetExport(_libraryHandle, exportName, out var export) ||
                    TryGetExport(_libraryHandle, $"SteamAPI_{interfaceName}_v{version:000}", out export))
                {
                    return Marshal.GetDelegateForFunctionPointer(export, typeof(T)) as T;
                }
            }

            return null;
        }

        private static void CacheInterfaces()
        {
            _steamFriends = _steamFriendsAccessor?.Invoke() ?? IntPtr.Zero;
            _steamUserStats = _steamUserStatsAccessor?.Invoke() ?? IntPtr.Zero;
        }

        private static string[] GetLibraryCandidates()
        {
            var libraryName = GetLibraryFileName();
            var runtimeIdentifier = GetRuntimeIdentifier();

            return new[]
            {
                Path.Combine(AppContext.BaseDirectory, "runtimes", runtimeIdentifier, "native", libraryName),
                Path.Combine(AppContext.BaseDirectory, libraryName),
                libraryName
            };
        }

        private static string GetRuntimeIdentifier()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Environment.Is64BitProcess ? "win-x64" : "win-x86";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return Environment.Is64BitProcess ? "osx-x64" : "osx-x86";
            }

            return Environment.Is64BitProcess ? "linux-x64" : "linux-x86";
        }

        private static string GetLibraryFileName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Environment.Is64BitProcess ? "steam_api64.dll" : "steam_api.dll";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "libsteam_api.dylib";
            }

            return "libsteam_api.so";
        }

        private static void ApplyAppIdEnvironment(uint appId)
        {
            var appIdValue = appId.ToString();
            Environment.SetEnvironmentVariable("SteamAppId", appIdValue);
            Environment.SetEnvironmentVariable("SteamGameId", appIdValue);
        }

        private static void TryWriteSteamAppIdFile(uint appId)
        {
            var appIdValue = appId.ToString();
            var candidatePaths = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "steam_appid.txt"),
                Path.Combine(Directory.GetCurrentDirectory(), "steam_appid.txt")
            };

            foreach (var path in candidatePaths)
            {
                try
                {
                    File.WriteAllText(path, appIdValue);
                }
                catch
                {
                    // Ignore write failures and continue with other candidates.
                }
            }
        }

        private static IntPtr LoadUnmanagedLibrary(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return LoadLibraryWindows(path);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return DlopenMac(path, RtldNow);
            }

            return DlopenLinux(path, RtldNow);
        }

        private static bool TryGetExport(IntPtr libraryHandle, string exportName, out IntPtr export)
        {
            if (libraryHandle == IntPtr.Zero)
            {
                export = IntPtr.Zero;
                return false;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                export = GetProcAddressWindows(libraryHandle, exportName);
                return export != IntPtr.Zero;
            }

            export = RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                ? DlsymMac(libraryHandle, exportName)
                : DlsymLinux(libraryHandle, exportName);
            return export != IntPtr.Zero;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "LoadLibraryW")]
        private static extern IntPtr LoadLibraryWindows(string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, EntryPoint = "GetProcAddress")]
        private static extern IntPtr GetProcAddressWindows(IntPtr hModule, string procName);

        [DllImport("libdl.so.2", EntryPoint = "dlopen")]
        private static extern IntPtr DlopenLinux(string fileName, int flags);

        [DllImport("libdl.so.2", EntryPoint = "dlsym")]
        private static extern IntPtr DlsymLinux(IntPtr handle, string symbol);

        [DllImport("libSystem.B.dylib", EntryPoint = "dlopen")]
        private static extern IntPtr DlopenMac(string fileName, int flags);

        [DllImport("libSystem.B.dylib", EntryPoint = "dlsym")]
        private static extern IntPtr DlsymMac(IntPtr handle, string symbol);

        private const int RtldNow = 2;

    }
}

