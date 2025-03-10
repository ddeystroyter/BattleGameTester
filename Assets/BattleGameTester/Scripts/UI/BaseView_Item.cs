using UnityEngine;

namespace BattleGameTester.UI
{
    public abstract class BaseView_Item : MonoBehaviour, IView_Item
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, false);
        }

        public void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}

