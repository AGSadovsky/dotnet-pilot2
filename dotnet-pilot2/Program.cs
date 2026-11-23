using DotnetPilot2;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        var gameSettings = new GameSettings();
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
            var currentGame = new GameResult();
            //var gamesArchive = currentGame.LoadStatistics();

            int currentPlayer = -1;
            bool canContinueGame = true;
 
            //play until a user enters an incorrect word or the timer is up
            while (canContinueGame)
            {
                currentPlayer = util.GetNextPlayer(currentPlayer);
                Console.WriteLine(string.Format(Messages.PlayerTurn, gameSettings.players[currentPlayer].GetName()));

                var consoleReader = new ConsoleReaderOnTimer(gameSettings.TimerDurationMs);
                var userWord = new UserWord(consoleReader.ReadLine() ?? string.Empty);

                if (userWord.IsNotEmpty())//empty line produces TimeIsUp error - ToDo: fix
                {
                    canContinueGame = userWord.IsValidInput(dictionary, baseWordLetters, out string errorMessage);
                    Console.Write(errorMessage);
                }
                else
                {
                    canContinueGame = false;
                    Console.Write($"\n{Messages.TimeIsUp}");
                }
            }

            currentGame.SaveStatistics(new GameResult(gameSettings.players, dictionary, gameSettings.players[util.GetWinner(currentPlayer)].GetName()));
            Console.Write($"\n{string.Format(Messages.PlayerLost, gameSettings.players[currentPlayer].GetName())}");
        }
    }
}