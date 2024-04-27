using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public int Money;
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Player>())
        {
            if (Player.Instance.Arcade)
            {
                Progress.Instance.PlayerData.Money += Money;
                GameManager.Instance.AddMoney();
                Progress.Instance.Save();
            }
            else
            {
                Player.Instance.Money += Money;
                GameManager.Instance.AddMoneyLevel();
            }
            Destroy(gameObject);
        }
    }
}
