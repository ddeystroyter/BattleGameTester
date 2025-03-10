using BattleGameTester.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace BattleGameTester.UI
{
    public class AttackTypeChoiceView : BaseView, IAttackTypeChoiceView
    {
        public event Action<AttackType> AttackType_Confirmed;
        public event Action CloseBtn_Clicked;

        [Header("Attack Type Buttons")]
        [SerializeField] private Button MeleeBtn;
        [SerializeField] private Button RangeBtn;
        [SerializeField] private Button CCBtn;
        [SerializeField] private Button SkipBtn;

        [Header("Confirm/Close Buttons")]
        [SerializeField] private Button ConfirmBtn;
        [SerializeField] private Button CloseBtn;

        [Header("Squad Panels & Avatars")]
        [SerializeField] public SquadInfoPanelView AttackingSquad_PanelView;
        [SerializeField] public Image AttackingSquad_Avatar;
        [SerializeField] public SquadInfoPanelView DefendingSquad_PanelView;
        [SerializeField] public Image DefendingSquad_Avatar;

        [Header("Attacking Stats")]
        [SerializeField] private TMP_Text L_Dmg;
        [SerializeField] private TMP_Text L_Def;
        [SerializeField] private TMP_Text L_Coef;
        [SerializeField] private TMP_Text L_AP;
        [SerializeField] private TMP_Text L_Crit;
        [SerializeField] private TMP_Text L_Throw;

        [Header("Attacked Stats")]
        [SerializeField] private TMP_Text R_Dmg;
        [SerializeField] private TMP_Text R_Def;
        [SerializeField] private TMP_Text R_Coef;
        [SerializeField] private TMP_Text R_AP;
        [SerializeField] private TMP_Text R_Crit;
        [SerializeField] private TMP_Text R_Throw;

        private Outline MeleeBtnOutline;
        private Outline RangeBtnOutline;
        private Outline CCBtnOutline;
        private Outline SkipBtnOutline;

        private Vector2 Picked = new Vector2(5,5);
        private Vector2 Default = new Vector2(2,2);

        private AttackType SelectedAttackType;
        private ISquad AttackingSquad;
        private ISquad DefendingSquad;


        private void Awake()
        {
            MeleeBtnOutline = MeleeBtn.GetComponent<Outline>();
            RangeBtnOutline = RangeBtn.GetComponent<Outline>();
            CCBtnOutline = CCBtn.GetComponent<Outline>();
            SkipBtnOutline = SkipBtn.GetComponent<Outline>();

            CloseBtn.onClick.AddListener(() => CloseBtn_Clicked?.Invoke());
            MeleeBtn.onClick.AddListener(() => UpdateAttackValues(AttackType.Melee));
            RangeBtn.onClick.AddListener(() => UpdateAttackValues(AttackType.Range));
            CCBtn.onClick.AddListener(() => UpdateAttackValues(AttackType.CC));
            SkipBtn.onClick.AddListener(() => UpdateAttackValues(AttackType.Skip));

            ConfirmBtn.onClick.AddListener(() => AttackType_Confirmed(SelectedAttackType));
        }
        private void OnEnable()
        {
            ConfirmBtn.interactable = false;
            SetButtonsOutlineToDefault();
            SetValuesToUnknown();
        }
        
        public void Show(ISquad attacking, ISquad defending)
        {
            AttackingSquad = attacking;
            DefendingSquad = defending;
            Debug.Log(attacking.Name + " ->" + defending.Name);
            AttackingSquad_PanelView.UpdateInfo(attacking);
            DefendingSquad_PanelView.UpdateInfo(defending);
            AttackingSquad_Avatar.sprite = CompositionRoot.GetSquadSprite(attacking.SpriteName);
            DefendingSquad_Avatar.sprite = CompositionRoot.GetSquadSprite(defending.SpriteName);
            Show();
        }

        private void UpdateAttackValues(AttackType attackType)
        {
            SelectedAttackType = attackType;
            SetButtonsOutlineToDefault();

            
            switch (attackType)
            {
                case AttackType.Melee:
                    SetValues(AttackingSquad.Attacks.Melee, DefendingSquad.Attacks.Melee);
                    MeleeBtnOutline.effectDistance = Picked;
                    break;
                case AttackType.Range:
                    SetValues(AttackingSquad.Attacks.Range, DefendingSquad.Attacks.Range); 
                    RangeBtnOutline.effectDistance = Picked;
                    break;
                case AttackType.CC:
                    SetValues(AttackingSquad.Attacks.CC, DefendingSquad.Attacks.CC);
                    CCBtnOutline.effectDistance = Picked; 
                    break;
                case AttackType.Skip:
                    SetValues(new Attacks.V6(), new Attacks.V6());
                    SkipBtnOutline.effectDistance = Picked;
                    break;

            }

            ConfirmBtn.interactable = true;
        }

        private void SetValues(Attacks.V6 l, Attacks.V6 r)
        {
            L_Dmg.text = l.Dmg.ToString();
            L_Def.text = l.Def.ToString();
            L_Coef.text = "x" + l.Coef;
            L_AP.text = l.AP.ToString();
            L_Crit.text = l.Crit ? "+" : "-";
            L_Throw.text = l.Throw + "+";

            R_Dmg.text = r.Dmg.ToString();
            R_Def.text = r.Def.ToString();
            R_Coef.text = "x" + r.Coef;
            R_AP.text = r.AP.ToString();
            R_Crit.text = r.Crit ? "+" : "-";
            R_Throw.text = r.Throw + "+";

        }
        private void SetValuesToUnknown()
        {
            L_Dmg.text = "??";
            L_Def.text = "??";
            L_Coef.text = "x??";
            L_AP.text = "??";
            L_Crit.text = "?";
            L_Throw.text = "?+";

            R_Dmg.text = "??";
            R_Def.text = "??";
            R_Coef.text = "x??";
            R_AP.text = "??";
            R_Crit.text = "?";
            R_Throw.text = "?+";

        }
        private void SetButtonsOutlineToDefault()
        {
            MeleeBtnOutline.effectDistance = Default;
            RangeBtnOutline.effectDistance = Default;
            CCBtnOutline.effectDistance = Default;
            SkipBtnOutline.effectDistance = Default;
        }
    }
}
