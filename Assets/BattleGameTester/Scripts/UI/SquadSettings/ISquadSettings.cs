using BattleGameTester.Core;
using System;

namespace BattleGameTester.UI
{
    public interface ISquadSettings
    {
        event Action<ISquad> Save_Clicked;
        event Action Delete_Clicked;
        event Action Closed;
        void Show(ISquad squad);
        void Hide();      
    }
}

