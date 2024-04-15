using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterDoor : NextLevelDoor
{
    void Start()
    {
        Level = Progress.Instance.PlayerData.Level;
    }
    
}
