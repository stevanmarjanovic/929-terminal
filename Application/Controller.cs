using NineTwoNineTerminal.Domain;
using NineTwoNineTerminal.Infrastructure;
using NineTwoNineTerminal.Presentation;

namespace NineTwoNineTerminal.Application;

public class Controller
{
    private readonly bool _connectedToInternet = NetworkChecker.IsConnected();
    
    public async Task<bool> Run(bool showLink = true, bool simplifiedView = false)
    {
        if (!_connectedToInternet)
        {
            TerminalOutput.WriteTitle("Chai");
            return false;
        }

        var sefaria = new SefariaClient();
        

        try
        {
            var todaysChapter = await sefaria.GetChapterAsync();

            if (todaysChapter.Ref == null)
            {
                throw new Exception("Error getting daily learning");
            }

            var summary = await SummariesService.GetSummaryByChapterName(todaysChapter.Ref);

            if (simplifiedView)
                TerminalOutput.WriteBoldLine(todaysChapter.DisplayValue.En);
            else
                TerminalOutput.WriteTitle(todaysChapter.DisplayValue.En);
            
            TerminalOutput.WriteLine(summary);
            
            if (showLink)
                TerminalOutput.WriteLine($"https://sefaria.org/{todaysChapter.Url}");

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("Chai");
            return false;
        }
    }
}