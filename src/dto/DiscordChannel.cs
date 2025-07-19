using System.Text.Json.Serialization;
public class DiscordChannel
{
  [JsonPropertyName("id")]
  public string? id { get; set; }
}