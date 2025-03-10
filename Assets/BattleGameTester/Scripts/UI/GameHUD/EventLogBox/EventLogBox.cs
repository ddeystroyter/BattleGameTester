using BattleGameTester.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace BattleGameTester.UI
{
    public interface IEventLogBox
    {
        void Log(string message);
        void Log(string message, LogBoxIcons icon);
    }
    public class EventLogBox : MonoBehaviour, IEventLogBox
    {
        [SerializeField] private Transform _content;
        [SerializeField] private IResourceManager _resourceManager;
        [SerializeField] private ScrollRect _scrollRect;
        //[SerializeField] private Scrollbar _scrollbar;

        void Start()
        {
            _resourceManager = CompositionRoot.GetResourceManager();
            ClearContent();
        }

        public void Log(string message)
        {
            var item = _resourceManager.CreatePrefabInstance<IEventLogBox_Item, EUI_Items>(EUI_Items.EventLogBox_Item);
            item.SetParent(_content);
            item.Init(message);
            _scrollRect.verticalScrollbar.value = -2f;
        }

        public void Log(string message, LogBoxIcons icon) 
        {
            var item = _resourceManager.CreatePrefabInstance<IEventLogBox_Item, EUI_Items>(EUI_Items.EventLogBox_Item);
            item.SetParent(_content);
            item.Init(message, icon);
            _scrollRect.verticalScrollbar.value = -2f;
        }

        private void ClearContent()
        {
            var childItems = _content.gameObject.GetComponentsInChildren<Transform>();
            for (int i = 1; i < childItems.Length; i++)
            {
                Destroy(childItems[i].gameObject);
            }
        }
    }
}

