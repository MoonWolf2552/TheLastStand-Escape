using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    void Awake()
    {
        PlayerMove.Instance.transform.position = transform.position;
        PlayerMove.Instance.transform.rotation = transform.rotation;
    }
}
