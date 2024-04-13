using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
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
    [SerializeField] private float _health = 10f;
    
    [SerializeField] private Transform _playerModel;

    [SerializeField] private Animator _animator;

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

        foreach (GunObject gunObject in _gunObjects)
        {
            gunObject.Gun.gameObject.SetActive(gunObject.GunType == Progress.Instance.PlayerData.ActiveGun);
        }
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        _staminaSlider = FindObjectOfType<Stamina>();
        _staminaSlider.GetComponent<Slider>().maxValue = _stamina;
        _healthSlider = FindObjectOfType<Health>();
        _healthSlider.GetComponent<Slider>().maxValue = _health;

        _health = _shopLibrary.UpgradePrices[UpgradeType.Health][Progress.Instance.PlayerData.HealthLevel].value;
        _stamina = _shopLibrary.UpgradePrices[UpgradeType.Stamina][Progress.Instance.PlayerData.StaminaLevel].value;
        
    }

    void Update()
    {
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

        if (IsRead || IsReload) return;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
            Input.mousePosition.y, -Camera.main.transform.position.y));
        _playerModel.LookAt(mouseWorldPosition);
        _playerModel.rotation = Quaternion.Euler(new Vector3(0, _playerModel.rotation.eulerAngles.y, 0));

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputVector = new Vector3(horizontalInput, 0, verticalInput);
        if (inputVector == Vector3.zero)
        {
            _animator.SetTrigger("Idle");
            _animator.ResetTrigger("Walk");
        }
        else
        {
            _animator.SetTrigger("Walk");
            _animator.ResetTrigger("Idle");
        }

        Vector3 worldVelocity;

        if (Input.GetKey(KeyCode.LeftShift) && _stamina > 0 && inputVector != Vector3.zero)
        {
            worldVelocity = transform.TransformVector(inputVector) * (_speed * 2);
            
            _stamina -= Time.deltaTime;
        }
        else
        {
            worldVelocity = transform.TransformVector(inputVector) * _speed;
            
            _stamina += 0.5f * Time.deltaTime;
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

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.GetComponent<Enemy>() is Enemy enemy && !IsDamaged)
        {
            GetHit(enemy.Damage);
        }
    }

    private void GetHit(float damage)
    {
        _health -= damage;
        _healthSlider.GetComponent<Slider>().value = _health;
    }

    private IEnumerator HitProcess()
    {
        IsDamaged = true;
        yield return new WaitForSeconds(0.3f);
        IsDamaged = false;
    }
}