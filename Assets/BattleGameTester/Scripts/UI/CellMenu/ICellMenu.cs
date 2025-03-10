using System;
using UnityEngine;

namespace BattleGameTester.UI
{
    public interface ICellMenu
    {
        //Empty
        event Action AddDefault_Clicked;
        event Action Import_Clicked;
        //Squad
        event Action Target_Clicked;
        event Action Move_Clicked;
        event Action Settings_Clicked;

        event Action Closed;

        void ShowEmptyMenu(Vector3 coords);
        void ShowSquadMenu(Vector3 coords);
        void CloseActiveMenu();
    }

}