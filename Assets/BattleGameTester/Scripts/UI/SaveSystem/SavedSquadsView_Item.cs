using BattleGameTester.Core;
using System;
using System.ComponentModel.Design.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BattleGameTester.UI
{
    public class SavedSquadsView_Item : BaseView_Item, ISavedSquadsView_Item
    {
        public event Action<int> AddBtn_Clicked;
        public event Action<int> DeleteBtn_Clicked;

        [SerializeField] private Button AddBtn;
        [SerializeField] private Button DeleteBtn;

        [SerializeField] private Image Avatar;
        [SerializeField] private TMP_Text Name;
        [SerializeField] private TMP_Text HP;

        [Header("Melee")]
        [SerializeField] private TMP_Text Melee;
        [SerializeField] private TMP_Text MeleeThrowTxt;
        [SerializeField] private Image MeleeThrowImg;
        [Header("Range")]
        [SerializeField] private TMP_Text Range;
        [SerializeField] private TMP_Text RangeThrowTxt;
        [SerializeField] private Image RangeThrowImg;
        [Header("CC")]
        [SerializeField] private TMP_Text CC;
        [SerializeField] private TMP_Text CCThrowTxt;
        [SerializeField] private Image CCThrowImg;

        private int _id;

        private void Awake()
        {
            AddBtn.onClick.AddListener(() => AddBtn_Clicked?.Invoke(_id));
            DeleteBtn.onClick.AddListener(() => DeleteBtn_Clicked?.Invoke(_id));

        }
        public void Create(int id, string spriteName, string name, Vector2Int hp, Attacks attacks)
        {
            _id = id;
            Avatar.sprite = CompositionRoot.GetSquadSprite(spriteName);
            Name.text = name;
            HP.text = $"{hp.y} HP";
            var m = attacks.Melee;
            var r = attacks.Range;
            var c = attacks.CC;
            Melee.text = $"{m.Dmg}/{m.Def}/x{m.Coef}/AP:{m.AP}";
            MeleeThrowTxt.text = $"{m.Throw}+";
            MeleeThrowImg.sprite = CompositionRoot.GetDiceFaceSprite(m.Throw);
            Range.text = $"{r.Dmg}/{r.Def}/x{r.Coef}/AP:{r.AP}";
            RangeThrowTxt.text = $"{r.Throw}+";
            RangeThrowImg.sprite = CompositionRoot.GetDiceFaceSprite(r.Throw);
            CC.text = $"{c.Dmg}/{c.Def}/x{c.Coef}/AP:{c.AP}";
            CCThrowTxt.text = $"{c.Throw}+";
            CCThrowImg.sprite = CompositionRoot.GetDiceFaceSprite(c.Throw);

        }
    }
}

