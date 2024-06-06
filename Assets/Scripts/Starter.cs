using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : MonoBehaviour
{
    // [SerializeField] private bool _titres;
    
    [ContextMenu("Start")]
    void Start()
    {
        // if (!_titres)
        // {
            Player.Instance.MoveToSpawn();
            StartCoroutine(Player.Instance.ScreenRemove());
            EnemyCounter.Instance.Count();
        // }
        // else
        // {
        //     StartCoroutine(ScreenRemove());
        // }
    }
    
    // private IEnumerator ScreenRemove()
    // {
    //     GameManager.Instance.BlackScreen.GetComponent<Animator>().SetTrigger("Hide");
    //     yield return new WaitForSeconds(0.25f);
    //     GameManager.Instance.BlackScreen.SetActive(false);
    // }
}