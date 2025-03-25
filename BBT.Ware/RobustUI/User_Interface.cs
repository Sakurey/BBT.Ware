using System.Net.Mime;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using BBT.Ware.NoFov;
using BBT.Ware;
using BBT.Ware.Helpers;
using BBT.Ware.Zoom;
using BBT.Ware.Config;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
namespace BBT.Ware.Ui // Убрана лишняя точка с запятой
{
    public sealed class MenuWindow : DefaultWindow
    {
        private readonly BoxContainer _box;
        
        private readonly Button _ToggleHuds;
        private readonly Button _ToggleFov;
        private readonly Button _ToggleAntiSlip;
        private readonly Button _ToggleSpam;
        private readonly LineEdit _CurrentZoom;
        
        public static event Action<bool> ToggleHudsChanged;
        public static event Action<float> OnZoomChanged;
        public MenuWindow()
        {
            Title = "BBT.Ware";
            Resizable = false;

            // Контейнер для элементов
            
            _box = new BoxContainer
            {
                Orientation = BoxContainer.LayoutOrientation.Vertical,
                SeparationOverride = 5
            };
            ContentsContainer.AddChild(_box);
            // Кнопка ToggleHuds
            /*
             var buttonStyle = new StyleBoxFlat()
            {
                BackgroundColor = Color.FromHex("#3A5FCD"),
                ContentMarginLeftOverride = 5,
                ContentMarginRightOverride = 5,
                ContentMarginTopOverride = 5,
                ContentMarginBottomOverride = 5
            };
            
            var buttonStyle = new StyleBoxTexture()
            {
                Texture = Texture.LoadFromPNGStream(File.OpenRead("Marsey/Textures/Sprite-0001.png")),
                ContentMarginLeftOverride = 1,
                ContentMarginRightOverride = 1,
                ContentMarginTopOverride = 1,
                ContentMarginBottomOverride = 1
            };
            */
            _ToggleHuds = new Button
            {
                Text = "ToggleHuds",
                ToggleMode = true,
                Pressed = BBTConfigVisuals.ToggleHuds,
                //StyleBoxOverride = buttonStyle,
            };
            _ToggleHuds.OnToggled += args =>
            {
                BBTConfigVisuals.ToggleHuds = args.Pressed;
                ToggleHudsChanged?.Invoke(args.Pressed);
            };
            _box.AddChild(_ToggleHuds);

            // Кнопка ToggleFov
            _ToggleFov = new Button
            {
                Text = "ToggleFov",
                ToggleMode = true,
                Pressed = BBTConfigVisuals.ToggleFov,
            };
            _ToggleFov.OnToggled += args =>
            {
                BBTConfigVisuals.ToggleFov = args.Pressed;
            };
            _box.AddChild(_ToggleFov);
            
            _ToggleAntiSlip = new Button
            {
                Text = "Toggle Anti-Slip",
                ToggleMode = true,
                Pressed = BBTConfigMovement.AntiSlip,
            };
            _ToggleAntiSlip.OnToggled += args =>
            {
                BBTConfigMovement.AntiSlip = args.Pressed; // Обновление состояния Anti-Slip
            };
            _box.AddChild(_ToggleAntiSlip);
            
            _CurrentZoom = new LineEdit()
            {
                HorizontalExpand = true,
                PlaceHolder = "Current Zoom",
                Text = BBTConfigVisuals.CurrentZoom.ToString(),
            };
            _CurrentZoom.OnTextChanged += args =>
            {
                // Пытаемся распарсить текст в поле
                if (float.TryParse(_CurrentZoom.Text, out var size))
                {
                    // Ограничиваем значение в разумных пределах
                    if (size < 0.5f) size = 0.5f;
                    if (size > 15f) size = 15f;

                    // Устанавливаем через свойство, чтобы сработало событие OnZoomChanged
                    BBTConfigVisuals.CurrentZoom = size;
                    OnZoomChanged?.Invoke(size);
                }
            };
            _box.AddChild(_CurrentZoom);
        }
    }
}