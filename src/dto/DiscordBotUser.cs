using System.Text.Json.Serialization;

public class DiscordBotUser
{

  [JsonPropertyName("id")]
  public string? id { get; set; }

  [JsonPropertyName("username")]
  public string? username { get; set; }

  [JsonPropertyName("discriminator")]
  public string? discriminator { get; set; }
}