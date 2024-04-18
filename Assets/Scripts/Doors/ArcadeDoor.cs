using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeDoor : Door
{
    public override void Enter()
    {
        Destroy(Player.Instance.gameObject);
        GameManager.Instance.InteractButton.gameObject.SetActive(false);
        SceneManager.LoadScene(2);
    }
}
