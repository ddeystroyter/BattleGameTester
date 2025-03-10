using BattleGameTester.Core;
using System;
using UnityEngine;

namespace BattleGameTester.UI
{
    public interface ICombatQueueView_Item : IView_Item
    {
        event Action<int> UpBtn_Clicked;
        event Action<int> DownBtn_Clicked;
        event Action<int> DeleteBtn_Clicked;

        Transform Transform { get; }
        CombatMove ActiveCM { get; }

        void Init(CombatMove cm);
        void ChangePriorityTo(byte priority);
        void EnableUpBtn();
        void DisableUpBtn();
        void EnableDownBtn();
        void DisableDownBtn();
    }
}
