using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class Map : InteractiveObject
{
    [SerializeField] private GameObject _mapObject;
    [SerializeField] private TMP_Text _levelText;

    protected override void Start()
    {
        base.Start();
        
        string prefix = "Уровень ";
        if (YandexGame.lang == "en")
        {
            prefix = "Level ";
        }

        _levelText.text = prefix + (Progress.Instance.PlayerData.Level + 1);
    }

    public override void Interact()
    {
        GameManager.Instance.InteractButton.SetActive(false);
        Player.Instance.IsRead = true;
        
        _mapObject.gameObject.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        
        Player.Instance.IsRead = false;
        _mapObject.gameObject.SetActive(false);
    }
}
