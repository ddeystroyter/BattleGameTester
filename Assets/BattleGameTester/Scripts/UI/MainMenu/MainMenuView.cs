using System;
using UnityEngine.UI;

namespace BattleGameTester.UI
{
    public class MainMenuView : BaseView, IMainMenuView
    {
        public event Action NewGameClicked = () => { };
        public event Action SettingsClicked = () => { };
        public event Action ExitClicked = () => { };

        public Button NewGameButton;
        public Button SettingsButton;
        public Button ExitButton;

        private void Awake()
        {
            NewGameButton.onClick.AddListener(OnNewGameClicked);
            SettingsButton.onClick.AddListener(OnSettingsClicked);
            ExitButton.onClick.AddListener(OnExitClicked);
        }

        private void OnNewGameClicked()
        {
            NewGameClicked();
        }

        private void OnSettingsClicked()
        {
            SettingsClicked();
        }

        private void OnExitClicked()
        {
            ExitClicked();
        }
    }
}