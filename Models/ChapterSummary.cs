using System.Text.Json.Serialization;

namespace NineTwoNineTerminal.Models;

public class ChapterSummary
{
    [JsonPropertyName("chapter")]
    public required string Chapter { get; set; }

    [JsonPropertyName("summary")]
    public required string Summary { get; set; }
}