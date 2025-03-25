using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using BBT.Ware.Config;

#nullable enable

namespace BBT.Ware.NoFov;


[HarmonyPatch]
public static class OverlaysPatch
{
    private static MethodInfo GetOverlayDraw(string type)
    {
        return AccessTools.Method(AccessTools.TypeByName(type), "Draw", (Type[])null, (Type[])null);
    }

    private static MethodInfo GetOverlayDrawDepth(string type)
    {
        return AccessTools.Method(AccessTools.TypeByName(type), "DrawOcclusionDepth", (Type[])null, (Type[])null);
    }

    private static IEnumerable<MethodBase> TargetMethods()
    {
        yield return (MethodBase)OverlaysPatch.GetOverlayDraw("Content.Client.UserInterface.Systems.DamageOverlays.Overlays.DamageOverlay");
        yield return (MethodBase)OverlaysPatch.GetOverlayDraw("Content.Client.Drunk.DrunkOverlay");
        yield return (MethodBase)OverlaysPatch.GetOverlayDraw("Content.Client.Drugs.RainbowOverlay");
        yield return (MethodBase)OverlaysPatch.GetOverlayDraw("Content.Client.Eye.Blinding.BlurryVisionOverlay");
        yield return (MethodBase)OverlaysPatch.GetOverlayDraw("Content.Client.Eye.Blinding.BlindOverlay");
        yield return (MethodBase)OverlaysPatch.GetOverlayDraw("Content.Client.Flash.FlashOverlay");
        yield return (MethodBase)OverlaysPatch.GetOverlayDrawDepth("Robust.Client.Graphics.Clyde.Clyde");
    }

    [HarmonyPrefix]
    private static bool Prefix() => !BBTConfigVisuals.ToggleFov;
}