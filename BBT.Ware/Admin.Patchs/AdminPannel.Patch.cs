using Content.Client.Administration.Managers;
using HarmonyLib;
using System;
using System.Reflection;

#nullable enable
[HarmonyPatch]
internal class IsActive
{
    private static MethodBase TargetMethod()
    {
        return (MethodBase) AccessTools.Method(AccessTools.TypeByName("Content.Client.Administration.Managers.ClientAdminManager"), nameof (IsActive), (Type[]) null, (Type[]) null);
    }

    [HarmonyPrefix]
    private static bool PrefSkip(ref bool __result, ClientAdminManager __instance)
    {
        __result = true;
        return false;
    }
}