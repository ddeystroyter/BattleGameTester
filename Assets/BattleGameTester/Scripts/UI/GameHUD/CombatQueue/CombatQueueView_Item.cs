using BattleGameTester.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BattleGameTester.UI
{

    public class CombatQueueView_Item : BaseView_Item, ICombatQueueView_Item
    {
        public event Action<int> UpBtn_Clicked;
        public event Action<int> DownBtn_Clicked;
        public event Action<int> DeleteBtn_Clicked;

        public Transform Transform { get => this.gameObject.transform; }
        public CombatMove ActiveCM { get => _cm; }

        public Button UpBtn;
        public Button DownBtn;
        public TMP_Text Priority;
        public TMP_Text AttackingSquadName;
        public Image AvatarAttacking;
        public Image AttackTypeImage;
        public Image AvatarDefending;
        public TMP_Text DefendingSquadName;
        public Button DeleteBtn;

        private CombatMove _cm;

        public void Awake()
        {
            UpBtn.onClick.AddListener(() => UpBtn_Clicked?.Invoke(transform.GetSiblingIndex()));
            DownBtn.onClick.AddListener(() => DownBtn_Clicked?.Invoke(transform.GetSiblingIndex()));
            DeleteBtn.onClick.AddListener(() => DeleteBtn_Clicked?.Invoke(transform.GetSiblingIndex()));
            DisableDownBtn();
            DisableUpBtn();
        }

        public void OnDestroy()
        {
            _cm.AttackingSquad.AvatarChanged -= ChangeAttackingAvatar;
            _cm.AttackingSquad.NameChanged -= ChangeAttackingName;
            _cm.DefendingSquad.AvatarChanged -= ChangeDefendingAvatar;
            _cm.DefendingSquad.NameChanged -= ChangeDefendingName;
        }

        public void Init(CombatMove cm)
        {
            _cm = cm;
            Priority.text = _cm.Priority.ToString();
            AttackingSquadName.text = _cm.AttackingSquad.Name;
            AvatarAttacking.sprite = CompositionRoot.GetSquadSprite(_cm.AttackingSquad.SpriteName);
            AttackTypeImage.sprite = CompositionRoot.GetAttackTypeSprite(_cm.AttackType);
            AvatarDefending.sprite = CompositionRoot.GetSquadSprite(_cm.DefendingSquad.SpriteName);
            DefendingSquadName.text = _cm.DefendingSquad.Name;

            _cm.AttackingSquad.AvatarChanged += ChangeAttackingAvatar;
            _cm.AttackingSquad.NameChanged += ChangeAttackingName;
            _cm.DefendingSquad.AvatarChanged += ChangeDefendingAvatar;
            _cm.DefendingSquad.NameChanged += ChangeDefendingName;
        }

        private void ChangeAttackingAvatar(string spriteName)
        {
            AvatarAttacking.sprite = CompositionRoot.GetSquadSprite(spriteName);
        }
        private void ChangeAttackingName(string name)
        {
            AttackingSquadName.text = name;
        }
        private void ChangeDefendingAvatar(string spriteName)
        {
            AvatarDefending.sprite = CompositionRoot.GetSquadSprite(spriteName);
        }
        private void ChangeDefendingName(string name)
        {
            DefendingSquadName.text = name;
        }

        public void ChangePriorityTo(byte priority)
        {
            _cm.Priority = priority;
            Priority.text = priority.ToString();
        }

        public void EnableUpBtn()
        {
            UpBtn.interactable = true;
        }
        public void DisableUpBtn()
        {
            UpBtn.interactable = false;
        }
        public void EnableDownBtn()
        {
            DownBtn.interactable = true;
        }
        public void DisableDownBtn()
        {
            DownBtn.interactable = false;
        }
    }
}

