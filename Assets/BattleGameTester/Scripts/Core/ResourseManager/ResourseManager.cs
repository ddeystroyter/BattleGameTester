using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BattleGameTester.Core
{
    public class ResourceManager : IResourceManager
    {
        public T CreatePrefabInstance<T, E>(E item) where E : Enum
        {
            var prefab = CreatePrefabInstance(item);
            var result = prefab.GetComponent<T>();

            return result;
        }

        public T CreatePrefabInstance<T>(GameObject item, Transform parent)
        {
            var prefab = GameObject.Instantiate(item);
            prefab.transform.parent = parent;
            var result = prefab.GetComponent<T>();

            return result;
        }
        public GameObject CreatePrefabInstance<E>(E item) where E : Enum
        {
            var path = string.Format("{0}/{1}", typeof(E).Name, item.ToString());
            var asset = Resources.Load<GameObject>(path);
            var result = GameObject.Instantiate(asset);

            return result;
        }

        public T GetAsset<T, E>(E item)
            where T : UnityEngine.Object
            where E : Enum
        {
            var path = string.Format("{0}/{1}", typeof(E).Name, item.ToString());
            var result = Resources.Load<T>(path);

            return result;
        }

        public List<KeyValuePair<string, Sprite>> LoadSquadSprites()
        {
            var AllSprites = new List<KeyValuePair<string, Sprite>>();
            string artPath = UnityEngine.Application.dataPath + @"\SquadImages";
            if (!Directory.Exists(artPath)) Directory.CreateDirectory(artPath);
            if (!File.Exists(artPath + @"\default.jpg"))
            {
                File.Create(artPath + @"\default.jpg");
                CompositionRoot.ShowPopUp("WARNING! New default.jpg file created!");
            }

            var allowedExtensions = new[] { ".png", ".jpg" };

            var allFilePaths = Directory
                .GetFiles(artPath, "." , SearchOption.AllDirectories)
                .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                .ToList();

            //Loop through allFilePaths 
            foreach (string filePath in allFilePaths)
            {
                //Ready the PNG file from the harddrive
                byte[] newFileData;
                newFileData = File.ReadAllBytes(filePath); //Read the PNG file's bytes. This loads the PNG file into memory.

                //Create a Unity TEXTURE from PNG/Jpg file
                Texture2D newTexture2D = new Texture2D(2, 2); //Create a new Texture. Size doesn't matter!
                newTexture2D.LoadImage(newFileData); //Load the PNG file into a Texture2D.       PngData ---> Texture2D

                //Create a Unity SPRITE from Texture
                Sprite newSprite = Sprite.Create(newTexture2D, new Rect(0, 0, newTexture2D.width, newTexture2D.height), new Vector2(0, 0), 1);

                string spriteName = @filePath.Replace(artPath, "").Substring(1);//Path.GetFileNameWithoutExtension(filePath);
                AllSprites.Add(new KeyValuePair<string, Sprite> (spriteName, newSprite));
            }

            Debug.Log("Loading Sprites Finished! Total Sprites Loaded: " + AllSprites.Count);
            return AllSprites.OrderBy(x => x.Key).ToList();
        }
    }
}
