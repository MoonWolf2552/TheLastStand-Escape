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

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<PlayerMove>())
        {
            GameManager.Instance.Enter.SetActive(true);
            PlayerMove.Instance.EnterDoor = this;
        }
    }
    
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.GetComponent<PlayerMove>())
        {
            GameManager.Instance.Enter.SetActive(false);
            PlayerMove.Instance.EnterDoor = null;
        }
    }

    public void EnterRoom()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int roomNumber = _roomsLibrary.GetSceneRoomNumber(_levelNumber, currentSceneIndex);

        PlayerMove.Instance.LastRoom = roomNumber;

        int roomIndex = _roomsLibrary.GetRoomSceneIndex(_levelNumber, _roomNumber);
        GameManager.Instance.LoadRoom(roomIndex);
    }
}