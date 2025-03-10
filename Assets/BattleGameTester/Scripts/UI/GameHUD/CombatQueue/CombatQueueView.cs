using BattleGameTester.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

namespace BattleGameTester.UI
{
    public class CombatQueueView : BaseView, ICombatQueueView
    {
        [SerializeField] private TMP_Text _playerName;
        [SerializeField] private Image _roundTypeImage;
        [SerializeField] private Transform _content;

        private IResourceManager _resourceManager;
        private EPlayer _activePlayer;
        private AttackType _activeTurnType;
        private List<CombatMove> _activeCms;

        private void Awake()
        {
            _resourceManager = CompositionRoot.GetResourceManager();
        }

        public void Show(EPlayer player, AttackType turnType, List<CombatMove> cms)
        {
            ClearContent();
            
            Show();
            _activePlayer = player;
            _activeCms = cms;
            _activeTurnType = turnType;
            _roundTypeImage.sprite = CompositionRoot.GetAttackTypeSprite(turnType);

            _playerName.text = player == EPlayer.P1 ? $"Player 1 | TURN ({turnType})" : $"Player 2 | TURN ({turnType})";


            cms.Sort((x,y) => x.Priority.CompareTo(y.Priority));

            
            for (int i = 0; i < cms.Count; i++)
            {
                cms[i].Priority = (byte)i;
                AddCombatMove(cms[i], false);
            }
            StartCoroutine(UpdateItemsButtons());
        }


        public void AddCombatMove(CombatMove cm, bool needUpdateButtons = true)
        {
            var item = _resourceManager.CreatePrefabInstance<ICombatQueueView_Item, EUI_Items>(EUI_Items.CombatQueue_Item);
            item.SetParent(_content);
            item.Init(cm);
            item.UpBtn_Clicked += ItemOnUpBtn_Clicked;
            item.DownBtn_Clicked += ItemOnDownBtn_Clicked;
            item.DeleteBtn_Clicked += ItemOnDeleteBtn_Clicked;
            if (needUpdateButtons) StartCoroutine(UpdateItemsButtons()); 
        }

        private IEnumerator UpdateItemsButtons()
        {
            yield return new WaitForSeconds(0.3f);
            if (_content.childCount == 0) yield break;
            if (_content.childCount == 1)
            {
                var item = _content.GetChild(0).GetComponent<ICombatQueueView_Item>();
                item.DisableUpBtn();
                item.DisableDownBtn();
                yield break;
            }
            for (int i = 1; i < _content.childCount - 1; i++)
            {
                var item = _content.GetChild(i).GetComponent<ICombatQueueView_Item>();
                item.EnableUpBtn();
                item.EnableDownBtn();
            }
            var firstItem = _content.GetChild(0).GetComponent<ICombatQueueView_Item>();
            firstItem.DisableUpBtn();
            firstItem.EnableDownBtn();
            var lastItem = _content.GetChild(_content.childCount - 1).GetComponent<ICombatQueueView_Item>();
            lastItem.EnableUpBtn();
            lastItem.DisableDownBtn();

            yield break;
        }
        private void ItemOnUpBtn_Clicked(int index)
        {
            try
            {
                var UppedItem = _content.GetChild(index).GetComponent<ICombatQueueView_Item>();
                var DownedItem = _content.GetChild(index - 1).GetComponent<ICombatQueueView_Item>();
                UppedItem.ChangePriorityTo((byte)(index - 1));
                UppedItem.Transform.SetSiblingIndex(index - 1);
                DownedItem.ChangePriorityTo((byte)index);
                StartCoroutine(UpdateItemsButtons());
            }
            catch (UnityException) { return; }
        }
        private void ItemOnDownBtn_Clicked(int index)
        {
            try
            {
                var DownedItem = _content.GetChild(index).GetComponent<ICombatQueueView_Item>();
                var UppedItem = _content.GetChild(index + 1).GetComponent<ICombatQueueView_Item>();
                DownedItem.ChangePriorityTo((byte)(index + 1));
                DownedItem.Transform.SetSiblingIndex(index + 1);
                UppedItem.ChangePriorityTo((byte)index);
                StartCoroutine(UpdateItemsButtons());
            }
            catch(UnityException) { return; }
        }
        private void ItemOnDeleteBtn_Clicked(int index)
        {
            var DeletedItem = _content.GetChild(index).GetComponent<ICombatQueueView_Item>();
            DeletedItem.Destroy();
            foreach (var cm in _activeCms.FindAll(cm => cm.Priority > DeletedItem.ActiveCM.Priority))
            {
                cm.Priority -= 1;
            }

            _activeCms.Remove(DeletedItem.ActiveCM);
            ClearContent();
            Show(_activePlayer, _activeTurnType, _activeCms);
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

