using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleGameTester.UI
{
    public interface IHelpMenuView : IView
    {
        event Action BackClicked;
    }
}

