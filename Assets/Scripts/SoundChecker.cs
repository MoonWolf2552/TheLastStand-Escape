using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class SoundChecker : MonoBehaviour
{
    [SerializeField] private Button _onSound;
    [SerializeField] private Button _offSound;


    private void Start()
    {
        Player.Instance.CheckSound();
    }

    public void CheckSound()
    {
        if (Progress.Instance.PlayerData.Sound)
        {
            _offSound.gameObject.SetActive(true);
            _onSound.gameObject.SetActive(false);
        }
        else
        {
            _offSound.gameObject.SetActive(false);
            _onSound.gameObject.SetActive(true);
        }
    }

    public void SoundOn()
    {
        Progress.Instance.PlayerData.Sound = true;
        Progress.Instance.Save();
        Player.Instance.CheckSound();
    }
    
    public void SoundOff()
    {
        Progress.Instance.PlayerData.Sound = false;
        Progress.Instance.Save();
        Player.Instance.CheckSound();
    }
}
