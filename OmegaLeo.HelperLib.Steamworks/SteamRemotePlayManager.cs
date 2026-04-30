using OmegaLeo.HelperLib.Shared.Attributes;

namespace OmegaLeo.HelperLib.Steamworks
{
    [Documentation(nameof(SteamRemotePlayManager), "Provides helper methods for Steam Remote Play session inspection.", null,
        @"```csharp
var remotePlay = SteamManager.Instance.RemotePlay;
var sessionCount = remotePlay.GetSessionCount();
```")]
    public class SteamRemotePlayManager
    {
        [Documentation(nameof(GetSessionCount), "Gets the number of active Remote Play sessions for the current user.", null, null)]
        public uint GetSessionCount()
        {
            return SteamNativeApi.GetRemotePlaySessionCount();
        }

        [Documentation(nameof(GetSessionId), "Gets a Remote Play session ID by zero-based session index.", new[] { "sessionIndex: Zero-based index into the active sessions list." }, null)]
        public uint GetSessionId(int sessionIndex)
        {
            return SteamNativeApi.GetRemotePlaySessionId(sessionIndex);
        }

        [Documentation(nameof(GetSessionSteamId), "Gets the SteamID for the remote user in a session.", new[] { "sessionId: Remote Play session ID." }, null)]
        public ulong GetSessionSteamId(uint sessionId)
        {
            return SteamNativeApi.GetRemotePlaySessionSteamId(sessionId);
        }

        [Documentation(nameof(GetSessionClientName), "Gets the client device name for a remote session.", new[] { "sessionId: Remote Play session ID." }, null)]
        public string GetSessionClientName(uint sessionId)
        {
            return SteamNativeApi.GetRemotePlaySessionClientName(sessionId);
        }

        [Documentation(nameof(GetSessionClientFormFactor), "Gets the client form-factor value for a session (matches Steam's ESteamDeviceFormFactor numeric values).", new[] { "sessionId: Remote Play session ID." }, null)]
        public int GetSessionClientFormFactor(uint sessionId)
        {
            return SteamNativeApi.GetRemotePlaySessionClientFormFactor(sessionId);
        }

        [Documentation(nameof(TryGetSessionClientResolution), "Gets the streaming client resolution for a session.", new[] { "sessionId: Remote Play session ID.", "resolutionX: Width output.", "resolutionY: Height output." }, null)]
        public bool TryGetSessionClientResolution(uint sessionId, out int resolutionX, out int resolutionY)
        {
            return SteamNativeApi.TryGetRemotePlaySessionClientResolution(sessionId, out resolutionX, out resolutionY);
        }
    }
}

