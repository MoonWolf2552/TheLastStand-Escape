using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
public class GunObject
{
    public GunType GunType;
    public Gun Gun;
}

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _stamina = 3f;
    [SerializeField] private float _maxStamina = 3f;
    [SerializeField] private float _health = 10f;

    [SerializeField] private Transform _playerModel;

    [SerializeField] private Animator _animator;

    [SerializeField] private Camera _camera;

    [SerializeField] private GunObject[] _gunObjects;

    private Stamina _staminaSlider;
    private Health _healthSlider;

    private Rigidbody _rigidbody;

    public bool HaveKey;

    public bool HaveSecondaryKey;

    public int LastRoom;

    public Door EnterDoor;

    public InteractiveObject ObjectToInteract;

    public bool IsRead;

    public bool IsReload;

    public bool IsDamaged;

    [SerializeField] private bool _exit;

    public bool Arcade;

    private bool _exitCoroutine;

    private bool _canRun = true;

    public int Money;

    [SerializeField] private ShopLibrary _shopLibrary;
    
    public AudioSource AudioSourceBackGround;
    
    [SerializeField] private AudioSource _audioSource;
    
    private float _soundTimer;
    private float _stepPeriod = 0.45f;
    private float _stepRunPeriod = 0.35f;

    [SerializeField] private bool _firstRoom;

    public int HealCount = 1;

    [SerializeField] private Armor[] _armor;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_firstRoom)
            {
                Destroy(gameObject);
                return;
            }
            Destroy(Instance.gameObject);
            Instance = this;
        }
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        ActivateGun();

        PlayerData playerData = Progress.Instance.PlayerData;

        _health = _shopLibrary.UpgradePrices[UpgradeType.Health][playerData.HealthLevel].value;
        _maxStamina = _shopLibrary.UpgradePrices[UpgradeType.Stamina][playerData.StaminaLevel].value;
        _stamina = _maxStamina;

        _staminaSlider = FindObjectOfType<Stamina>();
        _staminaSlider.GetComponent<Slider>().maxValue = _maxStamina;
        _healthSlider = FindObjectOfType<Health>();
        _healthSlider.GetComponent<Slider>().maxValue = _health;
        _healthSlider.GetComponent<Slider>().value = _health;

        GameManager.Instance.DisableObjects();
        if (Arcade)
        {
            GameManager.Instance.AddMoney();
        }
        else
        {
            GameManager.Instance.AddMoneyLevel();
        }

        CheckSound();
        StartCoroutine(ScreenRemove());

        GameManager.Instance.KeyImage.gameObject.SetActive(HaveKey && !Arcade);

        ActivateArmor();
    }

    void Update()
    {
        _soundTimer += Time.deltaTime;
        if (_exit)
        {
            if (_exitCoroutine) return;

            StartCoroutine(ExitProcess());
            _exitCoroutine = true;

            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.EscapeObject.activeSelf)
            {
                GameManager.Instance.Continue();
                return;
            }

            GameManager.Instance.Pause();
        }
        
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            if (EnterDoor)
            {
                GameManager.Instance.EnterRoom();
            }
            else if (IsRead && ObjectToInteract)
            {
                GameManager.Instance.CloseNote();
            }
            else if (ObjectToInteract)
            {
                GameManager.Instance.Interact();
            }
        }

        if (IsRead)
        {
            _animator.SetTrigger("Idle");
            _rigidbody.velocity = Vector3.zero;
            _stamina += 0.5f * Time.deltaTime;

            if (_stamina > _maxStamina)
            {
                _stamina = _maxStamina;
            }
            _staminaSlider.GetComponent<Slider>().value = _stamina;
            
            return;
        }

        if (Camera.main != null)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.y));
            _playerModel.LookAt(mouseWorldPosition);
        }

        _playerModel.rotation = Quaternion.Euler(new Vector3(0, _playerModel.rotation.eulerAngles.y, 0));

        if (IsReload)
        {
            _animator.SetTrigger("Idle");
            _rigidbody.velocity = Vector3.zero;
            
            _stamina += 0.5f * Time.deltaTime;

            if (_stamina > _maxStamina)
            {
                _stamina = _maxStamina;
            }
            _staminaSlider.GetComponent<Slider>().value = _stamina;
            
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputVector = new Vector3(horizontalInput, 0, verticalInput);

        Vector3 worldVelocity = Vector3.zero;
        if (inputVector == Vector3.zero)
        {
            _animator.SetTrigger("Idle");

            _stamina += 0.5f * Time.deltaTime;

            if (_stamina > _maxStamina)
            {
                _stamina = _maxStamina;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift) && _canRun && inputVector != Vector3.zero)
            {
                worldVelocity = transform.TransformVector(inputVector) * (_speed * 2);

                _stamina -= Time.deltaTime;
                
                if (_soundTimer >= _stepRunPeriod)
                {
                    _audioSource.Play();
                    _soundTimer = 0;
                }

                _animator.SetTrigger("Run");
            }
            else
            {
                worldVelocity = transform.TransformVector(inputVector) * _speed;

                _stamina += 0.5f * Time.deltaTime;
                
                if (_soundTimer >= _stepPeriod)
                {
                    _audioSource.Play();
                    _soundTimer = 0;
                }

                if (_stamina > _maxStamina)
                {
                    _stamina = _maxStamina;
                }

                _animator.SetTrigger("Walk");
            }
        }

        if (_stamina <= 0)
        {
            _canRun = false;
        }
        else if (_stamina >= _maxStamina / 2)
        {
            _canRun = true;
        }


        _staminaSlider.GetComponent<Slider>().value = _stamina;

        _rigidbody.velocity = new Vector3(worldVelocity.x, _rigidbody.velocity.y, worldVelocity.z);
    }

    public void MoveToSpawn()
    {
        PlayerSpawn[] playerSpawns = FindObjectsOfType<PlayerSpawn>();

        foreach (PlayerSpawn playerSpawn in playerSpawns)
        {
            if (playerSpawn.LastRoom == LastRoom)
            {
                playerSpawn.MovePlayerToSpawn();
                return;
            }
        }
    }

    public IEnumerator ScreenRemove()
    {
        GameManager.Instance.BlackScreen.GetComponent<Animator>().SetTrigger("Hide");
        IsRead = true;
        yield return new WaitForSeconds(0.7f);
        GameManager.Instance.BlackScreen.SetActive(false);
        IsRead = false;
    }
    
    public void ActivateArmor()
    {
        for (int i = 1; i <= Progress.Instance.PlayerData.HealthLevel; i++)
        {
            _armor[i - 1].gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.GetComponent<Enemy>() is Enemy enemy && !IsDamaged)
        {
            GetHit(enemy.Damage);
        }

        collider.gameObject.GetComponent<Collider>().enabled = false;
        collider.gameObject.GetComponent<Collider>().enabled = true;
    }

    private void GetHit(float damage)
    {
        _health -= damage;
        _healthSlider.GetComponent<Slider>().value = _health;

        if (_health <= 0)
        {
            if (Arcade)
            {
                GameManager.Instance.ShowLoseArcade();
                return;
            }

            GameManager.Instance.ShowLose();
        }

        StartCoroutine(HitProcess());
    }

    public void Heal()
    {
        HealCount--;
        _health = _healthSlider.GetComponent<Slider>().maxValue;
        _healthSlider.GetComponent<Slider>().value = _health;
    }

    private IEnumerator HitProcess()
    {
        IsDamaged = true;
        yield return new WaitForSeconds(0.6f);
        IsDamaged = false;
    }

    public void ActivateGun()
    {
        foreach (GunObject gunObject in _gunObjects)
        {
            gunObject.Gun.gameObject.SetActive(gunObject.GunType == Progress.Instance.PlayerData.ActiveGun);
        }
    }

    private IEnumerator ExitProcess()
    {
        _animator.SetTrigger("Walk");
        Vector3 vector = transform.TransformVector(new Vector3(1, 0, 0)) * _speed;


        for (float t = 0; t < 1f; t += Time.deltaTime / 5f)
        {
            if (_soundTimer >= _stepPeriod)
            {
                _audioSource.Play();
                _soundTimer = 0;
            }
            
            _rigidbody.velocity = new Vector3(vector.x, _rigidbody.velocity.y, vector.z);
            yield return null;
        }
        
        _animator.SetTrigger("Idle");

        GameManager.Instance.ShowWin();
    }

    public void CheckSound()
    {
        if (Progress.Instance.PlayerData.Sound)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }
    }
}