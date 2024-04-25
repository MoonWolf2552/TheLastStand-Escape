using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    protected override void Fire()
    {
        for (int i = 0; i < _bulletCount; i++)
        {
            Vector3 direction = _bulletSpawn.transform.forward; // your initial aim.
            Vector3 spread = Vector3.zero;
            spread += _bulletSpawn.transform.up *
                      Random.Range(-1f, 1f); // add random up or down (because random can get negative too)
            spread += _bulletSpawn.right * Random.Range(-1f, 1f); // add random left or right

            // Using random up and right values will lead to a square spray pattern. If we normalize this vector, we'll get the spread direction, but as a circle.
            // Since the radius is always 1 then (after normalization), we need another random call. 
            direction += spread.normalized * Random.Range(0f, 0.2f);

            RaycastHit hit;

            if (Physics.Raycast(_bulletSpawn.transform.position, direction, out hit, 3))
            {
                if (hit.collider.GetComponent<Enemy>() is Enemy enemy)
                {
                    enemy.GetShotgunHit(_damage);
                }
            }
        }

        _ammo--;
        _ammoText.text = $"{_ammo}/{_maxAmmo}";
    }
}