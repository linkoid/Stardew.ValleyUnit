using SmiteUnit;
using SmiteUnit.Injection;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;

namespace Linkoid.Stardew.ValleyUnit.Tests.TestMod;

public sealed class StardewUnitTestMod : Mod
{
	public static StardewUnitTestMod Instance { get; private set; } = null!;
	public static new IModHelper Helper => (Instance as Mod).Helper;
	public static new IMonitor Monitor => (Instance as Mod).Monitor;

	private TestsDisposer testsDisposer;

	public override void Entry(IModHelper helper)
	{
		Instance = this;

		Monitor.Log(Environment.CommandLine);

		testsDisposer = ValleyUnitAPI.RegisterTests(this);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			testsDisposer.Dispose();
		}
	}
}
