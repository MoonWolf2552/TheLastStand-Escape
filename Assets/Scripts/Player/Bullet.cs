using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() is Enemy enemy)
        {
            enemy.GetHit(Damage);
        }

        if (collision.gameObject.GetComponent<Bullet>()) return;
        Destroy(gameObject);
    }
}
