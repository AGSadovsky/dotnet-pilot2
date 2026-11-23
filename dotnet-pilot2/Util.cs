public class Util
{
    private GameSettings _gameSettings;

	public Util(GameSettings gameSettings)
	{
        _gameSettings = gameSettings;
	}

    public int GetNextPlayer(int currentPlayer)
    {
        if (currentPlayer < _gameSettings.PlayersNumber - 1)
            currentPlayer++;
        else
            currentPlayer = 0;

        return currentPlayer;
    }

    public int GetWinner(int loser)
    {
        return GetNextPlayer(loser);
    }
    
}
