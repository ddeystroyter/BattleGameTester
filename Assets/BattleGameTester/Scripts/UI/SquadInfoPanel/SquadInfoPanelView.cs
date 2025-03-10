using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BattleGameTester.Core;
using System;

namespace BattleGameTester.UI
{
    public class SquadInfoPanelView : BaseView, ISquadInfoPanelView
    {
        [SerializeField] private Image Avatar;
        [SerializeField] private TMP_Text nameBox;
        [SerializeField] private TMP_Text hpBox;
        [SerializeField] private Slider hpSlider;
        [SerializeField] private TMP_Text attackBox;
        [SerializeField] private TMP_Text apCoefBox;
        [SerializeField] private TMP_Text defenseBox;
        [SerializeField] private TMP_Text diceFaceBox;


        [SerializeField] private Image diceImgComponent;

        private ISquad _squad;

        private uint maxHP;

        public void Init(ISquad squad)
        {
            _squad = squad;
            SetPosition(_squad.Position);
            UpdateInfo(_squad);
        }

        public void UpdateInfo(ISquad squad)
        {
            if (squad == null) throw new NullReferenceException("Unassigned Squad to SquadInfoPanel");
            UpdateAvatar(squad.SpriteName);
            UpdateName(squad.Name);
            var hp = new Vector2Int((int)squad.Health, (int)squad.MaxHealth);
            UpdateHP(hp);
            //UpdateAttacks(squad.Attacks);
        }

        public void SetPosition(Vector3 pos)
        {
            this.gameObject.transform.position = pos + new Vector3(0,0.7f,0);
        }
        public void UpdateAvatar(string spriteName)
        {
            Avatar.sprite = CompositionRoot.GetSquadSprite(spriteName);
        }
        public void UpdateName(string name)
        {
            nameBox.text = name;
        }
        public void UpdateHP(uint val)
        {
            hpBox.text = $"{val}/{maxHP}";
            hpSlider.value = val;
        }
        public void UpdateHP(Vector2Int hp)
        {
            this.maxHP = (uint)hp.y;
            hpBox.text = $"{hp.x}/{maxHP}";
            hpSlider.maxValue = maxHP;
            hpSlider.value = (uint)hp.x;
        }
        public void UpdateAttacks(Attacks attacks)
        {
            //attackBox.text = $"{Atta.Melee}/{dmg.Range}/{dmg.CC}";
            //apCoefBox.text = $"x{dmg.Coef} | AP:{dmg.AP}";
            //defenseBox.text = $"{def.Melee}/{def.Range}/{def.CC}";
            //diceImgComponent.sprite = CompositionRoot.GetDiceFaceSprite(dice);
            //diceFaceBox.text = $"{dice}+";
        }
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}

