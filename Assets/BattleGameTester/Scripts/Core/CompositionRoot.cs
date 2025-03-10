using UnityEngine;
using UnityEngine.EventSystems;
using BattleGameTester.UI;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;

namespace BattleGameTester.Core
{
    public class CompositionRoot : MonoBehaviour
    {
        private static IUIRoot UIRoot;
        private static IUserInput UserInput;
        private static ICameraManager CameraManager;
        private static EventSystem EventSystem;
        private static IViewFactory ViewFactory;
        private static IGameSettings GameSettings;
        private static ISceneManager SceneManager;
        private static IAudioManager AudioManager;
        private static IResourceManager ResourceManager;

        private static IBattleSystem BattleSystem;
        private static IGridSystem GridSystem;
        private static ISaveSystem SaveSystem;

        private static IGameHUD GameHUD;
        private static IMainMenu MainMenu;
        private static ISettingsMenu SettingsMenu;
        private static ICellMenu CellMenu;
        private static ISquadSettings SquadSettings;

        private static List<Sprite> DiceFacesSprites;
        private static List<Sprite> AttackTypesSprites;
        private static List<Sprite> LogBoxIconsSprites;
        private static List<KeyValuePair<string, Sprite>> SquadSprites;

        private void OnDestroy()
        {
            UIRoot = null;
            UserInput = null;
            CameraManager = null;
            EventSystem = null;
            ViewFactory = null;
            AudioManager = null;

            BattleSystem = null;
            GridSystem = null;
            SaveSystem = null;

            GameHUD = null;
            MainMenu = null;
            SettingsMenu = null;
            CellMenu = null;
            SquadSettings = null;

            DiceFacesSprites = null;
            AttackTypesSprites = null;
            LogBoxIconsSprites = null;
            SquadSprites = null;
        }

        public static IBattleSystem GetBattleSystem()
        {
            if (BattleSystem == null)
            {
                BattleSystem = MonoExtensions.CreateComponent<BattleSystem>();
            }

            return BattleSystem;
        }

        public static IGridSystem GetGridSystem()
        {
            if (GridSystem == null)
            {
                var resourceManager = GetResourceManager();

                GridSystem = resourceManager.CreatePrefabInstance<IGridSystem, EComponents>(EComponents.GridSystem);
            }

            return GridSystem;
        }

        public static IResourceManager GetResourceManager()
        {
            if (ResourceManager == null)
            {
                ResourceManager = new ResourceManager();
            }

            return ResourceManager;
        }


        public static ISceneManager GetSceneManager()
        {
            if (SceneManager == null)
            {
                SceneManager = new SceneManager();
            }

            return SceneManager;
        }

        public static EventSystem GetEventSystem()
        {
            if (EventSystem == null)
            {
                var resourceManager = GetResourceManager();
                EventSystem = resourceManager.CreatePrefabInstance<EventSystem, EComponents>(EComponents.EventSystem);
            }

            return EventSystem;
        }

        public static IUserInput GetUserInput()
        {
            if (UserInput == null)
            {
                UserInput = MonoExtensions.CreateComponent<UserInput>();
            }

            return UserInput;
        }

        public static ICameraManager GetCameraManager()
        {
            if (CameraManager == null)
            {
                var resourceManager = GetResourceManager();

                CameraManager = resourceManager.CreatePrefabInstance<ICameraManager, EComponents>(EComponents.CameraManager);
            }

            return CameraManager;
        }

        public static IViewFactory GetViewFactory()
        {
            if (ViewFactory == null)
            {
                var uiRoot = GetUIRoot();
                var sceneManager = GetSceneManager();
                var resourceManager = GetResourceManager();

                ViewFactory = new ViewFactory(uiRoot, resourceManager, sceneManager);
            }

            return ViewFactory;
        }

        public static IGameSettings GetGameSettings()
        {
            if (GameSettings == null)
            {
                GameSettings = new GameSettings();
            }

            return GameSettings;
        }

        public static IAudioManager GetAudioManager()
        {
            if (AudioManager == null)
            {
                AudioManager = MonoExtensions.CreateComponent<AudioManager>();
            }

            return AudioManager;
        }

