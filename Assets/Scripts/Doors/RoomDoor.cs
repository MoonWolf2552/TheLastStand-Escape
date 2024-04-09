using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomDoor : Door
{
    [SerializeField] private int _roomNumber;
    [SerializeField] private int _levelNumber;
    [SerializeField] private RoomsLibrary _roomsLibrary;
    
    [SerializeField] private String _requirement;
    
    [SerializeField] private bool _isLocked;
    [SerializeField] private bool _needKey;

    private void Start()
    {
        if (_needKey)
        {
            _isLocked = true;
        }
    }

    public override void Enter()
    {
        if (_isLocked)
        {
            GameManager.Instance.Requirement.gameObject.SetActive(true);
            GameManager.Instance.Requirement.text = _requirement;
            if (!_needKey || (_needKey && !Player.Instance.HaveSecondaryKey))return;
        }
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int roomNumber = _roomsLibrary.GetSceneRoomNumber(_levelNumber, currentSceneIndex);

        Player.Instance.LastRoom = roomNumber;

        if (_needKey)
        {
            GameManager.Instance.Requirement.gameObject.SetActive(false);
            GameManager.Instance.Requirement.text = null;
        }

        int roomIndex = _roomsLibrary.GetRoomSceneIndex(_levelNumber, _roomNumber);
        GameManager.Instance.LoadRoom(roomIndex);
    }

    protected override void OnTriggerExit(Collider collider)
    {
        base.OnTriggerExit(collider);

        if (_isLocked)
        {
            GameManager.Instance.Requirement.gameObject.SetActive(false);
            GameManager.Instance.Requirement.text = null;
        }
    }
}