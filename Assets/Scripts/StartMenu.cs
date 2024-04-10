using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text _playButton;
    [SerializeField] private Button _shopButton;

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
        _playButton.GetComponentInParent<Button>().gameObject.SetActive(false);
        _shopButton.gameObject.SetActive(false);
        
        GunShop gunShop = FindObjectOfType<GunShop>();
        gunShop.NextGun();
        gunShop.NextButton.gameObject.SetActive(true);
    }
}
