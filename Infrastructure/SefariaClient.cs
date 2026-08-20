using System.Text.Json;
using NineTwoNineTerminal.Models.Sefaria;
using NineTwoNineTerminal.Serialization;

namespace NineTwoNineTerminal.Infrastructure;

public class SefariaClient
{
    private readonly HttpClient _client = new();
    private const string SefariaUrl = "https://www.sefaria.org/api";

    private async Task<string> Call(string endpoint)
    {
        var response = await _client.GetAsync($"{SefariaUrl}/{endpoint}");
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        return responseBody;
    }
    
    public async Task<CalendarItem> GetChapterAsync()
    {
        var response = await Call("calendars");
        var deserializedResponse = JsonSerializer.Deserialize(response, AppJsonContext.Default.SefariaCalendarResponse) ?? throw new Exception("Failed to deserialize SefariaCalendarResponse.");
        return deserializedResponse.CalendarItems.First(ci => ci.Title.En == "929");
    }
}