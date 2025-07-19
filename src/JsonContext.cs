using System.Collections.Generic;
using System.Text.Json.Serialization;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(DiscordBotUser))]
[JsonSerializable(typeof(DiscordChannel))]
[JsonSerializable(typeof(DiscordWebhook))]
[JsonSerializable(typeof(DiscordRatelimit))]
[JsonSerializable(typeof(DiscordGuild))]
[JsonSerializable(typeof(List<DiscordGuild>))]
[JsonSerializable(typeof(List<DiscordChannel>))]
public partial class JsonContext : JsonSerializerContext
{
}
