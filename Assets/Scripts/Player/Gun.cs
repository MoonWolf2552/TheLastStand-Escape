using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Gun : MonoBehaviour
{
    public GunType Name;
    public int GunLevel;
    
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private float _bulletSpeed = 20f;

    private float _damage = 15;
    private float _shotPeriod = 0.3f;

    private float _timer;

    private void Start()
    {
        GunData[] gunDatas = Progress.Instance.PlayerData.GunDatas;
        foreach (GunData data in gunDatas)
        {
            if (data.Name == Name)
            {
                GunLevel = data.GunLevel;
            }
        }

        if (Name == GunType.Pistol)
        {
            _damage += 3 * GunLevel;
            _shotPeriod = 0.6f;
        }
        else if (Name == GunType.Automatic)
        {
            _shotPeriod -= 0.03f * GunLevel;
            _damage = 7;
        }
        
        Debug.Log((_damage, _shotPeriod));
    }

    void Update()
    {
        _timer += Time.deltaTime;
        // if (Input.GetMouseButtonDown(0))
        // {
        //     if (_timer >= _shotPeriod)
        //     {
        //         Fire();
        //         _timer = 0;
        //     }
        // }
        if (Input.GetMouseButton(0))
        {
            if (_timer >= _shotPeriod)
            {
                Fire();
                _timer = 0;
            }
        }
    }

    private void Fire()
    {
        Bullet newBullet = Instantiate(_bullet, _bulletSpawn.position, _bulletSpawn.rotation);
        newBullet.Damage = _damage;
        newBullet.GetComponent<Rigidbody>().velocity = _bulletSpawn.forward * _bulletSpeed;
    }
}
