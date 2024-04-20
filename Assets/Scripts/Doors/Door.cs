using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class Door : MonoBehaviour
{
    [SerializeField] protected String _requirement;
    [SerializeField] protected String _requirementEn;

    protected virtual void Start()
    {
        if (YandexGame.lang == "en")
        {
            _requirement = _requirementEn;
        }
    }

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
    
    public virtual void Go()
    {
        
    }
}
