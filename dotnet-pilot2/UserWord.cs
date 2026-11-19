using DotnetPilot2;

public class UserWord
{
    private string _userWord;

	public UserWord(string userWord)
	{
        _userWord = userWord;
	}

    public string Get()
    {
        return _userWord;
    }

    public bool IsNotEmpty()
    {
        return _userWord != string.Empty;
    }

    private bool IsValidLength()
    {
        return _userWord.Length > 1;
    }

    private bool IsUnique(HashSet<string> uniqueWords)
    {
        return uniqueWords.Add(_userWord);
    }

    private bool CanBuildNewWord(List<char> baseWordLetters)
    {
        var letterCounts = new Dictionary<char, int>(30);

        foreach (char letter in baseWordLetters)
        {
            if (letterCounts.ContainsKey(letter))
                letterCounts[letter]++;
            else
                letterCounts[letter] = 1;
        }

        foreach (char letter in _userWord.ToLower())
        {
            if (!letterCounts.ContainsKey(letter) || letterCounts[letter] == 0)
                return false;

            letterCounts[letter]--;
        }

        return true;
    }

    public bool ValidateInput(HashSet<string> uniqueWords, List<char> baseWordLetters, out string errorMessage)
    {
        if (!IsValidLength())
        {
            errorMessage = Messages.InvalidWordLength;
            return false;
        }

        if (!IsUnique(uniqueWords))
        {
            errorMessage = Messages.DuplicateUserWord;
            return false;
        }

        if (!CanBuildNewWord(baseWordLetters))
        {
            errorMessage = Messages.IncorrectWord;
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }
}
