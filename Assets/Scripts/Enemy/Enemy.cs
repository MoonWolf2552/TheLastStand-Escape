using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _health = 100;
    [SerializeField] private float _foundRadius = 3f;
    [SerializeField] private int _money = 100;
    [SerializeField] private NavMeshAgent _agent;
    
    [SerializeField] private Animator _animator;
    
    [SerializeField] private Collider _collider;

    private bool _alive = true;
    private bool _found;
    private bool _isHitted;

    public int Room;

    private Transform _playerTransform;

    private void Start()
    {
        _playerTransform = Player.Instance.transform;
        DontDestroyOnLoad(gameObject);
    }

    public void Load()
    {
        if (_found)
        {
            _animator.SetTrigger("Found");
        }

        if (!_alive)
        {
            _animator.SetBool("Dead", true);
        }
    }

    private void Update()
    {
        if (_found && _alive && !_isHitted)
        {
            _agent.SetDestination(_playerTransform.position);
            return;
        }

        float distance = (_playerTransform.position - transform.position).magnitude;

        if (!_found && distance < _foundRadius)
        {
            _found = true;
            _animator.SetTrigger("Found");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody.GetComponent<Bullet>() is Bullet bullet)
        {
            Hit(bullet);
            Destroy(collision.gameObject);
        }
    }

    public void Hit(Bullet bullet)
    {
        _health -= bullet.Damage;
        
        if (_health <= 0)
        {
            Die();
        }
        else
        {
            _found = true;
            _animator.SetTrigger("Hit");

            StartCoroutine(HitProcess());
        }
        
        
    }
    
    private IEnumerator HitProcess()
    {
        _collider.enabled = false;
        _isHitted = true;
        _agent.SetDestination(transform.position);
        
        for (float t = 0; t < 1f; t += (Time.deltaTime / 0.53f))
        {
            yield return null;
        }
        _isHitted = false;
        _animator.ResetTrigger("Hit");
        _animator.SetTrigger("Found");
        
        for (float t = 0; t < 1f; t += (Time.deltaTime / 0.53f))
        {
            yield return null;
        }
        
        _collider.enabled = true;
    }

    private void Die()
    {
        GetComponent<Collider>().enabled = false;
        _alive = false;
        _agent.isStopped = true;
        _agent.speed = 0f;
        
        _animator.SetTrigger("Die");
        _collider.enabled = false;

        Progress.Instance.PlayerData.Money += _money;
        Progress.Instance.Save();
        GameManager.Instance.AddMoney();
    }
}
