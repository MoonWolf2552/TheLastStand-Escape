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
    public TMP_Text ReceivedMoney;
    public TMP_Text AllMoney;
    public GameObject LoseObject;
    public GameObject LoseArcadeObject;
    public GameObject EscapeObject;
    
    public Button RestartEscapeButton;
    
    public GameObject ArcadeMenu;

    public CanvasGroup PlayUI;

    public GameObject BlackScreen;

    private bool _wait;

    private string _prefix1 = "Получено монет ";
    private string _prefix2 = "Всего монет ";
    private int _levelMoney;

    [SerializeField] private AudioSource _audioSource;

    public Button HealButton;
    
    public Image KeyImage;

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
        RequirementGO.gameObject.SetActive(false);
        NoteImageBackgroung.gameObject.SetActive(false);
        NewsImageBackgroung.gameObject.SetActive(false);

        YandexGame.RewardVideoEvent += Reward;
    }

    public void DisableObjects()
    {
        EnterButton.gameObject.SetActive(false);
        InteractButton.gameObject.SetActive(false);
        RequirementGO.gameObject.SetActive(false);
        NoteImageBackgroung.gameObject.SetActive(false);
        NewsImageBackgroung.gameObject.SetActive(false);
    }

    private void Start()
    {
        Money.text = Progress.Instance.PlayerData.Money.ToString();

        if (YandexGame.lang == "en")
        {
            _prefix1 = "Сoins received ";
            _prefix2 = "Total coins ";
        }
    }

    public void LoadRoom(int roomIndex)
    {
        SceneManager.LoadScene(roomIndex);
        EnterButton.gameObject.SetActive(false);
    }

    public void LoadLevel(int level)
    {
        BlackScreen.SetActive(true);
        PlayUI.alpha = 1;

        int levelIndex = _levelLibrary.GetLevelSceneIndex(level);
        SceneManager.LoadScene(levelIndex);
    }

    public void LoadShelter()
    {
        Time.timeScale = 1;
        
        if (!_wait)
        {
            StartCoroutine(WaitProcess(LoadShelter));
            return;
        }

        _wait = false;
        
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
       

        PlayUI.alpha = 1;
        
        AddMoney();
        
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

    public void AddMoneyLevel()
    {
        Money.text = Player.Instance.Money.ToString();
    }

    public void ShowWin()
    {
        WinObject.SetActive(true);
        
        PlayUI.alpha = 0;

        Progress.Instance.PlayerData.Money += _levelMoney;
        
        ReceivedMoney.text = _prefix1 + _levelMoney;
        AllMoney.text =  _prefix2 + Progress.Instance.PlayerData.Money;
        
        Progress.Instance.Save();
    }

    public void AddRewardedMoney()
    {
        Progress.Instance.PlayerData.Money += _levelMoney;
        
        ReceivedMoney.text = _prefix1 + (_levelMoney * 2);
        AllMoney.text =  _prefix2 + Progress.Instance.PlayerData.Money;
        
        
        
        Progress.Instance.Save();
    }

    public void Next()
    {
        if (!_wait)
        {
            StartCoroutine(WaitProcess(Next));
            return;
        }

        _wait = false;
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
        Time.timeScale = 1;
        if (!_wait)
        {
            StartCoroutine(WaitProcess(Restart));
            return;
        }
        
        _wait = false;

        PlayUI.alpha = 1;

        if (EnemyCounter.Instance)
        {
            EnemyCounter.Instance.DestroyAllEnemies();
            Destroy(EnemyCounter.Instance.gameObject);
        }
        Destroy(Player.Instance.gameObject);

        LoadLevel(Progress.Instance.PlayerData.Level + 1);
    }

    public void StartArcade()
    {
        if (!_wait)
        {
            StartCoroutine(WaitProcess(StartArcade));
            return;
        }
        
        _wait = false;
        YandexGame.RewVideoShow(0);
    }

    private void Reward(int index)
    {
        if (index == 0)
        {
            RestartArcade();
        }
        else if (index == 1)
        {
            HealPlayer();
        }
        else if (index == 2)
        {
            AddRewardedMoney();
        }
    }

    private void HealPlayer()
    {
        Pause();
        Player.Instance.Heal();
        HealButton.gameObject.SetActive(Player.Instance.HealCount > 0);
    }

    private void RestartArcade()
    {
        PlayUI.alpha = 1;
        Time.timeScale = 1;

        if (EnemyCounter.Instance)
        {
            EnemyCounter.Instance.DestroyAllEnemiesArcade();
            Destroy(EnemyCounter.Instance.gameObject);
        }

        SceneManager.LoadScene(2);
    }

    [ContextMenu("Exit")]
    public void Exit()
    {
        _levelMoney = Convert.ToInt32(Money.text);
        PlayUI.alpha = 0;
        if (!_wait)
        {
            StartCoroutine(WaitProcess(Exit));
            return;
        }

        _wait = false;

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
        HealButton.gameObject.SetActive(!Player.Instance.Arcade && Player.Instance.HealCount > 0);

        RestartEscapeButton.gameObject.SetActive(!Player.Instance.Arcade);

        EscapeObject.SetActive(true);

        _soundChecker.CheckSound();

        Player.Instance.IsRead = true;
        Player.Instance.AudioSourceBackGround.mute = true;

        Time.timeScale = 0;
    }

    public void Continue()
    {
        EscapeObject.SetActive(false);
        ArcadeMenu.SetActive(false);

        Player.Instance.IsRead = false;
        Player.Instance.AudioSourceBackGround.mute = false;

        Time.timeScale = 1;
    }

    public void ArcadeMenuShow()
    {
        ArcadeMenu.SetActive(true);

        Player.Instance.IsRead = true;
    }

    public void Go()
    {
        if (!_wait)
        {
            StartCoroutine(WaitProcess(Go));
            return;
        }

        _wait = false;
        Player.Instance.EnterDoor.Go();
    }

    private IEnumerator WaitProcess(Action method)
    {
        _audioSource.Play();
        BlackScreen.SetActive(true);
        _wait = true;
        yield return new WaitForSeconds(0.7f);
        method();
    }
}