
public class BadAppleGameModeModel : AbstractSinglePlayerGameModeModel
{
    public BadAppleGameModeModel(string idIn, string nameIn, AbstractGameModeFactory gameModeFactoryIn, string explanationIdIn = null)
        : base(idIn, nameIn, gameModeFactoryIn, 0, explanationIdIn)
    {
    }
}
