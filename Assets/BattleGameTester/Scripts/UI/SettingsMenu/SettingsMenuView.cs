using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;


namespace BattleGameTester.UI
{
    public class SettingsMenuView : BaseView, ISettingsMenuView
    {
        private const string OnText = "On";
        private const string OffText = "Off";
        private const string ModelsModeParameterText = "ModelsMode";
        private const string ModelsOpacityParameterText = "ModelsOpacity";
        private const string TerrainParameterText = "Terrain";
        private const string MusicParameterText = "Music";
        private const string SoundEffectsParameterText = "Sound Effects";

        public event Action<int> ModelsOpacitySliderChanged = (value) => { };
        public event Action ModelsModeClicked = () => { };
        public event Action TerrainClicked = () => { };
        public event Action MusicClicked = () => { };
        public event Action SoundEffectsClicked = () => { };
        public event Action InfoButtonClicked = () => { };
        public event Action BackClicked = () => { };

        public TMP_Text ModelsOpacityText;
        public Slider ModelsOpacitySlider;

        public Button ModelsMode;
        public TMP_Text ModelsModeText;

        public Button TerrainButton;
        public TMP_Text TerrainText;

        public Button MusicButton;
        public TMP_Text MusicText;

        public Button SoundEffectsButton;
        public TMP_Text SoundEffectsText;

        public Button InfoButton;

        public Button BackButton;

        public Button ExitButton;

        private void Awake()
        {
            ModelsOpacitySlider.onValueChanged.AddListener(OnModelsOpacityChanged);
            ModelsMode.onClick.AddListener(OnModelsModeClicked);
            TerrainButton.onClick.AddListener(OnTerrainClicked);
            MusicButton.onClick.AddListener(OnMusicClicked);
            SoundEffectsButton.onClick.AddListener(OnSoundEffectsClicked);
            BackButton.onClick.AddListener(OnBackClicked);
            InfoButton.onClick.AddListener(OnInfoButtonClicked);
            ExitButton.onClick.AddListener(OnExitToWindowsClicked);
        }
        public void SetModelsOpacityParameter(int opacity)
        {
            ModelsOpacityText.text = string.Format("{0}: {1}", ModelsOpacityParameterText, opacity);
            ModelsOpacitySlider.value = opacity;
            ModelsOpacitySliderChanged.Invoke(opacity);
        }
        public void SetModelsModeParameter(string mode)
        {
            ModelsModeText.text = string.Format("{0}: {1}", ModelsModeParameterText, mode);
        }

        public void SetTerrainParameter(bool isOn)
        {
            var stateStr = isOn ? OnText : OffText;
            var text = string.Format("{0}: {1}", TerrainParameterText, stateStr);

            TerrainText.text = text;
        }

        public void SetMusicParameter(bool isOn)
        {
            var stateStr = isOn ? OnText : OffText;
            var text = string.Format("{0}: {1}", MusicParameterText, stateStr);

            MusicText.text = text;
        }

        public void SetSoundEffectsParameter(bool isOn)
        {
            var stateStr = isOn ? OnText : OffText;
            var text = string.Format("{0}: {1}", SoundEffectsParameterText, stateStr);

            SoundEffectsText.text = text;
        }
        private void OnModelsModeClicked()
        {
            ModelsModeClicked();
        }
        private void OnModelsOpacityChanged(float opacity)
        {
            var intOpacity = Mathf.RoundToInt(opacity);
            ModelsOpacityText.text = string.Format("{0}: {1}", ModelsOpacityParameterText, intOpacity);
            ModelsOpacitySliderChanged.Invoke(intOpacity);
        }
        private void OnTerrainClicked()
        {
            TerrainClicked();
        }

        private void OnMusicClicked()
        {
            MusicClicked();
        }

        private void OnSoundEffectsClicked()
        {
            SoundEffectsClicked();
        }

        private void OnInfoButtonClicked()
        {
            InfoButtonClicked();
        }

        private void OnBackClicked()
        {
            BackClicked();
        }
        private void OnExitToWindowsClicked()
        {
            Application.Quit();
        }
    }
}
