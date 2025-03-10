namespace BattleGameTester.UI
{
        public interface IGameHUDView : IView
        {
            ICombatQueueView GetCombatQueueView();
            IGameHUD_Buttons GetButtons();
            IEventLogBox GetEventLogBox();
            ISquadOverviewPanel GetSquadOverviewPanel();
            IGameHUD_RoundIndicator GetRoundIndicator();
        }
}
