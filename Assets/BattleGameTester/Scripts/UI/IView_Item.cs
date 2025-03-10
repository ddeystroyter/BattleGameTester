using UnityEngine;

namespace BattleGameTester.UI
{
    public interface IView_Item
    {
        void Show();
        void Hide();
        void SetParent(Transform parent);
        void Destroy();

    }
}

