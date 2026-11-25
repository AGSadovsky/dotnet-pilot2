namespace DotnetPilot2
{
    public class BaseWord
    {
        private string _baseWord;
        private GameSettings _gameSettings;

        public BaseWord(string baseWord, GameSettings settings)
        {
            _baseWord = baseWord;
            _gameSettings = settings;
        }

        public string Get()
        {
            return _baseWord;
        }

        public bool IsValid()
        {
            return _baseWord.Length >= _gameSettings.MinWordLength
                && _baseWord.Length <= _gameSettings.MaxWordLength
                && _baseWord.All(char.IsLetter);
        }

        public List<char> SplitIntoLetters()
        {
            return _baseWord.ToLower().ToList();
        }

    }
}