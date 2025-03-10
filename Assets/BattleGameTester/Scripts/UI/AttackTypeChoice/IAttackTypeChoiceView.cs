using System;
using BattleGameTester.Core;

namespace BattleGameTester.UI
{
    public interface IAttackTypeChoiceView : IView
    {
        event Action<AttackType> AttackType_Confirmed;
        event Action CloseBtn_Clicked;
        void Show(ISquad attacking, ISquad attacked);
    }
}
