using System;

namespace BattleGameTester.UI
{
    public interface ISettingsMenu : IScreen
    {
        event Action Closing;
    }
}