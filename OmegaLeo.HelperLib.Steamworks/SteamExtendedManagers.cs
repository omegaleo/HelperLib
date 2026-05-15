using OmegaLeo.HelperLib.Shared.Attributes;

namespace OmegaLeo.HelperLib.Steamworks
{
    [Documentation(nameof(SteamControllerManager), "Provides access checks for ISteamController.", null, null)]
    public class SteamControllerManager
    {
        public bool IsAvailable => SteamNativeApi.IsSteamControllerAvailable();
    }

    [Documentation(nameof(SteamFriendsManager), "Provides access checks for ISteamFriends.", null, null)]
    public class SteamFriendsManager
    {
        public bool IsAvailable => SteamNativeApi.IsSteamFriendsAvailable();

        [Documentation(nameof(GetFriendCount), "Gets the friend count filtered by Steam friend flags.", new[] { "friendFlags: Bitmask value matching Steam EFriendFlags." }, null)]
        public int GetFriendCount(int friendFlags)
        {
            return SteamNativeApi.GetFriendCount(friendFlags);
        }

        [Documentation(nameof(GetFriendPersonaName), "Gets the persona name of a friend by SteamID64.", new[] { "steamIdFriend: Friend SteamID64." }, null)]
        public string GetFriendPersonaName(ulong steamIdFriend)
        {
            return SteamNativeApi.GetFriendPersonaName(steamIdFriend);
        }

        [Documentation(nameof(ActivateGameOverlayInviteDialog), "Opens the Steam invite dialog for a lobby.", new[] { "steamIdLobby: Lobby SteamID64." }, null)]
        public void ActivateGameOverlayInviteDialog(ulong steamIdLobby)
        {
            SteamNativeApi.FriendsActivateGameOverlayInviteDialog(steamIdLobby);
        }

        [Documentation(nameof(SetRichPresence), "Sets a rich presence key/value pair for the local user.", new[] { "key: Rich presence key.", "value: Rich presence value." }, null)]
        public bool SetRichPresence(string key, string value)
        {
            return SteamNativeApi.FriendsSetRichPresence(key, value);
        }

        [Documentation(nameof(ClearRichPresence), "Clears all rich presence values for the local user.", null, null)]
        public void ClearRichPresence()
        {
            SteamNativeApi.FriendsClearRichPresence();
        }

        [Documentation(nameof(GetFriendRichPresence), "Gets a rich presence value for a specific friend.", new[] { "steamIdFriend: Friend SteamID64.", "key: Rich presence key." }, null)]
        public string GetFriendRichPresence(ulong steamIdFriend, string key)
        {
            return SteamNativeApi.FriendsGetFriendRichPresence(steamIdFriend, key);
        }

        [Documentation(nameof(InviteUserToGame), "Invites a friend to the current game session using a connect string.", new[] { "steamIdFriend: Friend SteamID64.", "connectString: Game connect string passed through Steam invite." }, null)]
        public bool InviteUserToGame(ulong steamIdFriend, string connectString)
        {
            return SteamNativeApi.FriendsInviteUserToGame(steamIdFriend, connectString);
        }
    }

    [Documentation(nameof(SteamGameCoordinatorManager), "Provides access checks for ISteamGameCoordinator.", null, null)]
    public class SteamGameCoordinatorManager
    {
        public bool IsAvailable => SteamNativeApi.IsSteamGameCoordinatorAvailable();
    }

    [Documentation(nameof(SteamGameServerManager), "Provides access checks for ISteamGameServer.", null, null)]
    public class SteamGameServerManager
    {
        public bool IsAvailable => SteamNativeApi.IsSteamGameServerAvailable();
    }

    [Documentation(nameof(SteamGameServerStatsManager), "Provides access checks for ISteamGameServerStats.", null, null)]
    public class SteamGameServerStatsManager
    {
        public bool IsAvailable => SteamNativeApi.IsSteamGameServerStatsAvailable();
    }

    [Documentation(nameof(SteamHtmlSurfaceManager), "Provides access checks for ISteamHTMLSurface.", null, null)]
    public class SteamHtmlSurfaceManager
    {
        public bool IsAvailable => SteamNativeApi.IsSteamHtmlSurfaceAvailable();
    }

    [Documentation(nameof(SteamInputManager), "Provides access checks for ISteamInput.", null, null)]
    public class SteamInputManager
    {
        public bool IsAvailable => SteamNativeApi.IsSteamInputAvailable();

        [Documentation(nameof(Init), "Initializes Steam Input for the application.", new[] { "explicitlyCallRunFrame: Set true when your game will manually call RunFrame each frame." }, null)]
        public bool Init(bool explicitlyCallRunFrame = false)
        {
            return SteamNativeApi.InputInit(explicitlyCallRunFrame);
        }

