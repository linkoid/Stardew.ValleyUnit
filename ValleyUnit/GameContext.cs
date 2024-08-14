using StardewValley;

namespace Linkoid.Stardew.ValleyUnit;

public readonly ref struct GameContext
{
	private readonly Game1 currentGame;
	private readonly Game1 previousGame;

	public GameContext(Game1 game)
	{
		previousGame = Game1.game1;
		currentGame = game;
		GameRunner.LoadInstance(currentGame);
	}

	public void Dispose()
	{
		GameRunner.SaveInstance(currentGame);
		GameRunner.LoadInstance(previousGame);
	}
}
