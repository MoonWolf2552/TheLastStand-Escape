using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCounter : MonoBehaviour
{
    public static EnemyCounter Instance;

    [SerializeField] private RoomsLibrary _roomsLibrary;
    [SerializeField] private int _levelNumber;

    private Dictionary<int, List<Enemy>> _enemies = new Dictionary<int, List<Enemy>>();

    private bool _canSpawn = true;

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
    }

    [ContextMenu("Count")]
    public void Count()
    {
        Enemy[] enemiesInScene = FindObjectsOfType<Enemy>();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int roomNumber = _roomsLibrary.GetSceneRoomNumber(_levelNumber, currentSceneIndex);

        if (!_enemies.ContainsKey(roomNumber))
        {
            List<Enemy> enemiesInRoom = new List<Enemy> { };
            foreach (Enemy enemy in enemiesInScene)
            {
                if (enemy.Room == roomNumber)
                {
                    enemiesInRoom.Add(enemy);
                }
            }

            _enemies.Add(roomNumber, enemiesInRoom);
        }

        foreach (Enemy enemy in enemiesInScene)
        {
            if (enemy.Room == roomNumber)
            {
                if (!_enemies[roomNumber].Contains(enemy))
                {
                    Destroy(enemy.gameObject);
                }
            }
            else
            {
                enemy.gameObject.SetActive(false);
            }
        }

        foreach (Enemy enemy in _enemies[roomNumber])
        {
            enemy.gameObject.SetActive(true);
            enemy.Load();
        }

        Key key = FindObjectOfType<Key>();
        SecondaryKey secondaryKey = FindObjectOfType<SecondaryKey>();
        if (key && Player.Instance.HaveKey)
        {
            Destroy(key.gameObject);
        }

        if (secondaryKey && Player.Instance.HaveSecondaryKey)
        {
            Destroy(secondaryKey.gameObject);
        }
    }

    public void DestroyAllEnemies()
    {
        foreach (List<Enemy> enemiesInRoom in _enemies.Values)
        {
            foreach (Enemy enemy in enemiesInRoom)
            {
                Destroy(enemy.gameObject);
            }
        }
    }

    private void Update()
    {
        if (_levelNumber != 0) return;
        Enemy[] _enemies = FindObjectsOfType<Enemy>();
        if (_enemies.Length == 0 && _canSpawn)
        {
            _canSpawn = false;
            StartCoroutine(Arcade.Instance.Spawn());
        }
    }

    public void CanSpawn()
    {
        _canSpawn = true;
    }

    public void DestroyAllEnemiesArcade()
    {
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            Destroy(enemy.gameObject);
        }
    }
}