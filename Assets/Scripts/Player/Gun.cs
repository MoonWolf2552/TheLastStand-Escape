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

    [SerializeField] private int _maxAmmo;
    private int _ammo;

    private float _timer;

    private bool _isReload;

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

        _ammo = _maxAmmo;

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
        else
        {
            _damage += 10 * GunLevel;
        }
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
            if (_timer >= _shotPeriod && !_isReload)
            {
                if (_ammo > 0)
                {
                    Fire();
                    _timer = 0;
                }
                else
                {
                    StartCoroutine(Reload());
                }
            }
        }
    }

    private void Fire()
    {
        Bullet newBullet = Instantiate(_bullet, _bulletSpawn.position, _bulletSpawn.rotation);
        newBullet.Damage = _damage;
        newBullet.GetComponent<Rigidbody>().velocity = _bulletSpawn.forward * _bulletSpeed;
        _ammo--;
    }
    
    private IEnumerator Reload()
    {
        Player.Instance.IsReload = true;

        _isReload = true;
        
        yield return new WaitForSeconds(1f);

        _isReload = false;

        _ammo = _maxAmmo;
        
        Player.Instance.IsReload = false;
    }
}
