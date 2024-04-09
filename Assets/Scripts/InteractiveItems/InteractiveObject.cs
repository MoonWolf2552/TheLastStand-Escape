using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    [SerializeField] protected String InteractText = "Взаимодействовать";
    
    public virtual void Interact()
    {
        GameManager.Instance.InteractButton.SetActive(false);
        Player.Instance.ObjectToInteract = null;
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Player>())
        {
            GameManager.Instance.InteractButton.SetActive(true);
            GameManager.Instance.InteractButtonText.text = InteractText;
            Player.Instance.ObjectToInteract = this;
        }
    }
    
    private void OnTriggerExit(Collider collider)
    {
        if (collider.GetComponent<Player>())
        {
            GameManager.Instance.InteractButton.SetActive(false);
            GameManager.Instance.InteractButtonText.text = null;
            Player.Instance.ObjectToInteract = null;
        }
    }
    
    public virtual void Close()
    {
        
    }
}
