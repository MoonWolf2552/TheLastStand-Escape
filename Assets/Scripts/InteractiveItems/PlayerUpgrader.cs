using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrader : InteractiveObject
{
    [SerializeField] private GameObject _playerUpgradeShop;
    [SerializeField] private GameObject _shopVisual;

    public override void Interact()
    {
        GameManager.Instance.InteractButton.SetActive(false);
        Player.Instance.IsRead = true;
        
        _shopVisual.gameObject.SetActive(true);
        _playerUpgradeShop.gameObject.SetActive(true);
        Shop.Instance.OpenPlayerShop();
    }

    public override void Close()
    {
        base.Close();
        Player.Instance.IsRead = false;
        _shopVisual.gameObject.SetActive(false);
        _playerUpgradeShop.gameObject.SetActive(false);
    }
}
