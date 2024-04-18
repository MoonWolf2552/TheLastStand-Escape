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

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private LevelLibrary _levelLibrary;
    
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
        
        if (EnemyCounter.Instance)
        {
            EnemyCounter.Instance.DestroyAllEnemies();
            Destroy(EnemyCounter.Instance.gameObject);
        }
    }

    public void Restart()
    {
        PlayUI.alpha = 1;
        Destroy(Player.Instance.gameObject);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
}