        public static IUIRoot GetUIRoot()
        {
            if (UIRoot == null)
            {
                var resourceManager = GetResourceManager();

                UIRoot = resourceManager.CreatePrefabInstance<IUIRoot, EComponents>(EComponents.UIRoot);
            }

            return UIRoot;
        }

        public static IMainMenu GetMainMenu()
        {
            if (MainMenu == null)
            {
                MainMenu = MonoExtensions.CreateComponent<MainMenu>();
            }

            return MainMenu;
        }

        public static ISettingsMenu GetSettingsMenu()
        {
            if (SettingsMenu == null)
            {
                SettingsMenu = MonoExtensions.CreateComponent<SettingsMenu>();
            }

            return SettingsMenu;
        }

        public static IGameHUD GetGameHUD()
        {
            if (GameHUD == null)
            {
                GameHUD = MonoExtensions.CreateComponent<GameHUD>();
            }

            return GameHUD;
        }

        public static ICellMenu GetCellMenu()
        {
            if (CellMenu == null)
            {
                CellMenu = MonoExtensions.CreateComponent<CellMenu>();
            }
            return CellMenu;
        }

        public static ISquadSettings GetSquadSettings()
        {
            if (SquadSettings == null)
            {
                SquadSettings = MonoExtensions.CreateComponent<SquadSettings>();
            }
            return SquadSettings;
        }
        public static ISaveSystem GetSaveSystem()
        {
            if (SaveSystem == null)
            {
                SaveSystem = MonoExtensions.CreateComponent<SaveSystem>();
            }
            return SaveSystem;
        }

        public static Sprite GetDiceFaceSprite(int val)
        {
            if (DiceFacesSprites == null)
            {
                DiceFacesSprites = Resources.LoadAll<Sprite>("UI_Icons/DiceFaces").ToList<Sprite>();
            }
            return DiceFacesSprites[val - 1];
        }

        public static Sprite GetAttackTypeSprite(AttackType type)
        {
            if (AttackTypesSprites == null)
            {
                AttackTypesSprites = Resources.LoadAll<Sprite>("UI_Icons/AttackTypes").ToList<Sprite>();
            }
            return AttackTypesSprites.Find(sprite => sprite.name == type.ToString());
        }

        public static void RefreshSquadSprites()
        {
            SquadSprites = ResourceManager.LoadSquadSprites();
            ShowPopUp("OK! SquadSprites Resfreshed :)");
        }

        public static List<KeyValuePair<string, Sprite>> GetSquadSprites()
        {
            if (SquadSprites == null)
            {
                SquadSprites = ResourceManager.LoadSquadSprites();
            }
            return SquadSprites;
        }

        public static Sprite GetSquadSprite(string spriteName)
        {
            if (SquadSprites == null)
            {
                SquadSprites = ResourceManager.LoadSquadSprites();
            }
            var sprite = SquadSprites.Find(x => x.Key == spriteName).Value;
            return sprite;
        }

        public static Sprite GetLogBoxSprite(LogBoxIcons icon)
        {
            if (LogBoxIconsSprites == null)
            {
                LogBoxIconsSprites = Resources.LoadAll<Sprite>("UI_Icons/LogBoxIcons").ToList<Sprite>();
            }
            return LogBoxIconsSprites.Find(sprite => sprite.name == icon.ToString());
        }

        public static int GetSquadSpriteIndex(string spriteName)
        {
            var index = GetSquadSprites().FindIndex(x => x.Key == spriteName);

            if (index < 0)
            {
                Debug.Log(GetSquadSprites().Count);
                Debug.Log("spriteName: " + spriteName);
                ShowPopUp($"ERROR! SquadImage doesn't exist! Check FILENAME \"{spriteName}\"!");
                index = 0;
            }
            return index;
        }

        public static void ShowPopUp(string message)
        {
            var popup = GetResourceManager().CreatePrefabInstance<IPopUp_Item, EUI_Items>(EUI_Items.PopUp_Item);
            popup.SetParent(GetUIRoot().PopUpCanvas);
            popup.Show(message);
        }
    }
}