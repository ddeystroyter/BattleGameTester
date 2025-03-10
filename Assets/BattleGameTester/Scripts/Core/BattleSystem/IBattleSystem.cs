using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleGameTester.Core 
{
    public interface IBattleSystem
    {
        ISquad GetActiveSquad();
        void AddSquad(SquadSave squad);
        void AddDefaultSquad();
        void MoveSquad();
        void ChooseTargetSquad();
        void DeleteActiveSquad();
    }
}

