using Content.Client.Administration;
using Content.Client.Ghost;
using Content.Client.Revenant;
using Content.Shared.Administration;
using Content.Shared.CombatMode;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Client.ResourceManagement;
using Robust.Shared.Console;
using Robust.Shared.Enums;
using BBT.Ware.Config;
using System.Numerics;
using BBT.Ware.Helpers;

namespace BBT.Ware.Huds.Patchs
{
    internal sealed class LocalAdminNameOverlay : Overlay
    {
        private readonly IEntityManager _entityManager = IoCManager.Resolve<IEntityManager>();
        private readonly IEyeManager _eyeManager = IoCManager.Resolve<IEyeManager>();
        private readonly IResourceCache _resourceCache = IoCManager.Resolve<IResourceCache>();
        private readonly IPlayerManager _playerManager = IoCManager.Resolve<IPlayerManager>();
        private Font? _font;
        private Font? _font2;

        public LocalAdminNameOverlay()
        {
            ZIndex = 200;
        }

        public override OverlaySpace Space => OverlaySpace.ScreenSpace;

        protected override void Draw(in OverlayDrawArgs args)
        {
            if (BBTConfigVisuals.ToggleHuds)
            {
                var viewport = args.WorldAABB;
                _font ??= new VectorFont(
                    _resourceCache.GetResource<FontResource>("/Fonts/NotoSans/NotoSans-Bold.ttf"), 8);

                foreach (var session in _playerManager.Sessions)
                {
                    if (session.AttachedEntity is not { } entity ||
                        !_entityManager.EntityExists(entity) ||
                        _entityManager.GetComponent<TransformComponent>(entity).MapID != _eyeManager.CurrentMap)
                    {
                        continue;
                    }

                    var metaData = _entityManager.GetComponent<MetaDataComponent>(entity);
                    var transform = _entityManager.GetComponent<TransformComponent>(entity);
                    var aabb = new Box2(transform.WorldPosition, transform.WorldPosition).Enlarged(0.5f);

                    if (!aabb.Intersects(in viewport))
                    {
                        continue;
                    }

                    var lineOffset = new Vector2(0f, 12f);
                    var screenCoordinates = _eyeManager.WorldToScreen(aabb.Center) + new Vector2(-30f, 30f);
                    var currentLine = screenCoordinates;

                    args.ScreenHandle.DrawString(_font, currentLine, metaData.EntityName, Color.Aquamarine);
                    currentLine += lineOffset;
                    args.ScreenHandle.DrawString(_font, currentLine, session.Name, Color.Yellow);
                    currentLine += lineOffset;


                    if (_entityManager.TryGetComponent<CombatModeComponent>(entity, out var combatMode))
                    {
                        if (combatMode.IsInCombatMode)
                        {
                            args.ScreenHandle.DrawString(_font, currentLine, "CombatMode", Color.Red);
                            currentLine += lineOffset;
                        }
                    }
                }
            }
        }
    }
}