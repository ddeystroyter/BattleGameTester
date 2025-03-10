using BattleGameTester.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BattleGameTester.UI
{
    public interface ICombatQueueView : IView
    {
        void Show(EPlayer player, AttackType turnType, List<CombatMove> cms);
        void AddCombatMove(CombatMove cm, bool needUpdateButtons);

    }
}

