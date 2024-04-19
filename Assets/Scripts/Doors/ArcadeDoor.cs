using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class ArcadeDoor : Door
{
    public override void Enter()
    {
        GameManager.Instance.EnterButton.gameObject.SetActive(false);
        GameManager.Instance.ArcadeMenuShow();
    }
}
