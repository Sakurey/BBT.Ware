using System.Threading.Tasks;
using Content.Shared.Bed.Sleep;
using Robust.Shared.GameObjects;
using Robust.Shared.Maths;
using Robust.Client.Graphics;
using Robust.Client.GameObjects;
using Content.Shared.Electrocution;
using Robust.Shared.IoC;
using BBT.Ware.Config;
using BBT.Ware.Helpers;
using BBT.Ware.Ui;
public class InsulationCheckerSystem : EntitySystem
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IEyeManager _eyeManager = default!;

    private HashSet<EntityUid> _foundEntities = new();
    private bool _isScanning = false; 

    public override void Initialize()
    {
        base.Initialize();

        // Подписываемся на изменение ToggleHuds
        MenuWindow.ToggleHudsChanged += OnToggleHudsChanged;
    }

    public override void Shutdown()
    {
        base.Shutdown();

        // Отписываемся при завершении работы системы
        MenuWindow.ToggleHudsChanged -= OnToggleHudsChanged;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (_isScanning)
            return; 

        _isScanning = true; 

        PerformScan(); 
        
        Task.Delay(500).ContinueWith(_ => _isScanning = false); 
    }

    private void PerformScan()
    {
        var viewBounds = _eyeManager.GetWorldViewport();
        var entitiesInView = GetEntitiesInView(viewBounds);

        foreach (var entity in entitiesInView)
        {
            if (_entityManager.TryGetComponent(entity, out InsulatedComponent? insulatedComponent) && !_foundEntities.Contains(entity))
            {
                _foundEntities.Add(entity);

                if (BBTConfigVisuals.ToggleHuds) // Проверяем активное состояние HUD
                {
                    if (insulatedComponent.Coefficient == 0)
                    {
                        ChangeEntityColor(entity, Color.FromHex("#00FF00")); 
                    }
                    else if (insulatedComponent.Coefficient > 0.1f && insulatedComponent.Coefficient <= 1f)
                    {
                        ChangeEntityColor(entity, Color.FromHex("#FFA500")); 
                    }
                    else if (insulatedComponent.Coefficient > 1f)
                    {
                        ChangeEntityColor(entity, Color.FromHex("#FF0000")); 
                    }
                }
            }
        }
    }

    private IEnumerable<EntityUid> GetEntitiesInView(Box2 viewBounds)
    {
        foreach (var entity in _entityManager.GetEntities())
        {
            if (_entityManager.TryGetComponent(entity, out TransformComponent? transformComponent))
            {
                var position = transformComponent.WorldPosition;
                if (viewBounds.Contains(position))
                {
                    yield return entity;
                }
            }
        }
    }

    private void ChangeEntityColor(EntityUid entity, Color color)
    {
        if (_entityManager.TryGetComponent(entity, out SpriteComponent? spriteComponent))
        {
            spriteComponent.Color = color;
        }
    }

    private void OnToggleHudsChanged(bool enabled)
    {
        if (enabled) // Если HUD включен
        {
            foreach (var entity in _foundEntities)
            {
                // Пересчитываем и обновляем цвет каждой сущности
                if (_entityManager.TryGetComponent(entity, out InsulatedComponent? insulatedComponent))
                {
                    if (insulatedComponent.Coefficient == 0)
                    {
                        ChangeEntityColor(entity, Color.FromHex("#00FF00")); 
                    }
                    else if (insulatedComponent.Coefficient > 0.1f && insulatedComponent.Coefficient <= 1f)
                    {
                        ChangeEntityColor(entity, Color.FromHex("#FFA500")); 
                    }
                    else if (insulatedComponent.Coefficient > 1f)
                    {
                        ChangeEntityColor(entity, Color.FromHex("#FF0000")); 
                    }
                }
            }
        }
        else
        {
            // Если HUD выключен, обнуляем цвета до белого
            foreach (var entity in _foundEntities)
            {
                ChangeEntityColor(entity, Color.FromHex("#FFFFFF"));
            }
        }
    }
}