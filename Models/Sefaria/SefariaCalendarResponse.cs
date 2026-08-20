using System.Text.Json.Serialization;

namespace NineTwoNineTerminal.Models.Sefaria;

public class SefariaCalendarResponse
{
    [JsonPropertyName("date")]
    public string? Date { get; set; }

    [JsonPropertyName("timezone")]
    public string? Timezone { get; set; }

    [JsonPropertyName("calendar_items")]
    public required List<CalendarItem> CalendarItems { get; set; }
}