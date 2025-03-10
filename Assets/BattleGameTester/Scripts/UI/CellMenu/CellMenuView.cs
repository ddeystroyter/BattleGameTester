using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleGameTester.UI
{
    public class CellMenuView : BaseView, ICellMenuView
    {
        public GameObject EmptyCellMenu;
        public GameObject SquadCellMenu;
        public Vector3 offset;

        //EmptyCellMenu
        public Button AddDefault;
        public Button Import;
        public Button CloseEmptyMenu;
        //SquadCellMenu
        public Button Target;
        public Button Move;
        public Button Settings;
        public Button CloseSquadMenu;

        public event Action AddDefault_Clicked = () => { };
        public event Action Import_Clicked = () => { };
        public event Action CloseEmptyMenu_Clicked = () => { };

        public event Action Target_Clicked = () => { };
        public event Action Move_Clicked = () => { };
        public event Action Settings_Clicked = () => { };
        public event Action CloseSquadMenu_Clicked = () => { };

        private void Awake()
        {
            AddDefault.onClick.AddListener(OnAddDefaultClicked);
            Import.onClick.AddListener(OnImportClicked);
            CloseEmptyMenu.onClick.AddListener(OnCloseEmptyMenuClicked);

            Target.onClick.AddListener(OnTargetClicked);
            Move.onClick.AddListener(OnMoveClicked);
            Settings.onClick.AddListener(OnSettingsClicked);
            CloseSquadMenu.onClick.AddListener(OnCloseSquadMenuClicked);

            EmptyCellMenu.SetActive(false);
            SquadCellMenu.SetActive(false);
        }
        private void Start()
        {
            offset = new Vector3(0.5f, 0.5f);
        }
        private void OnAddDefaultClicked()
        {
            AddDefault_Clicked();
        }

        private void OnImportClicked()
        {
            Import_Clicked();
        }
        private void OnCloseEmptyMenuClicked()
        {
            CloseActiveMenu();
            CloseEmptyMenu_Clicked();
        }

        private void OnTargetClicked()
        {
            Target_Clicked();
        }

        private void OnMoveClicked()
        {
            Move_Clicked();
        }

        private void OnSettingsClicked()
        {
            Settings_Clicked();
        }

        private void OnCloseSquadMenuClicked()
        {
            CloseActiveMenu();
            CloseSquadMenu_Clicked();
        }


        public void ShowEmptyMenu(Vector3 coords)
        {
            EmptyCellMenu.transform.position = coords + offset;
            EmptyCellMenu.SetActive(true);
        }
        public void HideEmptyMenu()
        {
            EmptyCellMenu.SetActive(false);
        }

        public void ShowSquadMenu(Vector3 coords)
        {
            SquadCellMenu.transform.position = coords + offset;
            SquadCellMenu.SetActive(true);
        }
        public void HideSquadMenu()
        {
            SquadCellMenu.SetActive(false);
        }

        public void CloseActiveMenu()
        {
            foreach (GameObject menu in new GameObject[] { EmptyCellMenu, SquadCellMenu })
            {
                if (menu.activeSelf) menu.SetActive(false);
            } 
        }
    }
}

