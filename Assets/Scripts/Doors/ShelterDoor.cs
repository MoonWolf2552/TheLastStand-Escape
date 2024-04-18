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
        Destroy(Player.Instance.gameObject);
        
        if (EnemyCounter.Instance)
        {
            EnemyCounter.Instance.DestroyAllEnemies();
            Destroy(EnemyCounter.Instance.gameObject);
        }
        
        GameManager.Instance.LoadLevel(Level + 1);
        GameManager.Instance.EnterButton.SetActive(false);
    }
}
