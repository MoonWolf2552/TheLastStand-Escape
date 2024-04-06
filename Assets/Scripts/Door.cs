using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private int _roomNumber;
    [SerializeField] private int _levelNumber;
    [SerializeField] private RoomsLibrary _roomsLibrary;

    private void OnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.GetComponent<PlayerMove>())
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int roomNumber = _roomsLibrary.GetSceneRoomNumber(_levelNumber, currentSceneIndex);

            PlayerMove.Instance.LastRoom = roomNumber;
            
            int roomIndex = _roomsLibrary.GetRoomSceneIndex(_levelNumber, _roomNumber);
            GameManager.Instance.LoadRoom(roomIndex);
        }
    }
}
