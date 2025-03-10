using System;

namespace BattleGameTester.Core
{
    public interface IGameSettings
    {
        event Action<int> ModelsOpacityChanged;
        event Action<string> ModelsModeChanged;
        event Action<bool> IsTerrainOnChanged;

        int ModelsOpacity {  get; set; }
        string ModelsMode {  get; set; }
        bool IsTerrainOn { get; set; }
        bool IsMusicOn { get; set; }
        bool IsSoundEffectsOn { get; set; }
    }
}