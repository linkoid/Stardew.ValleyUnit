using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Linkoid.Stardew.ValleyUnit.Assertions;

public class Assert
{
    internal Assert() { }

	[DoesNotReturn]
    public void Fail(string message)
    {
        throw new AssertionException(message);
    }

	public void That([DoesNotReturnIf(false)] bool condition,
		[CallerArgumentExpression(nameof(condition))] string? expression = "condition")
	{
		if (condition) return;
		throw new AssertionException($"Assert.That({expression}) failed.");
	}

	public void AreEqual(object? expected, object? actual,
		[CallerArgumentExpression(nameof(expected))] string expectedExpression = "expected",
		[CallerArgumentExpression(nameof(actual))] string actualExpression = "actual")
	{
		if (Equals(expected, actual)) return;
		throw new AssertionException(
			$"Assert.AreEqual({expectedExpression}, {actualExpression}) failed.\n" +
			$"Expected: {Repr(expected)}\n" +
			$"Actual: {Repr(actual)}"
		);

		static string Repr(object? obj)
		{
			if (obj is null) return "null";
			if (obj is string s) return $"\"{s}\"";
			return obj.ToString() ?? string.Empty;
		}
	}
}
