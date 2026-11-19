using System.Text;

public class ConsoleReaderOnTimer
{
	private int _timeoutMs;
	public ConsoleReaderOnTimer(int timeoutMs)
	{
		_timeoutMs = timeoutMs;
	}

	public string? ReadLine()
	{
        using var timer = new System.Timers.Timer(_timeoutMs);
        bool isElapsed = false;
        timer.AutoReset = false;

        timer.Elapsed += (_, _) =>
        {
            isElapsed = true;
        };

        var inputBuffer = new StringBuilder();

        timer.Start();

        while (!isElapsed)
        {

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    timer.Stop();
                    Console.WriteLine();
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && inputBuffer.Length > 0)
                {
                    inputBuffer.Remove(inputBuffer.Length - 1, 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    inputBuffer.Append(keyInfo.KeyChar);
                    Console.Write(keyInfo.KeyChar);
                }
            }

            Thread.Sleep(10);
        }

        if (isElapsed)
            return null;

        return inputBuffer.ToString();
    }
}
