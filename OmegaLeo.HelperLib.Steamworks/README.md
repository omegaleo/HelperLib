# OmegaLeo's Helper Library - Steamworks
Lightweight Steamworks helper library for C# game projects.

This package provides a simple Steam API wrapper for common tasks like initialization, callback processing, player name lookup, and achievements.

## Features
- Simple Steam initialization via `SteamManager.Instance`
- Per-frame callback runner with `SteamManager.Update()`
- Safe shutdown with `SteamManager.Shutdown()`
- Player persona name access with `SteamManager.GetSteamName()`
- Achievement helpers via `AchievementManager`
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

steam.Shutdown();
```

## Notes
- Steam must be running and your app must be launched in a valid Steam context.
- Configure your AppId before first access to `SteamManager.Instance`.
- `SteamManager.Update()` should be called continuously during runtime.
- If Steam is unavailable, `GetSteamName()` falls back to `"Player"`.

## License
Open source under the AGPL v3 License. See the repository `LICENSE` for details.

