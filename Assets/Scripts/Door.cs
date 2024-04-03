using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int _roomNumber;
    [SerializeField] private int _levelNumber;
    [SerializeField] private RoomsLibrary _roomsLibrary;

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.GetComponent<PlayerMove>())
        {
            int roomIndex = _roomsLibrary.GetRoomSceneIndex(_levelNumber, _roomNumber);
            GameManager.Instance.LoadRoom(roomIndex);
        }
    }
}
