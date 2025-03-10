using BattleGameTester.Core;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace BattleGameTester.UI
{
    public interface IGameHUD_Buttons
    {
        event Action PlayerChange_Clicked;
        event Action NextCamera_Clicked;
        event Action FreeCamera_Clicked;
        event Action Restart_Clicked;

        event Action MakeTurn_Clicked;
        event Action SkipTurn_Clicked;
        event Action DiceRoll_Clicked;
        event Action Settings_Clicked;

        void Lock();
        void Unlock();
    }
    public class GameHUD_Buttons : MonoBehaviour, IGameHUD_Buttons
    {
        public event Action PlayerChange_Clicked;
        public event Action NextCamera_Clicked;
        public event Action FreeCamera_Clicked;
        public event Action Restart_Clicked;

        public event Action MakeTurn_Clicked;
        public event Action SkipTurn_Clicked;
        public event Action DiceRoll_Clicked;
        public event Action Settings_Clicked;

        private IUserInput userInput;
        [SerializeField] private Button PlayerChange;
        [SerializeField] private Button NextCamera;
        [SerializeField] private Button FreeCamera;
        [SerializeField] private Button Restart;

        [SerializeField] private Button MakeTurn;
        [SerializeField] private Button SkipTurn;
        [SerializeField] private Button DiceRoll;
        [SerializeField] private Button Settings;

        private void Awake()
        {
            PlayerChange.onClick.AddListener(() => PlayerChange_Clicked?.Invoke());
            NextCamera.onClick.AddListener(() => NextCamera_Clicked?.Invoke());
            FreeCamera.onClick.AddListener(() => FreeCamera_Clicked?.Invoke());
            Restart.onClick.AddListener(() => Restart_Clicked?.Invoke());

            MakeTurn.onClick.AddListener(() => MakeTurn_Clicked?.Invoke());
            SkipTurn.onClick.AddListener(() => SkipTurn_Clicked?.Invoke());
            DiceRoll.onClick.AddListener(() => DiceRoll_Clicked?.Invoke());
            Settings.onClick.AddListener(() => Settings_Clicked?.Invoke());
            Unlock();
        }

        public void Lock()
        {
            PlayerChange.interactable = false;
            NextCamera.interactable = false;
            FreeCamera.interactable = false;
            Restart.interactable = false;

            MakeTurn.interactable = false;
            SkipTurn.interactable = false;
            DiceRoll.interactable = false;
            Settings.interactable = false;
        }

        public void Unlock()
        {
            PlayerChange.interactable = true;
            NextCamera.interactable = true;
            FreeCamera.interactable = true;
            Restart.interactable = true;

            MakeTurn.interactable = true;
            SkipTurn.interactable = true;
            DiceRoll.interactable = true;
            Settings.interactable = true;
        }
    }
}

