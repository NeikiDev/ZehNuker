class Util
{
  public static string GetRandomString()
  {
    List<string> list = ["you suck", "die", "retard", "dummy", "noo my server gone nooo", "hoden"];
    Random random = new();
    int num = random.Next(0, list.Count);
    return list[num];
  }
}