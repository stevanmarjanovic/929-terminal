using System.Text.Json.Serialization;

namespace NineTwoNineTerminal.Models.Sefaria;

public class ExtraDetails
{
    [JsonPropertyName("aliyot")]
    public List<string>? Aliyot { get; set; }
}