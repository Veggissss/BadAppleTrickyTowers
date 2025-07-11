using System;

public class BadAppleGameModeFactory : AbstractSinglePlayerGameModeFactory
{
    private readonly float _targetHeight;
    private readonly float _targetTime;
    public float targetHeight => _targetHeight;

    public BadAppleGameModeFactory(float targetHeight, float targetTime)
    {
        _targetHeight = targetHeight;
        _targetTime = targetTime;
        ambientAudio = new string[2] { "AMBIENCE_WATER", "AMBIENCE_BREEZE" }; // TODO: needed?
        musicAudio = new MusicStruct[1]
        {
            new MusicStruct("MUSIC_RACE", 1f) // TODO: change to bad apple track
        };
        backgroundFactory = new BackgroundsFactory(new Type[5]
        {
            typeof(RaceBackground),
            typeof(FinishLineSingleBackground),
            typeof(WinnerHighlightBackground),
            typeof(RaceForeground),
            typeof(LoserHighlightBackground)
        });
        worldId = 0;
    }

    protected override AbstractGameMode _CreateGameMode()
    {
        BadAppleGameMode singlePlayerRaceGameMode = new BadAppleGameMode
        {
            targetHeight = _targetHeight,
            targetTime = _targetTime
        };
        return singlePlayerRaceGameMode;
    }
}
