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
        
         Vector3 MouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.y));
         _playerModel.LookAt(MouseWorldPosition);
         _playerModel.rotation = Quaternion.Euler(new Vector3(0, _playerModel.rotation.eulerAngles.y, 0));
        
        float horizontaInput = Input.GetAxis("Horizontal");
        float verticalnput = Input.GetAxis("Vertical");
        
        Vector3 inputVector = new Vector3(horizontaInput, 0, verticalnput);
        Vector3 worldVelocity = transform.TransformVector(inputVector) * _speed;
        
        _rigidbody.velocity = new Vector3(worldVelocity.x, _rigidbody.velocity.y, worldVelocity.z);
        
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
