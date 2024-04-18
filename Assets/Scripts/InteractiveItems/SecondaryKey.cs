using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryKey : InteractiveObject
{
    public override void Interact()
    {
        Player.Instance.HaveSecondaryKey = true;

        base.Interact();
    }
}