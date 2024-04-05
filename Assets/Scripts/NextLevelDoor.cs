using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelDoor : MonoBehaviour
{
    public int Level;
    
    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.GetComponent<PlayerMove>())
        {
            GameManager.Instance.LoadLevel(Level + 1);
        }
    }
}
