using BattleGameTester.Core;
using UnityEngine;

namespace BattleGameTester.UI
{
    public class GameHUD : MonoBehaviour, IGameHUD
    {
        private IGameHUDView View;
        public ICombatQueueView CombatQueue { get => View.GetCombatQueueView(); }

        public IGameHUD_RoundIndicator RoundIndicator { get => View.GetRoundIndicator(); }
        public IGameHUD_Buttons Buttons { get => View.GetButtons();}
        public IEventLogBox EventLogBox { get => View.GetEventLogBox(); }
        public ISquadOverviewPanel SquadOverviewPanel { get => View.GetSquadOverviewPanel(); }
        private void Awake()
        {
            var viewFactory = CompositionRoot.GetViewFactory();
            View = viewFactory.CreateGameHUD();
        }

        public void Show()
        {
            View.Show();
        }

        public void Hide()
        {
            View.Hide();
        }
    }
}