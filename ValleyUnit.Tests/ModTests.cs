using Linkoid.Stardew.Tasks;
using SmiteUnit.Framework;
using StardewModdingAPI;
using System.Text;
using System.Threading.Tasks;
using System;
using StardewValley;
using StardewModdingAPI.Utilities;

namespace Linkoid.Stardew.ValleyUnit;

[SmiteProcess(@"S:\SteamLibrary\steamapps\common\Stardew Valley\StardewModdingAPI.exe",
	OutputEncoding = nameof(Encoding.Unicode))]
public static class ModTests
{
	[SmiteTest]
	public static async Task WaitForFullDay()
	{
		StardewTest.Monitor.Log("WaitForGameLaunched()");
		await StardewTest.Events.GameLoop.WaitForGameLaunched();

		StardewTest.Monitor.Log("LoadGame()");
		StardewTest.LoadGame("StardewTest_382729285");

		StardewTest.Monitor.Log("WaitForDayStarted()");
		await StardewTest.Events.GameLoop.WaitForDayStarted();

		//StardewTest.Monitor.Log("StartLocalFarmhand()");
		//await StardewTest.StartLocalFarmhand();

		while (Game1.timeOfDay < 250)
		{
			await StardewTest.Events.GameLoop.WaitForUpdateTicking();
			for (int i = 0; i < 60; i++)
			{
				Game1.game1.Instance_Update(Game1.currentGameTime);
			}
		}

		StardewTest.Monitor.Log("WaitForDayEnding()");
		await StardewTest.Events.GameLoop.WaitForDayEnding();
	}

	[SmiteTest]
	public static async Task EndlessTest()
	{
		while (true)
		{
			await StardewTest.Events.GameLoop.WaitForUpdateTicking();
		}
	}
}
