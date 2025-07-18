public class DiscordGuild
{
  public string id { get; set; }
  public string name { get; set; }
  public string icon { get; set; }
  public string banner { get; set; }
  public bool owner { get; set; }
  public string permissions { get; set; }
  public List<string> features { get; set; }
}
