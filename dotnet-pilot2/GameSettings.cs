using DotnetPilot2;
using System.Globalization;

public class GameSettings
{
    public int PlayersNumber { get; } = 2;
    public int TimerDurationMs { get; } = 5000;
    public int MinWordLength { get; } = 8;
    public int MaxWordLength { get; } = 30;
    public List<Player> players = [];

    private static void ChangeLanguage(string cultureName)
    {
        var culture = new CultureInfo(cultureName);

        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }

    public GameSettings()
    {
        bool isLanguageSelected = false;
        while (!isLanguageSelected)
        {
            Console.WriteLine(Messages.ChooseLang);
            Console.Write("> ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ChangeLanguage("en-US");
                    isLanguageSelected = true;
                    break;
                case "2":
                    ChangeLanguage("ru-RU");
                    isLanguageSelected = true;
                    break;
                default:
                    Console.WriteLine("Invalid input\n");
                    continue;
            }

            InitializePlayers();

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine(Messages.Greeting);
            Console.WriteLine(string.Format(Messages.GameRules, TimerDurationMs / 1000));
            Console.WriteLine("-----------------------------------------\n");
        }
    }

    private void InitializePlayers()
    {
        for (int playerNumber = 0; playerNumber < PlayersNumber; playerNumber++)
        {
            Console.Write(string.Format(Messages.PlayerIntroduce, playerNumber + 1));
            string? playerName = Console.ReadLine();

            if (string.IsNullOrEmpty(playerName))
                playerName = Messages.Player + " " + (playerNumber + 1);

            players.Add(new Player(playerName));
        }
    }

}