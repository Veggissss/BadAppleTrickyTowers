using System;

public class BadAppleGameModeFactory : AbstractSinglePlayerGameModeFactory
{
    public BadAppleGameModeFactory()
    {
        //ambientAudio = new string[2] { "AMBIENCE_WATER", "AMBIENCE_BREEZE" }; // TODO: needed?
        musicAudio = new MusicStruct[1]
        {
            new MusicStruct("MUSIC_RACE", 0.9f) // TODO: change to bad apple track
        };
        
        worldId = 0;
    }

    protected override AbstractGameMode _CreateGameMode()
    {
        BadAppleGameMode singlePlayerRaceGameMode = new BadAppleGameMode();
        return singlePlayerRaceGameMode;
    }
}
