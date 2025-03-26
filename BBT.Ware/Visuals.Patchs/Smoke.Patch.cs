using BBT.Ware.Config;
using Content.Client.Chemistry.Visualizers;
using HarmonyLib;
using Robust.Client.GameObjects;
using Robust.Shared.Maths;

namespace CerberusWare.Visuals
{
    //Petrosan Huesos Ez Cracked by Big Balls Team
    [HarmonyPatch(typeof(SmokeVisualizerSystem), "OnAppearanceChange")]
    public static class SmokePatch
    {
        [HarmonyPostfix]
        public static void Postfix(AppearanceChangeEvent args)
        {
            if (!BBTConfigVisuals.ToggleHuds || args.Sprite == null)
                return;

            var sprite = args.Sprite;
            sprite.Color = sprite.Color.WithAlpha(0.3f);
            sprite.DrawDepth = 1;
        }
    }
}