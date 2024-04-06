using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health = 3;
    [SerializeField] private NavMeshAgent _agent;

    private bool _alive = true;

    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = PlayerMove.Instance.transform;
    }

    private void LateUpdate()
    {
        if (_alive)
        {
            _agent.SetDestination(_playerTransform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody.GetComponent<Bullet>())
        {
            Destroy(collision.gameObject);
            Hit();
        }
    }

    public void Hit()
    {
        _health--;
        if (_health <= 0)
        {
            Die();
        }
    }

    private IEnumerator DieProcess()
    {
        for (float t = 0; t < 1f; t += (Time.deltaTime / 4f))
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    private void Die()
    {
        GetComponent<Collider>().enabled = false;
        _alive = false;
        _agent.isStopped = true;
        _agent.speed = 0f;
        EnemyCounter.Instance.DecreaseEnemyCount();
        StartCoroutine(DieProcess());
    }
}
