using System;
using Unity.VisualScripting;

namespace BattleGameTester.UI
{
    public interface ISettingsMenuView : IView
    {
        event Action<int> ModelsOpacitySliderChanged;
        event Action ModelsModeClicked;
        event Action TerrainClicked;
        event Action MusicClicked;
        event Action SoundEffectsClicked;
        event Action InfoButtonClicked;
        event Action BackClicked;
        
        void SetModelsOpacityParameter(int opacity);
        void SetModelsModeParameter(string mode);
        void SetTerrainParameter(bool isOn);
        void SetMusicParameter(bool isOn);
        void SetSoundEffectsParameter(bool isOn);
    }
}
