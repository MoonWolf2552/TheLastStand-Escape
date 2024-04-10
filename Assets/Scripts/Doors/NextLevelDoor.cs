using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelDoor : Door
{
    public int Level;

    [SerializeField] private String _requirement;

    public override void Enter()
    {
        if (Player.Instance.HaveKey)
        {
            GameManager.Instance.LoadLevel(Level + 1);
            GameManager.Instance.EnterButton.SetActive(false);
        }
        else
        {
            GameManager.Instance.Requirement.gameObject.SetActive(true);
            GameManager.Instance.Requirement.text = _requirement;
        }
    }

    protected override void OnTriggerExit(Collider collider)
    {
        base.OnTriggerExit(collider);
        
        GameManager.Instance.Requirement.gameObject.SetActive(false);
        GameManager.Instance.Requirement.text = null;
    }
}
