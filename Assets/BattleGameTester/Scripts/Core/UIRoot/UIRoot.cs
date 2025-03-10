using UnityEngine;

namespace BattleGameTester.Core
{
    public class UIRoot : MonoBehaviour, IUIRoot
    {
        public Transform MainCanvasContent;
        public Transform OverlayCanvasContent;
        public Transform PopUpCanvasContent;
        public Transform WorldSpaceCanvasContent;

        public Transform MainCanvas
        {
            get
            {
                return MainCanvasContent;
            }
        }

        public Transform OverlayCanvas
        {
            get
            {
                return OverlayCanvasContent;
            }
        }
        public Transform PopUpCanvas
        {
            get
            {
                return PopUpCanvasContent;
            }
        }

        public Transform WorldSpaceCanvas
        {
            get
            {
                return WorldSpaceCanvasContent;
            }
        }
    }
}