using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{
    void Start()
    {
        PlayerMove.Instance.MoveToSpawn();
        EnemyCounter.Instance.Count();
    }
}
