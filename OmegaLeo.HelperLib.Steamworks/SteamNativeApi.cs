using System;
using System.Collections.Generic;
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
        private delegate IntPtr SteamUgcAccessorDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SteamRemotePlayAccessorDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SteamInterfaceAccessorDelegate();

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

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong SteamUgcCreateQueryAllRequestDelegate(IntPtr steamUgc, uint queryType, uint matchingType, uint creatorAppId, uint consumerAppId, uint page);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong SteamUgcSendQueryRequestDelegate(IntPtr steamUgc, ulong queryHandle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamUgcReleaseQueryRequestDelegate(IntPtr steamUgc, ulong queryHandle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint SteamUgcGetNumSubscribedItemsDelegate(IntPtr steamUgc);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint SteamUgcGetSubscribedItemsDelegate(IntPtr steamUgc, [Out] ulong[] items, uint maxEntries);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong SteamUgcSubscribeItemDelegate(IntPtr steamUgc, ulong publishedFileId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong SteamUgcUnsubscribeItemDelegate(IntPtr steamUgc, ulong publishedFileId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong SteamUgcCreateItemDelegate(IntPtr steamUgc, uint consumerAppId, uint fileType);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong SteamUgcStartItemUpdateDelegate(IntPtr steamUgc, uint consumerAppId, ulong publishedFileId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamUgcSetItemTitleDelegate(IntPtr steamUgc, ulong updateHandle, [MarshalAs(UnmanagedType.LPStr)] string title);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamUgcSetItemDescriptionDelegate(IntPtr steamUgc, ulong updateHandle, [MarshalAs(UnmanagedType.LPStr)] string description);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamUgcSetItemMetadataDelegate(IntPtr steamUgc, ulong updateHandle, [MarshalAs(UnmanagedType.LPStr)] string metadata);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamUgcSetItemVisibilityDelegate(IntPtr steamUgc, ulong updateHandle, uint visibility);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamUgcSetItemTagsDelegate(IntPtr steamUgc, ulong updateHandle, IntPtr tags);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamUgcSetItemContentDelegate(IntPtr steamUgc, ulong updateHandle, [MarshalAs(UnmanagedType.LPStr)] string contentFolder);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamUgcSetItemPreviewDelegate(IntPtr steamUgc, ulong updateHandle, [MarshalAs(UnmanagedType.LPStr)] string previewFile);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong SteamUgcSubmitItemUpdateDelegate(IntPtr steamUgc, ulong updateHandle, [MarshalAs(UnmanagedType.LPStr)] string changeNote);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint SteamUgcGetItemUpdateProgressDelegate(IntPtr steamUgc, ulong updateHandle, out ulong bytesProcessed, out ulong bytesTotal);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint SteamRemotePlayGetSessionCountDelegate(IntPtr steamRemotePlay);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint SteamRemotePlayGetSessionIdDelegate(IntPtr steamRemotePlay, int sessionIndex);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong SteamRemotePlayGetSessionSteamIdDelegate(IntPtr steamRemotePlay, uint sessionId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SteamRemotePlayGetSessionClientNameDelegate(IntPtr steamRemotePlay, uint sessionId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SteamRemotePlayGetSessionClientFormFactorDelegate(IntPtr steamRemotePlay, uint sessionId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamRemotePlayGetSessionClientResolutionDelegate(IntPtr steamRemotePlay, uint sessionId, out int resolutionX, out int resolutionY);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SteamFriendsGetFriendCountDelegate(IntPtr steamFriends, int friendFlags);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SteamFriendsGetFriendPersonaNameDelegate(IntPtr steamFriends, ulong steamIdFriend);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SteamFriendsActivateGameOverlayInviteDialogDelegate(IntPtr steamFriends, ulong steamIdLobby);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamFriendsSetRichPresenceDelegate(IntPtr steamFriends, [MarshalAs(UnmanagedType.LPStr)] string key, [MarshalAs(UnmanagedType.LPStr)] string value);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SteamFriendsClearRichPresenceDelegate(IntPtr steamFriends);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SteamFriendsGetFriendRichPresenceDelegate(IntPtr steamFriends, ulong steamIdFriend, [MarshalAs(UnmanagedType.LPStr)] string key);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamFriendsInviteUserToGameDelegate(IntPtr steamFriends, ulong steamIdFriend, [MarshalAs(UnmanagedType.LPStr)] string connectString);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint SteamUtilsGetAppIdDelegate(IntPtr steamUtils);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SteamUtilsGetIpCountryDelegate(IntPtr steamUtils);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamUtilsIsOverlayEnabledDelegate(IntPtr steamUtils);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamRemoteStorageFileExistsDelegate(IntPtr steamRemoteStorage, [MarshalAs(UnmanagedType.LPStr)] string fileName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SteamRemoteStorageGetFileCountDelegate(IntPtr steamRemoteStorage);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamRemoteStorageIsCloudEnabledForAppDelegate(IntPtr steamRemoteStorage);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamRemoteStorageFileWriteDelegate(IntPtr steamRemoteStorage, [MarshalAs(UnmanagedType.LPStr)] string fileName, byte[] data, int dataSize);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SteamRemoteStorageFileReadDelegate(IntPtr steamRemoteStorage, [MarshalAs(UnmanagedType.LPStr)] string fileName, [Out] byte[] dataBuffer, int dataToRead);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamRemoteStorageFileDeleteDelegate(IntPtr steamRemoteStorage, [MarshalAs(UnmanagedType.LPStr)] string fileName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SteamRemoteStorageGetFileSizeDelegate(IntPtr steamRemoteStorage, [MarshalAs(UnmanagedType.LPStr)] string fileName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamInputInitDelegate(IntPtr steamInput, [MarshalAs(UnmanagedType.I1)] bool explicitlyCallRunFrame);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamInputShutdownDelegate(IntPtr steamInput);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SteamInputRunFrameDelegate(IntPtr steamInput, [MarshalAs(UnmanagedType.I1)] bool reservedValue);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamInventoryLoadItemDefinitionsDelegate(IntPtr steamInventory);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamInventoryGetAllItemsDelegate(IntPtr steamInventory, out int inventoryResultHandle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SteamInventoryDestroyResultDelegate(IntPtr steamInventory, int inventoryResultHandle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong SteamMatchmakingRequestLobbyListDelegate(IntPtr steamMatchmaking);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SteamMatchmakingAddRequestLobbyListResultCountFilterDelegate(IntPtr steamMatchmaking, int maxResults);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong SteamMatchmakingCreateLobbyDelegate(IntPtr steamMatchmaking, int lobbyType, int maxMembers);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong SteamMatchmakingJoinLobbyDelegate(IntPtr steamMatchmaking, ulong steamIdLobby);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SteamMatchmakingLeaveLobbyDelegate(IntPtr steamMatchmaking, ulong steamIdLobby);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SteamMatchmakingGetNumLobbyMembersDelegate(IntPtr steamMatchmaking, ulong steamIdLobby);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint SteamScreenshotsAddScreenshotToLibraryDelegate(IntPtr steamScreenshots, [MarshalAs(UnmanagedType.LPStr)] string fileNameJpg, [MarshalAs(UnmanagedType.LPStr)] string fileNameThumbnail, int width, int height);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SteamScreenshotsTriggerScreenshotDelegate(IntPtr steamScreenshots);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SteamScreenshotsHookScreenshotsDelegate(IntPtr steamScreenshots, [MarshalAs(UnmanagedType.I1)] bool hook);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamScreenshotsIsScreenshotsHookedDelegate(IntPtr steamScreenshots);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamMusicIsEnabledDelegate(IntPtr steamMusic);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamMusicIsPlayingDelegate(IntPtr steamMusic);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SteamMusicPlayDelegate(IntPtr steamMusic);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SteamMusicPauseDelegate(IntPtr steamMusic);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SteamMusicPlayNextDelegate(IntPtr steamMusic);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SteamMusicPlayPreviousDelegate(IntPtr steamMusic);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float SteamMusicGetVolumeDelegate(IntPtr steamMusic);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SteamMusicSetVolumeDelegate(IntPtr steamMusic, float volume);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SteamVideoGetVideoUrlDelegate(IntPtr steamVideo, uint videoAppId);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamVideoIsBroadcastingDelegate(IntPtr steamVideo, out int numberOfViewers);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong SteamUserStatsRequestUserStatsDelegate(IntPtr steamUserStats, ulong steamIdUser);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ulong SteamUserStatsRequestGlobalStatsDelegate(IntPtr steamUserStats, int historyDays);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamUserStatsResetAllStatsDelegate(IntPtr steamUserStats, [MarshalAs(UnmanagedType.I1)] bool achievementsToo);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamUserStatsGetStatInt32Delegate(IntPtr steamUserStats, [MarshalAs(UnmanagedType.LPStr)] string name, out int data);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private delegate bool SteamUserStatsSetStatInt32Delegate(IntPtr steamUserStats, [MarshalAs(UnmanagedType.LPStr)] string name, int data);

        private static readonly object SyncRoot = new object();

        private static IntPtr _libraryHandle;
        private static bool _libraryLoaded;
        private static bool _initialized;

        private static SteamApiInitDelegate _steamApiInit;
        private static SteamApiVoidDelegate _steamApiShutdown;
        private static SteamApiVoidDelegate _steamApiRunCallbacks;
        private static SteamFriendsAccessorDelegate _steamFriendsAccessor;
        private static SteamUserStatsAccessorDelegate _steamUserStatsAccessor;
        private static SteamUgcAccessorDelegate _steamUgcAccessor;
        private static SteamRemotePlayAccessorDelegate _steamRemotePlayAccessor;
        private static SteamFriendsGetPersonaNameDelegate _steamFriendsGetPersonaName;
        private static SteamUserStatsSetAchievementDelegate _steamUserStatsSetAchievement;
        private static SteamUserStatsStoreStatsDelegate _steamUserStatsStoreStats;
        private static SteamUserStatsGetAchievementDelegate _steamUserStatsGetAchievement;
        private static SteamUgcCreateQueryAllRequestDelegate _steamUgcCreateQueryAllRequest;
        private static SteamUgcSendQueryRequestDelegate _steamUgcSendQueryRequest;
        private static SteamUgcReleaseQueryRequestDelegate _steamUgcReleaseQueryRequest;
        private static SteamUgcGetNumSubscribedItemsDelegate _steamUgcGetNumSubscribedItems;
        private static SteamUgcGetSubscribedItemsDelegate _steamUgcGetSubscribedItems;
        private static SteamUgcSubscribeItemDelegate _steamUgcSubscribeItem;
        private static SteamUgcUnsubscribeItemDelegate _steamUgcUnsubscribeItem;
        private static SteamUgcCreateItemDelegate _steamUgcCreateItem;
        private static SteamUgcStartItemUpdateDelegate _steamUgcStartItemUpdate;
        private static SteamUgcSetItemTitleDelegate _steamUgcSetItemTitle;
        private static SteamUgcSetItemDescriptionDelegate _steamUgcSetItemDescription;
        private static SteamUgcSetItemMetadataDelegate _steamUgcSetItemMetadata;
        private static SteamUgcSetItemVisibilityDelegate _steamUgcSetItemVisibility;
        private static SteamUgcSetItemTagsDelegate _steamUgcSetItemTags;
        private static SteamUgcSetItemContentDelegate _steamUgcSetItemContent;
        private static SteamUgcSetItemPreviewDelegate _steamUgcSetItemPreview;
        private static SteamUgcSubmitItemUpdateDelegate _steamUgcSubmitItemUpdate;
        private static SteamUgcGetItemUpdateProgressDelegate _steamUgcGetItemUpdateProgress;
        private static SteamRemotePlayGetSessionCountDelegate _steamRemotePlayGetSessionCount;
        private static SteamRemotePlayGetSessionIdDelegate _steamRemotePlayGetSessionId;
        private static SteamRemotePlayGetSessionSteamIdDelegate _steamRemotePlayGetSessionSteamId;
        private static SteamRemotePlayGetSessionClientNameDelegate _steamRemotePlayGetSessionClientName;
        private static SteamRemotePlayGetSessionClientFormFactorDelegate _steamRemotePlayGetSessionClientFormFactor;
        private static SteamRemotePlayGetSessionClientResolutionDelegate _steamRemotePlayGetSessionClientResolution;
        private static SteamFriendsGetFriendCountDelegate _steamFriendsGetFriendCount;
        private static SteamFriendsGetFriendPersonaNameDelegate _steamFriendsGetFriendPersonaName;
        private static SteamFriendsActivateGameOverlayInviteDialogDelegate _steamFriendsActivateGameOverlayInviteDialog;
        private static SteamFriendsSetRichPresenceDelegate _steamFriendsSetRichPresence;
        private static SteamFriendsClearRichPresenceDelegate _steamFriendsClearRichPresence;
        private static SteamFriendsGetFriendRichPresenceDelegate _steamFriendsGetFriendRichPresence;
        private static SteamFriendsInviteUserToGameDelegate _steamFriendsInviteUserToGame;
        private static SteamUtilsGetAppIdDelegate _steamUtilsGetAppId;
        private static SteamUtilsGetIpCountryDelegate _steamUtilsGetIpCountry;
        private static SteamUtilsIsOverlayEnabledDelegate _steamUtilsIsOverlayEnabled;
        private static SteamRemoteStorageFileExistsDelegate _steamRemoteStorageFileExists;
        private static SteamRemoteStorageGetFileCountDelegate _steamRemoteStorageGetFileCount;
        private static SteamRemoteStorageIsCloudEnabledForAppDelegate _steamRemoteStorageIsCloudEnabledForApp;
        private static SteamRemoteStorageFileWriteDelegate _steamRemoteStorageFileWrite;
        private static SteamRemoteStorageFileReadDelegate _steamRemoteStorageFileRead;
        private static SteamRemoteStorageFileDeleteDelegate _steamRemoteStorageFileDelete;
        private static SteamRemoteStorageGetFileSizeDelegate _steamRemoteStorageGetFileSize;
        private static SteamInputInitDelegate _steamInputInit;
        private static SteamInputShutdownDelegate _steamInputShutdown;
        private static SteamInputRunFrameDelegate _steamInputRunFrame;
        private static SteamInventoryLoadItemDefinitionsDelegate _steamInventoryLoadItemDefinitions;
        private static SteamInventoryGetAllItemsDelegate _steamInventoryGetAllItems;
        private static SteamInventoryDestroyResultDelegate _steamInventoryDestroyResult;
        private static SteamMatchmakingRequestLobbyListDelegate _steamMatchmakingRequestLobbyList;
        private static SteamMatchmakingAddRequestLobbyListResultCountFilterDelegate _steamMatchmakingAddRequestLobbyListResultCountFilter;
        private static SteamMatchmakingCreateLobbyDelegate _steamMatchmakingCreateLobby;
        private static SteamMatchmakingJoinLobbyDelegate _steamMatchmakingJoinLobby;
        private static SteamMatchmakingLeaveLobbyDelegate _steamMatchmakingLeaveLobby;
        private static SteamMatchmakingGetNumLobbyMembersDelegate _steamMatchmakingGetNumLobbyMembers;
        private static SteamScreenshotsAddScreenshotToLibraryDelegate _steamScreenshotsAddScreenshotToLibrary;
        private static SteamScreenshotsTriggerScreenshotDelegate _steamScreenshotsTriggerScreenshot;
        private static SteamScreenshotsHookScreenshotsDelegate _steamScreenshotsHookScreenshots;
        private static SteamScreenshotsIsScreenshotsHookedDelegate _steamScreenshotsIsScreenshotsHooked;
        private static SteamMusicIsEnabledDelegate _steamMusicIsEnabled;
        private static SteamMusicIsPlayingDelegate _steamMusicIsPlaying;
        private static SteamMusicPlayDelegate _steamMusicPlay;
        private static SteamMusicPauseDelegate _steamMusicPause;
        private static SteamMusicPlayNextDelegate _steamMusicPlayNext;
        private static SteamMusicPlayPreviousDelegate _steamMusicPlayPrevious;
        private static SteamMusicGetVolumeDelegate _steamMusicGetVolume;
        private static SteamMusicSetVolumeDelegate _steamMusicSetVolume;
        private static SteamVideoGetVideoUrlDelegate _steamVideoGetVideoUrl;
        private static SteamVideoIsBroadcastingDelegate _steamVideoIsBroadcasting;
        private static SteamUserStatsRequestUserStatsDelegate _steamUserStatsRequestUserStats;
        private static SteamUserStatsRequestGlobalStatsDelegate _steamUserStatsRequestGlobalStats;
        private static SteamUserStatsResetAllStatsDelegate _steamUserStatsResetAllStats;
        private static SteamUserStatsGetStatInt32Delegate _steamUserStatsGetStatInt32;
        private static SteamUserStatsSetStatInt32Delegate _steamUserStatsSetStatInt32;

        private static IntPtr _steamFriends;
        private static IntPtr _steamUserStats;
        private static IntPtr _steamUgc;
        private static IntPtr _steamRemotePlay;

        private static readonly Dictionary<string, SteamInterfaceAccessorDelegate> AdditionalInterfaceAccessors = new Dictionary<string, SteamInterfaceAccessorDelegate>();
        private static readonly Dictionary<string, IntPtr> AdditionalInterfacePointers = new Dictionary<string, IntPtr>();

        private const string InterfaceSteamController = "SteamController";
        private const string InterfaceSteamGameCoordinator = "SteamGameCoordinator";
        private const string InterfaceSteamGameServer = "SteamGameServer";
        private const string InterfaceSteamGameServerStats = "SteamGameServerStats";
        private const string InterfaceSteamHtmlSurface = "SteamHTMLSurface";
        private const string InterfaceSteamInput = "SteamInput";
        private const string InterfaceSteamInventory = "SteamInventory";
        private const string InterfaceSteamMatchmaking = "SteamMatchmaking";
        private const string InterfaceSteamMatchmakingServers = "SteamMatchmakingServers";
        private const string InterfaceSteamMusic = "SteamMusic";
        private const string InterfaceSteamParties = "SteamParties";
        private const string InterfaceSteamRemoteStorage = "SteamRemoteStorage";
        private const string InterfaceSteamScreenshots = "SteamScreenshots";
        private const string InterfaceSteamUtils = "SteamUtils";
        private const string InterfaceSteamVideo = "SteamVideo";

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
                _steamUgc = IntPtr.Zero;
                _steamRemotePlay = IntPtr.Zero;
                AdditionalInterfacePointers.Clear();
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

        public static ulong CreateQueryAllUgcRequest(uint queryType, uint matchingType, uint creatorAppId, uint consumerAppId, uint page)
        {
            if (!EnsureUgcReady())
            {
                return 0;
            }

            return _steamUgcCreateQueryAllRequest?.Invoke(_steamUgc, queryType, matchingType, creatorAppId, consumerAppId, page) ?? 0;
        }

        public static ulong SendQueryUgcRequest(ulong queryHandle)
        {
            if (!EnsureUgcReady())
            {
                return 0;
            }

            return _steamUgcSendQueryRequest?.Invoke(_steamUgc, queryHandle) ?? 0;
        }

        public static bool ReleaseQueryUgcRequest(ulong queryHandle)
        {
            if (!EnsureUgcReady())
            {
                return false;
            }

            return _steamUgcReleaseQueryRequest?.Invoke(_steamUgc, queryHandle) ?? false;
        }

        public static uint GetNumSubscribedItems()
        {
            if (!EnsureUgcReady())
            {
                return 0;
            }

            return _steamUgcGetNumSubscribedItems?.Invoke(_steamUgc) ?? 0;
        }

        public static ulong[] GetSubscribedItems(uint maxEntries)
        {
            if (!EnsureUgcReady() || maxEntries == 0)
            {
                return Array.Empty<ulong>();
            }

            var buffer = new ulong[maxEntries];
            var count = _steamUgcGetSubscribedItems?.Invoke(_steamUgc, buffer, maxEntries) ?? 0;
            if (count == 0)
            {
                return Array.Empty<ulong>();
            }

            if (count == buffer.Length)
            {
                return buffer;
            }

            var result = new ulong[count];
            Array.Copy(buffer, result, count);
            return result;
        }

        public static ulong SubscribeItem(ulong publishedFileId)
        {
            if (!EnsureUgcReady())
            {
                return 0;
            }

            return _steamUgcSubscribeItem?.Invoke(_steamUgc, publishedFileId) ?? 0;
        }

        public static ulong UnsubscribeItem(ulong publishedFileId)
        {
            if (!EnsureUgcReady())
            {
                return 0;
            }

            return _steamUgcUnsubscribeItem?.Invoke(_steamUgc, publishedFileId) ?? 0;
        }

        public static ulong CreateItem(uint consumerAppId, uint fileType)
        {
            if (!EnsureUgcReady())
            {
                return 0;
            }

            return _steamUgcCreateItem?.Invoke(_steamUgc, consumerAppId, fileType) ?? 0;
        }

        public static ulong StartItemUpdate(uint consumerAppId, ulong publishedFileId)
        {
            if (!EnsureUgcReady())
            {
                return 0;
            }

            return _steamUgcStartItemUpdate?.Invoke(_steamUgc, consumerAppId, publishedFileId) ?? 0;
        }

        public static bool SetItemTitle(ulong updateHandle, string title)
        {
            if (!EnsureUgcReady())
            {
                return false;
            }

            return _steamUgcSetItemTitle?.Invoke(_steamUgc, updateHandle, title) ?? false;
        }

        public static bool SetItemDescription(ulong updateHandle, string description)
        {
            if (!EnsureUgcReady())
            {
                return false;
            }

            return _steamUgcSetItemDescription?.Invoke(_steamUgc, updateHandle, description) ?? false;
        }

        public static bool SetItemMetadata(ulong updateHandle, string metadata)
        {
            if (!EnsureUgcReady())
            {
                return false;
            }

            return _steamUgcSetItemMetadata?.Invoke(_steamUgc, updateHandle, metadata) ?? false;
        }

        public static bool SetItemVisibility(ulong updateHandle, uint visibility)
        {
            if (!EnsureUgcReady())
            {
                return false;
            }

            return _steamUgcSetItemVisibility?.Invoke(_steamUgc, updateHandle, visibility) ?? false;
        }

        public static bool SetItemTags(ulong updateHandle, string[] tags)
        {
            if (!EnsureUgcReady() || tags == null)
            {
                return false;
            }

            return WithSteamStringArray(tags, nativePtr => _steamUgcSetItemTags?.Invoke(_steamUgc, updateHandle, nativePtr) ?? false);
        }

        public static bool SetItemContent(ulong updateHandle, string contentFolder)
        {
            if (!EnsureUgcReady())
            {
                return false;
            }

            return _steamUgcSetItemContent?.Invoke(_steamUgc, updateHandle, contentFolder) ?? false;
        }

        public static bool SetItemPreview(ulong updateHandle, string previewFile)
        {
            if (!EnsureUgcReady())
            {
                return false;
            }

            return _steamUgcSetItemPreview?.Invoke(_steamUgc, updateHandle, previewFile) ?? false;
        }

        public static ulong SubmitItemUpdate(ulong updateHandle, string changeNote)
        {
            if (!EnsureUgcReady())
            {
                return 0;
            }

            return _steamUgcSubmitItemUpdate?.Invoke(_steamUgc, updateHandle, changeNote ?? string.Empty) ?? 0;
        }

        public static uint GetItemUpdateProgress(ulong updateHandle, out ulong bytesProcessed, out ulong bytesTotal)
        {
            bytesProcessed = 0;
            bytesTotal = 0;

            if (!EnsureUgcReady())
            {
                return 0;
            }

            return _steamUgcGetItemUpdateProgress?.Invoke(_steamUgc, updateHandle, out bytesProcessed, out bytesTotal) ?? 0;
        }

        public static uint GetRemotePlaySessionCount()
        {
            if (!EnsureRemotePlayReady())
            {
                return 0;
            }

            return _steamRemotePlayGetSessionCount?.Invoke(_steamRemotePlay) ?? 0;
        }

        public static uint GetRemotePlaySessionId(int sessionIndex)
        {
            if (!EnsureRemotePlayReady())
            {
                return 0;
            }

            return _steamRemotePlayGetSessionId?.Invoke(_steamRemotePlay, sessionIndex) ?? 0;
        }

        public static ulong GetRemotePlaySessionSteamId(uint sessionId)
        {
            if (!EnsureRemotePlayReady())
            {
                return 0;
            }

            return _steamRemotePlayGetSessionSteamId?.Invoke(_steamRemotePlay, sessionId) ?? 0;
        }

        public static string GetRemotePlaySessionClientName(uint sessionId)
        {
            if (!EnsureRemotePlayReady())
            {
                return string.Empty;
            }

            var namePtr = _steamRemotePlayGetSessionClientName?.Invoke(_steamRemotePlay, sessionId) ?? IntPtr.Zero;
            return namePtr == IntPtr.Zero ? string.Empty : Marshal.PtrToStringAnsi(namePtr) ?? string.Empty;
        }

        public static int GetRemotePlaySessionClientFormFactor(uint sessionId)
        {
            if (!EnsureRemotePlayReady())
            {
                return 0;
            }

            return _steamRemotePlayGetSessionClientFormFactor?.Invoke(_steamRemotePlay, sessionId) ?? 0;
        }

        public static bool TryGetRemotePlaySessionClientResolution(uint sessionId, out int resolutionX, out int resolutionY)
        {
            resolutionX = 0;
            resolutionY = 0;

            if (!EnsureRemotePlayReady())
            {
                return false;
            }

            return _steamRemotePlayGetSessionClientResolution?.Invoke(_steamRemotePlay, sessionId, out resolutionX, out resolutionY) ?? false;
        }

        public static bool IsSteamControllerAvailable() => EnsureAdditionalInterfaceReady(InterfaceSteamController);

        public static bool IsSteamFriendsAvailable() => EnsureFriendsReady();

        public static bool IsSteamGameCoordinatorAvailable() => EnsureAdditionalInterfaceReady(InterfaceSteamGameCoordinator);

        public static bool IsSteamGameServerAvailable() => EnsureAdditionalInterfaceReady(InterfaceSteamGameServer);

        public static bool IsSteamGameServerStatsAvailable() => EnsureAdditionalInterfaceReady(InterfaceSteamGameServerStats);

        public static bool IsSteamHtmlSurfaceAvailable() => EnsureAdditionalInterfaceReady(InterfaceSteamHtmlSurface);

        public static bool IsSteamInputAvailable() => EnsureAdditionalInterfaceReady(InterfaceSteamInput);

        public static bool IsSteamInventoryAvailable() => EnsureAdditionalInterfaceReady(InterfaceSteamInventory);

        public static bool IsSteamMatchmakingAvailable() => EnsureAdditionalInterfaceReady(InterfaceSteamMatchmaking);

        public static bool IsSteamMatchmakingServersAvailable() => EnsureAdditionalInterfaceReady(InterfaceSteamMatchmakingServers);

        public static bool IsSteamMusicAvailable() => EnsureAdditionalInterfaceReady(InterfaceSteamMusic);

        public static bool IsSteamPartiesAvailable() => EnsureAdditionalInterfaceReady(InterfaceSteamParties);

        public static bool IsSteamRemoteStorageAvailable() => EnsureAdditionalInterfaceReady(InterfaceSteamRemoteStorage);

        public static bool IsSteamScreenshotsAvailable() => EnsureAdditionalInterfaceReady(InterfaceSteamScreenshots);

        public static bool IsSteamUserStatsAvailable() => EnsureUserStatsReady();

        public static bool IsSteamUgcAvailable() => EnsureUgcReady();

        public static bool IsSteamRemotePlayAvailable() => EnsureRemotePlayReady();

        public static bool IsSteamUtilsAvailable() => EnsureAdditionalInterfaceReady(InterfaceSteamUtils);

        public static bool IsSteamVideoAvailable() => EnsureAdditionalInterfaceReady(InterfaceSteamVideo);

        public static int GetFriendCount(int friendFlags)
        {
            if (!EnsureFriendsReady())
            {
                return 0;
            }

            return _steamFriendsGetFriendCount?.Invoke(_steamFriends, friendFlags) ?? 0;
        }

        public static string GetFriendPersonaName(ulong steamIdFriend)
        {
            if (!EnsureFriendsReady())
            {
                return string.Empty;
            }

            var namePtr = _steamFriendsGetFriendPersonaName?.Invoke(_steamFriends, steamIdFriend) ?? IntPtr.Zero;
            return namePtr == IntPtr.Zero ? string.Empty : Marshal.PtrToStringAnsi(namePtr) ?? string.Empty;
        }

        public static void FriendsActivateGameOverlayInviteDialog(ulong steamIdLobby)
        {
            if (!EnsureFriendsReady())
            {
                return;
            }

            _steamFriendsActivateGameOverlayInviteDialog?.Invoke(_steamFriends, steamIdLobby);
        }

        public static bool FriendsSetRichPresence(string key, string value)
        {
            if (!EnsureFriendsReady())
            {
                return false;
            }

            return _steamFriendsSetRichPresence?.Invoke(_steamFriends, key, value) ?? false;
        }

        public static void FriendsClearRichPresence()
        {
            if (!EnsureFriendsReady())
            {
                return;
            }

            _steamFriendsClearRichPresence?.Invoke(_steamFriends);
        }

        public static string FriendsGetFriendRichPresence(ulong steamIdFriend, string key)
        {
            if (!EnsureFriendsReady())
            {
                return string.Empty;
            }

            var valuePtr = _steamFriendsGetFriendRichPresence?.Invoke(_steamFriends, steamIdFriend, key) ?? IntPtr.Zero;
            return valuePtr == IntPtr.Zero ? string.Empty : Marshal.PtrToStringAnsi(valuePtr) ?? string.Empty;
        }

        public static bool FriendsInviteUserToGame(ulong steamIdFriend, string connectString)
        {
            if (!EnsureFriendsReady())
            {
                return false;
            }

            return _steamFriendsInviteUserToGame?.Invoke(_steamFriends, steamIdFriend, connectString) ?? false;
        }

        public static uint GetAppId()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamUtils))
            {
                return 0;
            }

            var steamUtils = GetAdditionalInterfacePointer(InterfaceSteamUtils);
            return _steamUtilsGetAppId?.Invoke(steamUtils) ?? 0;
        }

        public static string GetIpCountry()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamUtils))
            {
                return string.Empty;
            }

            var steamUtils = GetAdditionalInterfacePointer(InterfaceSteamUtils);
            var countryPtr = _steamUtilsGetIpCountry?.Invoke(steamUtils) ?? IntPtr.Zero;
            return countryPtr == IntPtr.Zero ? string.Empty : Marshal.PtrToStringAnsi(countryPtr) ?? string.Empty;
        }

        public static bool IsOverlayEnabled()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamUtils))
            {
                return false;
            }

            var steamUtils = GetAdditionalInterfacePointer(InterfaceSteamUtils);
            return _steamUtilsIsOverlayEnabled?.Invoke(steamUtils) ?? false;
        }

        public static bool RemoteStorageFileExists(string fileName)
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamRemoteStorage))
            {
                return false;
            }

            var steamRemoteStorage = GetAdditionalInterfacePointer(InterfaceSteamRemoteStorage);
            return _steamRemoteStorageFileExists?.Invoke(steamRemoteStorage, fileName) ?? false;
        }

        public static int RemoteStorageGetFileCount()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamRemoteStorage))
            {
                return 0;
            }

            var steamRemoteStorage = GetAdditionalInterfacePointer(InterfaceSteamRemoteStorage);
            return _steamRemoteStorageGetFileCount?.Invoke(steamRemoteStorage) ?? 0;
        }

        public static bool RemoteStorageIsCloudEnabledForApp()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamRemoteStorage))
            {
                return false;
            }

            var steamRemoteStorage = GetAdditionalInterfacePointer(InterfaceSteamRemoteStorage);
            return _steamRemoteStorageIsCloudEnabledForApp?.Invoke(steamRemoteStorage) ?? false;
        }

        public static bool RemoteStorageFileWrite(string fileName, byte[] data)
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamRemoteStorage) || data == null)
            {
                return false;
            }

            var steamRemoteStorage = GetAdditionalInterfacePointer(InterfaceSteamRemoteStorage);
            return _steamRemoteStorageFileWrite?.Invoke(steamRemoteStorage, fileName, data, data.Length) ?? false;
        }

        public static bool RemoteStorageFileRead(string fileName, out byte[] data)
        {
            data = Array.Empty<byte>();

            if (!EnsureAdditionalInterfaceReady(InterfaceSteamRemoteStorage))
            {
                return false;
            }

            var steamRemoteStorage = GetAdditionalInterfacePointer(InterfaceSteamRemoteStorage);
            var size = _steamRemoteStorageGetFileSize?.Invoke(steamRemoteStorage, fileName) ?? -1;
            if (size <= 0)
            {
                return false;
            }

            var buffer = new byte[size];
            var bytesRead = _steamRemoteStorageFileRead?.Invoke(steamRemoteStorage, fileName, buffer, size) ?? 0;
            if (bytesRead <= 0)
            {
                return false;
            }

            if (bytesRead != size)
            {
                var resized = new byte[bytesRead];
                Array.Copy(buffer, resized, bytesRead);
                data = resized;
                return true;
            }

            data = buffer;
            return true;
        }

        public static bool RemoteStorageFileDelete(string fileName)
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamRemoteStorage))
            {
                return false;
            }

            var steamRemoteStorage = GetAdditionalInterfacePointer(InterfaceSteamRemoteStorage);
            return _steamRemoteStorageFileDelete?.Invoke(steamRemoteStorage, fileName) ?? false;
        }

        public static int RemoteStorageGetFileSize(string fileName)
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamRemoteStorage))
            {
                return 0;
            }

            var steamRemoteStorage = GetAdditionalInterfacePointer(InterfaceSteamRemoteStorage);
            return _steamRemoteStorageGetFileSize?.Invoke(steamRemoteStorage, fileName) ?? 0;
        }

        public static bool InputInit(bool explicitlyCallRunFrame)
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamInput))
            {
                return false;
            }

            var steamInput = GetAdditionalInterfacePointer(InterfaceSteamInput);
            return _steamInputInit?.Invoke(steamInput, explicitlyCallRunFrame) ?? false;
        }

        public static bool InputShutdown()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamInput))
            {
                return false;
            }

            var steamInput = GetAdditionalInterfacePointer(InterfaceSteamInput);
            return _steamInputShutdown?.Invoke(steamInput) ?? false;
        }

        public static void InputRunFrame()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamInput))
            {
                return;
            }

            var steamInput = GetAdditionalInterfacePointer(InterfaceSteamInput);
            _steamInputRunFrame?.Invoke(steamInput, false);
        }

        public static bool InventoryLoadItemDefinitions()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamInventory))
            {
                return false;
            }

            var steamInventory = GetAdditionalInterfacePointer(InterfaceSteamInventory);
            return _steamInventoryLoadItemDefinitions?.Invoke(steamInventory) ?? false;
        }

        public static bool InventoryGetAllItems(out int inventoryResultHandle)
        {
            inventoryResultHandle = 0;

            if (!EnsureAdditionalInterfaceReady(InterfaceSteamInventory))
            {
                return false;
            }

            var steamInventory = GetAdditionalInterfacePointer(InterfaceSteamInventory);
            return _steamInventoryGetAllItems?.Invoke(steamInventory, out inventoryResultHandle) ?? false;
        }

        public static void InventoryDestroyResult(int inventoryResultHandle)
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamInventory))
            {
                return;
            }

            var steamInventory = GetAdditionalInterfacePointer(InterfaceSteamInventory);
            _steamInventoryDestroyResult?.Invoke(steamInventory, inventoryResultHandle);
        }

        public static ulong MatchmakingRequestLobbyList()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamMatchmaking))
            {
                return 0;
            }

            var steamMatchmaking = GetAdditionalInterfacePointer(InterfaceSteamMatchmaking);
            return _steamMatchmakingRequestLobbyList?.Invoke(steamMatchmaking) ?? 0;
        }

        public static void MatchmakingAddRequestLobbyListResultCountFilter(int maxResults)
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamMatchmaking))
            {
                return;
            }

            var steamMatchmaking = GetAdditionalInterfacePointer(InterfaceSteamMatchmaking);
            _steamMatchmakingAddRequestLobbyListResultCountFilter?.Invoke(steamMatchmaking, maxResults);
        }

        public static ulong MatchmakingCreateLobby(int lobbyType, int maxMembers)
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamMatchmaking))
            {
                return 0;
            }

            var steamMatchmaking = GetAdditionalInterfacePointer(InterfaceSteamMatchmaking);
            return _steamMatchmakingCreateLobby?.Invoke(steamMatchmaking, lobbyType, maxMembers) ?? 0;
        }

        public static ulong MatchmakingJoinLobby(ulong steamIdLobby)
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamMatchmaking))
            {
                return 0;
            }

            var steamMatchmaking = GetAdditionalInterfacePointer(InterfaceSteamMatchmaking);
            return _steamMatchmakingJoinLobby?.Invoke(steamMatchmaking, steamIdLobby) ?? 0;
        }

        public static void MatchmakingLeaveLobby(ulong steamIdLobby)
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamMatchmaking))
            {
                return;
            }

            var steamMatchmaking = GetAdditionalInterfacePointer(InterfaceSteamMatchmaking);
            _steamMatchmakingLeaveLobby?.Invoke(steamMatchmaking, steamIdLobby);
        }

        public static int MatchmakingGetNumLobbyMembers(ulong steamIdLobby)
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamMatchmaking))
            {
                return 0;
            }

            var steamMatchmaking = GetAdditionalInterfacePointer(InterfaceSteamMatchmaking);
            return _steamMatchmakingGetNumLobbyMembers?.Invoke(steamMatchmaking, steamIdLobby) ?? 0;
        }

        public static uint ScreenshotsAddScreenshotToLibrary(string fileNameJpg, string fileNameThumbnail, int width, int height)
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamScreenshots))
            {
                return 0;
            }

            var steamScreenshots = GetAdditionalInterfacePointer(InterfaceSteamScreenshots);
            return _steamScreenshotsAddScreenshotToLibrary?.Invoke(steamScreenshots, fileNameJpg, fileNameThumbnail, width, height) ?? 0;
        }

        public static void ScreenshotsTriggerScreenshot()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamScreenshots))
            {
                return;
            }

            var steamScreenshots = GetAdditionalInterfacePointer(InterfaceSteamScreenshots);
            _steamScreenshotsTriggerScreenshot?.Invoke(steamScreenshots);
        }

        public static void ScreenshotsHookScreenshots(bool hook)
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamScreenshots))
            {
                return;
            }

            var steamScreenshots = GetAdditionalInterfacePointer(InterfaceSteamScreenshots);
            _steamScreenshotsHookScreenshots?.Invoke(steamScreenshots, hook);
        }

        public static bool ScreenshotsIsScreenshotsHooked()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamScreenshots))
            {
                return false;
            }

            var steamScreenshots = GetAdditionalInterfacePointer(InterfaceSteamScreenshots);
            return _steamScreenshotsIsScreenshotsHooked?.Invoke(steamScreenshots) ?? false;
        }

        public static bool MusicIsEnabled()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamMusic))
            {
                return false;
            }

            var steamMusic = GetAdditionalInterfacePointer(InterfaceSteamMusic);
            return _steamMusicIsEnabled?.Invoke(steamMusic) ?? false;
        }

        public static bool MusicIsPlaying()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamMusic))
            {
                return false;
            }

            var steamMusic = GetAdditionalInterfacePointer(InterfaceSteamMusic);
            return _steamMusicIsPlaying?.Invoke(steamMusic) ?? false;
        }

        public static void MusicPlay()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamMusic))
            {
                return;
            }

            var steamMusic = GetAdditionalInterfacePointer(InterfaceSteamMusic);
            _steamMusicPlay?.Invoke(steamMusic);
        }

        public static void MusicPause()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamMusic))
            {
                return;
            }

            var steamMusic = GetAdditionalInterfacePointer(InterfaceSteamMusic);
            _steamMusicPause?.Invoke(steamMusic);
        }

        public static void MusicPlayNext()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamMusic))
            {
                return;
            }

            var steamMusic = GetAdditionalInterfacePointer(InterfaceSteamMusic);
            _steamMusicPlayNext?.Invoke(steamMusic);
        }

        public static void MusicPlayPrevious()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamMusic))
            {
                return;
            }

            var steamMusic = GetAdditionalInterfacePointer(InterfaceSteamMusic);
            _steamMusicPlayPrevious?.Invoke(steamMusic);
        }

        public static float MusicGetVolume()
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamMusic))
            {
                return 0f;
            }

            var steamMusic = GetAdditionalInterfacePointer(InterfaceSteamMusic);
            return _steamMusicGetVolume?.Invoke(steamMusic) ?? 0f;
        }

        public static void MusicSetVolume(float volume)
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamMusic))
            {
                return;
            }

            var steamMusic = GetAdditionalInterfacePointer(InterfaceSteamMusic);
            _steamMusicSetVolume?.Invoke(steamMusic, volume);
        }

        public static void VideoGetVideoUrl(uint videoAppId)
        {
            if (!EnsureAdditionalInterfaceReady(InterfaceSteamVideo))
            {
                return;
            }

            var steamVideo = GetAdditionalInterfacePointer(InterfaceSteamVideo);
            _steamVideoGetVideoUrl?.Invoke(steamVideo, videoAppId);
        }

        public static bool VideoIsBroadcasting(out int numberOfViewers)
        {
            numberOfViewers = 0;

            if (!EnsureAdditionalInterfaceReady(InterfaceSteamVideo))
            {
                return false;
            }

            var steamVideo = GetAdditionalInterfacePointer(InterfaceSteamVideo);
            return _steamVideoIsBroadcasting?.Invoke(steamVideo, out numberOfViewers) ?? false;
        }

        public static ulong UserStatsRequestUserStats(ulong steamIdUser)
        {
            if (!EnsureUserStatsReady())
            {
                return 0;
            }

            return _steamUserStatsRequestUserStats?.Invoke(_steamUserStats, steamIdUser) ?? 0;
        }

        public static ulong UserStatsRequestGlobalStats(int historyDays)
        {
            if (!EnsureUserStatsReady())
            {
                return 0;
            }

            return _steamUserStatsRequestGlobalStats?.Invoke(_steamUserStats, historyDays) ?? 0;
        }

        public static bool UserStatsResetAllStats(bool achievementsToo)
        {
            if (!EnsureUserStatsReady())
            {
                return false;
            }

            return _steamUserStatsResetAllStats?.Invoke(_steamUserStats, achievementsToo) ?? false;
        }

        public static bool UserStatsGetStatInt32(string name, out int data)
        {
            data = 0;

            if (!EnsureUserStatsReady())
            {
                return false;
            }

            return _steamUserStatsGetStatInt32?.Invoke(_steamUserStats, name, out data) ?? false;
        }

        public static bool UserStatsSetStatInt32(string name, int data)
        {
            if (!EnsureUserStatsReady())
            {
                return false;
            }

            return _steamUserStatsSetStatInt32?.Invoke(_steamUserStats, name, data) ?? false;
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

        private static bool EnsureFriendsReady()
        {
            if (!EnsureLoaded() || !_initialized)
            {
                return false;
            }

            if (_steamFriends == IntPtr.Zero)
            {
                CacheInterfaces();
            }

            return _steamFriends != IntPtr.Zero;
        }

        private static bool EnsureUserStatsReady()
        {
            if (!EnsureLoaded() || !_initialized)
            {
                return false;
            }

            if (_steamUserStats == IntPtr.Zero)
            {
                CacheInterfaces();
            }

            return _steamUserStats != IntPtr.Zero;
        }

        private static bool EnsureUgcReady()
        {
            if (!EnsureLoaded())
            {
                return false;
            }

            if (!_initialized)
            {
                return false;
            }

            if (_steamUgc == IntPtr.Zero)
            {
                CacheInterfaces();
            }

            return _steamUgc != IntPtr.Zero;
        }

        private static bool EnsureRemotePlayReady()
        {
            if (!EnsureLoaded())
            {
                return false;
            }

            if (!_initialized)
            {
                return false;
            }

            if (_steamRemotePlay == IntPtr.Zero)
            {
                CacheInterfaces();
            }

            return _steamRemotePlay != IntPtr.Zero;
        }

        private static bool EnsureAdditionalInterfaceReady(string key)
        {
            if (!EnsureLoaded() || !_initialized)
            {
                return false;
            }

            if (!AdditionalInterfacePointers.TryGetValue(key, out var pointer) || pointer == IntPtr.Zero)
            {
                CacheAdditionalInterface(key);
                AdditionalInterfacePointers.TryGetValue(key, out pointer);
            }

            return pointer != IntPtr.Zero;
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
            _steamUgcAccessor = BindInterfaceAccessor<SteamUgcAccessorDelegate>("SteamUGC", "ISteamUGC");
            _steamRemotePlayAccessor = BindInterfaceAccessor<SteamRemotePlayAccessorDelegate>("SteamRemotePlay", "ISteamRemotePlay");
            _steamFriendsGetPersonaName = Bind<SteamFriendsGetPersonaNameDelegate>("SteamAPI_ISteamFriends_GetPersonaName");
            _steamUserStatsSetAchievement = Bind<SteamUserStatsSetAchievementDelegate>("SteamAPI_ISteamUserStats_SetAchievement");
            _steamUserStatsStoreStats = Bind<SteamUserStatsStoreStatsDelegate>("SteamAPI_ISteamUserStats_StoreStats");
            _steamUserStatsGetAchievement = Bind<SteamUserStatsGetAchievementDelegate>("SteamAPI_ISteamUserStats_GetAchievement");
            _steamUgcCreateQueryAllRequest = Bind<SteamUgcCreateQueryAllRequestDelegate>("SteamAPI_ISteamUGC_CreateQueryAllUGCRequest");
            _steamUgcSendQueryRequest = Bind<SteamUgcSendQueryRequestDelegate>("SteamAPI_ISteamUGC_SendQueryUGCRequest");
            _steamUgcReleaseQueryRequest = Bind<SteamUgcReleaseQueryRequestDelegate>("SteamAPI_ISteamUGC_ReleaseQueryUGCRequest");
            _steamUgcGetNumSubscribedItems = Bind<SteamUgcGetNumSubscribedItemsDelegate>("SteamAPI_ISteamUGC_GetNumSubscribedItems");
            _steamUgcGetSubscribedItems = Bind<SteamUgcGetSubscribedItemsDelegate>("SteamAPI_ISteamUGC_GetSubscribedItems");
            _steamUgcSubscribeItem = Bind<SteamUgcSubscribeItemDelegate>("SteamAPI_ISteamUGC_SubscribeItem");
            _steamUgcUnsubscribeItem = Bind<SteamUgcUnsubscribeItemDelegate>("SteamAPI_ISteamUGC_UnsubscribeItem");
            _steamUgcCreateItem = Bind<SteamUgcCreateItemDelegate>("SteamAPI_ISteamUGC_CreateItem");
            _steamUgcStartItemUpdate = Bind<SteamUgcStartItemUpdateDelegate>("SteamAPI_ISteamUGC_StartItemUpdate");
            _steamUgcSetItemTitle = Bind<SteamUgcSetItemTitleDelegate>("SteamAPI_ISteamUGC_SetItemTitle");
            _steamUgcSetItemDescription = Bind<SteamUgcSetItemDescriptionDelegate>("SteamAPI_ISteamUGC_SetItemDescription");
            _steamUgcSetItemMetadata = Bind<SteamUgcSetItemMetadataDelegate>("SteamAPI_ISteamUGC_SetItemMetadata");
            _steamUgcSetItemVisibility = Bind<SteamUgcSetItemVisibilityDelegate>("SteamAPI_ISteamUGC_SetItemVisibility");
            _steamUgcSetItemTags = Bind<SteamUgcSetItemTagsDelegate>("SteamAPI_ISteamUGC_SetItemTags");
            _steamUgcSetItemContent = Bind<SteamUgcSetItemContentDelegate>("SteamAPI_ISteamUGC_SetItemContent");
            _steamUgcSetItemPreview = Bind<SteamUgcSetItemPreviewDelegate>("SteamAPI_ISteamUGC_SetItemPreview");
            _steamUgcSubmitItemUpdate = Bind<SteamUgcSubmitItemUpdateDelegate>("SteamAPI_ISteamUGC_SubmitItemUpdate");
            _steamUgcGetItemUpdateProgress = Bind<SteamUgcGetItemUpdateProgressDelegate>("SteamAPI_ISteamUGC_GetItemUpdateProgress");
            _steamRemotePlayGetSessionCount = Bind<SteamRemotePlayGetSessionCountDelegate>("SteamAPI_ISteamRemotePlay_GetSessionCount");
            _steamRemotePlayGetSessionId = Bind<SteamRemotePlayGetSessionIdDelegate>("SteamAPI_ISteamRemotePlay_GetSessionID");
            _steamRemotePlayGetSessionSteamId = Bind<SteamRemotePlayGetSessionSteamIdDelegate>("SteamAPI_ISteamRemotePlay_GetSessionSteamID");
            _steamRemotePlayGetSessionClientName = Bind<SteamRemotePlayGetSessionClientNameDelegate>("SteamAPI_ISteamRemotePlay_GetSessionClientName");
            _steamRemotePlayGetSessionClientFormFactor = Bind<SteamRemotePlayGetSessionClientFormFactorDelegate>("SteamAPI_ISteamRemotePlay_GetSessionClientFormFactor");
            _steamRemotePlayGetSessionClientResolution = Bind<SteamRemotePlayGetSessionClientResolutionDelegate>("SteamAPI_ISteamRemotePlay_BGetSessionClientResolution");
            _steamFriendsGetFriendCount = Bind<SteamFriendsGetFriendCountDelegate>("SteamAPI_ISteamFriends_GetFriendCount");
            _steamFriendsGetFriendPersonaName = Bind<SteamFriendsGetFriendPersonaNameDelegate>("SteamAPI_ISteamFriends_GetFriendPersonaName");
            _steamFriendsActivateGameOverlayInviteDialog = Bind<SteamFriendsActivateGameOverlayInviteDialogDelegate>("SteamAPI_ISteamFriends_ActivateGameOverlayInviteDialog");
            _steamFriendsSetRichPresence = Bind<SteamFriendsSetRichPresenceDelegate>("SteamAPI_ISteamFriends_SetRichPresence");
            _steamFriendsClearRichPresence = Bind<SteamFriendsClearRichPresenceDelegate>("SteamAPI_ISteamFriends_ClearRichPresence");
            _steamFriendsGetFriendRichPresence = Bind<SteamFriendsGetFriendRichPresenceDelegate>("SteamAPI_ISteamFriends_GetFriendRichPresence");
            _steamFriendsInviteUserToGame = Bind<SteamFriendsInviteUserToGameDelegate>("SteamAPI_ISteamFriends_InviteUserToGame");
            _steamUtilsGetAppId = Bind<SteamUtilsGetAppIdDelegate>("SteamAPI_ISteamUtils_GetAppID");
            _steamUtilsGetIpCountry = Bind<SteamUtilsGetIpCountryDelegate>("SteamAPI_ISteamUtils_GetIPCountry");
            _steamUtilsIsOverlayEnabled = Bind<SteamUtilsIsOverlayEnabledDelegate>("SteamAPI_ISteamUtils_IsOverlayEnabled");
            _steamRemoteStorageFileExists = Bind<SteamRemoteStorageFileExistsDelegate>("SteamAPI_ISteamRemoteStorage_FileExists");
            _steamRemoteStorageGetFileCount = Bind<SteamRemoteStorageGetFileCountDelegate>("SteamAPI_ISteamRemoteStorage_GetFileCount");
            _steamRemoteStorageIsCloudEnabledForApp = Bind<SteamRemoteStorageIsCloudEnabledForAppDelegate>("SteamAPI_ISteamRemoteStorage_IsCloudEnabledForApp");
            _steamRemoteStorageFileWrite = Bind<SteamRemoteStorageFileWriteDelegate>("SteamAPI_ISteamRemoteStorage_FileWrite");
            _steamRemoteStorageFileRead = Bind<SteamRemoteStorageFileReadDelegate>("SteamAPI_ISteamRemoteStorage_FileRead");
            _steamRemoteStorageFileDelete = Bind<SteamRemoteStorageFileDeleteDelegate>("SteamAPI_ISteamRemoteStorage_FileDelete");
            _steamRemoteStorageGetFileSize = Bind<SteamRemoteStorageGetFileSizeDelegate>("SteamAPI_ISteamRemoteStorage_GetFileSize");
            _steamInputInit = Bind<SteamInputInitDelegate>("SteamAPI_ISteamInput_Init");
            _steamInputShutdown = Bind<SteamInputShutdownDelegate>("SteamAPI_ISteamInput_Shutdown");
            _steamInputRunFrame = Bind<SteamInputRunFrameDelegate>("SteamAPI_ISteamInput_RunFrame");
            _steamInventoryLoadItemDefinitions = Bind<SteamInventoryLoadItemDefinitionsDelegate>("SteamAPI_ISteamInventory_LoadItemDefinitions");
            _steamInventoryGetAllItems = Bind<SteamInventoryGetAllItemsDelegate>("SteamAPI_ISteamInventory_GetAllItems");
            _steamInventoryDestroyResult = Bind<SteamInventoryDestroyResultDelegate>("SteamAPI_ISteamInventory_DestroyResult");
            _steamMatchmakingRequestLobbyList = Bind<SteamMatchmakingRequestLobbyListDelegate>("SteamAPI_ISteamMatchmaking_RequestLobbyList");
            _steamMatchmakingAddRequestLobbyListResultCountFilter = Bind<SteamMatchmakingAddRequestLobbyListResultCountFilterDelegate>("SteamAPI_ISteamMatchmaking_AddRequestLobbyListResultCountFilter");
            _steamMatchmakingCreateLobby = Bind<SteamMatchmakingCreateLobbyDelegate>("SteamAPI_ISteamMatchmaking_CreateLobby");
            _steamMatchmakingJoinLobby = Bind<SteamMatchmakingJoinLobbyDelegate>("SteamAPI_ISteamMatchmaking_JoinLobby");
            _steamMatchmakingLeaveLobby = Bind<SteamMatchmakingLeaveLobbyDelegate>("SteamAPI_ISteamMatchmaking_LeaveLobby");
            _steamMatchmakingGetNumLobbyMembers = Bind<SteamMatchmakingGetNumLobbyMembersDelegate>("SteamAPI_ISteamMatchmaking_GetNumLobbyMembers");
            _steamScreenshotsAddScreenshotToLibrary = Bind<SteamScreenshotsAddScreenshotToLibraryDelegate>("SteamAPI_ISteamScreenshots_AddScreenshotToLibrary");
            _steamScreenshotsTriggerScreenshot = Bind<SteamScreenshotsTriggerScreenshotDelegate>("SteamAPI_ISteamScreenshots_TriggerScreenshot");
            _steamScreenshotsHookScreenshots = Bind<SteamScreenshotsHookScreenshotsDelegate>("SteamAPI_ISteamScreenshots_HookScreenshots");
            _steamScreenshotsIsScreenshotsHooked = Bind<SteamScreenshotsIsScreenshotsHookedDelegate>("SteamAPI_ISteamScreenshots_IsScreenshotsHooked");
            _steamMusicIsEnabled = Bind<SteamMusicIsEnabledDelegate>("SteamAPI_ISteamMusic_BIsEnabled");
            _steamMusicIsPlaying = Bind<SteamMusicIsPlayingDelegate>("SteamAPI_ISteamMusic_BIsPlaying");
            _steamMusicPlay = Bind<SteamMusicPlayDelegate>("SteamAPI_ISteamMusic_Play");
            _steamMusicPause = Bind<SteamMusicPauseDelegate>("SteamAPI_ISteamMusic_Pause");
            _steamMusicPlayNext = Bind<SteamMusicPlayNextDelegate>("SteamAPI_ISteamMusic_PlayNext");
            _steamMusicPlayPrevious = Bind<SteamMusicPlayPreviousDelegate>("SteamAPI_ISteamMusic_PlayPrevious");
            _steamMusicGetVolume = Bind<SteamMusicGetVolumeDelegate>("SteamAPI_ISteamMusic_GetVolume");
            _steamMusicSetVolume = Bind<SteamMusicSetVolumeDelegate>("SteamAPI_ISteamMusic_SetVolume");
            _steamVideoGetVideoUrl = Bind<SteamVideoGetVideoUrlDelegate>("SteamAPI_ISteamVideo_GetVideoURL");
            _steamVideoIsBroadcasting = Bind<SteamVideoIsBroadcastingDelegate>("SteamAPI_ISteamVideo_IsBroadcasting");
            _steamUserStatsRequestUserStats = Bind<SteamUserStatsRequestUserStatsDelegate>("SteamAPI_ISteamUserStats_RequestUserStats");
            _steamUserStatsRequestGlobalStats = Bind<SteamUserStatsRequestGlobalStatsDelegate>("SteamAPI_ISteamUserStats_RequestGlobalStats");
            _steamUserStatsResetAllStats = Bind<SteamUserStatsResetAllStatsDelegate>("SteamAPI_ISteamUserStats_ResetAllStats");
            _steamUserStatsGetStatInt32 = Bind<SteamUserStatsGetStatInt32Delegate>("SteamAPI_ISteamUserStats_GetStatInt32");
            _steamUserStatsSetStatInt32 = Bind<SteamUserStatsSetStatInt32Delegate>("SteamAPI_ISteamUserStats_SetStatInt32");

            // Additional interface accessors requested for broad API coverage.
            BindAdditionalInterfaceAccessor(InterfaceSteamController, "SteamController", "ISteamController");
            BindAdditionalInterfaceAccessor(InterfaceSteamGameCoordinator, "SteamGameCoordinator", "ISteamGameCoordinator");
            BindAdditionalInterfaceAccessor(InterfaceSteamGameServer, "SteamGameServer", "ISteamGameServer");
            BindAdditionalInterfaceAccessor(InterfaceSteamGameServerStats, "SteamGameServerStats", "ISteamGameServerStats");
            BindAdditionalInterfaceAccessor(InterfaceSteamHtmlSurface, "SteamHTMLSurface", "ISteamHTMLSurface");
            BindAdditionalInterfaceAccessor(InterfaceSteamInput, "SteamInput", "ISteamInput");
            BindAdditionalInterfaceAccessor(InterfaceSteamInventory, "SteamInventory", "ISteamInventory");
            BindAdditionalInterfaceAccessor(InterfaceSteamMatchmaking, "SteamMatchmaking", "ISteamMatchmaking");
            BindAdditionalInterfaceAccessor(InterfaceSteamMatchmakingServers, "SteamMatchmakingServers", "ISteamMatchmakingServers");
            BindAdditionalInterfaceAccessor(InterfaceSteamMusic, "SteamMusic", "ISteamMusic");
            BindAdditionalInterfaceAccessor(InterfaceSteamParties, "SteamParties", "ISteamParties");
            BindAdditionalInterfaceAccessor(InterfaceSteamRemoteStorage, "SteamRemoteStorage", "ISteamRemoteStorage");
            BindAdditionalInterfaceAccessor(InterfaceSteamScreenshots, "SteamScreenshots", "ISteamScreenshots");
            BindAdditionalInterfaceAccessor(InterfaceSteamUtils, "SteamUtils", "ISteamUtils");
            BindAdditionalInterfaceAccessor(InterfaceSteamVideo, "SteamVideo", "ISteamVideo");
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
            _steamUgc = _steamUgcAccessor?.Invoke() ?? IntPtr.Zero;
            _steamRemotePlay = _steamRemotePlayAccessor?.Invoke() ?? IntPtr.Zero;

            CacheAdditionalInterfaces();
        }

        private static void BindAdditionalInterfaceAccessor(string key, string friendlyName, string interfaceName)
        {
            AdditionalInterfaceAccessors[key] = BindInterfaceAccessor<SteamInterfaceAccessorDelegate>(friendlyName, interfaceName);
        }

        private static void CacheAdditionalInterfaces()
        {
            foreach (var key in AdditionalInterfaceAccessors.Keys)
            {
                CacheAdditionalInterface(key);
            }
        }

        private static void CacheAdditionalInterface(string key)
        {
            if (!AdditionalInterfaceAccessors.TryGetValue(key, out var accessor))
            {
                AdditionalInterfacePointers[key] = IntPtr.Zero;
                return;
            }

            AdditionalInterfacePointers[key] = accessor?.Invoke() ?? IntPtr.Zero;
        }

        private static IntPtr GetAdditionalInterfacePointer(string key)
        {
            return AdditionalInterfacePointers.TryGetValue(key, out var pointer) ? pointer : IntPtr.Zero;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SteamParamStringArrayNative
        {
            public IntPtr Strings;
            public int NumStrings;
        }

        private static bool WithSteamStringArray(string[] values, Func<IntPtr, bool> action)
        {
            var stringPointers = new IntPtr[values.Length];
            IntPtr arrayPtr = IntPtr.Zero;
            IntPtr structPtr = IntPtr.Zero;

            try
            {
                for (var i = 0; i < values.Length; i++)
                {
                    stringPointers[i] = Marshal.StringToHGlobalAnsi(values[i] ?? string.Empty);
                }

                arrayPtr = Marshal.AllocHGlobal(IntPtr.Size * values.Length);
                Marshal.Copy(stringPointers, 0, arrayPtr, values.Length);

                var native = new SteamParamStringArrayNative
                {
                    Strings = arrayPtr,
                    NumStrings = values.Length
                };

                structPtr = Marshal.AllocHGlobal(Marshal.SizeOf<SteamParamStringArrayNative>());
                Marshal.StructureToPtr(native, structPtr, false);

                return action(structPtr);
            }
            finally
            {
                if (structPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(structPtr);
                }

                if (arrayPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(arrayPtr);
                }

                for (var i = 0; i < stringPointers.Length; i++)
                {
                    if (stringPointers[i] != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(stringPointers[i]);
                    }
                }
            }
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

