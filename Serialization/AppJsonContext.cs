using System.Text.Json.Serialization;
using NineTwoNineTerminal.Models;
using NineTwoNineTerminal.Models.Sefaria;

namespace NineTwoNineTerminal.Serialization;

[JsonSerializable(typeof(List<ChapterSummary>))]
[JsonSerializable(typeof(SefariaCalendarResponse))]
internal partial class AppJsonContext : JsonSerializerContext
{
}