using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace DotnetPilot2
{
    public class GameResult
    {
        [JsonPropertyName("gameId")]
        public string gameId { get; set; } = string.Empty;
        [JsonPropertyName("player1")]
        public string player1 { get; set; } = string.Empty;
        [JsonPropertyName("player2")]
        public string player2 { get; set; } = string.Empty;
        [JsonPropertyName("dictionary")]
        public HashSet<string> dictionary { get; set; } = new();
        [JsonPropertyName("winner")]
        public string winner { get; set; } = string.Empty;
        
        private string _filePath = "wordGameStats.json";
        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
        };

        public GameResult() 
        {
        }
        
        public GameResult(Player[] players, HashSet<string> dictionary, string winner)
        {
            gameId = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_fffffff");
            player1 = players[0].GetName();
            player2 = players[1].GetName();
            this.dictionary = dictionary;
            this.winner = winner;
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
