using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Transform _playerModel;

    [SerializeField] private Transform _sword;
    
    private Rigidbody _rigidbody;

    private float _xAngle;
    private bool _grounded;

    public static PlayerMove Instance;

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
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.C))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }*/
        
        Vector3 mousePos = Input.mousePosition;
        float screenWidth = Screen.width / 2f;

        float horizontaInput = Input.GetAxis("Horizontal");
        float verticalnput = Input.GetAxis("Vertical");
        
        Vector3 inputVector = new Vector3(horizontaInput, 0, verticalnput);
        Vector3 worldVelocity = transform.TransformVector(inputVector) * _speed;
        
        if (mousePos.x < screenWidth)
        {
            _playerModel.localEulerAngles = new Vector3(0, 180, 0);
        }
        else if (mousePos.x > screenWidth)
        {
            _playerModel.localEulerAngles = new Vector3(0, 0, 0);
        }
        _rigidbody.velocity = new Vector3(worldVelocity.x, _rigidbody.velocity.y, worldVelocity.z);
        
    }

    /*private void OnCollisionStay(Collision collision)
    {
        if (Vector3.Angle(collision.contacts[0].normal, Vector3.up) < 45f)
        {
            _grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        _grounded = false;
    }*/
}
