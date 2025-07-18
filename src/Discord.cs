using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Discord
{

  public static async Task<List<DiscordGuild>?> GetGuilds(string token)
  {
    using HttpClient httpClient = new();
    httpClient.DefaultRequestHeaders.Add("Authorization", "Bot " + token);
    HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("https://discord.com/api/v10/users/@me/guilds");
    HttpResponseMessage response = httpResponseMessage;

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
    HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("https://discord.com/api/v10/users/@me");
    HttpResponseMessage response = httpResponseMessage;

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
      return null;
    }
  }


}