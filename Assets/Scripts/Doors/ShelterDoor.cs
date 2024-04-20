using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class ShelterDoor : NextLevelDoor
{
    protected override void Start()
    {
        base.Start();
        
        Level = Progress.Instance.PlayerData.Level;
    }

    public override void Enter()
    {
        if (EnemyCounter.Instance)
        {
            EnemyCounter.Instance.DestroyAllEnemies();
            Destroy(EnemyCounter.Instance.gameObject);
        }
        
        GameManager.Instance.Go();
    }

    public override void Go()
    {
        base.Go();

        YandexGame.FullscreenShow();
        
        Destroy(Player.Instance.gameObject);
        
        GameManager.Instance.LoadLevel(Level + 1);
        GameManager.Instance.EnterButton.SetActive(false);
    }
}
