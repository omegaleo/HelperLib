// See https://aka.ms/new-console-template for more information

// /media/omegaleo/Development/Library Dev/HelperLib

using System;
using System.Collections.Generic;
using OmegaLeo.HelperLib.Steamworks;

public class Program
{
    public static void Main()
    {
        SteamManager.ConfigureAppId(480); // 480 is the AppId for Spacewar, a test app provided by Valve
        
        var steam = SteamManager.Instance;
        if (steam.IsSteamworksInitialized)
        {
            Console.WriteLine($"Welcome {steam.GetSteamName()}");
        }

        // Call this in your game loop (Unity Update, Godot _Process, etc.)
        steam.Update();

        //var achievements = new AchievementManager();
        //achievements.UnlockAchievement("ACH_FIRST_WIN");

        steam.Shutdown();
    }
}