using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private float _bulletSpeed = 20f;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        Bullet newBullet = Instantiate(_bullet, _bulletSpawn.position, _bulletSpawn.rotation);
        newBullet.GetComponent<Rigidbody>().velocity = _bulletSpawn.forward * _bulletSpeed;
    }
}
