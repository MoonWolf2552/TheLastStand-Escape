using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelDoor : Door
{
    public int Level;

    public override void Enter()
    {
        if (Player.Instance.HaveKey)
        {
            GameManager.Instance.EnterButton.SetActive(false);
            GameManager.Instance.ShowWin();
        }
        else
        {
            GameManager.Instance.RequirementGO.gameObject.SetActive(true);
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
