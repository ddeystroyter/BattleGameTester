using System;

namespace BattleGameTester.UI
{
    public interface IMainMenuView : IView
    {
        event Action NewGameClicked;
        event Action SettingsClicked;
        event Action ExitClicked;
    }
}
