using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;

namespace BattleGameTester.UI
{
    public class PopUp_Item : BaseView_Item, IPopUp_Item
    {
        private float lifeTime = 2f;
        [SerializeField] private TMP_Text _message;
        [SerializeField] private Animator _anim;

        private void Awake()
        {
            gameObject.SetActive(false);
        }


        public void Show(string text)
        {
            Show(text, lifeTime);

        }

        public void Show(string text, float sec)
        {
            gameObject.SetActive(true);
            StartCoroutine(Hide(text, sec));
        }

        private IEnumerator Hide(string text, float time)
        {
            _message.text = text;
            yield return new WaitForSeconds(time);
            _anim.SetTrigger("Disable");
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
            Destroy();
        }
    }

}
