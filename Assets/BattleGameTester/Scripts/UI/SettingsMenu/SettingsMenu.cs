using BattleGameTester.Core;
using System;
using UnityEngine;

namespace BattleGameTester.UI
{
    public class SettingsMenu : MonoBehaviour, ISettingsMenu
    {
        public event Action Closing = () => { };

        private ISettingsMenuView View;
        private IHelpMenuView HelpView;

        private IAudioManager AudioManager;
        private IGameSettings GameSettings;

        private int _modelsOpacity;

        private void Awake()
        {
            AudioManager = CompositionRoot.GetAudioManager();
            GameSettings = CompositionRoot.GetGameSettings();

            var viewFactory = CompositionRoot.GetViewFactory();

            View = viewFactory.CreateSettingsMenu();

            View.ModelsModeClicked += OnModelsModeClicked;
            View.ModelsOpacitySliderChanged += OnModelsOpacitySliderChanged;
            View.TerrainClicked += OnTerrainClicked;
            View.MusicClicked += OnMusicClicked;
            View.SoundEffectsClicked += OnSoundEffectsClicked;
            View.InfoButtonClicked += OnInfoButtonClicked;

            View.BackClicked += OnBackClicked;


            HelpView = viewFactory.CreateHelpMenu();

            HelpView.BackClicked += HelpView_BackClicked;
        }

        private void HelpView_BackClicked()
        {
            HelpView.Hide();
        }

        public void Show()
        {
            var isTerrainOn = GameSettings.IsTerrainOn;
            var isMusicOn = GameSettings.IsMusicOn;
            var isSoundEffectsOn = GameSettings.IsSoundEffectsOn;
            _modelsOpacity = GameSettings.ModelsOpacity;

            View.SetModelsOpacityParameter(_modelsOpacity);
            View.SetModelsModeParameter(GameSettings.ModelsMode);
            View.SetTerrainParameter(GameSettings.IsTerrainOn);
            View.SetMusicParameter(isMusicOn);
            View.SetSoundEffectsParameter(isSoundEffectsOn);

            View.Show();
        }

        public void Hide()
        {
            View.Hide();
        }
        private void OnModelsOpacitySliderChanged(int opacity)
        {
            _modelsOpacity = opacity;
        }
        private void ConfirmNewModelsOpacity()
        {
            if (_modelsOpacity != GameSettings.ModelsOpacity)
            {
                GameSettings.ModelsOpacity = _modelsOpacity;
            }
        }
        private void OnModelsModeClicked()
        {
            ESquadModels currentMode;
            Enum.TryParse(GameSettings.ModelsMode, out currentMode);
            ESquadModels nextMode = ESquadModels.Cube;
            switch (currentMode)
            {
                case ESquadModels.Image:
                    nextMode = ESquadModels.Cube;
                    break;
                case ESquadModels.Cube:
                    nextMode = ESquadModels.Image;
                    break;
            }
            GameSettings.ModelsMode = nextMode.ToString();
            View.SetModelsModeParameter(nextMode.ToString());
        }

        private void OnTerrainClicked()
        {
            var isTerrainOn = !GameSettings.IsTerrainOn;

            GameSettings.IsTerrainOn = isTerrainOn;
            View.SetTerrainParameter(isTerrainOn);
        }

        private void OnMusicClicked()
        {
            var isMusicOn = !GameSettings.IsMusicOn;

            GameSettings.IsMusicOn = isMusicOn;
            View.SetMusicParameter(isMusicOn);

            AudioManager.SetMusicActive(isMusicOn);
        }

        private void OnSoundEffectsClicked()
        {
            var isSoundEffectsOn = !GameSettings.IsSoundEffectsOn;

            GameSettings.IsSoundEffectsOn = isSoundEffectsOn;
            View.SetSoundEffectsParameter(isSoundEffectsOn);

            AudioManager.SetEffectsActive(isSoundEffectsOn);
        }

        private void OnInfoButtonClicked()
        {
            HelpView.Show();
        }

        private void OnBackClicked()
        {
            ConfirmNewModelsOpacity();
            Closing();
        }
    }
}