        [Documentation(nameof(Shutdown), "Shuts down Steam Input.", null, null)]
        public bool Shutdown()
        {
            return SteamNativeApi.InputShutdown();
        }

        [Documentation(nameof(RunFrame), "Runs a Steam Input frame update.", null, null)]
        public void RunFrame()
        {
            SteamNativeApi.InputRunFrame();
        }
    }

    [Documentation(nameof(SteamInventoryManager), "Provides access checks for ISteamInventory.", null, null)]
    public class SteamInventoryManager
    {
        public bool IsAvailable => SteamNativeApi.IsSteamInventoryAvailable();

        [Documentation(nameof(LoadItemDefinitions), "Requests latest inventory item definitions from Steam.", null, null)]
        public bool LoadItemDefinitions()
        {
            return SteamNativeApi.InventoryLoadItemDefinitions();
        }

        [Documentation(nameof(GetAllItems), "Requests all inventory items and returns a result handle.", new[] { "inventoryResultHandle: Inventory result handle to use with additional inventory API calls." }, null)]
        public bool GetAllItems(out int inventoryResultHandle)
        {
            return SteamNativeApi.InventoryGetAllItems(out inventoryResultHandle);
        }

        [Documentation(nameof(DestroyResult), "Releases an inventory result handle returned from Steam Inventory API calls.", new[] { "inventoryResultHandle: Inventory result handle to release." }, null)]
        public void DestroyResult(int inventoryResultHandle)
        {
            SteamNativeApi.InventoryDestroyResult(inventoryResultHandle);
        }
    }

    [Documentation(nameof(SteamMatchmakingManager), "Provides access checks for ISteamMatchmaking.", null, null)]
    public class SteamMatchmakingManager
    {
        public bool IsAvailable => SteamNativeApi.IsSteamMatchmakingAvailable();

        [Documentation(nameof(RequestLobbyList), "Requests a lobby list and returns a Steam API call handle.", null, null)]
        public ulong RequestLobbyList()
        {
            return SteamNativeApi.MatchmakingRequestLobbyList();
        }

        [Documentation(nameof(AddRequestLobbyListResultCountFilter), "Sets the maximum number of lobbies returned by RequestLobbyList.", new[] { "maxResults: Maximum number of lobby results." }, null)]
        public void AddRequestLobbyListResultCountFilter(int maxResults)
        {
            SteamNativeApi.MatchmakingAddRequestLobbyListResultCountFilter(maxResults);
        }

        [Documentation(nameof(CreateLobby), "Creates a lobby and returns a Steam API call handle.", new[] { "lobbyType: Numeric ELobbyType value.", "maxMembers: Maximum lobby member count." }, null)]
        public ulong CreateLobby(int lobbyType, int maxMembers)
        {
            return SteamNativeApi.MatchmakingCreateLobby(lobbyType, maxMembers);
        }

        [Documentation(nameof(JoinLobby), "Joins a lobby by SteamID64 and returns a Steam API call handle.", new[] { "steamIdLobby: Lobby SteamID64." }, null)]
        public ulong JoinLobby(ulong steamIdLobby)
        {
            return SteamNativeApi.MatchmakingJoinLobby(steamIdLobby);
        }

        [Documentation(nameof(LeaveLobby), "Leaves a lobby by SteamID64.", new[] { "steamIdLobby: Lobby SteamID64." }, null)]
        public void LeaveLobby(ulong steamIdLobby)
        {
            SteamNativeApi.MatchmakingLeaveLobby(steamIdLobby);
        }

        [Documentation(nameof(GetNumLobbyMembers), "Gets the number of members currently in a lobby.", new[] { "steamIdLobby: Lobby SteamID64." }, null)]
        public int GetNumLobbyMembers(ulong steamIdLobby)
        {
            return SteamNativeApi.MatchmakingGetNumLobbyMembers(steamIdLobby);
        }
    }

    [Documentation(nameof(SteamMatchmakingServersManager), "Provides access checks for ISteamMatchmakingServers.", null, null)]
    public class SteamMatchmakingServersManager
    {
        public bool IsAvailable => SteamNativeApi.IsSteamMatchmakingServersAvailable();
    }

    [Documentation(nameof(SteamMusicManager), "Provides access checks for ISteamMusic.", null, null)]
    public class SteamMusicManager
    {
        public bool IsAvailable => SteamNativeApi.IsSteamMusicAvailable();

        [Documentation(nameof(IsEnabled), "Checks whether Steam Music is enabled.", null, null)]
        public bool IsEnabled()
        {
            return SteamNativeApi.MusicIsEnabled();
        }

