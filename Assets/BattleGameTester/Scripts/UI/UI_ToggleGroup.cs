using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BattleGameTester.UI
{
    public class UI_ToggleGroup : MonoBehaviour
    {
        public ToggleGroup toggleGroup;
        private Toggle[] _toggles = new Toggle[6];
        public bool interactable
        { 
            set
            {
                foreach (Toggle toggle in _toggles)
                {
                    toggle.interactable = value;
                }
            } 
        }
        public void Awake()
        {
            _toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        }
        public string GetValue()
        {
            return toggleGroup.ActiveToggles().Where(x => x.isOn).FirstOrDefault().gameObject.name;
        }
        public void SetValue(string val)
        {
            _toggles.Where(x=>x.gameObject.name == val).FirstOrDefault().isOn = true;
        }

    }
}

