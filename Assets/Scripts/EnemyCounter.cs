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

    private Enemy[] _enemiesInScene;
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
        _enemiesInScene = FindObjectsOfType<Enemy>();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int roomNumber = _roomsLibrary.GetSceneRoomNumber(_levelNumber, currentSceneIndex);
        Debug.Log(_roomsLibrary.GetSceneRoomNumber(_levelNumber, currentSceneIndex));
        int enemyCount = Enemies[roomNumber];
        for (int i = 0; i < (_enemiesInScene.Length - enemyCount); i++)
        {
            Destroy(_enemiesInScene[i].gameObject);
        }
    }

    public void DecreaseEnemyCount()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int roomNumber = _roomsLibrary.GetSceneRoomNumber(_levelNumber, currentSceneIndex);
        Enemies[roomNumber]--;
    }
}
