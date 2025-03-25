using Content.Client.Administration.Managers;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable enable
[HarmonyPatch]
internal static class LocalAdminPatch
{
    [HarmonyTargetMethods]
    private static IEnumerable<MethodBase> TargetMethods()
    {
        yield return (MethodBase) AccessTools.Method(typeof (ClientAdminManager), "CanCommand", (Type[]) null, (Type[]) null);
        yield return (MethodBase) AccessTools.Method(typeof (ClientAdminManager), "CanScript", (Type[]) null, (Type[]) null);
        yield return (MethodBase) AccessTools.Method(typeof (ClientAdminManager), "CanAdminMenu", (Type[]) null, (Type[]) null);
        yield return (MethodBase) AccessTools.Method(typeof (ClientAdminManager), "CanAdminPlace", (Type[]) null, (Type[]) null);
        yield return (MethodBase) AccessTools.Method(typeof (ClientAdminManager), "IsActive", (Type[]) null, (Type[]) null);
    }

    [HarmonyPostfix]
    private static void Postfix(ref bool __result) => __result = true;
}