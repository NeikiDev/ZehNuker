using System.Threading.Tasks;

class Program
{

  public static async Task Main()
  {
    try
    {
      await Init();
    }
    catch (System.Exception ex)
    {
      Console.WriteLine(ex.Message);
      Exit();
    }
  }

  public static async Task Init()
  {
    Console.Title = "YUKER";
    Console.Clear();
    Console.Write("[!] Please enter your Discord Bot Token: ");

    String? token = Console.ReadLine();

    if (token == null)
    {
      Console.WriteLine("[!] Discord Bot Token was not entered");
      Exit();
      return;
    }

    DiscordBotUser? user = await Discord.GetInfo(token);

    if (user == null)
    {
      Console.WriteLine("[!] The provided token is invalid.");
      Exit();
      return;
    }

    Console.WriteLine($"[!] Successfully logged in as {user.username}#{user.discriminator} (ID: {user.id})");
    Console.Title = $"YUKER - Logged in as {user.username}";

    Console.WriteLine("[!] Retrieving bot's guilds ...");
    List<DiscordGuild>? guilds = await Discord.GetGuilds(token);

    if (guilds == null || guilds.Count() <= 0)
    {
      Console.WriteLine("[!] The bot is not a member of any guilds.");
      Exit();
      return;
    }
    Console.WriteLine("[!] The bot is a member of the following guilds:");
    foreach (DiscordGuild guild in guilds)
    {
      Console.WriteLine($">> {guild.name} ({guild.id}) - {guild.permissions}");
    }

    // TODO:2025:07:18:23:58:  
    Console.ReadLine();
  }



  public static void Exit()
  {
    Console.WriteLine("\n[!] Exiting Program in 5 seconds ...");
    Thread.Sleep(5000);
    Environment.Exit(0);
  }
}