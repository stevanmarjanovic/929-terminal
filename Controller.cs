using DailyLearning.Infrastructure;
using DailyLearning.Libraries;
using DailyLearning.Seed;

namespace DailyLearning;

public class Controller
{
    private readonly bool _connectedToInternet = NetworkChecker.IsConnected();
    private readonly PropertiesRepository _propertiesRepository = new PropertiesRepository();
    
    public async Task<bool> UpdateFromLocalJsonAsync() // TODO This should be removed after debugging
    {
        try
        {
            // Get current chapter
            var sefaria = new Sefaria();
            var currentChapter = await sefaria.GetChapterAsync();
            
            // Fetch Chapters
            var fs = new FetchSummaries();
            await fs.GetChapterSummaries();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<bool> Initialize()
    {
        try
        {
            var fs = new FetchSummaries();
            await fs.GetChapterSummaries(true);
            _propertiesRepository.Insert("initialized", "true");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<bool> Run(bool showLink = true, bool simplifiedView = false)
    {
        var intialized = _propertiesRepository.GetValue("initialized");
        if (intialized != "true") // TODO convert to string before comparing
        {
            TerminalOutput.WriteLine("Please run intialize");
            return false;
        }
        
        if (!_connectedToInternet)
        {
            TerminalOutput.WriteTitle("Chai");
            return false;
        }

        var sefaria = new Sefaria();
        var summaryRepository = new SummaryRepository();

        try
        {
            var dailyTanakh = await sefaria.GetChapterAsync();

            if (dailyTanakh.Ref == null)
            {
                throw new Exception("Error getting daily learning");
            }

            var summary = summaryRepository.GetByChapterReference(dailyTanakh.Ref);

            if (simplifiedView)
                TerminalOutput.WriteBoldLine(dailyTanakh.DisplayValue.En);
            else
                TerminalOutput.WriteTitle(dailyTanakh.DisplayValue.En);
            
            TerminalOutput.WriteLine(summary);
            
            if (showLink)
                TerminalOutput.WriteLine($"https://sefaria.org/{dailyTanakh.Url}");

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