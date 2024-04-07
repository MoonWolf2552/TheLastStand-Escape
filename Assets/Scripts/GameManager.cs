using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private LevelLibrary _levelLibrary;
    
    public GameObject Enter;
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
        Enter.gameObject.SetActive(false);
    }
    
    public void LoadRoom(int roomIndex)
    {
        SceneManager.LoadScene(roomIndex);
        Enter.gameObject.SetActive(false);
    }

    public void LoadLevel(int level)
    {
        int levelIndex = _levelLibrary.GetLevelSceneIndex(level);
        SceneManager.LoadScene(levelIndex);
        
        EnemyCounter.Instance.DestroyAllEnemies();
        Destroy(PlayerMove.Instance.gameObject);
        Destroy(EnemyCounter.Instance.gameObject);
    }

    [ContextMenu("enter")]
    public void EnterRoom()
    {
        PlayerMove.Instance.EnterDoor.EnterRoom();
    }
}
