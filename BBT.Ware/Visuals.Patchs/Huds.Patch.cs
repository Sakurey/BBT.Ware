using BBT.Ware.Helpers;
using Content.Shared.Overlays;
using Content.Shared.Mobs.Components;
using Content.Shared.StatusIcon;
using Content.Shared.StatusIcon.Components;
using Content.Shared.Store.Components;
using Content.Shared.StoreDiscount.Components;
using BBT.Ware.Config;
using Robust.Shared.Prototypes;

namespace BBT.Ware.Huds.Patchs;

public class BbtJobIconsSystem : LocalPlayerAddCompSystem<ShowJobIconsComponent>;
public class BbtCriminalRecordIcons : LocalPlayerAddCompSystem<ShowCriminalRecordIconsComponent>;
public class BbtMindShieldIcons : LocalPlayerAddCompSystem<ShowMindShieldIconsComponent>;
public class BbtSyndicateIcons : LocalPlayerAddCompSystem<ShowSyndicateIconsComponent>;

public class BbtHealthBarSystem : LocalPlayerAddCompSystem<ShowHealthBarsComponent>
{
    protected override ShowHealthBarsComponent CompOverride => new ShowHealthBarsComponent { DamageContainers = ["Biological", "Silicon", "Inorganic"] };
}

public sealed class BBTWareDetectorSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    private FactionIconPrototype? _syndicateIcon;
    
    public override void Initialize()
    {
        if (_prototype.TryIndex<FactionIconPrototype>("SyndicateFaction", out var iconPrototype))
        {
            _syndicateIcon = iconPrototype;
            SubscribeLocalEvent<MobStateComponent, GetStatusIconsEvent>(OnGetStatusIconsEvent);
        }
    }

    private bool CheckUplink(EntityUid target)
    {
        if (HasComp<StoreDiscountComponent>(target)) return true; // Detect PDA uplink
        if (!TryComp<StoreComponent>(target, out var storeComponent)) return false; // Detect nukeops uplink
        return storeComponent.Balance.Sum(item => (float)item.Value) != 0;
    }

    private void OnGetStatusIconsEvent(EntityUid uid, MobStateComponent _, ref GetStatusIconsEvent ev)
    {
        if (!BBTConfigVisuals.ToggleHuds) return;
        if (!TryComp<TransformComponent>(uid, out var transform)) return;
        var children = transform.ChildEnumerator;
        while (children.MoveNext(out var child))
        {
            if (!CheckUplink(child)) continue;

            ev.StatusIcons.Add(_syndicateIcon!);
            return;
        }
    }
}