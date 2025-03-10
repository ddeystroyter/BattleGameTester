using System;

namespace BattleGameTester.Core
{
    public interface IHealth
    {
        event Action<uint> HealthChanged;
        event Action<uint> MaxHealthChanged;

        uint Health { get; set; }
        uint MaxHealth { get; set;}

        void DecreaseHealth(uint value);
        void IncreaseHealth(uint value);
        void RestoreHealth();
    }
}
