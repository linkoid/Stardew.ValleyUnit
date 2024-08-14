using System;
using System.Collections.Generic;
using System.Text;

namespace Linkoid.Stardew.ValleyUnit;

/// <summary>
/// Any <see cref="TestsDisposer"/>s recieved should be disposed when the mod is disposed.
/// </summary>
public readonly struct TestsDisposer : IDisposable
{
	private readonly Action OnDispose;

	internal TestsDisposer(Action onDispose)
	{
		OnDispose = onDispose;
	}

	public void Dispose()
	{
		OnDispose?.Invoke();
	}
}
