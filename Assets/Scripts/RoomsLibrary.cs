using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public int LevelNumber;
    public int RoomNumber;
    public int RoomSceneIndex;
}

[CreateAssetMenu]
public class RoomsLibrary : ScriptableObject
{
    public Room[] Rooms;

    public int GetRoomSceneIndex(int levelNum, int roomNum)
    {
        foreach (Room room in Rooms)
        {
            if (room.LevelNumber == levelNum && room.RoomNumber == roomNum)
            {
                return room.RoomSceneIndex;
            }
        }

        return 0;
    }
}
