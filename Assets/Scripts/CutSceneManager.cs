using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG.Utils.Pay;

public class CutSceneManager : MonoBehaviour
{
    private bool _firstClick;
    private int _titresSceneIndex = 48;
    [SerializeField] private Button SkipCutsceneButton;
    
    void Start()
    {
        Destroy(FindObjectOfType<GameManager>().gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_firstClick)
            {
                LoadTitres();
            }
            else
            {
                _firstClick = true;
                SkipCutsceneButton.gameObject.SetActive(true);
                StartCoroutine(WaitingUntilHide());
            }
        }
        
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!_firstClick)
            {
                _firstClick = true;
                SkipCutsceneButton.gameObject.SetActive(true);
                StartCoroutine(WaitingUntilHide());
            }
        }
    }

    public void LoadTitres()
    {
        SceneManager.LoadScene(_titresSceneIndex);
    }

    private IEnumerator WaitingUntilHide()
    {
        yield return new WaitForSeconds(10);
        SkipCutsceneButton.gameObject.SetActive(false);
        _firstClick = false;
    }
}
