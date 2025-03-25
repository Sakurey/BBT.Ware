using System.Collections.Generic;
using Robust.Shared.GameObjects;
using Robust.Shared.Player;
using BBT.Ware.Config;
using BBT.Ware.Ui;
namespace BBT.Ware.Helpers;

public abstract class LocalPlayerAddCompSystem<TComp> : LocalPlayerSystem where TComp : Component, new()
{
    // Возможность переопределения для кастомизации компонента
    protected virtual TComp CompOverride => new();

    private readonly HashSet<EntityUid> _trackedEntities = new();

    public override void Initialize()
    {
        base.Initialize();

        // Подписываемся на изменение ToggleHuds
        MenuWindow.ToggleHudsChanged += OnToggleHudsChanged;
    }

    public override void Shutdown()
    {
        base.Shutdown();

        // Отписываемся от события
        MenuWindow.ToggleHudsChanged -= OnToggleHudsChanged;
    }

    // Метод для создания компонента
    private TComp CreateComponent()
    {
        var comp = CompOverride;
        comp.NetSyncEnabled = false; // Отключение сетевой синхронизации
        return comp;
    }

    // Метод для удаления компонента
    private void RemoveComponent(EntityUid entity)
    {
        // Удаляем компонент с сущности
        RemComp<TComp>(entity);
    }

    // Событие, вызываемое при подключении игрока
    protected override void OnAttached(LocalPlayerAttachedEvent ev)
    {
        _trackedEntities.Add(ev.Entity);

        // Добавляем компонент только если включен ToggleHuds
        if (BBTConfigVisuals.ToggleHuds)
        {
            AddComp(ev.Entity, CreateComponent());
        }
    }

    // Событие, вызываемое при отключении игрока
    protected override void OnDetached(LocalPlayerDetachedEvent ev)
    {
        _trackedEntities.Remove(ev.Entity);

        // Удаляем компонент
        RemoveComponent(ev.Entity);
    }

    // Обработка изменения состояния ToggleHuds
    private void OnToggleHudsChanged(bool enabled)
    {
        foreach (var entity in _trackedEntities)
        {
            if (enabled)
            {
                // Добавляем компонент
                AddComp(entity, CreateComponent());
            }
            else
            {
                // Удаляем компонент
                RemoveComponent(entity);
            }
        }
    }
}