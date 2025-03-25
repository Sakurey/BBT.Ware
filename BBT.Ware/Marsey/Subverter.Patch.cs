using HarmonyLib;

public static class SubverterPatch
{
    public static string Name = "BBT.Ware";
    public static string Description = "By Zakurey";
    public static Harmony Harm = new("com.BBT.Ware");
}