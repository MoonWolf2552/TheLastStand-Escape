using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _playButton;

    [SerializeField] private LevelLibrary _levelLibrary;
    
    void Start()
    {
        _playButton.text = "Начать уровень " + (Progress.Instance.PlayerData.Level + 1);
    }

    public void StartGame()
    {
        int levelIndex = _levelLibrary.GetLevelSceneIndex(Progress.Instance.PlayerData.Level + 1);
        SceneManager.LoadScene(levelIndex);
    }

    public void OpenShop()
    {
        Shop shop = FindObjectOfType<Shop>();
        shop.OpenShop();
    }
}
