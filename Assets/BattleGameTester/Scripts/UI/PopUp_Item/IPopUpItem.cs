using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleGameTester.UI
{
    public interface IPopUp_Item : IView_Item
    {
        void Show(string text);
        void Show(string text, float seconds);
    }
}