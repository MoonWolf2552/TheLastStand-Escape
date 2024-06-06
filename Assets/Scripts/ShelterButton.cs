using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShelterButton : MonoBehaviour
{
    public void Click()
    {
        SceneManager.LoadScene(0);
    }
}
