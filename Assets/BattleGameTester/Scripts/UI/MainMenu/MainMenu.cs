using BattleGameTester.Core;
using UnityEngine;

namespace BattleGameTester.UI
{
    public class MainMenu : MonoBehaviour, IMainMenu
    {
        private IMainMenuView View;
        private ISettingsMenu SettingsMenu;

        private ISceneManager SceneManager;

        private void Awake()
        {
            SceneManager = CompositionRoot.GetSceneManager();
            SettingsMenu = CompositionRoot.GetSettingsMenu();
            var viewFactory = CompositionRoot.GetViewFactory();

            View = viewFactory.CreateMainMenu();

            View.SettingsClicked += OnSettingsClicked;
            View.NewGameClicked += OnNewGameClicked;
            View.ExitClicked += OnExitClicked;

            SettingsMenu.Closing += OnSettingsMenuClosing;
        }

        public void Show()
        {
            View.Show();
        }

        public void Hide()
        {
            View.Hide();
        }

        private void OnSettingsClicked()
        {
            Hide();
            SettingsMenu.Show();
        }

        private void OnSettingsMenuClosing()
        {
            SettingsMenu.Hide();
            Show();
        }

        private void OnNewGameClicked()
        {
            SceneManager.LoadScene(EScenes.GameScene);
        }

        private void OnExitClicked()
        {
            Application.Quit();
        }
    }
}