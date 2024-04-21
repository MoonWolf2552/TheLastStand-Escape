using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using YG;

public enum GunType
{
    None,
    Pistol,
    Automatic,
    Shotgun
}

[System.Serializable]
public class GunData
{
    public GunType Name;
    public bool IsOpened;
    public int GunLevel;

    public GunData(GunType name, bool isOpened, int gunLevel)
    {
        Name = name;
        IsOpened = isOpened;
        GunLevel = gunLevel;
    }
}

[System.Serializable]
public class PlayerData
{
    public int Level;
    public int Money;
    public GunType ActiveGun = GunType.Pistol;
    public GunData[] GunDatas =
    {
        new GunData(GunType.Pistol, true, 1),
        new GunData(GunType.Automatic, false, 0),
        new GunData(GunType.Shotgun, false, 0)
    };
    public int HealthLevel;
    public int StaminaLevel;
    public bool Sound = true;
    public int Kills;
}

public class Progress : MonoBehaviour
{
    public static Progress Instance;

    public PlayerData PlayerData;
    
    private void Awake()
    {
        Instance = this;
        Load();
    }
    
    [ContextMenu("Save")]
    public void Save()
    {
        YandexGame.savesData.PlayerData = PlayerData;
        YandexGame.SaveProgress();
    }
    
    [ContextMenu("Load")]
    public void Load()
    {
        PlayerData playerSaves = YandexGame.savesData.PlayerData;
        if (playerSaves == null)
        {
            PlayerData = new PlayerData();
        }
        else
        {
            YandexGame.LoadProgress();
            PlayerData = playerSaves;
        }
    }
}
