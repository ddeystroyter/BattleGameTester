using BattleGameTester.Core;

namespace BattleGameTester.UI
{
    public interface IGameHUD_RoundIndicator
    {
        void UpdateRound(EPlayer player, AttackType attackType);
    }
}