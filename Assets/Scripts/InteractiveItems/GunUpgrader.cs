using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUpgrader : InteractiveObject
{
    [SerializeField] private GameObject _gunUpgradeShop;
    [SerializeField] private GameObject _shopVisual;

    public override void Interact()
    {
        GameManager.Instance.InteractButton.SetActive(false);
        Player.Instance.IsRead = true;
        
        _shopVisual.gameObject.SetActive(true);
        _gunUpgradeShop.gameObject.SetActive(true);
        Shop.Instance.OpenGunShop();
    }

    public override void Close()
    {
        base.Close();
        Player.Instance.IsRead = false;
        _shopVisual.gameObject.SetActive(false);
        _gunUpgradeShop.gameObject.SetActive(false);
    }
}
