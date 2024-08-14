using HarmonyLib;
using StardewModdingAPI;
using Linkoid.Stardew.ValleyUnit.Assertions;
using StardewValley.SDKs.Steam;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Linkoid.Stardew.ValleyUnit;

public static partial class StardewTest
{
	private static IMod? _mod;
	private static Harmony? _harmony;

	internal static void SetMod(IMod? mod)
	{
		if (mod != null)
		{
			_mod = mod;
			_harmony = new Harmony($"{mod.ModManifest.UniqueID}.{nameof(StardewTest)}");
		}
		else
		{
			_mod = null;
			_harmony = null;
		}
	}

	[MemberNotNull(nameof(_mod), nameof(_harmony))]
	private static void EnsureValidContext()
	{
		if (_mod != null && _harmony != null) return;
		throw new InvalidOperationException("StardewTest methods can only be used when running a test.");
	}

	private static T EnsureValidContextPassthrough<T>(in T? value)
	{
		EnsureValidContext();
		if (value is null)
			throw new InvalidOperationException("StardewTest methods can only be used when running a test.");
		return value;
	}
}
