using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private LevelLibrary _levelLibrary;
    [SerializeField] private SoundChecker _soundChecker;

    public GameObject EnterButton;
    public GameObject InteractButton;
    public TMP_Text InteractButtonText;

    public Image NoteImage;
    public Image NoteImageBackgroung;
    public TMP_Text NoteText;
    public TMP_Text NoteCloseButtonText;

    public Image NewsImage;
    public Image NewsImageBackgroung;
    public TMP_Text NewsText;
    public TMP_Text NewsCloseButtonText;

    public TMP_Text Requirement;
    public GameObject RequirementGO;

    public TMP_Text Money;

    public GameObject WinObject;
    public GameObject LoseObject;
    public GameObject LoseArcadeObject;
    public GameObject EscapeObject;
    
    public Button RestartEscapeButton;
    
    public GameObject ArcadeMenu;

    public CanvasGroup PlayUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        EnterButton.gameObject.SetActive(false);
        InteractButton.gameObject.SetActive(false);
        NoteImageBackgroung.gameObject.SetActive(false);
        NewsImageBackgroung.gameObject.SetActive(false);
        RequirementGO.gameObject.SetActive(false);

        YandexGame.RewardVideoEvent += RestartArcade;
    }

    private void Start()
    {
        Money.text = Progress.Instance.PlayerData.Money.ToString();
    }

    public void LoadRoom(int roomIndex)
    {
        SceneManager.LoadScene(roomIndex);
        EnterButton.gameObject.SetActive(false);
    }

    public void LoadLevel(int level)
    {
        PlayUI.alpha = 1;
        Destroy(Player.Instance.gameObject);

        int levelIndex = _levelLibrary.GetLevelSceneIndex(level);
        SceneManager.LoadScene(levelIndex);
    }

    public void LoadShelter()
    {
        Time.timeScale = 1;
        
        if (EnemyCounter.Instance)
        {
            if (Player.Instance.Arcade)
            {
                EnemyCounter.Instance.DestroyAllEnemiesArcade();
            }
            else
            {
                EnemyCounter.Instance.DestroyAllEnemies();
            }
            Destroy(EnemyCounter.Instance.gameObject);
        }

        if (Player.Instance)
        {
            Destroy(Player.Instance.gameObject);
        }

        PlayUI.alpha = 1;
        SceneManager.LoadScene(0);
    }

    public void EnterRoom()
    {
        Player.Instance.EnterDoor.Enter();
    }

    public void Interact()
    {
        Player.Instance.ObjectToInteract.Interact();
    }

    public void CloseNote()
    {
        Player.Instance.ObjectToInteract.Close();
    }

    public void AddMoney()
    {
        Money.text = Progress.Instance.PlayerData.Money.ToString();
    }

    public void ShowWin()
    {
        WinObject.SetActive(true);
        PlayUI.alpha = 0;
    }

    public void Next()
    {
        Debug.Log(Progress.Instance.PlayerData.Level);
        LoadLevel(Progress.Instance.PlayerData.Level + 1);
    }

    public void ShowLose()
    {
        LoseObject.SetActive(true);
        PlayUI.alpha = 0;

        Player.Instance.IsRead = true;

        if (EnemyCounter.Instance)
        {
            EnemyCounter.Instance.DestroyAllEnemies();
            Destroy(EnemyCounter.Instance.gameObject);
        }
    }

    public void ShowLoseArcade()
    {
        LoseArcadeObject.SetActive(true);
        PlayUI.alpha = 0;

        Player.Instance.IsRead = true;

        EnemyCounter.Instance.DestroyAllEnemiesArcade();
    }

    public void Restart()
    {
        if (Player.Instance.Arcade)
        {
            YandexGame.RewVideoShow(0);
            return;
        }

        PlayUI.alpha = 1;
        Time.timeScale = 1;

        if (EnemyCounter.Instance)
        {
            EnemyCounter.Instance.DestroyAllEnemies();
            Destroy(EnemyCounter.Instance.gameObject);
        }
        
        if (Player.Instance)
        {
            Destroy(Player.Instance.gameObject);
        }

        

        LoadLevel(Progress.Instance.PlayerData.Level + 1);
    }

    private void RestartArcade(int index)
    {
        PlayUI.alpha = 1;
        Time.timeScale = 1;

        if (EnemyCounter.Instance)
        {
            EnemyCounter.Instance.DestroyAllEnemiesArcade();
            Destroy(EnemyCounter.Instance.gameObject);
        }
        
        if (Player.Instance)
        {
            Destroy(Player.Instance.gameObject);
        }

        SceneManager.LoadScene(2);
    }

    [ContextMenu("Exit")]
    public void Exit()
    {
        PlayUI.alpha = 0;
        Destroy(Player.Instance.gameObject);

        Progress.Instance.PlayerData.Level++;
        Progress.Instance.Save();

        if (EnemyCounter.Instance)
        {
            EnemyCounter.Instance.DestroyAllEnemies();
            Destroy(EnemyCounter.Instance.gameObject);
        }

        SceneManager.LoadScene(1);
    }

    public void Pause()
    {
        RestartEscapeButton.gameObject.SetActive(!Player.Instance.Arcade);

        EscapeObject.SetActive(true);

        _soundChecker.CheckSound();

        Player.Instance.IsRead = true;

        Time.timeScale = 0;
    }

    public void Continue()
    {
        EscapeObject.SetActive(false);
        ArcadeMenu.SetActive(false);

        Player.Instance.IsRead = false;

        Time.timeScale = 1;
    }

    public void ArcadeMenuShow()
    {
        ArcadeMenu.SetActive(true);

        Player.Instance.IsRead = true;

        Time.timeScale = 0;
    }
}