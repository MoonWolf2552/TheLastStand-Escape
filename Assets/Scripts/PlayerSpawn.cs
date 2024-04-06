using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public int LastRoom; // комната, из которой ведёт дверь
    
    public void MovePlayerToSpawn()
    {
        PlayerMove.Instance.gameObject.transform.position = transform.position;
        PlayerMove.Instance.gameObject.transform.rotation = transform.rotation;
    }
}
