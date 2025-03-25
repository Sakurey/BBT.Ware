using BBT.Ware.Huds.Patchs;
using Robust.Client.Graphics;
using Robust.Shared.ContentPack;

namespace BBT.Ware.Huds.Helper
{
    public class MainPatch : GameShared
    {
        public override async void PostInit()
        {
            IoCManager.InitThread();
            IoCManager.Register<LocalAdminNameOverlay>();
            var _overlayManager = IoCManager.Resolve<IOverlayManager>();
            var _localAdminNameOverlay = new LocalAdminNameOverlay();
            _overlayManager.AddOverlay(_localAdminNameOverlay);
            IoCManager.BuildGraph();
        }
    }
}