class Util
{
  public static string GetRandomString()
  {
    List<string> list = ["you suck", "die", "retard", "dummy", "noo my server gone nooo", "hoden"];
    Random random = new();
    int num = random.Next(0, list.Count);
    return list[num];
  }

  public static String BotIsAdmin(String? s)
  {
    return s != null && s.Equals("4503599627370495") ? "Yes" : "No";
  }

  public static String GlobalTimeout(bool? b)
  {
    return b != null && b == true ? "GLOBAL" : "NORMAL";
  }
}