using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class Note : InteractiveObject
{
    [SerializeField] private Sprite _noteSprite;
    [TextArea(20, 25)] [SerializeField] private String _note;
    [TextArea(20, 25)] [SerializeField] private String _noteEn;
    [SerializeField] private String _noteCloseButtonText = "Закрыть";
    [SerializeField] private String _noteCloseButtonTextEn = "Close";

    protected override void Start()
    {
        base.Start();

        if (YandexGame.lang == "en")
        {
            _note = _noteEn;
            _noteCloseButtonText = _noteCloseButtonTextEn;
        }
    }

    public override void Interact()
    {
        GameManager.Instance.InteractButton.SetActive(false);
        Player.Instance.IsRead = true;
        
        GameManager.Instance.InteractButton.SetActive(false);
        GameManager.Instance.NoteImage.sprite = _noteSprite;
        GameManager.Instance.NoteText.text = _note;
        GameManager.Instance.NoteCloseButtonText.text = _noteCloseButtonText;
        
        GameManager.Instance.NoteImageBackgroung.gameObject.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        
        GameManager.Instance.NoteImage.sprite = null;
        GameManager.Instance.NoteText.text = null;
        GameManager.Instance.NoteCloseButtonText.text = null;
        
        GameManager.Instance.NoteImageBackgroung.gameObject.SetActive(false);
        
        Player.Instance.IsRead = false;
        GameManager.Instance.InteractButton.SetActive(true);
    }
}