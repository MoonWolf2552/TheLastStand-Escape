using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShelterButton : MonoBehaviour
{
    public void Click()
    {
        GameManager.Instance.LoadShelter();
    }
}
