using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class EnemyData
{
    public int RoomNumber;
    public int EnemyCount;
}

public class EnemyCounter : MonoBehaviour
{
    public static EnemyCounter Instance;
    
    [SerializeField] private RoomsLibrary _roomsLibrary;
    [SerializeField] private int _levelNumber;

    public EnemyData[] EnemyDatas;

    public Dictionary<int, int> Enemies = new Dictionary<int, int>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        foreach (EnemyData enemyData in EnemyDatas)
        {
            Enemies.Add(enemyData.RoomNumber, enemyData.EnemyCount);
        }
    }

    public void Count()
    {
        Enemy[] enemiesInScene = FindObjectsOfType<Enemy>();
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int roomNumber = _roomsLibrary.GetSceneRoomNumber(_levelNumber, currentSceneIndex);
        
        int enemyCount = Enemies[roomNumber];
        for (int i = 0; i < (enemiesInScene.Length - enemyCount); i++)
        {
            enemiesInScene[i].IsDied = true;
        }
    }

    public void DestroyAllEnemies()
    {
        Enemy[] enemiesInScene = FindObjectsOfType<Enemy>();


        foreach (Enemy enemy in enemiesInScene)
        {
            Destroy(enemy.gameObject);
        }
    }

    public void DecreaseEnemyCount()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int roomNumber = _roomsLibrary.GetSceneRoomNumber(_levelNumber, currentSceneIndex);
        Enemies[roomNumber]--;
    }
}
