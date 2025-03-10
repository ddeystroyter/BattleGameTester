using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleGameTester.Core
{
    public interface IResourceManager
    {
        T CreatePrefabInstance<T, E>(E item) where E : Enum;
        T CreatePrefabInstance<T>(GameObject item, Transform parent);
        GameObject CreatePrefabInstance<E>(E item) where E : Enum;
        T GetAsset<T, E>(E item) where T : UnityEngine.Object where E : Enum;

        List<KeyValuePair<string, Sprite>> LoadSquadSprites();
    }
}