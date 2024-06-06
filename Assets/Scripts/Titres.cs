using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Titres : MonoBehaviour
{
    private int _titresSceneIndex = 48;
    
    void Start()
    {
        SceneManager.LoadScene(_titresSceneIndex);
    }
}
