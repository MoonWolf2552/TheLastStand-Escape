using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class ShelterDoor : NextLevelDoor
{
    [SerializeField] private TMP_Text _nextLevelText;
    
    private String _prefix;
    private String _prefix2;
    
    [SerializeField] private LevelLibrary _levelLibrary;
    
    protected override void Start()
    {
        base.Start();
        
        Level = Progress.Instance.PlayerData.Level;
        
        if (YandexGame.lang == "ru")
        {
            _prefix = "Этаж ";
            _prefix2 = "Титры";
        }
        else if (YandexGame.lang == "en")
        {
            _prefix = "Floor ";
            _prefix2 = "Titres";
        }
        else
        {
            _prefix = "Floor ";
            _prefix2 = "Titres";
        }
        
        if ((_levelLibrary.LevelDatas.Length - (Level + 1)) == 0)
        {
            _nextLevelText.text = _prefix2;
        }
        else
        {
            _nextLevelText.text = _prefix + (_levelLibrary.LevelDatas.Length - (Level + 1));
        }
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
