using System.Text.Json;
using System.Text.Json.Serialization;
using DailyLearning.Libraries;

public class Book
{
    [JsonPropertyName("_id")]
    public required string Id { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("titleShort")]
    public string? TitleShort { get; set; }

    [JsonPropertyName("titleOfficial")]
    public string? TitleOfficial { get; set; }

    [JsonPropertyName("subtitle")]
    public string? Subtitle { get; set; }

    [JsonPropertyName("summary")]
    public string? Summary { get; set; }

    [JsonPropertyName("chapterDelineation")]
    public string? ChapterDelineation { get; set; }

    [JsonPropertyName("chapters")]
    public List<Chapter>? Chapters { get; set; }

    public override string ToString()
    {
        return TitleOfficial ?? Title;
    }

    public async Task<bool> Load()
    {
        try
        {
            var os = new OpenScripture();
            var book = await os.GetBookById(Id);

            TitleShort = book.TitleShort;
            TitleOfficial = book.TitleOfficial;
            Subtitle = book.Subtitle;
            Summary = book.Summary;
            ChapterDelineation = book.ChapterDelineation;

            if (book.Chapters == null) throw new Exception("No chapters loaded");
            Chapters = [.. book.Chapters];

            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }
    
    public string FormattedTitle => Title.Replace("1", "I").Replace("2", "II").Replace("3", "III").Replace("4", "IV");

    public string FormatTitle()
    {
        // Dirty fix, but doesn't need more then 1 and 2
        string title = Title;
        title = title.Replace("1", "I");
        title = title.Replace("2", "II");
        title = title.Replace("3", "III");
        title = title.Replace("4", "IV");

        return title;
    }
}