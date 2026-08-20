using System.Text.Json.Serialization;

namespace NineTwoNineTerminal.Models.Sefaria;

public class CalendarItem
{
    [JsonPropertyName("title")]
    public required Multilingual Title { get; set; }

    [JsonPropertyName("displayValue")]
    public required Multilingual DisplayValue { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("ref")]
    public string? Ref { get; set; }

    [JsonPropertyName("heRef")]
    public string? HeRef { get; set; }

    [JsonPropertyName("order")]
    public required int Order { get; set; }

    [JsonPropertyName("category")]
    public required string Category { get; set; }

    [JsonPropertyName("extraDetails")]
    public ExtraDetails? ExtraDetails { get; set; }

    [JsonPropertyName("description")]
    public Multilingual? Description { get; set; }
}