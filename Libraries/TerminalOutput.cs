namespace DailyLearning.Libraries;

public class TerminalOutput
{
    public static void WriteTitle(string title)
    {
        var titleLength = title.Length + 2;
        
        Console.WriteLine("┌" + string.Concat(Enumerable.Repeat("─", titleLength)) + "┐");
        Console.WriteLine("│ \u001b[1m" + title + "\u001b[0m │");
        Console.WriteLine("└" + string.Concat(Enumerable.Repeat("─", titleLength)) + "┘");
    }

    public static void WriteLine(string? line, bool safe = true)
    {
        var output = line ?? string.Empty;
        if (safe)
        {
            output = output.Replace("Lord", "Lᴏʀᴅ");
        }
        Console.WriteLine(output);
    }
    
    public static void WriteBoldLine(string? line)
    {
        Console.WriteLine($"\e[1m{line}\e[0m");
    }

    public static void WriteSuccessLine(string? line)
    {
        Console.WriteLine($"\e[32m{line}\e[0m");
    }
    
    public static void RewriteLine(string? line)
    {
        Console.Write("\r" + line);
    }

    public static void NewLine()
    {
        Console.WriteLine();
    }
}