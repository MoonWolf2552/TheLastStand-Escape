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

    [SerializeField] private bool _isLocked;
    [SerializeField] private bool _needKey;

    protected override void Start()
    {
        base.Start();
        
        if (_needKey)
        {
            _isLocked = true;
        }
    }

    public override void Enter()
    {
        if (_isLocked)
        {
            GameManager.Instance.RequirementGO.gameObject.SetActive(true);
            GameManager.Instance.Requirement.text = _requirement;
            if (!_needKey || (_needKey && !Player.Instance.HaveSecondaryKey)) return;
        }
        
        GameManager.Instance.RequirementGO.gameObject.SetActive(false);
        GameManager.Instance.Go();
    }

    public override void Go()
    {
        base.Go();
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int roomNumber = _roomsLibrary.GetSceneRoomNumber(_levelNumber, currentSceneIndex);

        Player.Instance.LastRoom = roomNumber;

        int roomIndex = _roomsLibrary.GetRoomSceneIndex(_levelNumber, _roomNumber);
        GameManager.Instance.BlackScreen.SetActive(true);
        GameManager.Instance.LoadRoom(roomIndex);
    }

    protected override void OnTriggerExit(Collider collider)
    {
        base.OnTriggerExit(collider);

        if (_isLocked)
        {
            GameManager.Instance.RequirementGO.gameObject.SetActive(false);
            GameManager.Instance.Requirement.text = null;
        }
    }
}