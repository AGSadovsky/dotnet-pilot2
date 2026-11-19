using DotnetPilot2;
using System.Globalization;

    public class GameSettings
    {
        public int PlayersNumber { get; } = 2;
        public int TimerDurationMs { get; } = 5000;
        public int MinWordLength { get; } = 8;
        public int MaxWordLength { get; } = 30;

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

                Console.WriteLine("-----------------------------------------");
                Console.WriteLine(Messages.Greeting);
                Console.WriteLine(string.Format(Messages.GameRules, TimerDurationMs / 1000));
                Console.WriteLine("-----------------------------------------\n");
            }
        }
    }