using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace BattleGameTester.Core
{
    public class UserInput : MonoBehaviour, IUserInput
    {
        public event Action Escaped = () => { };
        public event Action Clicked = () => { };
        public event Action ClickedR = () => { };

        public event Action EnterPressed =() => { };
        public event Action Q_Pressed = () => { };
        public event Action W_Pressed = () => { };
        public event Action E_Pressed = () => { };
        public event Action Z_Pressed = () => { };
        public event Action P_Pressed = () => { };
        public event Action X_Pressed = () => { };
        public event Action Greater_Pressed = () => { };
        

        public bool IsLocked { get; set; }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {Escaped(); return;}
            if (Input.GetKeyDown(KeyCode.Return)) {EnterPressed(); return;}
            if (Input.GetMouseButtonDown(0)) { Clicked(); return; }

            if (IsLocked == true) {return;}
            if (Input.GetMouseButtonDown(1)) { ClickedR(); return; }
            if (Input.GetKeyDown(KeyCode.Q)) {Q_Pressed(); return; }
            if (Input.GetKeyDown(KeyCode.W)) {W_Pressed(); return; }
            if (Input.GetKeyDown(KeyCode.E)) {E_Pressed(); return; }
            if (Input.GetKeyDown(KeyCode.Z)) {Z_Pressed(); return; }
            if (Input.GetKeyDown(KeyCode.P)) {P_Pressed(); return;}
            if (Input.GetKeyDown(KeyCode.X)) {X_Pressed(); return;}
            if (Input.GetKeyDown(KeyCode.Period)) {Greater_Pressed(); return;}
        }
        public void Lock()
        {
            IsLocked = true;
        }
        public void Unlock()
        {
            IsLocked = false;
        }
    }
}
