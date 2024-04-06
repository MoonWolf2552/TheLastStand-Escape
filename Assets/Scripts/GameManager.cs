using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private LevelLibrary _levelLibrary;
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
    }
    
    public void LoadRoom(int roomIndex)
    {
        SceneManager.LoadScene(roomIndex);
    }

    public void LoadLevel(int level)
    {
        int levelIndex = _levelLibrary.GetLevelSceneIndex(level);
        SceneManager.LoadScene(levelIndex);
        
        EnemyCounter.Instance.DestroyAllEnemies();
        Destroy(PlayerMove.Instance.gameObject);
        Destroy(EnemyCounter.Instance.gameObject);
    }
}
