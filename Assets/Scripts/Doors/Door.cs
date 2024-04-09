using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Player>())
        {
            GameManager.Instance.EnterButton.SetActive(true);
            Player.Instance.EnterDoor = this;
        }
    }
    
    protected virtual void OnTriggerExit(Collider collider)
    {
        if (collider.GetComponent<Player>())
        {
            GameManager.Instance.EnterButton.SetActive(false);
            Player.Instance.EnterDoor = null;
        }
    }
    
    public virtual void Enter()
    {
        
    }
}
