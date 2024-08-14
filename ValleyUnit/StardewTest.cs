using HarmonyLib;
using Linkoid.Stardew.ValleyUnit.Assertions;
using Linkoid.Stardew.Tasks;
using StardewModdingAPI;
using StardewValley.SDKs.Steam;
using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley.Menus;
using StardewValley;
using System.Reflection;
using System.Linq;
using StardewModdingAPI.Events;
using System.Collections.Generic;
using System.IO;

namespace Linkoid.Stardew.ValleyUnit;

public static partial class StardewTest
{
	public static IMod Mod => EnsureValidContextPassthrough(_mod);
	public static Harmony Harmony => EnsureValidContextPassthrough(_harmony);
	public static IModHelper Helper => Mod.Helper;
	public static IMonitor Monitor => Mod.Monitor;
	public static IModEvents Events => Helper.Events;

	public static readonly Assert Assert = new();

	public static void DisableSteamAchievements()
	{
		EnsureValidContext();
		bool DontGetAchievement() { return false; }
		Harmony.Patch(
			AccessTools.DeclaredMethod(typeof(SteamHelper), nameof(SteamHelper.GetAchievement)),
			prefix: new(((Delegate)DontGetAchievement).Method));
	}

	public static void LoadGame(string filename)
	{
		TitleMenu titleMenu = new();
		Game1.activeClickableMenu = titleMenu;
		titleMenu.skipToTitleButtons();

		SaveGame.Load(filename);
		Game1.exitActiveMenu();
	}

	public static async ValueTask LoadGameOnGameLaunched(string filename)
	{
		EnsureValidContext();
		await Helper.Events.GameLoop.WaitForGameLaunched();
		LoadGame(filename);
	}

	private static MethodInfo? _tempPatch;
	public static async ValueTask StartLocalFarmhand()
	{
		EnsureValidContext();

		if (!Game1.hasLoadedGame)
			throw new InvalidOperationException("Cannot start local farmhand before game has loaded.");

		GameRunner.instance.gameInstances[0].instancePlayerOneIndex = (PlayerIndex)(-1);
		GameRunner.instance.AddGameInstance(PlayerIndex.One);

		_tempPatch = Harmony.Patch(AccessTools.Method(typeof(FarmhandMenu), "update"),
			prefix: new(((Delegate)PickFirstFarmhand).Method));

		static void PickFirstFarmhand(FarmhandMenu __instance)
		{
			var first = __instance.MenuSlots.FirstOrDefault();
			if (first == null) return;
			first.Activate();
			Harmony.Unpatch(AccessTools.Method(typeof(FarmhandMenu), "update"), _tempPatch);
		}

		await Helper.Events.GameLoop.WaitForSaveLoaded();
	}

	public static async ValueTask StartLocalFarmhandOnSaveLoaded()
	{
		EnsureValidContext();
		await Helper.Events.GameLoop.WaitForSaveLoaded();
		await StartLocalFarmhand();
	}
}
