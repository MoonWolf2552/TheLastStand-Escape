using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help : InteractiveObject
{
    [SerializeField] private GameObject _helpObject;
    
    public override void Interact()
    {
        GameManager.Instance.InteractButton.SetActive(false);
        Player.Instance.IsRead = true;
        
        _helpObject.gameObject.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        
        Player.Instance.IsRead = false;
        _helpObject.gameObject.SetActive(false);
    }
}
