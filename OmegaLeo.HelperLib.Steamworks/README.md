# OmegaLeo's Helper Library - Steamworks
Lightweight Steamworks helper library for C# game projects.

This package provides a simple Steam API wrapper for common tasks like initialization, callback processing, player name lookup, and achievements.

## Features
- Simple Steam initialization via `SteamManager.Instance`
- Per-frame callback runner with `SteamManager.Update()`
- Safe shutdown with `SteamManager.Shutdown()`
- Player persona name access with `SteamManager.GetSteamName()`
- Achievement helpers via `AchievementManager`
- Steam Workshop (UGC) helpers via `SteamManager.Instance.Ugc`
- Steam Remote Play helpers via `SteamManager.Instance.RemotePlay`
- Additional interface managers: Controller, Friends, GameServer, Input, Inventory, Matchmaking, Parties, RemoteStorage, Screenshots, UserStats, Utils, Video, and more.
- Includes native Steam runtime files in NuGet package `runtimes/`

## Installation
```bash
dotnet add package OmegaLeo.HelperLib.Steamworks
```

## Quick Start
```cs
using OmegaLeo.HelperLib.Steamworks;

// Optional but recommended for local/editor runs.
SteamManager.ConfigureAppId(480); // Replace with your real AppId.

var steam = SteamManager.Instance;
if (steam.IsSteamworksInitialized)
{
    Console.WriteLine($"Welcome {steam.GetSteamName()}");
}

// Call this in your game loop (Unity Update, Godot _Process, etc.)
steam.Update();

var achievements = new AchievementManager();
achievements.UnlockAchievement("ACH_FIRST_WIN");

// Steam Workshop (UGC) example
var subscribedIds = steam.Ugc.GetSubscribedItems();

// Steam Remote Play example
var remotePlaySessionCount = steam.RemotePlay.GetSessionCount();

steam.Shutdown();
```

## More API Examples
```cs
var steam = SteamManager.Instance;

// Friends / rich presence
steam.Friends.SetRichPresence("status", "In Match");
steam.Friends.SetRichPresence("map", "Arena_01");
var friendName = steam.Friends.GetFriendPersonaName(76561198000000000UL);
var friendStatus = steam.Friends.GetFriendRichPresence(76561198000000000UL, "status");

// Remote storage
var saveBytes = System.Text.Encoding.UTF8.GetBytes("save-data-v1");
steam.RemoteStorage.FileWrite("save_slot_1.dat", saveBytes);
if (steam.RemoteStorage.FileRead("save_slot_1.dat", out var loadedBytes))
{
    var loadedText = System.Text.Encoding.UTF8.GetString(loadedBytes);
}

// User stats
steam.UserStats.SetStatInt32("KILLS", 25);
steam.UserStats.StoreStats();
steam.UserStats.RequestGlobalStats(7);

// UGC update convenience APIs (existing item)
var options = new SteamUgcItemUpdateOptions
{
    Title = "My Workshop Item",
    Description = "Updated description.",
    Tags = new[] { "map", "coop" },
    ContentFolder = "/absolute/path/to/content",
    PreviewFile = "/absolute/path/to/preview.png"
};
var submitCallHandle = steam.Ugc.UpdateItem(480, 123456789012345678UL, options, "Updated content", out var failedStep);

// Fluent session alternative
var session = steam.Ugc.BeginUpdateSession(480, 123456789012345678UL)
    .WithTitle("My Workshop Item")
    .WithDescription("Updated description")
    .WithTags("map", "coop")
    .WithContentFolder("/absolute/path/to/content")
    .WithPreviewFile("/absolute/path/to/preview.png");
var submitCallHandle2 = session.Submit("Updated content", out var failedStep2);
```

## Notes
- Steam must be running and your app must be launched in a valid Steam context.
- Configure your AppId before first access to `SteamManager.Instance`.
- `SteamManager.Update()` should be called continuously during runtime.
- If Steam is unavailable, `GetSteamName()` falls back to `"Player"`.

## License
Open source under the AGPL v3 License. See the repository `LICENSE` for details.

