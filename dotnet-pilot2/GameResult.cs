using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace DotnetPilot2
{
    public class GameResult
    {
        [JsonPropertyName("gameId")]
        public string GameId { get; set; } = string.Empty;
        [JsonPropertyName("player1")]
        public string Player1 { get; set; } = string.Empty;
        [JsonPropertyName("player2")]
        public string Player2 { get; set; } = string.Empty;
        [JsonPropertyName("dictionary")]
        public HashSet<string> Dictionary { get; set; } = new();
        [JsonPropertyName("winner")]
        public string Winner { get; set; } = string.Empty;
        
        private string _filePath = "wordGameStats.json";
        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
        };

        public GameResult() 
        {
        }
        
        public GameResult(List<Player> players, HashSet<string> dictionary, string winner)
        {
            GameId = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_fffffff");
            Player1 = players[0].GetName();
            Player2 = players[1].GetName();
            this.Dictionary = dictionary;
            this.Winner = winner;
        }

        public List<GameResult> LoadStatistics()
        {
            string json = string.Empty;

            try
            {
                json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<GameResult>>(json) ?? new List<GameResult>();
            }
            catch (FileNotFoundException)
            {
                return new List<GameResult>();
            }
        }

        public void SaveStatistics(GameResult currentGame)
        {
            List<GameResult> gamesStatistics = LoadStatistics();
            gamesStatistics.Add(currentGame);

            string updatedJson = JsonSerializer.Serialize(gamesStatistics, _options);
            File.WriteAllText(_filePath, updatedJson);
        }

     }
}
