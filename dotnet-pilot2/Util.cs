namespace DotnetPilot2
{
    public class Util(GameSettings gameSettings)
    {
        private GameSettings _gameSettings = gameSettings;

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

        public void ProcessUserCommand(string userCommand, HashSet<string> dictionary, List<GameResult> archivedGameResults, List<Player> players)
        {
            switch (userCommand)
            {
                case "/show-words":
                    PrintWords(dictionary);
                    break;
                case "/score":
                    PrintScore(archivedGameResults, players);
                    break;
                case "/total-score":
                    PrintTotalScore(archivedGameResults);
                    break;
                default:
                    Console.WriteLine(string.Format(Messages.UserCommandNotImplemented, userCommand));
                    break;
            }
        }

        private static void PrintWords(HashSet<string> dictionary)
        {
            Console.WriteLine(Messages.CurrentGameWords);
            foreach (var word in dictionary)
                Console.WriteLine(word);
        }

        ///<summary>
        ///Prints number of wins for current game players
        ///</summary>
        private static void PrintScore(List<GameResult> archivedGameResults, List<Player> players)
        {
            Dictionary<string, int> winners = new(2);

            string player1Name = players[0].GetName();
            string player2Name = players[1].GetName();

            foreach (var player in players)
            {
                winners.Add(player.GetName(), 0);
            }

            foreach (var result in archivedGameResults)
            {
                if ((result.Winner == player1Name) || (result.Winner == player2Name))
                {
                    winners[result.Winner]++;
                }
            }

            Console.WriteLine(Messages.CurrentPlayersStatistics);
            foreach (var winner in winners)
                Console.WriteLine(string.Format(Messages.TotalScore, winner.Key, winner.Value));
        }

        ///<summary>
        ///Prints leaderboard in descending order
        ///</summary>
        private static void PrintTotalScore(List<GameResult> archivedGameResults)
        {
            Dictionary<string, int> winners = new(archivedGameResults.Count);
            foreach (var result in archivedGameResults)
            {
                if (!winners.TryAdd(result.Winner, 1))
                    winners[result.Winner]++;
            }

            Console.WriteLine(Messages.Leaderboard);
            foreach (var winner in winners.OrderByDescending(winner => winner.Value))
                Console.WriteLine($"{winner.Key}: {winner.Value}");
        }
    }
}