using System.Text.Json.Serialization;

public class DiscordRatelimit
{
  [JsonPropertyName("retry_after")]
  public double? retry_after { get; set; }

  [JsonPropertyName("global")]
  public bool? global { get; set; }
}