using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOpenByLevel : MonoBehaviour
{
    public int Level;
    
    void Start()
    {
        if (Progress.Instance.PlayerData.Level < Level)
        {
            gameObject.SetActive(false);
        }
    }
}
