using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Gun : MonoBehaviour
{
    public GunType Name;
    public int GunLevel;
    
    [SerializeField] protected Bullet _bullet;
    [SerializeField] protected Transform _bulletSpawn;
    [SerializeField] protected float _bulletSpeed = 20f;

    protected float _damage = 15;
    protected float _shotPeriod = 0.3f;
    protected float _bulletCount = 16;

    [SerializeField] protected int _maxAmmo;
    protected int _ammo;

    protected float _timer;

    protected bool _isReload;
    
    protected TMP_Text _ammoText;

    [SerializeField] protected ShopLibrary _shopLibrary;
    
    [SerializeField] protected AudioSource _fireAudio;
    [SerializeField] protected AudioSource _reloadAudio;

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

        if (Name == GunType.Pistol && GunLevel > 0)
        {
            _damage = _shopLibrary.GunPrices[Name][GunLevel].value;
            _shotPeriod = 0.6f;
        }
        else if (Name == GunType.Automatic && GunLevel > 0)
        {
            _shotPeriod = _shopLibrary.GunPrices[Name][GunLevel].value;
            _damage = 10;
        }
        else if (Name == GunType.Shotgun && GunLevel > 0)
        {
            _shotPeriod = 1f;
            _damage = _shopLibrary.GunPrices[Name][GunLevel].value;
        }

        _ammoText = FindObjectOfType<Ammo>().GetComponent<TMP_Text>();
        _ammoText.text = $"{_ammo}/{_maxAmmo}";
    }

    void Update()
    {
        _ammoText.text = $"{_ammo}/{_maxAmmo}";
        _timer += Time.deltaTime;
        
        if (Player.Instance.IsRead) return;
        
        if (Input.GetMouseButton(0))
        {
            if (_timer >= _shotPeriod && !_isReload)
            {
                if (_ammo > 0)
                {
                    _fireAudio.Play();
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

    protected virtual void Fire()
    {
        Bullet newBullet = Instantiate(_bullet, _bulletSpawn.position, _bulletSpawn.rotation);
        newBullet.Damage = _damage;
        newBullet.GetComponent<Rigidbody>().velocity = _bulletSpawn.forward * _bulletSpeed;
        
        _ammo--;
        _ammoText.text = $"{_ammo}/{_maxAmmo}";
    }
    
    private IEnumerator Reload()
    {
        Player.Instance.IsReload = true;
        
        _reloadAudio.Play();

        _isReload = true;
        
        yield return new WaitForSeconds(1.8f);

        _isReload = false;

        _ammo = _maxAmmo;
        _ammoText.text = $"{_ammo}/{_maxAmmo}";
        
        Player.Instance.IsReload = false;
    }
}
