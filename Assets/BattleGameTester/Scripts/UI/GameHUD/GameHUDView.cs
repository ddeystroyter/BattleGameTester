using System;
using UnityEngine;
using UnityEngine.UI;

namespace BattleGameTester.UI
{
    public class GameHUDView : BaseView, IGameHUDView
    {
        public CombatQueueView CombatQueueView;
        public GameHUD_Buttons Buttons;
        public GameHUD_RoundIndicator RoundIndicator;
        public EventLogBox EventLogBox;
        public SquadOverviewPanel SquadOverviewPanel;

        new public void Show()
        {
            gameObject.SetActive(true);
        }
        new public void Hide()
        {
            gameObject.SetActive(false);
        }

        public ISquadOverviewPanel GetSquadOverviewPanel()
        {
            return SquadOverviewPanel;
        }
        public IGameHUD_RoundIndicator GetRoundIndicator()
        {
            return RoundIndicator;
        }
        public IGameHUD_Buttons GetButtons()
        {
            return Buttons;
        }
        public ICombatQueueView GetCombatQueueView()
        {
            return CombatQueueView;
        }
        public IEventLogBox GetEventLogBox()
        {
            return EventLogBox;
        }

    }
}