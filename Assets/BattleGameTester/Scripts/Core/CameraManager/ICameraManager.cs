using System;
using UnityEngine;

namespace BattleGameTester.Core
{
    public interface ICameraManager
    {
        event Action <Camera> CameraChanged;
        GameObject CameraObject { get; }
        Camera CameraComponent { get; }
        void SetTarget(Vector3 target);
        void SmoothMoveTo(Vector3 coords);
        void ChangeCameraTo(Camera camera);
        void NextPreset();

    }
}
