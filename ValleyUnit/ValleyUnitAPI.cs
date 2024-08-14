using SmiteUnit;
using SmiteUnit.Injection;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Linkoid.Stardew.ValleyUnit;

public static class ValleyUnitAPI
{
	private static SmiteInjection? activeInjection;

	public static TestsDisposer RegisterTests(IMod mod)
	{
		var smiteInjection = new SmiteInjection(mod.GetType().Assembly);
		return RegisterTests(mod, smiteInjection);
	}

	public static TestsDisposer RegisterTests(IMod mod, string testAssemblyName)
	{
		var smiteInjection = new SmiteInjection(testAssemblyName);
		return RegisterTests(mod, smiteInjection);
	}

	[Obsolete("Specifying a default test will prevent other mods from running tests. Use only for temporary debugging.")]
	public static TestsDisposer RegisterTests(IMod mod, string testAssemblyName, string defaultTestClassName, string defaultTestMethodName)
	{
		var smiteInjection = new SmiteInjection(testAssemblyName);
		var defaultTestId = SmiteId.Method(new(testAssemblyName), defaultTestClassName, defaultTestMethodName);
		return RegisterTests(mod, smiteInjection, defaultTestId);
	}
	
	[Obsolete("Specifying a default test will prevent other mods from running tests. Use only for temporary debugging.")]
	public static TestsDisposer RegisterTests(IMod mod, Delegate? defaultTest = null)
	{
		var smiteInjection = new SmiteInjection(mod.GetType().Assembly);
		var defaultTestId = defaultTest != null ? SmiteId.Method(defaultTest) : null;
		return RegisterTests(mod, smiteInjection, defaultTestId);
	}

	private static TestsDisposer RegisterTests(IMod mod, SmiteInjection smiteInjection, ISmiteId? defaultTestId = null)
	{
		smiteInjection.ExitStrategy = GameRunnerExit;
		smiteInjection.UsingDelayedExitStrategy = true;

		EntryPoint();
		void EntryPoint()
		{
			if (activeInjection != null) return;

			StardewTest.SetMod(mod);
			if (!smiteInjection.EntryPoint())
			{
				if (defaultTestId != null)
				{
					smiteInjection.RunTest(defaultTestId);
					activeInjection = smiteInjection;
				}
				else
				{
					StardewTest.SetMod(null);
				}
			}
			else
			{
				activeInjection = smiteInjection;
			}
		}

		mod.Helper.Events.GameLoop.GameLaunched += OnGameLaunched;
		void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
		{
			//EntryPoint();
		}

		mod.Helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
		void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
		{
			smiteInjection.UpdatePoint();
		}

		return new TestsDisposer(smiteInjection.ExitPoint);
	}

	private static void GameRunnerExit(int exitCode)
	{
		Environment.ExitCode = exitCode;
		GameRunner.instance.Exit();
	}
}
