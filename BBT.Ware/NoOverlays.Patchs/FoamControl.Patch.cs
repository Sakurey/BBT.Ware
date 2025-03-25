using Robust.Shared;
using Robust.Shared.Configuration;

#nullable disable
namespace BBT.Ware.FoamControl.Patchs
{
    [CVarDefs]
    public sealed class ModCVars : CVars
    {
        public static readonly CVarDef<float> FoamFading = CVarDef.Create<float>("FoamControll.foamFading", 0.35f, (CVar) 128, (string) null);
    }
}