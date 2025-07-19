class Util
{
  public static readonly string ADMIN_PERMISSION = "4503599627370495";


  public static string GetRandomAvatar()
  {
    List<string> list = [
      "https://static.vecteezy.com/system/resources/previews/022/038/819/large_2x/cute-girl-hacker-with-laptop-avatar-in-cartoon-style-balck-backdrop-generative-ai-photo.jpg",
      "https://media.tenor.com/kU4hTI-jXyoAAAAM/minion-lol.gif",
      "https://t4.ftcdn.net/jpg/03/21/43/07/360_F_321430761_qQi0CU9tzI5w1k1vJgdA02LMtXtsXvJE.jpg",
      "https://www.pandasecurity.com/en/mediacenter/src/uploads/2019/07/pandasecurity-How-do-hackers-pick-their-targets-960x960.jpg",
      "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/e35656ff-9190-4f2d-85c0-0cd74a1c2ee8/dfjuzi4-9308aabe-50db-4291-a86a-740554e5d11f.png/v1/fill/w_894%2Ch_894%2Cq_70%2Cstrp/anime_hacker_2_by_taggedzi_dfjuzi4-pre.jpg?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7ImhlaWdodCI6Ijw9MTYwMCIsInBhdGgiOiJcL2ZcL2UzNTY1NmZmLTkxOTAtNGYyZC04NWMwLTBjZDc0YTFjMmVlOFwvZGZqdXppNC05MzA4YWFiZS01MGRiLTQyOTEtYTg2YS03NDA1NTRlNWQxMWYucG5nIiwid2lkdGgiOiI8PTE2MDAifV1dLCJhdWQiOlsidXJuOnNlcnZpY2U6aW1hZ2Uub3BlcmF0aW9ucyJdfQ.DNvtjlkjDUHGhg6kqtfPuMlU2jpNReJjy4wc8IFHOBU",
      "https://i.pinimg.com/736x/c1/9f/4b/c19f4bee388e6a8e4a22a98f45a359bb.jpg",
      "https://wallpapers-clan.com/wp-content/uploads/2023/02/hacker-dark-background.jpg"
    ];
    Random random = new();
    int num = random.Next(0, list.Count);
    return list[num];
  }

  public static string GetRandomName()
  {
    List<string> list = [
      "Skidders",
      "Larps",
      "AuaMeinZeh",
      "MrWebhook",
      "Pinger9000",
      "UWU",
      "ShadowPinger",
      "EchoBot",
      "StealthySniper",
      "LogMaster",
      "PingWizard",
      "MinionBot",
      "DataDumper",
      "Retards",
      "Webhooker",
      "you suck",
      "die",
      "retard",
      "dummy",
      "noo my server gone nooo",
    ];
    Random random = new();
    int num = random.Next(0, list.Count);
    return list[num];
  }

  public static string BotIsAdmin(String? s)
  {
    return s != null && s.Equals(ADMIN_PERMISSION) ? "Yes" : "No";
  }

  public static string GlobalTimeout(bool? b)
  {
    return b != null && b == true ? "GLOBAL" : "NORMAL";
  }
}