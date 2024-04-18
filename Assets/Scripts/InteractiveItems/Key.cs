using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : InteractiveObject
{
    public override void Interact()
    {
        Player.Instance.HaveKey = true;
        
        base.Interact();
    }
}
