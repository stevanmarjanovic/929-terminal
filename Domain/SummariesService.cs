using System.Reflection;
using System.Text.Json;
using NineTwoNineTerminal.Models;
using NineTwoNineTerminal.Serialization;

namespace NineTwoNineTerminal.Domain;

public static class SummariesService
{
    private static Task<List<ChapterSummary>> GetChapterSummariesFromJson()
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();

            using var stream = assembly.GetManifestResourceStream("NineTwoNineTerminal.Data.summaries.json")
                ?? throw new InvalidOperationException("Embedded summaries not found.");

            return Task.FromResult(JsonSerializer.Deserialize(stream, AppJsonContext.Default.ListChapterSummary) ?? []);
        }
        catch (Exception exception)
        {
            return Task.FromException<List<ChapterSummary>>(exception);
        }
    }

    public static async Task<string> GetSummaryByChapterName(string chapter)
    {
        var chapters = await GetChapterSummariesFromJson();
        
        return chapters.First(c => c.Chapter == chapter).Summary;
    }
}