        [Documentation(nameof(IsPlaying), "Checks whether Steam Music is currently playing.", null, null)]
        public bool IsPlaying()
        {
            return SteamNativeApi.MusicIsPlaying();
        }

        [Documentation(nameof(Play), "Starts Steam Music playback.", null, null)]
        public void Play()
        {
            SteamNativeApi.MusicPlay();
        }

        [Documentation(nameof(Pause), "Pauses Steam Music playback.", null, null)]
        public void Pause()
        {
            SteamNativeApi.MusicPause();
        }

        [Documentation(nameof(PlayNext), "Advances to the next music track.", null, null)]
        public void PlayNext()
        {
            SteamNativeApi.MusicPlayNext();
        }

        [Documentation(nameof(PlayPrevious), "Moves to the previous music track.", null, null)]
        public void PlayPrevious()
        {
            SteamNativeApi.MusicPlayPrevious();
        }

        [Documentation(nameof(GetVolume), "Gets Steam Music volume (0..1 range).", null, null)]
        public float GetVolume()
        {
            return SteamNativeApi.MusicGetVolume();
        }

        [Documentation(nameof(SetVolume), "Sets Steam Music volume (0..1 range).", new[] { "volume: New volume value." }, null)]
        public void SetVolume(float volume)
        {
            SteamNativeApi.MusicSetVolume(volume);
        }
    }

    [Documentation(nameof(SteamPartiesManager), "Provides access checks for ISteamParties.", null, null)]
    public class SteamPartiesManager
    {
        public bool IsAvailable => SteamNativeApi.IsSteamPartiesAvailable();
    }

    [Documentation(nameof(SteamRemoteStorageManager), "Provides access checks for ISteamRemoteStorage.", null, null)]
    public class SteamRemoteStorageManager
    {
        public bool IsAvailable => SteamNativeApi.IsSteamRemoteStorageAvailable();

        [Documentation(nameof(FileExists), "Checks whether a file exists in Steam Remote Storage.", new[] { "fileName: Cloud file path/name." }, null)]
        public bool FileExists(string fileName)
        {
            return SteamNativeApi.RemoteStorageFileExists(fileName);
        }

        [Documentation(nameof(GetFileCount), "Gets the number of files in Steam Remote Storage for the current app/user context.", null, null)]
        public int GetFileCount()
        {
            return SteamNativeApi.RemoteStorageGetFileCount();
        }

        [Documentation(nameof(IsCloudEnabledForApp), "Checks whether Steam Cloud is enabled for this app.", null, null)]
        public bool IsCloudEnabledForApp()
        {
            return SteamNativeApi.RemoteStorageIsCloudEnabledForApp();
        }

        [Documentation(nameof(FileWrite), "Writes a byte array to a Steam Cloud file.", new[] { "fileName: Cloud file path/name.", "data: Raw file bytes to write." }, null)]
        public bool FileWrite(string fileName, byte[] data)
        {
            return SteamNativeApi.RemoteStorageFileWrite(fileName, data);
        }

        [Documentation(nameof(FileRead), "Reads a Steam Cloud file into a byte array.", new[] { "fileName: Cloud file path/name.", "data: Output file bytes when read succeeds." }, null)]
        public bool FileRead(string fileName, out byte[] data)
        {
            return SteamNativeApi.RemoteStorageFileRead(fileName, out data);
        }

        [Documentation(nameof(FileDelete), "Deletes a Steam Cloud file.", new[] { "fileName: Cloud file path/name." }, null)]
        public bool FileDelete(string fileName)
        {
            return SteamNativeApi.RemoteStorageFileDelete(fileName);
        }

        [Documentation(nameof(GetFileSize), "Gets file size in bytes for a Steam Cloud file.", new[] { "fileName: Cloud file path/name." }, null)]
        public int GetFileSize(string fileName)
        {
            return SteamNativeApi.RemoteStorageGetFileSize(fileName);
        }
    }

    [Documentation(nameof(SteamScreenshotsManager), "Provides access checks for ISteamScreenshots.", null, null)]
    public class SteamScreenshotsManager
    {
        public bool IsAvailable => SteamNativeApi.IsSteamScreenshotsAvailable();

        [Documentation(nameof(AddScreenshotToLibrary), "Adds an existing screenshot to the Steam screenshot library.", new[] { "fileNameJpg: Screenshot image path.", "fileNameThumbnail: Thumbnail path.", "width: Image width.", "height: Image height." }, null)]
        public uint AddScreenshotToLibrary(string fileNameJpg, string fileNameThumbnail, int width, int height)
        {
            return SteamNativeApi.ScreenshotsAddScreenshotToLibrary(fileNameJpg, fileNameThumbnail, width, height);
        }

