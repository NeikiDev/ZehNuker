class Util
{
  public static readonly string ADMIN_PERMISSION = "4503599627370495";


  public static string GetRandomString()
  {
    List<string> list = ["you suck", "die", "retard", "dummy", "noo my server gone nooo", "hoden"];
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