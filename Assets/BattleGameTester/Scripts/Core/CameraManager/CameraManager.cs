using BattleGameTester.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleGameTester.Core
{
    public class CameraManager : MonoBehaviour, ICameraManager
    {
        public event Action<Camera> CameraChanged;
        public GameObject CameraObject { get; private set; }
        public Camera CameraComponent { get => _cameraComponent; set => _cameraComponent = value; }
        [SerializeField] private Camera _cameraComponent;

        //private const float MinFOV = 45f;
        //private const float MaxFOV = 120f;
        //private const float MaxSpeed = 10f;

        //private const float CameraLerpFOV = 0.02f;
        //private const float CameraLerpSpeed = 0.2f;
        //private readonly Vector3 LookAtOffset = Vector3.up * 1f;


        private Dictionary<ushort, Tuple<Vector3, Quaternion>> Presets = new Dictionary<ushort, Tuple<Vector3, Quaternion>>();
        private ushort _currentPreset = 0;
        //private Vector3 Target;
        //private Vector3 PreviousTargetPosition;

        private void Awake()
        {
            if (_cameraComponent == null)
            {
                CameraComponent = GetComponentInChildren<Camera>();
            }

            var pos = new Vector3(0.31f, 5.47f, -6.93f);
            var rot = Quaternion.Euler(new Vector3(40f, 0f, 0f));

            Presets.Add(0, new Tuple<Vector3, Quaternion>(pos, rot));
            pos = new Vector3(-0.21f, 8.33f, -0.3f);
            rot = Quaternion.Euler(new Vector3(90f, 0f, 0f));
            Presets.Add(1, new Tuple<Vector3, Quaternion>(pos, rot));
            pos = new Vector3(-8.814f, 4.45f, 0f);
            rot = Quaternion.Euler(new Vector3(36.71f, 90f, 0f));
            Presets.Add(2, new Tuple<Vector3, Quaternion>(pos, rot));
            pos = new Vector3(8.814f, 4.45f, 0f);
            rot = Quaternion.Euler(new Vector3(36.71f, -90f, 0f));
            Presets.Add(3, new Tuple<Vector3, Quaternion>(pos, rot));
            pos = new Vector3(0f, 5.47f, -9.67f);
            rot = Quaternion.Euler(new Vector3(12f, 0f, 0f));
            Presets.Add(4, new Tuple<Vector3, Quaternion>(pos, rot));
            CameraChanged?.Invoke(CameraComponent);
        }
        private void OnEnable()
        {
            CameraChanged?.Invoke(CameraComponent);
        }
        private void Start()
        {
            SetPreset(0);
            CameraChanged?.Invoke(CameraComponent);
        }

        public void NextPreset()
        {
            _currentPreset++;
            if (_currentPreset >= Presets.Count)
            {
                _currentPreset = 0;
            }
            SetPreset(_currentPreset);
            
            CameraChanged?.Invoke(CameraComponent);
        }
        public void SetPreset(ushort id)
        {
            CameraComponent.transform.position = Presets[id].Item1;
            CameraComponent.transform.rotation = Presets[id].Item2;
        }
        public void SetTarget(Vector3 target)
        {
            throw new System.NotImplementedException();
        }

        public void SmoothMoveTo(Vector3 coords)
        {
            throw new System.NotImplementedException();
        }

        public void ChangeCameraTo(Camera camera)
        {
            CameraComponent = camera;
            CameraObject = camera.gameObject;
            CameraChanged?.Invoke(camera);
        }
    }
}