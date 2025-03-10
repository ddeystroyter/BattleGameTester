using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using BattleGameTester.Core;

namespace BattleGameTester.UI
{
    public enum LogBoxIcons
    {
        Info,
        Damage,
        DiceRoll,
        Death,
        Skip
    }
    public interface IEventLogBox_Item : IView_Item
    {
        void Init(string str);
        void Init(string str, LogBoxIcons icon);
    }
    public class EventLogBox_Item : BaseView_Item, IEventLogBox_Item
    {
        [SerializeField] private TMP_Text _message;
        [SerializeField] private Image _img;
        [SerializeField] private uint _stringHeight = 30;
        [SerializeField] private uint _maxStringLength = 65;
        

        public void Init(string str)
        {
            if (str.Length > _maxStringLength)
            {
                var rt = this.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, _stringHeight * (str.Length/_maxStringLength + 1));
            }
            _message.text = str;
        }

        public void Init(string str, LogBoxIcons icon)
        {
            Init(str);
            var tmp = CompositionRoot.GetLogBoxSprite(icon);
            _img.sprite = tmp;
        }

    }
}

