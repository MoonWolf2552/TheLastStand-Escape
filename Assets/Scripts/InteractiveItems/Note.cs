using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : InteractiveObject
{
    [SerializeField] private Sprite _noteSprite;
    [TextArea(20, 25)] [SerializeField] private String _note;
    [SerializeField] private String _noteCloseButtonText = "Закрыть";
    
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