using System.Text.Json.Serialization;

public class DiscordGuild
{
  [JsonPropertyName("id")]
  public string? id { get; set; }

  [JsonPropertyName("name")]
  public string? name { get; set; }

  [JsonPropertyName("permissions")]
  public string? permissions { get; set; }
}
