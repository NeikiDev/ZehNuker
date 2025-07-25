using System.Net;
using System.Text;
using System.Text.Json;
class Discord
{

  public static async Task SendMessageToWebhook(string webhookUrl, String message)
  {
    try
    {
      using HttpClient httpClient = new();
      var requestData = new
      {
        content = message,
        username = Util.GetRandomName(),
        avatar_url = Util.GetRandomAvatar()
      };
      string requestJson = JsonSerializer.Serialize(requestData);
      StringContent content = new(requestJson, Encoding.UTF8, "application/json");
      HttpResponseMessage response = await httpClient.PostAsync(webhookUrl, content);
      if (response.IsSuccessStatusCode)
      {
        Console.WriteLine("[+] Successfully sent message to webhook.");
      }
      else if (response.StatusCode == HttpStatusCode.TooManyRequests)
      {
        string json = await response.Content.ReadAsStringAsync();
        DiscordRatelimit? ratelimit = JsonSerializer.Deserialize<DiscordRatelimit>(json);
        if (ratelimit != null)
        {
          int tryAgainTimeout = ratelimit.retry_after == null ? 1000 : (int)(ratelimit.retry_after * 1000);
          Console.WriteLine($"[RATELIMIT:{Util.GlobalTimeout(ratelimit.global)}] Retrying in {tryAgainTimeout} ms.");
          await Task.Delay(tryAgainTimeout);
          await Task.Run(async delegate
          {
            await Discord.SendMessageToWebhook(webhookUrl, message);
          });
        }
      }
      else
      {
        Console.WriteLine($"[-] Unable to send message to webhook. Status code: {response.StatusCode}");
      }
    }
    catch (System.Exception ex)
    {
      Console.WriteLine("[!] Failed to send message to webhook. " + ex.Message);
    }
  }

  public static async Task<DiscordWebhook?> CreateWebhookForChannel(string token, string channelId)
  {
    using HttpClient httpClient = new();
    httpClient.DefaultRequestHeaders.Add("Authorization", "Bot " + token);
    var requestData = new
    {
      name = "you are retarded",
    };
    string requestJson = JsonSerializer.Serialize(requestData);
    StringContent content = new(requestJson, Encoding.UTF8, "application/json");
    HttpResponseMessage response = await httpClient.PostAsync($"https://discord.com/api/v10/channels/{channelId}/webhooks", content);
    if (response.IsSuccessStatusCode)
    {
      string json = await response.Content.ReadAsStringAsync();
      var webhook = JsonSerializer.Deserialize<DiscordWebhook>(json);

      if (webhook != null && webhook.url != null)
      {
        Console.WriteLine($"[+] Webhook successfully created for channel ID: {channelId}");
        return webhook;
      }
      else
      {
        Console.WriteLine("[-] Failed to get discord webhook url");
        return null;
      }
    }
    else if (response.StatusCode == HttpStatusCode.TooManyRequests)
    {
      string json = await response.Content.ReadAsStringAsync();
      DiscordRatelimit? ratelimit = JsonSerializer.Deserialize<DiscordRatelimit>(json);
      if (ratelimit != null)
      {
        int tryAgainTimeout = ratelimit.retry_after == null ? 1000 : (int)(ratelimit.retry_after * 1000);
        Console.WriteLine($"[RATELIMIT:{Util.GlobalTimeout(ratelimit.global)}] Retrying in {tryAgainTimeout} ms.");
        await Task.Delay(tryAgainTimeout);
        return await Task.Run(async delegate
        {
          return await Discord.CreateWebhookForChannel(token, channelId);
        });
      }
      return null;
    }
    else
    {
      Console.WriteLine($"[-] Failed to webhook channel {response.StatusCode}");
      return null;
    }
  }

