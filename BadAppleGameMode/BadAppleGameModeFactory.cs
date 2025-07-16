using System;

public class BadAppleGameModeFactory : AbstractSinglePlayerGameModeFactory
{
    public BadAppleGameModeFactory()
    {
        worldId = 0;
    }

    protected override AbstractGameMode _CreateGameMode()
    {
        BadAppleGameMode singlePlayerRaceGameMode = new BadAppleGameMode();
        return singlePlayerRaceGameMode;
    }
}
