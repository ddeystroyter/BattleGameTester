using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BattleGameTester.Core;

namespace BattleGameTester.UI
{
    public interface ISquadOverviewPanel
    {
        void Show(ISquad squad);
        void Hide();
    }
    public class SquadOverviewPanel : MonoBehaviour, ISquadOverviewPanel
    {
        [SerializeField] private Image Avatar;
        [SerializeField] private TMP_Text Name;
        [Header("Melee")]
        [SerializeField] private TMP_Text MeleeDmg;
        [SerializeField] private TMP_Text MeleeDef;
        [SerializeField] private TMP_Text MeleeCoef;
        [SerializeField] private TMP_Text MeleeAP;
        [SerializeField] private TMP_Text MeleeCrit;
        [SerializeField] private Image MeleeThrowImage;
        [SerializeField] private TMP_Text MeleeThrow;
        [Header("Range")]
        [SerializeField] private TMP_Text RangeDmg;
        [SerializeField] private TMP_Text RangeDef;
        [SerializeField] private TMP_Text RangeCoef;
        [SerializeField] private TMP_Text RangeAP;
        [SerializeField] private TMP_Text RangeCrit;
        [SerializeField] private Image RangeThrowImage;
        [SerializeField] private TMP_Text RangeThrow;
        [Header("CC")]
        [SerializeField] private TMP_Text CCDmg;
        [SerializeField] private TMP_Text CCDef;
        [SerializeField] private TMP_Text CCCoef;
        [SerializeField] private TMP_Text CCAP;
        [SerializeField] private TMP_Text CCCrit;
        [SerializeField] private Image CCThrowImage;
        [SerializeField] private TMP_Text CCThrow;

        private void Awake()
        {
            Hide();
        }

        public void Show(ISquad squad)
        {
            this.gameObject.SetActive(true);
            Avatar.sprite = CompositionRoot.GetSquadSprite(squad.SpriteName);
            Name.text = squad.Name;
            var melee = squad.Attacks.Melee;
            var range = squad.Attacks.Range;
            var cc = squad.Attacks.CC;

            //Melee
            MeleeDmg.text = melee.Dmg.ToString();
            MeleeDef.text = melee.Def.ToString();
            MeleeCoef.text = melee.Coef.ToString();
            MeleeAP.text = melee.AP.ToString();
            MeleeCrit.text = melee.Crit ?  "+" : "-";
            MeleeThrow.text = melee.Throw + "+";
            MeleeThrowImage.sprite = CompositionRoot.GetDiceFaceSprite(melee.Throw);

            //Range
            RangeDmg.text = range.Dmg.ToString();
            RangeDef.text = range.Def.ToString();
            RangeCoef.text = range.Coef.ToString();
            RangeAP.text = range.AP.ToString();
            RangeCrit.text = range.Crit ? "+" : "-";
            RangeThrow.text = range.Throw + "+";
            RangeThrowImage.sprite = CompositionRoot.GetDiceFaceSprite(range.Throw);

            //CC
            CCDmg.text = cc.Dmg.ToString();
            CCDef.text = cc.Def.ToString();
            CCCoef.text = cc.Coef.ToString();
            CCAP.text = cc.AP.ToString();
            CCCrit.text = cc.Crit ? "+" : "-";
            CCThrow.text = cc.Throw + "+";
            CCThrowImage.sprite = CompositionRoot.GetDiceFaceSprite(cc.Throw);

        }
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}

