using BattleGameTester.Core;
using BattleGameTester.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleGameTester.Scenes
{
    public class FirstScene : MonoBehaviour
    {
        private IMainMenu MainMenu;

        private void Awake()
        {
            MainMenu = CompositionRoot.GetMainMenu();
            var eventSystem = CompositionRoot.GetEventSystem();
            var audioManager = CompositionRoot.GetAudioManager();
            var gameSettings = CompositionRoot.GetGameSettings();

            var isMusicOn = gameSettings.IsMusicOn;
            var isSoundEffectsOn = gameSettings.IsSoundEffectsOn;

            audioManager.SetMusicActive(isMusicOn);
            audioManager.SetEffectsActive(isSoundEffectsOn);

            //MainMenu.
        }

        private void Start()
        {
            MainMenu.Show();
        }
    }
}

