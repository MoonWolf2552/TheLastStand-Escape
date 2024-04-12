using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody.GetComponent<Enemy>() is Enemy enemy)
        {
            enemy.GetHit(Damage);
        }
        Destroy(gameObject);
    }
}
