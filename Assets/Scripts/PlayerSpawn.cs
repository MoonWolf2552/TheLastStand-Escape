using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public int LastRoom; // комната, из которой ведёт дверь
    
    public void MovePlayerToSpawn()
    {
        Player.Instance.transform.position = transform.position;
        Player.Instance.transform.rotation = transform.rotation;
    }
}
