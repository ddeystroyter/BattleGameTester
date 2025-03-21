using UnityEngine;

namespace BattleGameTester.UI
{
    public interface IView
    {
        void Show();
        void Hide();
        void SetParent(Transform parent);
    }
}
