using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleGameTester.Core
{
    public interface ICellIndicator
    {
        CellIndicatorState State { get; set; }

        void Show() { }
        void Hide() { }
        void MoveTo(Vector3 coords) { }
        void MakeAvailable() { }
        void MakeUnavailable() { }
        void OnCellOpened() { }
        void OnCellClosed() { }
    }
}