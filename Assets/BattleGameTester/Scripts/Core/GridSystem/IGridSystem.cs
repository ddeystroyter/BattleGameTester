using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleGameTester.Core
{
    public interface IGridSystem
    {
        event Action<GridState> GridStateChanged;
        GridState State { get; set; }

        event Action<Vector2Int> CursorEnterNotEmptyCell;
        event Action CursorEnterEmptyCell;

        event Action<Vector3> EmptyCellOpened;
        event Action<Vector3> SquadCellOpened;
        event Action CellClosed;

        event Action<Vector2Int> CellPicked;

        

        Camera ActiveCamera { get; set; }
        EPlayer ActivePlayer { get; set; }
        Vector2Int activeCell { get; }

        void CloseCell();
        void SetCellEmptyState(Vector2Int cell, bool isEmpty);
        void MakeActiveCellNotEmpty();
        void MakeActiveCellEmpty();

        Vector2Int GetActiveCell();

        Vector3 GetActiveCellWorldPosition();
        Vector3 GetCellWorldPosition(Vector2Int cell);
        

    }

    public interface ICell
    {
        bool IsEmpty { get; }
        Vector2Int CellPosition { get; }

    }
}

