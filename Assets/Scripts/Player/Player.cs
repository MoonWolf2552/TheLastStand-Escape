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
        GameManager.Instance.AddMoneyLevel();

        CheckSound();
        StartCoroutine(ScreenRemove());
    }

    void Update()
    {
        if (_exit)
        {
            if (_exitCoroutine) return;

            StartCoroutine(ExitProcess());
            _exitCoroutine = true;

            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.EscapeObject.active)
            {
                GameManager.Instance.Continue();
                return;
            }

            GameManager.Instance.Pause();
        }
        
        if (Input.GetKeyDown(KeyCode.F))
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

        if (IsRead) return;

        if (Camera.main != null)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, -Camera.main.transform.position.y));
            _playerModel.LookAt(mouseWorldPosition);
        }

        _playerModel.rotation = Quaternion.Euler(new Vector3(0, _playerModel.rotation.eulerAngles.y, 0));

        if (IsReload) return;

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

                _animator.SetTrigger("Run");
            }
            else
            {
                worldVelocity = transform.TransformVector(inputVector) * _speed;

                _stamina += 0.5f * Time.deltaTime;

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
        yield return new WaitForSeconds(0.25f);
        GameManager.Instance.BlackScreen.SetActive(false);
        IsRead = false;
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
            _rigidbody.velocity = new Vector3(vector.x, _rigidbody.velocity.y, vector.z);
            yield return null;
        }

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