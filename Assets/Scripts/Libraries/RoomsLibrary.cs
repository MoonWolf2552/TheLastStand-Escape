using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Room
{
    public int RoomNumber;
    public int RoomSceneIndex;
}

[System.Serializable]
public class Level
{
    public int LevelNumber;
    public List<Room> Rooms;
}


[CreateAssetMenu]
public class RoomsLibrary : ScriptableObject
{
    public Level[] LevelDatas;
    
    private Dictionary<int, Dictionary<int, int>> _roomDictionary = new Dictionary<int, Dictionary<int, int>>();

    private void OnEnable()
    {
        foreach (Level level in LevelDatas)
        {
            Dictionary<int, int> rooms = new Dictionary<int, int>{};
            foreach (Room room in level.Rooms)
            {
                rooms.Add(room.RoomNumber, room.RoomSceneIndex);
            }
            _roomDictionary.Add(level.LevelNumber, rooms);
        }
    }

    public int GetRoomSceneIndex(int levelNum, int roomNum)
    {
        return _roomDictionary[levelNum][roomNum];
    }
    
    public int GetSceneRoomNumber(int levelNum, int sceneIndex)
    {
        Dictionary<int, int> reverseDictionary = _roomDictionary[levelNum].ToDictionary(x => x.Value, x => x.Key);
        return reverseDictionary[sceneIndex];
    }
}
