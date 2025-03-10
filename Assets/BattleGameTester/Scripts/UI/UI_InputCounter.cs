using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BattleGameTester.UI
{
    public class UI_InputCounter : MonoBehaviour
    {
        public event Action<ushort> ValueChanged;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Button increaseBtn;
        [SerializeField] private Button decreaseBtn;
        [SerializeField] private uint step; 
        [SerializeField] private ushort MAX_VAL = 99;
        [SerializeField] private UI_InputCounter AdditionalMAX;
        [SerializeField] public ushort Value { get => ushort.Parse(inputField.text); 
            set 
            {
                ushort tempVal;
                if (AdditionalMAX != null)
                    tempVal = (ushort)Mathf.Clamp(value, 0, AdditionalMAX.Value);
                else
                    tempVal = (ushort)Mathf.Clamp(value, 0, MAX_VAL);
                inputField.text = tempVal.ToString();
                ValueChanged?.Invoke(tempVal);
            }
        }

        public bool interactable {set { inputField.interactable = value; increaseBtn.interactable = value; decreaseBtn.interactable = value; }}

        private void Awake()
        {
            increaseBtn.onClick.AddListener(OnIncreaseClicked);
            decreaseBtn.onClick.AddListener(OnDecreaseClicked);
            inputField.onValueChanged.AddListener(OnValueChanged);
            if (AdditionalMAX != null) { AdditionalMAX.ValueChanged += (maxValue) => { if (Value > maxValue) Value = maxValue; }; }
        }

        private void OnIncreaseClicked()
        {
            Value = ushort.Parse(inputField.text);
            inputField.text = (Value + step).ToString();
        }
        private void OnDecreaseClicked()
        {
            Value = ushort.Parse(inputField.text);
            inputField.text = (Value - step) >= 0 ? (Value - step).ToString() : "0";
        }
        private void OnValueChanged(string val)
        {
            int temp;
            if (int.TryParse(val, out temp))
            {
                if (temp > MAX_VAL) Value = MAX_VAL;
                else Value = (ushort)Math.Abs(temp);
            }
            else 
            {
                inputField.text = "0";
            }
        }
    }

}
