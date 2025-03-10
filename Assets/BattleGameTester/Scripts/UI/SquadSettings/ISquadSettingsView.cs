using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleGameTester.Core;

namespace BattleGameTester.UI
{
    public interface ISquadSettingsView : IView
    {
        //event Action OKBtn_Clicked;
        event Action<string> SaveNameBtn_Clicked;
        event Action CloseBtn_Clicked;
        event Action SaveBtn_Clicked;
        event Action DeleteBtn_Clicked;
        event Action Closed;

        void Show(string name, Vector2Int HP, Attacks attacks, string spriteName);
        string GetName();
        Vector2Int GetHPSettings();
        Attacks.V6 GetMelee();
        Attacks.V6 GetRange();
        Attacks.V6 GetCC();
        string GetSpriteName();

    }
}

