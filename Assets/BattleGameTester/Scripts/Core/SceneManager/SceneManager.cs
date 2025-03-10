using BattleGameTester.Core;
using System;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace BattleGameTester.Core
{
    public class SceneManager : ISceneManager
    {
        public EScenes CurrentScene { get; private set; }

        public SceneManager()
        {
            var unityScene = UnitySceneManager.GetActiveScene();

            EScenes scene;
            if (Enum.TryParse<EScenes>(unityScene.name, out scene))
            {
                CurrentScene = scene;
            }
            else
            {
                throw new ArgumentException("Unknown scene: " + unityScene.name);
            }
        }

        public void LoadScene(EScenes scene)
        {
            UnitySceneManager.LoadScene(scene.ToString());

            CurrentScene = scene;
        }
    }
}