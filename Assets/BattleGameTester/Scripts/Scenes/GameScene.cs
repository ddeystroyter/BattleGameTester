using BattleGameTester.Core;
using BattleGameTester.UI;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

namespace BattleGameTester.Scenes
{
    public class GameScene : MonoBehaviour
    {
        private IUserInput UserInput;
        private ICameraManager CameraManager;
        //private IConfiguration Configuration;

        private IBattleSystem BattleSystem;
        private IGridSystem GridSystem;
        private ISaveSystem SaveSystem;

        private IGameHUD GameHUD;
        private ISceneManager SceneManager;
        private ISettingsMenu SettingsMenu;
        private ICellMenu CellMenu;
        private ISquadSettings SquadSettings;

        private GameObject Terrain;

        private void Awake()
        {
            var temp = CompositionRoot.GetSquadSprites();
            UserInput = CompositionRoot.GetUserInput();
            CameraManager = CompositionRoot.GetCameraManager();
            SceneManager = CompositionRoot.GetSceneManager();

            BattleSystem = CompositionRoot.GetBattleSystem();
            GridSystem = CompositionRoot.GetGridSystem();
            SaveSystem = CompositionRoot.GetSaveSystem();

            GameHUD = CompositionRoot.GetGameHUD();
            SettingsMenu = CompositionRoot.GetSettingsMenu();
            CellMenu = CompositionRoot.GetCellMenu();
            SquadSettings = CompositionRoot.GetSquadSettings();

            var eventSystem = CompositionRoot.GetEventSystem();
            var gameSettings = CompositionRoot.GetGameSettings();
            var audioManager = CompositionRoot.GetAudioManager();


            GameHUD.Show();
            SettingsMenu.Hide();

            //GameHUD.Buttons
            GameHUD.Buttons.Restart_Clicked += () => SceneManager.LoadScene(EScenes.GameScene);
            GameHUD.Buttons.NextCamera_Clicked += CameraManager.NextPreset;
            GameHUD.Buttons.Settings_Clicked += OpenSettings;

            //UserInput
            UserInput.Greater_Pressed += () => SceneManager.LoadScene(EScenes.GameScene);
            UserInput.X_Pressed += CameraManager.NextPreset;
            UserInput.P_Pressed += OpenSettings;

            //GridSystem -> CellMenu
            GridSystem.EmptyCellOpened += CellMenu.ShowEmptyMenu;
            GridSystem.SquadCellOpened += CellMenu.ShowSquadMenu;
            GridSystem.CellClosed += CellMenu.CloseActiveMenu;

            //CellMenu -> GridSystem
            //CellMenu.Closed += GridSystem.CloseCell ;
            //CellMenu -> BattleSystem
            CellMenu.AddDefault_Clicked += BattleSystem.AddDefaultSquad;
            CellMenu.Move_Clicked += BattleSystem.MoveSquad;
            CellMenu.Target_Clicked += BattleSystem.ChooseTargetSquad;
            //CellMenu -> SaveSystem
            CellMenu.Import_Clicked += () => {SaveSystem.ShowSavedSquads();};
            //CellMenu -> SquadSettings
            CellMenu.Settings_Clicked += () => {SquadSettings.Show(BattleSystem.GetActiveSquad());};
            CellMenu.Closed += GridSystem.CloseCell;

            //SquadSettings -> SavedSquads
            SquadSettings.Save_Clicked += SaveSystem.SaveSquad;
            
            SquadSettings.Delete_Clicked += BattleSystem.DeleteActiveSquad;

            SquadSettings.Closed += GridSystem.CloseCell;
            //SaveSystem -> BattleSystem
            SaveSystem.Squad_Added += BattleSystem.AddSquad;
            SaveSystem.SavedSquadsView_Closed += () => {GridSystem.CloseCell();};

            var isMusicOn = gameSettings.IsMusicOn;
            var isSoundEffectsOn = gameSettings.IsSoundEffectsOn;
            var isTerrainOn = gameSettings.IsTerrainOn;

            if (isTerrainOn) SpawnTerrain();
            gameSettings.IsTerrainOnChanged += (value) => { if (value) SpawnTerrain(); else DestroyTerrain(); }; 
            //audioManager.SetMusicActive(isMusicOn);
            audioManager.SetEffectsActive(isSoundEffectsOn);

            //audioManager.PlayMusic(EAudio.MainTheme);
            SettingsMenu.Closing += CloseSettings;
        }

        private void SpawnTerrain()
        {
            if (Terrain == null) Terrain = CompositionRoot.GetResourceManager().CreatePrefabInstance(EComponents.Terrain);

        }
        private void DestroyTerrain()
        {
            if (Terrain != null)
            {
                Destroy(Terrain);
                Terrain = null;
            }
        }
        private void OpenSettings()
        {
            GameHUD.Hide();
            SettingsMenu.Show();
        }

        private void CloseSettings()
        {
            SettingsMenu.Hide();
            GameHUD.Show();
        }
        private void ExitGame()
        {
            SceneManager.LoadScene(EScenes.FirstScene);
        }

    }
}
