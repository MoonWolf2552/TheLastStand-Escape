using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{
    [ContextMenu("Start")]
    void Start()
    {
        Player.Instance.MoveToSpawn();
        StartCoroutine(Player.Instance.ScreenRemove());
        EnemyCounter.Instance.Count();
    }
}