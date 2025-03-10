namespace BattleGameTester.UI
{
    public interface IGameHUD : IScreen
    {
        ICombatQueueView CombatQueue { get; }
        IGameHUD_Buttons Buttons { get; }
        IEventLogBox EventLogBox { get; }
        ISquadOverviewPanel SquadOverviewPanel { get; }
        IGameHUD_RoundIndicator RoundIndicator { get; }
    }
}