        [Documentation(nameof(TriggerScreenshot), "Triggers an in-game screenshot event.", null, null)]
        public void TriggerScreenshot()
        {
            SteamNativeApi.ScreenshotsTriggerScreenshot();
        }

        [Documentation(nameof(HookScreenshots), "Enables or disables screenshot hooks for custom handling.", new[] { "hook: True to hook screenshots." }, null)]
        public void HookScreenshots(bool hook)
        {
            SteamNativeApi.ScreenshotsHookScreenshots(hook);
        }

        [Documentation(nameof(IsScreenshotsHooked), "Checks whether screenshot hooks are currently enabled.", null, null)]
        public bool IsScreenshotsHooked()
        {
            return SteamNativeApi.ScreenshotsIsScreenshotsHooked();
        }
    }

    [Documentation(nameof(SteamUserStatsManager), "Provides access checks for ISteamUserStats.", null, null)]
    public class SteamUserStatsManager
    {
        public bool IsAvailable => SteamNativeApi.IsSteamUserStatsAvailable();

        [Documentation(nameof(RequestUserStats), "Requests user stats for a target SteamID64 and returns a Steam API call handle.", new[] { "steamIdUser: Target SteamID64." }, null)]
        public ulong RequestUserStats(ulong steamIdUser)
        {
            return SteamNativeApi.UserStatsRequestUserStats(steamIdUser);
        }

        [Documentation(nameof(RequestGlobalStats), "Requests global stats history and returns a Steam API call handle.", new[] { "historyDays: Number of days of history to request." }, null)]
        public ulong RequestGlobalStats(int historyDays)
        {
            return SteamNativeApi.UserStatsRequestGlobalStats(historyDays);
        }

        [Documentation(nameof(ResetAllStats), "Resets local user stats, optionally including achievements.", new[] { "achievementsToo: True to also clear achievement state." }, null)]
        public bool ResetAllStats(bool achievementsToo)
        {
            return SteamNativeApi.UserStatsResetAllStats(achievementsToo);
        }

        [Documentation(nameof(GetStatInt32), "Gets a named integer stat value.", new[] { "name: Stat API name.", "data: Output integer stat value." }, null)]
        public bool GetStatInt32(string name, out int data)
        {
            return SteamNativeApi.UserStatsGetStatInt32(name, out data);
        }

        [Documentation(nameof(SetStatInt32), "Sets a named integer stat value.", new[] { "name: Stat API name.", "data: Integer stat value to set." }, null)]
        public bool SetStatInt32(string name, int data)
        {
            return SteamNativeApi.UserStatsSetStatInt32(name, data);
        }

        [Documentation(nameof(StoreStats), "Stores pending stats and achievement changes to Steam.", null, null)]
        public bool StoreStats()
        {
            return SteamNativeApi.StoreStats();
        }
    }

    [Documentation(nameof(SteamUtilsManager), "Provides access checks for ISteamUtils.", null, null)]
    public class SteamUtilsManager
    {
        public bool IsAvailable => SteamNativeApi.IsSteamUtilsAvailable();

        [Documentation(nameof(GetAppId), "Gets the current Steam AppId reported by SteamUtils.", null, null)]
        public uint GetAppId()
        {
            return SteamNativeApi.GetAppId();
        }

        [Documentation(nameof(GetIpCountry), "Gets the two-letter country code inferred by Steam based on the current IP.", null, null)]
        public string GetIpCountry()
        {
            return SteamNativeApi.GetIpCountry();
        }

        [Documentation(nameof(IsOverlayEnabled), "Checks whether Steam Overlay is currently enabled.", null, null)]
        public bool IsOverlayEnabled()
        {
            return SteamNativeApi.IsOverlayEnabled();
        }
    }

    [Documentation(nameof(SteamVideoManager), "Provides access checks for ISteamVideo.", null, null)]
    public class SteamVideoManager
    {
        public bool IsAvailable => SteamNativeApi.IsSteamVideoAvailable();

        [Documentation(nameof(GetVideoUrl), "Requests a video URL for a given Steam video app id.", new[] { "videoAppId: Steam video app id." }, null)]
        public void GetVideoUrl(uint videoAppId)
        {
            SteamNativeApi.VideoGetVideoUrl(videoAppId);
        }

        [Documentation(nameof(IsBroadcasting), "Checks whether the user is broadcasting and returns current viewer count.", new[] { "numberOfViewers: Viewer count output when broadcasting." }, null)]
        public bool IsBroadcasting(out int numberOfViewers)
        {
            return SteamNativeApi.VideoIsBroadcasting(out numberOfViewers);
        }
    }
}

