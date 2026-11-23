namespace DotnetPilot2
{
    public class Player
    {
        private string _name;

        public Player(string name)
        {
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }

    }
}