﻿using Linkoid.Stardew.ValleyUnit;
using SmiteUnit;
using SmiteUnit.Injection;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;

[Microsoft.VisualStudio.TestPlatform.TestSDKAutoGeneratedCode]
internal sealed partial class TestMod : Mod
{
	public static TestMod Instance { get; private set; } = null!;

	private TestsDisposer testsDisposer;

	public override void Entry(IModHelper helper)
	{
		Instance = this;
		testsDisposer = ValleyUnitAPI.RegisterTests(this);
		OnEntry(helper);
	}

	partial void OnEntry(IModHelper helper);

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			testsDisposer.Dispose();
		}
		OnDispose(disposing);
	}
	partial void OnDispose(bool disposing);
}
