using BattleGameTester.Core;
using BattleGameTester.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BattleGameTester.UI
{
    public class SquadSettings : MonoBehaviour, ISquadSettings
    {
        public event Action<ISquad> Save_Clicked;
        public event Action Delete_Clicked;
        public event Action Closed;

        private static ISquadSettingsView View;
        private static ISquad _squad;

        private void Awake()
        {
            var viewFactory = CompositionRoot.GetViewFactory();
            View = viewFactory.CreateSquadSettings();
            View.SetParent(CompositionRoot.GetUIRoot().OverlayCanvas);
            View.SaveBtn_Clicked += OnSaveButtonClicked;
            View.DeleteBtn_Clicked += () => { Delete_Clicked?.Invoke(); View.Hide(); Closed?.Invoke(); };
            View.Closed += () => Closed();
            View.CloseBtn_Clicked += Hide;
            View.SaveNameBtn_Clicked += SaveName;
        }

        public void Show(ISquad squad)
        {
            _squad = squad;
            var hp = new Vector2Int((int)squad.Health, (int)squad.MaxHealth);
            var attacks = squad.Attacks;
            var spriteName = squad.SpriteName;

            View.Show(squad.Name, hp, attacks, spriteName);
        }

        public void Hide()
        {
            if (_squad == null) { CompositionRoot.ShowPopUp("ERROR! No Squad to Save!"); return; }
            SaveSquadSettings();
            _squad = null;
            View.Hide();
            Closed?.Invoke();
        }
        private void SaveName(string name)
        {
            _squad.Name = name;
        }
        private void OnSaveButtonClicked()
        {
            SaveSquadSettings();
            Save_Clicked?.Invoke(_squad);
        }
        private void SaveSquadSettings()
        {
            if (_squad == null) { CompositionRoot.ShowPopUp("ERROR! No Squad to Save!"); return; }

            _squad.Name = View.GetName();
            var hp = View.GetHPSettings();
            _squad.MaxHealth = (uint)hp.y;
            _squad.Health = (uint)hp.x;

            _squad.UpdateSettings(new Attacks(View.GetMelee(), View.GetRange(), View.GetCC()));
            _squad.SpriteName = View.GetSpriteName();
        }
    }

}
