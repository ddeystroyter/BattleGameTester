using BattleGameTester.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace BattleGameTester.UI
{
    public class SavedSquadsView : BaseView, ISavedSquadsView
    {
        public event Action RefreshBtn_Clicked;
        public event Action CloseBtn_Clicked;

        [SerializeField] private Transform _content;
        [SerializeField] public Transform Content { get => _content; }

        [SerializeField] private Button RefreshBtn;
        [SerializeField] private Button CloseBtn;

        private void Awake()
        {
            Hide();
            ClearContent();

            RefreshBtn.onClick.AddListener(() => RefreshBtn_Clicked?.Invoke());
            CloseBtn.onClick.AddListener(() => { this.Hide(); CloseBtn_Clicked?.Invoke(); });
        }

        public void ClearContent()
        {
            var childItems = _content.gameObject.GetComponentsInChildren<Transform>();
            for (int i = 1; i < childItems.Length; i++)
            {
                Destroy(childItems[i].gameObject);
            }
        }
        new public void Show()
        {
            gameObject.SetActive(true);
            ClearContent();
            //TODO
            //Load from Prefs And Create Prefabs
        }
    }

}
