using System;
using System.Collections.Generic;

namespace BattleGameTester.Core
{
    public interface ISaveSystem
    {
        event Action<SquadSave> Squad_Added;
        event Action SavedSquadsView_Closed;

        void ShowSavedSquads();
        void SaveSquad(ISquad squad);
    }
}

