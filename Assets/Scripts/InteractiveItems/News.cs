using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class News : InteractiveObject
{
    [SerializeField] private Sprite _newsSprite;
    [SerializeField] private Sprite _newsSpriteEn;
    [TextArea(20, 25)] [SerializeField] private String _news;
    [TextArea(20, 25)] [SerializeField] private String _newsEn;
    [SerializeField] private String _newsCloseButtonText = "Закрыть";
    [SerializeField] private String _newsCloseButtonTextEn = "Close";

    protected override void Start()
    {
        base.Start();

        if (YandexGame.lang == "en")
        {
            _newsSprite = _newsSpriteEn;
            _news = _newsEn;
            _newsCloseButtonText = _newsCloseButtonTextEn;
        }
    }

    public override void Interact()
    {
        GameManager.Instance.InteractButton.SetActive(false);
        Player.Instance.IsRead = true;
        
        GameManager.Instance.InteractButton.SetActive(false);
        GameManager.Instance.NewsImage.sprite = _newsSprite;
        GameManager.Instance.NewsText.text = _news;
        GameManager.Instance.NewsCloseButtonText.text = _newsCloseButtonText;
        
        GameManager.Instance.NewsImageBackgroung.gameObject.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        
        GameManager.Instance.NewsImage.sprite = null;
        GameManager.Instance.NewsText.text = null;
        GameManager.Instance.NewsCloseButtonText.text = null;
        
        GameManager.Instance.NewsImageBackgroung.gameObject.SetActive(false);
        
        Player.Instance.IsRead = false;
        GameManager.Instance.InteractButton.SetActive(true);
    }
}
