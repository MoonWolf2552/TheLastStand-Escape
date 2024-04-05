using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{
    void Start()
    {
        EnemyCounter.Instance.Count();
    }
}
