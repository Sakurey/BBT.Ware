using Content.Client.Chemistry.Visualizers;
using Robust.Client.GameObjects;
using Robust.Shared.Configuration;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using System;
using BBT.Ware.FoamControl.Patchs;

#nullable enable

namespace FoamControll.Systems
{
    public sealed class FoamFadingSystem : EntitySystem
    {
        [Dependency]
        private readonly IConfigurationManager _cfg = default!;

        public override void Initialize()
        {
            base.Initialize();
            _cfg.OnValueChanged(ModCVars.FoamFading, UpdateFoamFading, invokeImmediately: false);
        }

        public override void Update(float frameTime)
        {
            base.Update(frameTime);

            var fadingValue = _cfg.GetCVar(ModCVars.FoamFading);

            foreach (var (visualsComponent, spriteComponent) in EntityQuery<FoamVisualsComponent, SpriteComponent>())
            {
                spriteComponent.Color = spriteComponent.Color.WithAlpha(fadingValue);
            }
        }

        private void UpdateFoamFading(float fadingValue)
        {
            foreach (var (visualsComponent, spriteComponent) in EntityQuery<FoamVisualsComponent, SpriteComponent>())
            {
                spriteComponent.Color = spriteComponent.Color.WithAlpha(fadingValue);
            }
        }
    }
}