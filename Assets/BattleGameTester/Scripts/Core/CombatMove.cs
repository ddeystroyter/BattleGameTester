namespace BattleGameTester.Core
{
    public class CombatMove
    {
        public byte Priority;
        public ISquad AttackingSquad;
        public ISquad DefendingSquad;
        public AttackType AttackType;
        public CombatMove(ISquad attack, ISquad def, AttackType type)
        {
            this.AttackingSquad = attack;
            this.DefendingSquad = def;
            this.AttackType = type;
        }
    }
}