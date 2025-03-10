namespace BattleGameTester.UI
{
    public interface IViewFactory
    {
        IGameHUDView CreateGameHUD();
        IMainMenuView CreateMainMenu();
        ISettingsMenuView CreateSettingsMenu();
        IHelpMenuView CreateHelpMenu();
        ICellMenuView CreateCellMenu();
        ISquadInfoPanelView CreateSquadInfoPanel();
        ISquadSettingsView CreateSquadSettings();
        ISavedSquadsView CreateSavedSquads();
        IAttackTypeChoiceView CreateAttackTypeChoice();
    }
}
