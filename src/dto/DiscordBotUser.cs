public class DiscordBotUser
{
  public string id { get; set; }
  public string username { get; set; }
  public string avatar { get; set; }
  public string discriminator { get; set; }
  public bool bot { get; set; }
  public bool mfa_enabled { get; set; }
  public string locale { get; set; }
  public bool verified { get; set; }
  public string bio { get; set; }
}