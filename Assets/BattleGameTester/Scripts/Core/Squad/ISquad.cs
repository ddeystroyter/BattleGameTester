using System;
using UnityEngine;

namespace BattleGameTester.Core 
{

    public interface ISquad : IHealth
    {
        event Action<string> NameChanged;
        event Action<string> AvatarChanged;
        event Action SettingsUpdated;
        event Action<ISquad> Died;
        Vector3 Position { get; set; }
        Quaternion Rotation { get; set; }

        string Name { get; set; }
        Attacks Attacks { get; set; }
        string SpriteName { get; set; }

        Attacks.V6 GetV6(AttackType type);
        ushort GetDmgVal(AttackType type);
        ushort GetDefVal(AttackType type);

        void CreateSquadModel(ESquadModels model);
        void ChangeModelOpacity(int opacity);
        void CopyStats(SquadSave squad);
        void UpdateSettings(Attacks attacks);
        void Die();
        void Delete();

    }
}