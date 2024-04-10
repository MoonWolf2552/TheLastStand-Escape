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
    public TMP_Text NoteText;
    public TMP_Text NoteCloseButtonText;
    
    public Image NewsImage;
    public TMP_Text NewsText;
    public TMP_Text NewsCloseButtonText;

    public TMP_Text Requirement;

    public TMP_Text Money;
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
        NoteImage.gameObject.SetActive(false);
        NewsImage.gameObject.SetActive(false);
        Requirement.gameObject.SetActive(false);
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
        int levelIndex = _levelLibrary.GetLevelSceneIndex(level);
        SceneManager.LoadScene(levelIndex);
        
        EnemyCounter.Instance.DestroyAllEnemies();
        Destroy(Player.Instance.gameObject);
        Destroy(EnemyCounter.Instance.gameObject);
        
        Progress.Instance.PlayerData.Level = level - 1;
        Progress.Instance.Save();
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
}
