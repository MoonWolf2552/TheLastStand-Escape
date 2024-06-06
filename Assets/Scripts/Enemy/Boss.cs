using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private Key _keyPrefab;
    
    protected override void Die()
    {
        base.Die();

        Vector3 keySpawnPosition = new Vector3(transform.position.x, 0.05f, transform.position.z);
        Instantiate(_keyPrefab, keySpawnPosition, Quaternion.identity);
    }
}
