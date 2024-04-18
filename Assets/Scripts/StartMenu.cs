using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _playButton;

    [SerializeField] private LevelLibrary _levelLibrary;

    [SerializeField] private Button _onSound;
    [SerializeField] private Button _offSound;
    [SerializeField] private AudioListener _audioListener;
    
    void Start()
    {
        String prefix = "Начать уровень ";
        if (YandexGame.lang == "en")
        {
            prefix = "Start level ";
        }
        
        _playButton.text = prefix + (Progress.Instance.PlayerData.Level + 1);

        if (Progress.Instance.PlayerData.Sound)
        {
            _offSound.gameObject.SetActive(true);
            _onSound.gameObject.SetActive(false);
        }
        else
        {
            _offSound.gameObject.SetActive(false);
            _onSound.gameObject.SetActive(true);
            _audioListener.enabled = false;
        }
    }

    public void StartGame()
    {
        int levelIndex = _levelLibrary.GetLevelSceneIndex(Progress.Instance.PlayerData.Level + 1);
        SceneManager.LoadScene(levelIndex);
    }

    public void SoundOn()
    {
        Progress.Instance.PlayerData.Sound = true;
        _audioListener.enabled = true;
        Progress.Instance.Save();
    }
    
    public void SoundOff()
    {
        Progress.Instance.PlayerData.Sound = false;
        _audioListener.enabled = false;
        Progress.Instance.Save();
    }
}
