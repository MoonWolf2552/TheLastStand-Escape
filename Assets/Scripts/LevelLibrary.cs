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
    
    public int GetLevelSceneIndex(int levelNum)
    {
        foreach (LevelData levelData in LevelDatas)
        {
            if (levelData.LevelNumber == levelNum)
            {
                return levelData.SceneIndex;
            }
        }

        return 0;
    }
    
}
