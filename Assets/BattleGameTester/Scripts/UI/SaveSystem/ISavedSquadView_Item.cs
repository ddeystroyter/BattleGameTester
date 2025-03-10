using System;
using UnityEngine;
using BattleGameTester.Core;

namespace BattleGameTester.UI
{
    public interface ISavedSquadsView_Item : IView_Item
    {
        event Action<int> AddBtn_Clicked;
        event Action<int> DeleteBtn_Clicked;
        void Create(int id, string spriteName, string name, Vector2Int HP, Attacks attacks);
    }
}

