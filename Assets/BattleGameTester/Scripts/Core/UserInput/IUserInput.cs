using System;
using UnityEngine;

namespace BattleGameTester.Core
{
    public interface IUserInput
    {
        event Action Escaped;
        event Action Clicked;
        event Action ClickedR;

        event Action Q_Pressed;
        event Action W_Pressed;
        event Action E_Pressed;
        event Action Z_Pressed;
        event Action X_Pressed;
        event Action P_Pressed;
        event Action Greater_Pressed;
        event Action EnterPressed;

        bool IsLocked { get; set; }
        void Lock();
        void Unlock();
    }
}