using BattleGameTester.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleGameTester.GameParts
{
    public class WorldSpaceUI_CameraFacing : MonoBehaviour
    {
        private Camera activeCamera;
        private ICameraManager cameraManager;
        private void Awake()
        {
            cameraManager = CompositionRoot.GetCameraManager();
            activeCamera = cameraManager.CameraComponent;
            cameraManager.CameraChanged += OnCameraChanged;
        }

        private void Start()
        {
            transform.rotation = activeCamera.transform.rotation;
        }
        private void OnCameraChanged(Camera camera)
        {
            activeCamera = camera;
            transform.rotation = activeCamera.transform.rotation;
        }

        private void OnDestroy()
        {
            if (cameraManager != null)
            {
                cameraManager.CameraChanged -= OnCameraChanged;
            }

        }
    }
}

