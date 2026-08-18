using System.Text.Json;

namespace DailyLearning.Libraries;

public class Sefaria
{
    private readonly HttpClient client = new HttpClient();
    private readonly string sefariaUrl = "https://www.sefaria.org/api";

    private async Task<string> Call(string endpoint)
    {
        var response = await client.GetAsync($"{sefariaUrl}/{endpoint}");
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        return responseBody;
    }
    
    public async Task<CalendarItem> GetChapterAsync()
    {
        var response = await Call("calendars");
        var deserializedResponse = JsonSerializer.Deserialize<SefariaCalendarResponse>(response) ?? throw new Exception("Failed to deserialize SefariaCalendarResponse.");
        return deserializedResponse.CalendarItems.First(ci => ci.Title.En == "929");
    }
}