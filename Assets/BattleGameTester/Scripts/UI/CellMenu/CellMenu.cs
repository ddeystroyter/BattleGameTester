using BattleGameTester.Core;
using System;
using UnityEngine;

namespace BattleGameTester.UI
{
    public class CellMenu : MonoBehaviour, ICellMenu
    {
        public event Action AddDefault_Clicked;
        public event Action Import_Clicked;

        public event Action Target_Clicked;
        public event Action Move_Clicked;
        public event Action Settings_Clicked;

        public event Action Closed;

        private ICellMenuView View;


        private void Awake()
        {
            var viewFactory = CompositionRoot.GetViewFactory();
            View = viewFactory.CreateCellMenu();

            View.AddDefault_Clicked += OnAddDefault_Clicked;
            View.Import_Clicked += OnImport_Clicked;
            View.CloseEmptyMenu_Clicked += OnCloseEmptyMenu_Clicked;

            View.Target_Clicked += OnTarget_Clicked;
            View.Move_Clicked += OnMove_Clicked;
            View.Settings_Clicked += OnSettings_Clicked;
            View.CloseSquadMenu_Clicked += OnCloseSquadMenu_Clicked;
        }
        public void ShowEmptyMenu(Vector3 coords)
        {
            View.ShowEmptyMenu(coords);
        }
        public void ShowSquadMenu(Vector3 coords)
        {
            View.ShowSquadMenu(coords);
        }
        public void CloseActiveMenu()
        {
            View.CloseActiveMenu();
        }

        private void OnAddDefault_Clicked()
        {
            AddDefault_Clicked?.Invoke();
            View.HideEmptyMenu();
        }
        private void OnImport_Clicked()
        {
            Import_Clicked?.Invoke();
            View.HideEmptyMenu();
        }
        private void OnCloseEmptyMenu_Clicked()
        {
            View.HideEmptyMenu();
            Closed?.Invoke();
        }

        private void OnTarget_Clicked()
        {
            Target_Clicked?.Invoke();
            View.HideSquadMenu();
        }

        private void OnMove_Clicked()
        {
            Move_Clicked?.Invoke();
            View.HideSquadMenu();
        }
        private void OnSettings_Clicked()
        {
            Settings_Clicked?.Invoke();
            View.HideSquadMenu();
        }
        private void OnCloseSquadMenu_Clicked()
        {
            View.HideSquadMenu();
            Closed?.Invoke();
        }

    }
}

