using DotnetPilot2;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        var gameSettings = new GameSettings();
        var players = gameSettings.players;
        var util = new Util(gameSettings);

        Console.WriteLine(string.Format(Messages.BaseWordEnter, gameSettings.MinWordLength, gameSettings.MaxWordLength));
        var baseWord = new BaseWord(Console.ReadLine() ?? string.Empty, gameSettings);

        if (baseWord.IsValid())
        {
            Console.WriteLine(string.Format(Messages.BaseWordInfo, baseWord.Get()));
            StartGame(baseWord.Get());
        }
        else
            Console.WriteLine(Messages.InitialCriteriaError);

        void StartGame(string validBaseWord)
        {
            var dictionary = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { validBaseWord };
            var baseWordLetters = baseWord.SplitIntoLetters();
            var currentGameResult = new GameResult();
            var archivedGameResults = currentGameResult.LoadStatistics();

            int currentPlayerId = -1;
            bool canContinueGame = true;
 
            //play until a user enters an incorrect word or the timer is up
            while (canContinueGame)
            {
                currentPlayerId = util.GetNextPlayer(currentPlayerId);
                Console.WriteLine(string.Format(Messages.PlayerTurn, players[currentPlayerId].GetName()));

                var consoleReader = new ConsoleReaderOnTimer(gameSettings.TimerDurationMs);
                var userInput = new UserWord(consoleReader.ReadLine() ?? string.Empty);

                if (userInput.IsCommand())
                {
                    util.ProcessUserCommand(userInput.Get(), dictionary, archivedGameResults, players);
                    currentPlayerId = util.GetNextPlayer(currentPlayerId);
                }
                else if (userInput.IsNotEmpty())//empty line produces TimeIsUp error - ToDo: fix
                {
                    canContinueGame = userInput.IsValid(dictionary, baseWordLetters, out string errorMessage);
                    Console.Write(errorMessage);
                }
                else
                {
                    canContinueGame = false;
                    Console.Write($"\n{Messages.TimeIsUp}");
                }
            }

            currentGameResult.SaveStatistics(new GameResult(players, dictionary, players[util.GetWinner(currentPlayerId)].GetName()));
            Console.Write($"\n{string.Format(Messages.PlayerLost, players[currentPlayerId].GetName())}");
        }
    }
}