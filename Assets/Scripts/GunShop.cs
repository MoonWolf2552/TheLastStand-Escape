using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

[System.Serializable]
public class LevelPrice
{
    public int Level;
    public int Price;
}

[System.Serializable]
public class GunLevelsPrices
{
    public GunType Type;
    public LevelPrice[] LevelsPrices;
}

public class GunShop : MonoBehaviour
{
    public GunLevelsPrices[] GunLevelsPrices;
    
    public Dictionary<GunType, Dictionary<int, int>> GunPrices = new Dictionary<GunType, Dictionary<int, int>>();

    public Button BuyButton;
    public Button UpgradeButton;
    public Button EquipButton;
    public TMP_Text GunLevelText;
    public TMP_Text GunLevelPriceText;

    public GunButtonTypes[] Guns;

    private int _gun = -1;

    private void Awake()
    {
        foreach (GunLevelsPrices gunLevelPrice in GunLevelsPrices)
        {
            var levelPrice = new Dictionary<int, int>();
            foreach (LevelPrice price in gunLevelPrice.LevelsPrices)
            {
                levelPrice.Add(price.Level, price.Price);
            }
            GunPrices.Add(gunLevelPrice.Type, levelPrice);
        }
    }

    public void NextGun()
    {
        _gun = (_gun + 1) % Guns.Length;
        Guns[_gun].gameObject.SetActive(true);
        
        GunType name = Guns[_gun].Type;
        foreach (GunData gunData in Progress.Instance.PlayerData.GunDatas)
        {
            if (gunData.Name == name)
            {
                if (!gunData.IsOpened)
                {
                    BuyButton.gameObject.SetActive(true);
                    
                    GunLevelPriceText.gameObject.SetActive(true);
                    
                    Debug.Log(gunData.GunLevel);
                    
                    int price = GunPrices[name][gunData.GunLevel];

                    GunLevelPriceText.text = price.ToString();
                }
                else
                {
                    if (Progress.Instance.PlayerData.ActiveGun != name)
                    {
                        EquipButton.gameObject.SetActive(true);
                    }
                    
                    GunLevelText.gameObject.SetActive(true);
                    
                    GunLevelText.text = "Уровень " + gunData.GunLevel;

                    if (gunData.GunLevel + 1 > GunPrices[name].Count)
                    {
                        return;
                    }
                    int price = GunPrices[name][gunData.GunLevel + 1];
                    
                    GunLevelPriceText.text = price.ToString();

                    GunLevelPriceText.gameObject.SetActive(true);
                    
                    UpgradeButton.gameObject.SetActive(true);
                }
            }
        }
    }

    [ContextMenu("Upgrade")]
    public void Upgade()
    {
        GunType name = Guns[_gun].Type;

        PlayerData playerData = Progress.Instance.PlayerData;

        foreach (GunData gunData in playerData.GunDatas)
        {
            if (gunData.Name == name)
            {
                int price = GunPrices[name][gunData.GunLevel + 1];

                if (playerData.Money >= price)
                {
                    playerData.Money -= price;
                    gunData.GunLevel++;
                    Progress.Instance.Save();
                    
                    GunLevelText.text = "Уровень " + gunData.GunLevel;

                    if (gunData.GunLevel + 1 > GunPrices[name].Count)
                    {
                        GunLevelPriceText.gameObject.SetActive(false);
                        UpgradeButton.gameObject.SetActive(false);
                        return;
                    }
                    price = GunPrices[name][gunData.GunLevel + 1];
                    
                    
                    GunLevelPriceText.text = price.ToString();
                }
                
                return;
            }
        }
    }
    
    [ContextMenu("Buy")]
    public void Buy()
    {
        GunType name = Guns[_gun].Type;
        
        PlayerData playerData = Progress.Instance.PlayerData;

        foreach (GunData gunData in playerData.GunDatas)
        {
            if (gunData.Name == name)
            {
                int price = GunPrices[name][gunData.GunLevel];
                
                if (playerData.Money >= price)
                {
                    playerData.Money -= price;
                    gunData.IsOpened = true;
                    gunData.GunLevel++;
                    Progress.Instance.Save();
                    BuyButton.gameObject.SetActive(false);
                    UpgradeButton.gameObject.SetActive(true);
                    
                    GunLevelText.gameObject.SetActive(true);
                    
                    EquipButton.gameObject.SetActive(true);
                    
                    price = GunPrices[name][gunData.GunLevel + 1];
                    
                    GunLevelText.text = "Уровень " + gunData.GunLevel;
                    GunLevelPriceText.text = price.ToString();
                }
                
                return;
            }
        }
    }

    public void Equip()
    {
        GunType name = Guns[_gun].Type;
        
        Debug.Log(name);

        Progress.Instance.PlayerData.ActiveGun = name;
        Progress.Instance.Save();
    }
}
