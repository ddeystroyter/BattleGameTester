using System;
using UnityEngine;

namespace BattleGameTester.UI
{
    public abstract class BaseView : MonoBehaviour, IView
    {
        public static event Action Showed;
        public static event Action Hided;

        public void Show()
        {
            gameObject.SetActive(true);
            Showed?.Invoke();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            Hided?.Invoke();
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
        }
    }
}
