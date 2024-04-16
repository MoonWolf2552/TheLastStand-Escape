using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string _ruText;
    [SerializeField] private string _enText;
    void Start()
    {
        if (YandexGame.lang == "ru")
        {
            GetComponent<TMP_Text>().text = _ruText;
        }
        else if (YandexGame.lang == "en")
        {
            GetComponent<TMP_Text>().text = _enText;
        }
        else
        {
            GetComponent<TMP_Text>().text = _ruText;
        }
    }
}
