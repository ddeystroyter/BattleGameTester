using UnityEngine;

namespace BattleGameTester.Core
{
    public interface IUIRoot
    {
        Transform MainCanvas { get; }
        Transform OverlayCanvas { get; }
        Transform PopUpCanvas {get; }
        Transform WorldSpaceCanvas { get; }
    }
}
