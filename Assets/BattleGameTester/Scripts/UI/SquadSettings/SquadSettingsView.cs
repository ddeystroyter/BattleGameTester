using BattleGameTester.Core;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace BattleGameTester.UI
{

    public class SquadSettingsView : BaseView, ISquadSettingsView
    {
        public event Action<string> SaveNameBtn_Clicked;
        public event Action CloseBtn_Clicked;
        public event Action SaveBtn_Clicked;
        public event Action DeleteBtn_Clicked;
        public event Action Closed;

        [SerializeField] private Image Avatar;
        [Header("Name")]
        [SerializeField] private TMP_InputField Name_InputField;
        [SerializeField] private Button EditNameBtn;
        [SerializeField] private Button SaveNameBtn;
        [Header("HealthBar")]
        [SerializeField] private TMP_Text HealthBarText;
        [SerializeField] private Slider HealthBarSlider;
        [SerializeField] private Button RestoreHPBtn;
        [Header("MainButtons")]
        [SerializeField] private Button SaveBtn;
        [SerializeField] private Button DeleteBtn;
        [SerializeField] private Button CloseBtn;
        [SerializeField] private Button OKBtn;
        [Header("HP")]
        [SerializeField] private UI_InputCounter CurrentHP_InputCounter;
        [SerializeField] private UI_InputCounter MaxHP_InputCounter;
        [Header("Melee")]
        [SerializeField] private UI_InputCounter MeleeDmg_InputCounter;
        [SerializeField] private UI_InputCounter MeleeDef_InputCounter;
        [SerializeField] private UI_InputCounter MeleeCoef_InputCounter;
        [SerializeField] private UI_InputCounter MeleeAP_InputCounter;
        [SerializeField] private Toggle MeleeCrit_Toggle;
        [Header("Range")]
        [SerializeField] private UI_InputCounter RangeDmg_InputCounter;
        [SerializeField] private UI_InputCounter RangeDef_InputCounter;
        [SerializeField] private UI_InputCounter RangeCoef_InputCounter;
        [SerializeField] private UI_InputCounter RangeAP_InputCounter;
        [SerializeField] private Toggle RangeCrit_Toggle;
        [Header("CC")]
        [SerializeField] private UI_InputCounter CCDmg_InputCounter;
        [SerializeField] private UI_InputCounter CCDef_InputCounter;
        [SerializeField] private UI_InputCounter CCCoef_InputCounter;
        [SerializeField] private UI_InputCounter CCAP_InputCounter;
        [SerializeField] private Toggle CCCrit_Toggle;

        [Header("Throws (Toggle groups)")]
        [SerializeField] private UI_ToggleGroup MeleeThrow;
        [SerializeField] private UI_ToggleGroup RangeThrow;
        [SerializeField] private UI_ToggleGroup CCThrow;


        [Header("SquadImages")]
        [SerializeField] private TMP_Dropdown SquadImageDropdown;
        [SerializeField] private Button RefreshSquadSprites;

        private IUserInput _userInput;
 
        private void Awake()
        {
            _userInput = CompositionRoot.GetUserInput();
            EditNameBtn.onClick.AddListener(OnEditName_Clicked);
            SaveNameBtn.onClick.AddListener(OnSaveName_Clicked);
            RestoreHPBtn.onClick.AddListener(() => RestoreHP());
            SaveBtn.onClick.AddListener(() => SaveBtn_Clicked?.Invoke());
            DeleteBtn.onClick.AddListener(() => DeleteBtn_Clicked?.Invoke());
            OKBtn.onClick.AddListener(() => CloseBtn_Clicked?.Invoke());
            CloseBtn.onClick.AddListener(() => CloseBtn_Clicked?.Invoke());
            RefreshSquadSprites.onClick.AddListener(() => RefreshDropdown());
            CurrentHP_InputCounter.ValueChanged += (val) => { SetHealthBar(new Vector2Int(val, MaxHP_InputCounter.Value)); };
            MaxHP_InputCounter.ValueChanged += (val) => { SetHealthBar(new Vector2Int(CurrentHP_InputCounter.Value, val)); };
            gameObject.SetActive(false);
            RefreshDropdown();
        }

        public void Show(string name, Vector2Int HP, Attacks attacks, string spriteName)
        {
            _userInput.Escaped += Hide;
            this.Show();
            Debug.Log("Input Locked");
            //Set Avatar
            Avatar.sprite = CompositionRoot.GetSquadSprite(spriteName);
            //Set Name
            Name_InputField.text = name;
            Name_InputField.interactable = false;

            SetHP(HP);

            SetAttacks(attacks);

            SquadImageDropdown.value = CompositionRoot.GetSquadSpriteIndex(spriteName);
        }
        new public void Hide()
        {
            Closed();
            _userInput.Escaped -= Hide;
            gameObject.SetActive(false);
        }
        public void RefreshDropdown()
        {
            var temp = SquadImageDropdown.options[SquadImageDropdown.value].text;
            Debug.Log(temp);
            temp = temp == "Option A" ? "default.jpg" : temp;

            SquadImageDropdown.ClearOptions();
            CompositionRoot.RefreshSquadSprites();
            CompositionRoot.ShowPopUp("OK! Images Refresh. It may take some time....");
            var sprites = CompositionRoot.GetSquadSprites();
            var DropdownOptions = new List<TMP_Dropdown.OptionData>();
            foreach (var sprite in sprites)
            {
                DropdownOptions.Add(new TMP_Dropdown.OptionData(sprite.Key, sprite.Value));
            }
            SquadImageDropdown.AddOptions(DropdownOptions);
            SquadImageDropdown.value = CompositionRoot.GetSquadSpriteIndex(temp);
        }

        public string GetName()
        {
            return Name_InputField.text;
        }
        public Vector2Int GetHPSettings()
        {
            return new Vector2Int(CurrentHP_InputCounter.Value, MaxHP_InputCounter.Value);
        }
        public Attacks.V6 GetMelee ()
        {
            return new Attacks.V6(MeleeDmg_InputCounter.Value, MeleeDef_InputCounter.Value, MeleeCoef_InputCounter.Value, MeleeAP_InputCounter.Value, 
                ushort.Parse(MeleeThrow.GetValue()), MeleeCrit_Toggle.isOn);
        }
        public Attacks.V6 GetRange()
        {
            return new Attacks.V6(RangeDmg_InputCounter.Value, RangeDef_InputCounter.Value, RangeCoef_InputCounter.Value, RangeAP_InputCounter.Value,
                ushort.Parse(RangeThrow.GetValue()), RangeCrit_Toggle.isOn);
        }
        public Attacks.V6 GetCC()
        {
            return new Attacks.V6(CCDmg_InputCounter.Value, CCDef_InputCounter.Value, CCCoef_InputCounter.Value, CCAP_InputCounter.Value,
                ushort.Parse(CCThrow.GetValue()), CCCrit_Toggle.isOn);
        }

        public string GetSpriteName()
        {
            return SquadImageDropdown.options[SquadImageDropdown.value].text;
        }

        #region Save/Edit Name
        private void OnEditName_Clicked()
        {
            Name_InputField.interactable = true;
            Name_InputField.caretPosition = Name_InputField.text.Length;
            EditNameBtn.gameObject.SetActive(false);
            SaveNameBtn.gameObject.SetActive(true);

            SetInteractability(false);
            _userInput.Escaped += OnSaveName_Clicked;
            _userInput.EnterPressed += OnSaveName_Clicked;
        }
        private void OnSaveName_Clicked()
        {
            Name_InputField.interactable = false;
            Name_InputField.caretPosition = 0;
            SaveNameBtn.gameObject.SetActive(false);
            EditNameBtn.gameObject.SetActive(true);
            SaveNameBtn_Clicked?.Invoke(Name_InputField.text);

            SetInteractability(true);
            _userInput.Escaped -= OnSaveName_Clicked;
            _userInput.EnterPressed -= OnSaveName_Clicked;
        }
        private void SetInteractability(bool value)
        {
            RestoreHPBtn.interactable = value;
            SaveBtn.interactable = value;
            DeleteBtn.interactable = value;
            OKBtn.interactable = value;
            CloseBtn.interactable = value;
            OKBtn.interactable = value;

            CurrentHP_InputCounter.interactable = value;
            MaxHP_InputCounter.interactable = value;

            MeleeDmg_InputCounter.interactable = value;
            MeleeDef_InputCounter.interactable = value;
            MeleeCoef_InputCounter.interactable = value;
            MeleeAP_InputCounter.interactable = value;
            MeleeCrit_Toggle.interactable = value;
            MeleeThrow.interactable = value;

            RangeDmg_InputCounter.interactable = value;
            RangeDef_InputCounter.interactable = value;
            RangeCoef_InputCounter.interactable = value;
            RangeAP_InputCounter.interactable = value;
            RangeCrit_Toggle.interactable = value;
            RangeThrow.interactable = value;

            CCDmg_InputCounter.interactable = value;
            CCDef_InputCounter.interactable = value;
            CCCoef_InputCounter.interactable = value;
            CCAP_InputCounter.interactable = value;
            CCCrit_Toggle.interactable = value;
            CCThrow.interactable = value;

            RefreshSquadSprites.interactable = value;
            SquadImageDropdown.interactable = value;
        }
        #endregion
        private void SetHealthBar(Vector2Int HP)
        {
            HealthBarText.text = $"{HP.x}/{HP.y}";
            HealthBarSlider.minValue = 0;
            HealthBarSlider.maxValue = HP.y;
            HealthBarSlider.value = HP.x;
        }
        private void SetHP(Vector2Int HP)
        {
            SetHealthBar(HP);
            //Set HP
            MaxHP_InputCounter.Value = (ushort)HP.y;
            CurrentHP_InputCounter.Value = (ushort)HP.x;
        }
        private void RestoreHP()
        {
            var maxHP = MaxHP_InputCounter.Value;
            SetHP(new Vector2Int(maxHP, maxHP));
        }
        private void SetAttacks(Attacks attacks)
        {
            //Melee
            MeleeDmg_InputCounter.Value = attacks.Melee.Dmg;
            MeleeDef_InputCounter.Value = attacks.Melee.Def;
            MeleeCoef_InputCounter.Value = attacks.Melee.Coef;
            MeleeAP_InputCounter.Value = attacks.Melee.AP;
            MeleeCrit_Toggle.isOn = attacks.Melee.Crit;
            MeleeThrow.SetValue(attacks.Melee.Throw.ToString());
            //Range
            RangeDmg_InputCounter.Value = attacks.Range.Dmg;
            RangeDef_InputCounter.Value = attacks.Range.Def;
            RangeCoef_InputCounter.Value = attacks.Range.Coef;
            RangeAP_InputCounter.Value = attacks.Range.AP;
            RangeCrit_Toggle.isOn = attacks.Range.Crit;
            RangeThrow.SetValue(attacks.Range.Throw.ToString());
            //CC
            CCDmg_InputCounter.Value = attacks.CC.Dmg;
            CCDef_InputCounter.Value = attacks.CC.Def;
            CCCoef_InputCounter.Value = attacks.CC.Coef;
            CCAP_InputCounter.Value = attacks.CC.AP;
            CCCrit_Toggle.isOn = attacks.CC.Crit;
            CCThrow.SetValue(attacks.CC.Throw.ToString());
        }
    }
}