using System.Text.Json.Serialization;

public class DiscordWebhook
{
  [JsonPropertyName("url")]
  public string? url { get; set; }
}