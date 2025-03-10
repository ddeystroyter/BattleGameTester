using BattleGameTester.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace BattleGameTester.UI
{
    public class GameHUD_RoundIndicator : MonoBehaviour, IGameHUD_RoundIndicator
    {
        [Header("Cursors")]
        [SerializeField] private Transform P1_Indicator;
        [SerializeField] private Transform P2_Indicator;
        [Header("P1")]
        [SerializeField] private Transform P1_Melee;
        [SerializeField] private Transform P1_Range;
        [SerializeField] private Transform P1_CC;
        [SerializeField] private TMP_Text P1_RoundCounter;
        [Header("P2")]
        [SerializeField] private Transform P2_Melee;
        [SerializeField] private Transform P2_Range;
        [SerializeField] private Transform P2_CC;
        [SerializeField] private TMP_Text P2_RoundCounter;

        private ushort _p1_rc = 0;
        private ushort _p2_rc = 0;
        private void Awake()
        {
            P1_RoundCounter.text = "0";
            P2_RoundCounter.text = "0";
        }
        private void Start()
        {
            StartCoroutine(IndicatorPositioning());

        }
        private IEnumerator IndicatorPositioning()
        {
            yield return new WaitForSeconds(1f);
            P1_Indicator.position = P1_Range.position;
            P2_Indicator.position = P2_Range.position;
            yield break;
        }
        public void UpdateRound(EPlayer player, AttackType attackType)
        {
            switch (player, attackType)
            {
                case (EPlayer.P1, AttackType.Range):
                    P1_Indicator.position = P1_Range.position;
                    P1_RoundCounter.text = _p1_rc.ToString();
                    _p1_rc++;
                    break;
                case (EPlayer.P1, AttackType.Melee):
                    P1_Indicator.position = P1_Melee.position;
                    break;

                case (EPlayer.P1, AttackType.CC):
                    P1_Indicator.position = P1_CC.position;
                    break;
                case (EPlayer.P2, AttackType.Melee):
                    P2_Indicator.position = P2_Melee.position;
                    P2_RoundCounter.text = _p2_rc.ToString();
                    _p2_rc++;
                    break;
                case (EPlayer.P2, AttackType.Range):
                    P2_Indicator.position = P2_Range.position;
                    break;
                case (EPlayer.P2, AttackType.CC):
                    P2_Indicator.position = P2_CC.position;
                    break;
            }
        }

    }
}