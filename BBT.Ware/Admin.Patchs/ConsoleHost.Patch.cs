using System.Reflection;
using HarmonyLib;

namespace BBT.Ware.Admin.Patches;

internal class ConsoleHostPatch
{
    [HarmonyTargetMethod]
    private static MethodBase TargetMethod()
    {
        return AccessTools.Method(AccessTools.TypeByName("Robust.Client.Console.ClientConsoleHost"),
            "CanExecute");
        
    }
    
    [HarmonyPostfix]
    private static void Postfix(ref bool __result) => __result = true;
}