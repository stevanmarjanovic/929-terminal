using System.Text.Json;

namespace DailyLearning.Libraries;

internal class OpenScripture
{
    private readonly HttpClient _client = new HttpClient();
    private const string OpenScriptureUrl = "https://openscriptureapi.org/api/scriptures/v1/lds/en";
    private bool _connectedToInternet = NetworkChecker.IsConnected();

    private async Task<string> Call(string endpoint)
    {
        var response = await _client.GetAsync($"{OpenScriptureUrl}/{endpoint}");
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        return responseBody;
    }

    public async Task<List<Book>> GetAllBooksByVolumeId(string volumeId)
    {
        var responseBody = await Call($"volume/{volumeId}");
        var volume = JsonSerializer.Deserialize<Volume>(responseBody) ?? throw new Exception("Failed to deserialize Volume");
        var books = volume.Books;
        
        return books ?? throw new Exception($"No books in Volume {volume.Title}");
    }
    
    public async Task<Book> GetBookById(string bookId)
    {
        var responseBody = await Call($"book/{bookId}");
        var book = JsonSerializer.Deserialize<Book>(responseBody) ?? throw new Exception("Failed to deserialize Book");

        return book;
    }
    
    public async Task<Chapter> GetChapterById(string chapterId)
    {
        var responseBody = await Call($"chapter/{chapterId}");
        var chapter = JsonSerializer.Deserialize<Chapter>(responseBody) ?? throw new Exception("Failed to deserialize Chapter");

        return chapter;
    }
}