  public static async Task<DiscordChannel?> CreateChannel(String token, String guildId)
  {
    using HttpClient httpClient = new();
    httpClient.DefaultRequestHeaders.Add("Authorization", "Bot " + token);
    string randomName = Util.GetRandomName();
    var requestData = new
    {
      name = randomName,
      type = 0
    };
    string requestJson = JsonSerializer.Serialize(requestData);
    StringContent content = new(requestJson, Encoding.UTF8, "application/json");
    HttpResponseMessage response = await httpClient.PostAsync($"https://discord.com/api/v10/guilds/{guildId}/channels", content);

    if (response.IsSuccessStatusCode)
    {
      string jsonBody = await response.Content.ReadAsStringAsync();
      var channel = JsonSerializer.Deserialize<DiscordChannel>(jsonBody);

      if (channel != null)
      {
        Console.WriteLine($"[+] Channel created with ID: {channel.id}");
        return channel;
      }
      return null;
    }
    else if (response.StatusCode == HttpStatusCode.TooManyRequests)
    {
      string json = await response.Content.ReadAsStringAsync();
      DiscordRatelimit? ratelimit = JsonSerializer.Deserialize<DiscordRatelimit>(json);
      if (ratelimit != null)
      {
        int tryAgainTimeout = ratelimit.retry_after == null ? 1000 : (int)(ratelimit.retry_after * 1000);
        Console.WriteLine($"[RATELIMIT:{Util.GlobalTimeout(ratelimit.global)}] Retrying in {tryAgainTimeout} ms.");
        await Task.Delay(tryAgainTimeout);
        return await Task.Run(async delegate
        {
          return await Discord.CreateChannel(token, guildId);
        });
      }
      return null;
    }
    else
    {
      Console.WriteLine("[-] Unable to create channel");
      return null;
    }
  }

  public static async Task DeleteChannel(String token, String channelId)
  {
    using HttpClient httpClient = new();
    httpClient.DefaultRequestHeaders.Add("Authorization", "Bot " + token);
    HttpResponseMessage response = await httpClient.DeleteAsync($"https://discord.com/api/v10/channels/{channelId}");
    if (response.IsSuccessStatusCode)
    {
      Console.WriteLine($"[+] Deleted channel with id {channelId}");
    }
    else if (response.StatusCode == HttpStatusCode.TooManyRequests)
    {
      string json = await response.Content.ReadAsStringAsync();
      DiscordRatelimit? ratelimit = JsonSerializer.Deserialize<DiscordRatelimit>(json);
      if (ratelimit != null)
      {
        int tryAgainTimeout = ratelimit.retry_after == null ? 1000 : (int)(ratelimit.retry_after * 1000);
        Console.WriteLine($"[RATELIMIT:{Util.GlobalTimeout(ratelimit.global)}] Retrying in {tryAgainTimeout} ms.");
        await Task.Delay(tryAgainTimeout);
        await Task.Run(async delegate
        {
          await Discord.DeleteChannel(token, channelId);
        });
      }
    }
    else
    {
      Console.WriteLine($"[-] Failed to delete {channelId}");
    }
  }

  public static async Task<List<DiscordChannel>?> GetChannels(string token, string guildId)
  {
    using HttpClient httpClient = new();
    httpClient.DefaultRequestHeaders.Add("Authorization", "Bot " + token);
    HttpResponseMessage response = await httpClient.GetAsync($"https://discord.com/api/v10/guilds/{guildId}/channels");
    if (response.IsSuccessStatusCode)
    {
      string json = await response.Content.ReadAsStringAsync();
      var channels = JsonSerializer.Deserialize<List<DiscordChannel>>(json);

      if (channels == null) return null;

      return channels;
    }
    return null;
  }

  public static async Task<List<DiscordGuild>?> GetGuilds(string token)
  {
    using HttpClient httpClient = new();
    httpClient.DefaultRequestHeaders.Add("Authorization", "Bot " + token);
    HttpResponseMessage response = await httpClient.GetAsync("https://discord.com/api/v10/users/@me/guilds");
    if (response.IsSuccessStatusCode)
    {
      string json = await response.Content.ReadAsStringAsync();
      var guilds = JsonSerializer.Deserialize<List<DiscordGuild>>(json);
      if (guilds == null) return null;
      return guilds;
    }
    else
    {
      return null;
    }
  }

  public static async Task<DiscordBotUser?> GetInfo(string token)
  {
    using HttpClient httpClient = new();
    httpClient.DefaultRequestHeaders.Add("Authorization", "Bot " + token);
    HttpResponseMessage response = await httpClient.GetAsync("https://discord.com/api/v10/users/@me");
    if (response.IsSuccessStatusCode)
    {
      string json = await response.Content.ReadAsStringAsync();
      var user = JsonSerializer.Deserialize<DiscordBotUser>(json);
      if (user != null)
      {
        return user;
      }
      return null;
    }
    else
    {
      Console.WriteLine($"[!] Failed to retrieve bot info. Status code: {response.StatusCode}");
      return null;
    }
  }


}