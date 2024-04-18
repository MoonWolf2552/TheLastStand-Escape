using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelPriceValue
{
    public int Level;
    public int Price;
    public float Value;
}

[System.Serializable]
public class GunLevelsPrices
{
    public GunType Type;
    public LevelPriceValue[] LevelsPrices;
}

public enum UpgradeType
{
    None,
    Health,
    Stamina
}

[System.Serializable]
public class UpgradeLevelsPrices
{
    public UpgradeType Type;
    public LevelPriceValue[] LevelsPrices;
}

[CreateAssetMenu]
public class ShopLibrary : ScriptableObject
{
    public GunLevelsPrices[] GunLevelsPrices;
    public UpgradeLevelsPrices[] UpgradeLevelsPrices;
    
    public Dictionary<GunType, Dictionary<int, (int price, float value)>> GunPrices = new Dictionary<GunType, Dictionary<int, (int, float)>>();
    
    public Dictionary<UpgradeType, Dictionary<int, (int price, float value)>> UpgradePrices = new Dictionary<UpgradeType, Dictionary<int, (int, float)>>();
    
    private void OnEnable()
    {
        foreach (GunLevelsPrices gunLevelPrice in GunLevelsPrices)
        {
            var levelPriceValue = new Dictionary<int, (int, float)>();
            foreach (LevelPriceValue price in gunLevelPrice.LevelsPrices)
            {
                levelPriceValue.Add(price.Level, (price.Price, price.Value));
            }
            GunPrices.Add(gunLevelPrice.Type, levelPriceValue);
        }
        
        foreach (UpgradeLevelsPrices upgradeLevelPrice in UpgradeLevelsPrices)
        {
            var levelPriceValue = new Dictionary<int, (int, float)>();
            foreach (LevelPriceValue price in upgradeLevelPrice.LevelsPrices)
            {
                levelPriceValue.Add(price.Level, (price.Price, price.Value));
            }
            UpgradePrices.Add(upgradeLevelPrice.Type, levelPriceValue);
        }
    }
}
