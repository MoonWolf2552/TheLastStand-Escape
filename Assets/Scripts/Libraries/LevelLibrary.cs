using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int LevelNumber;
    public int SceneIndex;
}

[CreateAssetMenu]
public class LevelLibrary : ScriptableObject
{
    public LevelData[] LevelDatas;

    private Dictionary<int, int> _levelDictionary = new Dictionary<int, int>();

    private void OnEnable()
    {
        foreach (LevelData levelData in LevelDatas)
        {
            _levelDictionary.Add(levelData.LevelNumber, levelData.SceneIndex);
        }
    }

    public int GetLevelSceneIndex(int levelNum)
    {
        return _levelDictionary[levelNum];
    }
}
