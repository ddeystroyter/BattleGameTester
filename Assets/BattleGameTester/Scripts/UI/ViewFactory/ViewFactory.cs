using BattleGameTester.Core;
using Unity.VisualScripting;

namespace BattleGameTester.UI
{
    public class ViewFactory : IViewFactory
    {
        private IUIRoot UIRoot;
        private ISceneManager SceneManager;
        private IResourceManager ResourceManager;

        public ViewFactory(IUIRoot uiRoot, IResourceManager resourceManager, ISceneManager sceneManager)
        {
            UIRoot = uiRoot;
            SceneManager = sceneManager;
            ResourceManager = resourceManager;
        }

        public IGameHUDView CreateGameHUD()
        {
            var view = ResourceManager.CreatePrefabInstance<IGameHUDView, EViews>(EViews.GameHUD);
            view.SetParent(UIRoot.MainCanvas);
            return view;
        }

        public IMainMenuView CreateMainMenu()
        {
            var view = ResourceManager.CreatePrefabInstance<IMainMenuView, EViews>(EViews.MainMenu);
            view.SetParent(UIRoot.OverlayCanvas);
            return view;
        }

        public ISettingsMenuView CreateSettingsMenu()
        {
            var view = ResourceManager.CreatePrefabInstance<ISettingsMenuView, EViews>(EViews.SettingsMenu);
            view.SetParent(UIRoot.OverlayCanvas);
            return view;
        }
        public IHelpMenuView CreateHelpMenu()
        {
            var view = ResourceManager.CreatePrefabInstance<IHelpMenuView, EViews>(EViews.HelpMenu);
            view.SetParent(UIRoot.OverlayCanvas);
            return view;
        }

        public ICellMenuView CreateCellMenu()
        {
            var view = ResourceManager.CreatePrefabInstance<ICellMenuView, EViews>(EViews.CellMenu);
            view.SetParent(UIRoot.WorldSpaceCanvas);
            return view;

        }

        public ISquadInfoPanelView CreateSquadInfoPanel()
        {
            var view = ResourceManager.CreatePrefabInstance<ISquadInfoPanelView, EViews>(EViews.SquadInfoPanel);
            view.SetParent(UIRoot.WorldSpaceCanvas);
            return view;
        }
        public ISquadSettingsView CreateSquadSettings() 
        { 
            var view = ResourceManager.CreatePrefabInstance<ISquadSettingsView, EViews>(EViews.SquadSettings);
            view.SetParent(UIRoot.OverlayCanvas);
            return view;
        }
        
        public ISavedSquadsView CreateSavedSquads()
        {
            var view = ResourceManager.CreatePrefabInstance<ISavedSquadsView, EViews>(EViews.SavedSquads);
            view.SetParent(UIRoot.OverlayCanvas);
            return view;

        }

        public IAttackTypeChoiceView CreateAttackTypeChoice()
        {
            var view = ResourceManager.CreatePrefabInstance<IAttackTypeChoiceView, EViews>(EViews.AttackTypeChoice);
            view.SetParent(UIRoot.OverlayCanvas);
            return view;
        }

    }
}
