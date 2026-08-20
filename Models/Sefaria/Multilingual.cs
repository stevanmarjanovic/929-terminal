using System.Text.Json.Serialization;

namespace NineTwoNineTerminal.Models.Sefaria;

public class Multilingual
{
    [JsonPropertyName("en")]
    public required string En { get; set; }

    [JsonPropertyName("he")]
    public required string He { get; set; }
}