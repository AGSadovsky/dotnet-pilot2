public class Util
{
    private GameSettings _gameSettings;
	public Util(GameSettings gameSettings)
	{
        _gameSettings = gameSettings;
	}

    public int GetNextPlayer(int currentPlayer)
    {
        if (currentPlayer < _gameSettings.PlayersNumber)
            currentPlayer++;
        else
            currentPlayer = 1;

        return currentPlayer;
    }
        
}
