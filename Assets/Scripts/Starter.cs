using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{
    [ContextMenu("Start")]
    void Start()
    {
        FindObjectOfType<PlayerMove>().MoveToSpawn();
        EnemyCounter.Instance.Count();
    }
}