using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeDoor : Door
{
    public override void Enter()
    {
        Destroy(Player.Instance.gameObject);
        SceneManager.LoadScene(2);
    }
}
