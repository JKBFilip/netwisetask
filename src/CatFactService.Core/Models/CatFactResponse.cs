using System.Text.Json.Serialization;

namespace CatFactService.Core.Models;

public class CatFactResponse
{
    [JsonPropertyName("fact")]
    public string Fact { get; set; } = string.Empty;

    [JsonPropertyName("length")]
    public int Length { get; set; }
}