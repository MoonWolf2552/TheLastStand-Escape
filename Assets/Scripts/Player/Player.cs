using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance;
    
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Transform _playerModel;

    [SerializeField] private Animator _animator;
    
    private Rigidbody _rigidbody;

    public bool HaveKey;
    
    public bool HaveSecondaryKey;
    
    public int LastRoom;

    public Door EnterDoor;
    
    public InteractiveObject ObjectToInteract;

    public bool IsRead;

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
        if (IsRead) return;
        
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.y));
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
        Vector3 worldVelocity = transform.TransformVector(inputVector) * _speed;
        
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
}
