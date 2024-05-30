using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private float _spawnPeriod;

    private float _timer;

    /*void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _spawnPeriod)
        {
            _timer = 0;
            Spawn();
        }
    }*/

    public void Spawn()
    {
        Enemy newEnemy = Instantiate(_enemyPrefab, transform.position, transform.rotation);
        newEnemy.Found();
    }
}
