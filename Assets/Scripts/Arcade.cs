using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class Arcade : MonoBehaviour
{
    public static Arcade Instance;

    private EnemySpawner[] _enemySpawners;

    private float _timer;
    private float _spawnPeriod = 10f;
    private int _waveNumber;

    [SerializeField] private TMP_Text _waveNumberText;

    private String prefix;
    private String prefix2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        _enemySpawners = FindObjectsOfType<EnemySpawner>();

        if (YandexGame.lang == "ru")
        {
            prefix = "Волна ";
            prefix2 = "Передышка";
        }
        else if (YandexGame.lang == "en")
        {
            prefix = "Wave ";
            prefix2 = "Respite";
        }
        else
        {
            prefix = "Wave ";
            prefix2 = "Respite";
        }
    }

    public IEnumerator Spawn()
    {
        _waveNumberText.text = prefix2;
        
        if (_waveNumber > 0 && Progress.Instance.PlayerData.Waves < _waveNumber)
        {
            Progress.Instance.PlayerData.Waves = _waveNumber;
            YandexGame.NewLeaderboardScores("WaveLeaderboard", Progress.Instance.PlayerData.Waves);
            Progress.Instance.Save();
        }

        yield return new WaitForSeconds(_spawnPeriod);

        _waveNumber++;
        _waveNumberText.text = prefix + _waveNumber;


        foreach (EnemySpawner enemySpawner in _enemySpawners)
        {
            enemySpawner.Spawn();
        }

        EnemyCounter.Instance.CanSpawn();
    }
}