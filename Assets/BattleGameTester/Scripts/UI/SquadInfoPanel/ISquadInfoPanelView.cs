using UnityEngine;
using BattleGameTester.Core;

namespace BattleGameTester.UI
{
    public interface ISquadInfoPanelView : IView
    {
        void Init(ISquad squad);
        void UpdateInfo(ISquad squad);
        void SetPosition(Vector3 pos);
        void UpdateAvatar(string spriteName);
        void UpdateName(string name);
        void UpdateHP(uint val);
        void UpdateHP(Vector2Int hp);
        void UpdateAttacks(Attacks attacks);
        void Destroy();

    }
}

