using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Transform _playerModel;

    [SerializeField] private Animator _animator;

    public Door EnterDoor;
    
    private Rigidbody _rigidbody;

    public static PlayerMove Instance;

    public int LastRoom;

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
         Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.y));
         _playerModel.LookAt(mouseWorldPosition);
         _playerModel.rotation = Quaternion.Euler(new Vector3(0, _playerModel.rotation.eulerAngles.y, 0));
        
        float horizontaInput = Input.GetAxis("Horizontal");
        float verticalnput = Input.GetAxis("Vertical");
        
        Vector3 inputVector = new Vector3(horizontaInput, 0, verticalnput);
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
