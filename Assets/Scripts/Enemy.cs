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
    
    [SerializeField] private Animator _animator;
    
    [SerializeField] private Collider _collider;

    private bool _alive = true;
    private bool _found;
    private bool _isHitted;

    public bool IsDied;

    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = PlayerMove.Instance.transform;
    }

    private void Update()
    {
        if (_found && _alive && !_isHitted)
        {
            _agent.SetDestination(_playerTransform.position);
            return;
        }

        float distance = (_playerTransform.position - transform.position).magnitude;

        if (!_found && distance < 1)
        {
            _found = true;
            _animator.SetTrigger("Found");
        }

        if (IsDied)
        {
            _alive = false;
            _health = 0;
            _collider.enabled = false;
            _animator.SetTrigger("Died");
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
        else
        {
            _found = true;
            _animator.SetTrigger("Hit");
            _animator.SetTrigger("Found");

            StartCoroutine(HitProcess());
        }
        
        
    }
    
    private IEnumerator HitProcess()
    {
        _collider.enabled = false;
        _isHitted = true;
        _agent.SetDestination(transform.position);
        
        for (float t = 0; t < 1f; t += (Time.deltaTime / 1.06f))
        {
            yield return null;
        }
        
        _collider.enabled = true;
        _isHitted = false;
        _animator.ResetTrigger("Hit");
    }

    private IEnumerator DieProcess()
    {
        for (float t = 0; t < 1f; t += (Time.deltaTime / 2f))
        {
            yield return null;
        }

        _collider.enabled = false;
    }

    private void Die()
    {
        GetComponent<Collider>().enabled = false;
        _alive = false;
        _agent.isStopped = true;
        _agent.speed = 0f;
        EnemyCounter.Instance.DecreaseEnemyCount();
        
        _animator.SetTrigger("Die");
        StartCoroutine(DieProcess());
    }
}
