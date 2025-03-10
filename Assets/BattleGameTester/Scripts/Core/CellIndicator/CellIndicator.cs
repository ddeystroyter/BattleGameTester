using BattleGameTester.Core;
using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace BattleGameTester.Core
{
    public enum CellIndicatorState
    {
        Frame,
        Move,
        Target
    }
    public class CellIndicator : MonoBehaviour, ICellIndicator
    {

        [SerializeField] private Color available;
        [SerializeField] private Color unavailable;
        private Color moveBlue;
        private Color targetRed;
        [SerializeField] private Animator animator;

        [SerializeField] private Sprite Frame;
        [SerializeField] private Sprite Move;
        [SerializeField] private Sprite Target;

        private CellIndicatorState _state = CellIndicatorState.Frame;
        public CellIndicatorState State
        {
            get => _state;
            set { _state = value; OnStateChanged(value); }

        }

        private SpriteRenderer spriteRenderer;
        
        private Quaternion defaultRotation = Quaternion.Euler(90, 0, 0);
        

        private Vector3 cursorOffset = new Vector3(0.50f, 0.015f, 0.50f);

        private int isPickedHash = Animator.StringToHash("isPicked");

        private void Awake()
        {
            ColorUtility.TryParseHtmlString("#1A88BE", out moveBlue);
            ColorUtility.TryParseHtmlString("#C41E3A", out targetRed);

            if (animator == null)
            {
                if (!TryGetComponent(out animator))
                {
                    throw new Exception("No CellIndicator <Animator>.");

                }
            }
            if (spriteRenderer == null)
            {
                if (!TryGetComponent(out spriteRenderer))
                {
                    throw new Exception("No CellIndicator <SpriteRenderer>.");

                }
            }

            if (available == null || unavailable == null)
            {
                available = Color.white;
                unavailable = Color.grey;
            }
            OnStateChanged(_state);

        }

        private void OnDisable()
        {
            this.transform.rotation = defaultRotation;
        }

        private void OnStateChanged(CellIndicatorState state)
        {
            switch (state)
            {
                case CellIndicatorState.Frame:
                    available = Color.white;
                    spriteRenderer.sprite = Frame;
                    break;
                case CellIndicatorState.Move:
                    available = moveBlue;
                    spriteRenderer.sprite = Move;
                    break;
                case CellIndicatorState.Target:
                    available = targetRed;
                    spriteRenderer.sprite = Target;
                    break;
            }
            MakeAvailable();
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }
        public void Hide()
        {
            this.gameObject.SetActive(false);

        }
        public void MoveTo(Vector3 cellCoords)
        {
            this.transform.position = cellCoords + cursorOffset;
        }
        public void MakeAvailable()
        {
            spriteRenderer.color = available;
        }
        public void MakeUnavailable()
        {
            spriteRenderer.color = unavailable;
        }
        public void OnCellOpened()
        {
            animator.SetBool(isPickedHash, true);
        }
        public void OnCellClosed()
        {
            animator.SetBool(isPickedHash, false);
        }


    }
}

