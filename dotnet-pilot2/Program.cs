using DotnetPilot2;
using System.Runtime.InteropServices;
using System.Text;

internal class Program
{
    [DllImport("Kernel32")]
    private static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handler, bool add);

    private delegate bool ConsoleCtrlDelegate(CtrlTypes ctrlType);

    private enum CtrlTypes : uint
    {
        CTRL_C_EVENT = 0,
        CTRL_BREAK_EVENT = 1,
        CTRL_CLOSE_EVENT = 2,
        CTRL_LOGOFF_EVENT = 5,
        CTRL_SHUTDOWN_EVENT = 6
    }

    private static ConsoleCtrlDelegate? _consoleCtrlHandler;

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

            int currentPlayerId = 0;
            bool canContinueGame = true;

            bool isResultSaved = false;
            Lock saveLock = new();

            void CheckUnexpectedApplicationShutDown()
            {
                _consoleCtrlHandler = ctrlType =>
                {
                    SaveGameResults(currentPlayerId);
                    return false;
                };

                SetConsoleCtrlHandler(_consoleCtrlHandler, true);
            }

            void SaveGameResults(int loserId)
            {
                lock (saveLock)
                {
                    if (isResultSaved) return;
                    isResultSaved = true;

                    currentGameResult.SaveStatistics(new GameResult(players, dictionary, players[util.GetWinner(loserId)].GetName()));
                }
            }

            CheckUnexpectedApplicationShutDown();

            //play until a user enters an incorrect word or the timer is up
            while (canContinueGame)
            {
                Console.WriteLine(string.Format(Messages.PlayerTurn, players[currentPlayerId].GetName()));

                var consoleReader = new ConsoleReaderOnTimer(gameSettings.TimerDurationMs);
                var userInput = new UserWord(consoleReader.ReadLine() ?? string.Empty);

                if (userInput.IsCommand())
                {
                    util.ProcessUserCommand(userInput.Get(), dictionary, archivedGameResults, players);
                }
                else if (userInput.IsNotEmpty())//empty line produces TimeIsUp error - ToDo: fix
                {
                    canContinueGame = userInput.IsValid(dictionary, baseWordLetters, out string errorMessage);
                    Console.Write(errorMessage);

                    if (canContinueGame)
                        currentPlayerId = util.GetNextPlayer(currentPlayerId);
                }
                else
                {
                    canContinueGame = false;
                    Console.Write($"\n{Messages.TimeIsUp}");
                }
            }

            SaveGameResults(currentPlayerId);
            Console.Write($"\n{string.Format(Messages.PlayerLost, players[currentPlayerId].GetName())}");
        }
    }
}