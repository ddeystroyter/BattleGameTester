using System;
using UnityEngine;

namespace BattleGameTester.UI
{
    public interface ICellMenuView : IView
    {
        //Empty
        event Action AddDefault_Clicked;
        event Action Import_Clicked;
        event Action CloseEmptyMenu_Clicked;
        //Squad
        event Action Target_Clicked;
        event Action Move_Clicked;
        event Action Settings_Clicked;
        event Action CloseSquadMenu_Clicked;

        //Methods
        void ShowEmptyMenu(Vector3 coords);
        void HideEmptyMenu();
        void ShowSquadMenu(Vector3 coords);
        void HideSquadMenu();
        void CloseActiveMenu();

    }
}

