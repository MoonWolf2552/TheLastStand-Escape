using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class News : InteractiveObject
{
    [SerializeField] private Sprite _newsSprite;
    [TextArea(20, 25)] [SerializeField] private String _news;
    [SerializeField] private String _newsCloseButtonText = "Закрыть";
    
    public override void Interact()
    {
        GameManager.Instance.InteractButton.SetActive(false);
        Player.Instance.IsRead = true;
        
        GameManager.Instance.InteractButton.SetActive(false);
        GameManager.Instance.NewsImage.sprite = _newsSprite;
        GameManager.Instance.NewsText.text = _news;
        GameManager.Instance.NewsCloseButtonText.text = _newsCloseButtonText;
        
        GameManager.Instance.NewsImage.gameObject.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        
        GameManager.Instance.NewsImage.sprite = null;
        GameManager.Instance.NewsText.text = null;
        GameManager.Instance.NewsCloseButtonText.text = null;
        
        GameManager.Instance.NewsImage.gameObject.SetActive(false);
        
        Player.Instance.IsRead = false;
        GameManager.Instance.InteractButton.SetActive(true);
    }
}
