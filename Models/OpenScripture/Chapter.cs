using System.Text.Json;
using System.Text.Json.Serialization;
using DailyLearning.Libraries;

public class Chapter
{
    [JsonPropertyName("_id")]
    public required string Id { get; set; }

    [JsonPropertyName("summary")]
    public string? Summary { get; set; }

    [JsonPropertyName("nextChapterId")]
    public string? NextChapterId { get; set; }

    [JsonPropertyName("prevChapterId")]
    public string? PreviousChapterId { get; set; }

    [JsonPropertyName("volume")]
    public Volume? Volume { get; set; }

    [JsonPropertyName("book")]
    public Book? Book { get; set; }

    [JsonPropertyName("chapter")]
    public ChapterData? Data { get; set; }

    public override string ToString()
    {
        return Id;
    }

    public async Task<bool> Load()
    {
        try
        {
            var os = new OpenScripture();
            var chapter = await os.GetChapterById(Id);

            NextChapterId = chapter.NextChapterId;
            PreviousChapterId = chapter.PreviousChapterId;
            Summary = chapter.Summary;

            Data = chapter.Data ?? throw new Exception("No chapter Data loaded");

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
}