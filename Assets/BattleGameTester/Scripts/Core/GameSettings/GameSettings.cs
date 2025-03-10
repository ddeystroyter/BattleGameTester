using UnityEngine;
using System;

namespace BattleGameTester.Core
{
    public class GameSettings : IGameSettings
    {
        public event Action<int> ModelsOpacityChanged;
        public event Action<string> ModelsModeChanged;
        public event Action<bool> IsTerrainOnChanged;

        private const string IsMusicOnKey = "IsMusicOn";
        private const string IsSoundEffectsOnKey = "IsSoundEffectsOn";
        private const string IsTerrainOnKey = "IsTerrainOn";
        private const string ModelsOpacityKey = "ModelsOpacity";
        private const string ModelsModeKey = "ModelsMode";

        public int ModelsOpacity
        {
            get
            {
                var value = PlayerPrefs.GetInt(ModelsOpacityKey, 255);
                return value;
            }
            set
            {
                PlayerPrefs.SetInt(ModelsOpacityKey, value);
                ModelsOpacityChanged?.Invoke(value);
            }
        }
        public string ModelsMode
        {
            get
            {
                var value = PlayerPrefs.GetString(ModelsModeKey, "Image");
                return value;
            }
            set
            {
                PlayerPrefs.SetString(ModelsModeKey, value);
                ModelsModeChanged?.Invoke(value);
            }
        }
        public bool IsTerrainOn
        {
            get
            {
                var value = PlayerPrefs.GetInt(IsTerrainOnKey, 1);
                return value != 0;
            }

            set
            {
                PlayerPrefs.SetInt(IsTerrainOnKey, value ? 1 : 0);
                IsTerrainOnChanged?.Invoke(value);
            }
        }
        public bool IsMusicOn
        {
            get
            {
                var value = PlayerPrefs.GetInt(IsMusicOnKey, 1);
                return value != 0;
            }

            set
            {
                PlayerPrefs.SetInt(IsMusicOnKey, value ? 1 : 0);
            }
        }

        public bool IsSoundEffectsOn
        {
            get
            {
                var value = PlayerPrefs.GetInt(IsSoundEffectsOnKey, 1);
                return value != 0;
            }

            set
            {
                PlayerPrefs.SetInt(IsSoundEffectsOnKey, value ? 1 : 0);
            }
        }
    }
}
