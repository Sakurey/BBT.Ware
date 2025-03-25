using JetBrains.Annotations;
using Robust.Shared.GameObjects;
using Robust.Shared.Player;

namespace BBT.Ware.Helpers;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class LocalPlayerSystem : EntitySystem
{
    public override void Initialize()
    {
        // Подписываемся на нужные события
        SubscribeLocalEvent<LocalPlayerAttachedEvent>(OnAttached);
        SubscribeLocalEvent<LocalPlayerDetachedEvent>(OnDetached);
    }

    // Метод вызывается при подключении игрока
    protected abstract void OnAttached(LocalPlayerAttachedEvent ev);

    // Метод вызывается при отключении игрока
    protected abstract void OnDetached(LocalPlayerDetachedEvent ev);
}