using System.Numerics;
using BBT.Ware.Helpers;
using BBT.Ware.Config;
using Robust.Client.GameObjects;
using Robust.Client.Player;
using Robust.Shared.Player;
using BBT.Ware.Ui;
using Robust.Client.UserInterface.Controls;

namespace BBT.Ware.Zoom;

public class BbtAutoZoomSystem : LocalPlayerSystem
{
    [Dependency] private readonly IPlayerManager _player = default!;

    private float _zoom;

    public override void Initialize()
    {
        base.Initialize();
        _zoom = BBTConfigVisuals.CurrentZoom;

        // Подписываемся на изменение зума
        MenuWindow.OnZoomChanged += UpdateZoom;
    }

    public override void Shutdown()
    {
        base.Shutdown();

        // Не забудьте отписаться
        MenuWindow.OnZoomChanged -= UpdateZoom;
    }
    private void UpdateZoom(float zoom)
    {
        _zoom = zoom;

        if (_player.LocalEntity == null)
            return;

        if (!TryComp<EyeComponent>(_player.LocalEntity.Value, out var eyeComponent))
            return;

        eyeComponent.Eye.Zoom = new Vector2(_zoom, _zoom);
    }

    protected override void OnAttached(LocalPlayerAttachedEvent ev)
    {
        if (!TryComp<EyeComponent>(ev.Entity, out var eyeComponent))
            return;

        eyeComponent.NetSyncEnabled = false;
        eyeComponent.Eye.Zoom = new Vector2(_zoom, _zoom);
    }

    protected override void OnDetached(LocalPlayerDetachedEvent ev)
    {
        if (!TryComp<EyeComponent>(ev.Entity, out var eyeComponent))
            return;

        eyeComponent.Eye.Zoom = eyeComponent.Zoom;
        eyeComponent.NetSyncEnabled = true;
    }
}