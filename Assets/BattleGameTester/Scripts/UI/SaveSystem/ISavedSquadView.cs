using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleGameTester.UI
{
    public interface ISavedSquadsView : IView
    {
        event Action RefreshBtn_Clicked;
        event Action CloseBtn_Clicked;
        Transform Content { get; }
        void ClearContent(); 
        new void Show();
    }
}

