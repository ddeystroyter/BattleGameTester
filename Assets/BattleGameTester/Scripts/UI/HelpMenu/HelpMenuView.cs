using System;
using UnityEngine;
using UnityEngine.UI;

namespace BattleGameTester.UI
{
    public class HelpMenuView : BaseView, IHelpMenuView
    {
       public event Action BackClicked = () => { };

        [SerializeField] private Button BackButton;
        private void Awake()
        {
            Hide();
            BackButton.onClick.AddListener(OnBackButtonClicked);
        }
        private void OnBackButtonClicked()
        {
            BackClicked();
        }
    }

}
