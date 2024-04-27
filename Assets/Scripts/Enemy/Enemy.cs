using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using YG;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _health = 100;
    [SerializeField] private float _foundRadius = 3f;
    [SerializeField] private int _money = 100;
    [SerializeField] private NavMeshAgent _agent;

    [SerializeField] private Animator _animator;

    [SerializeField] private Collider _collider;

    public float Damage = 5;

    private bool _alive = true;
    private bool _found;
    private bool _isHitted;

    public int Room;

    private Transform _playerTransform;

    [SerializeField] private AudioSource _idleAudio;
    [SerializeField] private AudioSource _hitAudio;

    private int _hitCount;
    private bool _hitAudioCheck;

    [SerializeField] private Trash _trashPrefab;

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

    public void GetHit(float damage)
    {
        _health -= damage;

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

    public void GetShotgunHit(float damage)
    {
        _health -= damage;
        _hitCount++;

        if (_health <= 0)
        {
            Die();
        }
        else
        {
            _found = true;
            _animator.SetTrigger("Hit");
            
            if (!_hitAudioCheck)
            {
                _hitAudio.Play();
                StartCoroutine(Audio());
            }

            if (_hitCount >= 5)
            {
                _hitCount = 0;
                StartCoroutine(HitProcess());
            }
        }
    }

    public void Found()
    {
        _found = true;
        _animator.SetTrigger("Found");
    }

    private IEnumerator Audio()
    {
        _idleAudio.mute = true;
        _hitAudioCheck = true;
        yield return new WaitForSeconds(0.4f);
        _idleAudio.mute = false;
        _hitAudioCheck = false;
    }

    private IEnumerator HitProcess()
    {
        _idleAudio.mute = true;
        if (!_hitAudioCheck)
        {
            _hitAudio.Play();
            _hitAudioCheck = true;
        }
        _collider.enabled = false;
        _isHitted = true;
        _agent.SetDestination(transform.position);

        yield return new WaitForSeconds(0.15f);

        _isHitted = false;
        _animator.SetTrigger("Found");

        yield return new WaitForSeconds(0.25f);

        _idleAudio.mute = false;
        _hitAudioCheck = false;

        yield return new WaitForSeconds(0.28f);

        _collider.enabled = true;
    }

    private void Die()
    {
        GetComponent<Collider>().enabled = false;
        _alive = false;
        _agent.isStopped = true;
        _agent.speed = 0f;

        _idleAudio.mute = true;
        _hitAudio.Play();

        _animator.SetTrigger("Die");
        _collider.enabled = false;

        Progress.Instance.PlayerData.Kills++;
        YandexGame.NewLeaderboardScores("KillsLeaderboard", Progress.Instance.PlayerData.Kills);
        Progress.Instance.Save();

        Trash newTrash = Instantiate(_trashPrefab, transform.position, Quaternion.identity);
        newTrash.Money = _money;

        if (Player.Instance.Arcade)
        {
            StartCoroutine(DieCountdown());
        }
    }

    public IEnumerator DieCountdown()
    {
        yield return new WaitForSeconds(20);
        Destroy(gameObject);
    }
}