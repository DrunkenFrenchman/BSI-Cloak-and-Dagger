using BSI.CloakDagger.Screens.ViewModels;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.TwoDimension;

namespace BSI.CloakDagger.Screens
{
    public class PlotScreen : ScreenBase
    {
        private PlotsVM _dataSource;
        private GauntletLayer _gauntletLayer;
        private SpriteCategory _plotCategory;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            TogglePause();

            var spriteData = UIResourceManager.SpriteData;
            var resourceContext = UIResourceManager.ResourceContext;
            var uiResourceDepot = UIResourceManager.UIResourceDepot;
            _plotCategory = spriteData.SpriteCategories["ui_quest"];
            _plotCategory.Load(resourceContext, uiResourceDepot);

            _dataSource = new PlotsVM(CloseScreen);

            _gauntletLayer = new GauntletLayer(1337);
            _gauntletLayer.InputRestrictions.SetInputRestrictions();
            _gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
            _gauntletLayer.LoadMovie("PlotsScreen", _dataSource);
            _gauntletLayer.IsFocusLayer = true;

            AddLayer(_gauntletLayer);
            ScreenManager.TrySetFocus(_gauntletLayer);
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();

            _plotCategory.Unload();
            _gauntletLayer.IsFocusLayer = false;

            ScreenManager.TryLoseFocus(_gauntletLayer);
            RemoveLayer(_gauntletLayer);
        }

        protected override void OnFinalize()
        {
            base.OnFinalize();

            _dataSource = null;
            _gauntletLayer = null;
        }

        protected override void OnFrameTick(float dt)
        {
            base.OnFrameTick(dt);

            if (!_gauntletLayer.Input.IsHotKeyReleased("Exit"))
            {
                return;
            }

            CloseScreen();
        }

        private void CloseScreen()
        {
            TogglePause();
            ScreenManager.PopScreen();
        }

        private void TogglePause()
        {
            if (Game.Current == null)
            {
                return;
            }

            var isPaused = Game.Current.GameStateManager.ActiveStateDisabledByUser;
            Game.Current.GameStateManager.ActiveStateDisabledByUser = !isPaused;

            if (isPaused)
            {
                LoadingWindow.DisableGlobalLoadingWindow();
            }
            else
            {
                LoadingWindow.EnableGlobalLoadingWindow();
            }
        }
    }
}