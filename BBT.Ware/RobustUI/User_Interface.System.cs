using Content.Shared.Administration;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Client.Console;
using Robust.Shared.Console;
using Robust.Client.Input;
using Robust.Shared.Input.Binding;
using Robust.Shared.Input;

namespace BBT.Ware.Ui;
public sealed class MenuSystem : EntitySystem
    {
        [Dependency] private readonly IInputManager _inputManager = default!;

        private MenuWindow? _window;

        private static readonly BoundKeyFunction ToggleMenuKey = new("inv_check");

        public override void Initialize()
        {
            base.Initialize();
            IoCManager.InjectDependencies(this);
            
            _window = new MenuWindow();
            
            var registration = new KeyBindingRegistration
            {
                Function = ToggleMenuKey,
                BaseKey = Keyboard.Key.Insert,
                Type = KeyBindingType.Command
            };

            _inputManager.RegisterBinding(registration);
            
            CommandBinds.Builder.Bind(ToggleMenuKey, InputCmdHandler.FromDelegate(_ => ToggleMenuWindow())).Register<MenuSystem>();
        }

        public override void Shutdown()
        {
            base.Shutdown();
            
            CommandBinds.Unregister<MenuSystem>();
        }

        public void ToggleMenuWindow()
        {
            if (_window == null)
                return;

            if (_window.IsOpen)
                _window.Close();
            else
                _window.OpenCentered();
        }
    }
    
    [AnyCommand]
    public sealed class MenuCommandd : IConsoleCommand
    {
        public string Command => "inv_check";
        public string Description => "Open or close the mod menu.";
        public string Help => "Usage: inv_check";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var menuSystem = EntitySystem.Get<MenuSystem>();
            menuSystem?.ToggleMenuWindow();
        }
    }

