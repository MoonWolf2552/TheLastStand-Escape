using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterDoor : NextLevelDoor
{
    protected override void Start()
    {
        base.Start();
        
        Level = Progress.Instance.PlayerData.Level;
    }

    public override void Enter()
    {
        GameManager.Instance.LoadLevel(Level + 1);
        GameManager.Instance.EnterButton.SetActive(false);
    }
}
