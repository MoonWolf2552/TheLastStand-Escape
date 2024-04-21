using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Debug = UnityEngine.Debug;

public class Shop : MonoBehaviour
{
    public static Shop Instance;

    public Button BuyButton;
    public Button UpgradeButton;
    public Button EquipButton;
    public TMP_Text GunLevelText;
    public TMP_Text GunLevelPriceText;
    public TMP_Text MoneyText;

    public GunButtonTypes[] Guns;

    private int _gun = 0;

    public Dictionary<GunType, Dictionary<int, (int price, float value)>> GunPrices;
    public Dictionary<UpgradeType, Dictionary<int, (int price, float value)>> UpgradePrices;

    [SerializeField] private ShopLibrary _shopLibrary;

    [SerializeField] private Slider _staminaSlider;
    [SerializeField] private Button _staminaUpgradeButton;
    [SerializeField] private TMP_Text _staminaLevelText;
    [SerializeField] private TMP_Text _staminaPriceText;

    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Button _healthUpgradeButton;
    [SerializeField] private TMP_Text _healthLevelText;
    [SerializeField] private TMP_Text _healthPriceText;

    private String _prefix = "Уровень ";

    private void Awake()
    {
        GunPrices = _shopLibrary.GunPrices;
        UpgradePrices = _shopLibrary.UpgradePrices;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (YandexGame.lang == "en")
        {
            _prefix = "Level ";
        }
    }

    public void OpenGunShop()
    {
        foreach (GunButtonTypes gun in Guns)
        {
            gun.gameObject.SetActive(false);
        }
        BuyButton.gameObject.SetActive(false);
        UpgradeButton.gameObject.SetActive(false);
        EquipButton.gameObject.SetActive(false);
        
        _gun = 0;
        Guns[_gun].gameObject.SetActive(true);

        MoneyText.text = Progress.Instance.PlayerData.Money.ToString();

        GunType name = Guns[_gun].Type;
        foreach (GunData gunData in Progress.Instance.PlayerData.GunDatas)
        {
            if (gunData.Name == name)
            {
                if (!gunData.IsOpened)
                {
                    BuyButton.gameObject.SetActive(true);

                    GunLevelPriceText.gameObject.SetActive(true);

                    Debug.Log(gunData.GunLevel);

                    int price = GunPrices[name][gunData.GunLevel + 1].price;

                    GunLevelPriceText.text = price.ToString();
                }
                else
                {
                    if (Progress.Instance.PlayerData.ActiveGun != name)
                    {
                        EquipButton.gameObject.SetActive(true);
                    }

                    GunLevelText.gameObject.SetActive(true);

                    GunLevelText.text = _prefix + gunData.GunLevel;

                    if (gunData.GunLevel >= GunPrices[name].Count)
                    {
                        return;
                    }

                    int price = GunPrices[name][gunData.GunLevel + 1].price;

                    GunLevelPriceText.text = price.ToString();

                    GunLevelPriceText.gameObject.SetActive(true);

                    UpgradeButton.gameObject.SetActive(true);
                }
            }
        }
    }

    public void NextGun()
    {
        _gun = (_gun + 1) % Guns.Length;
        Guns[_gun].gameObject.SetActive(true);

        MoneyText.text = Progress.Instance.PlayerData.Money.ToString();

        GunType name = Guns[_gun].Type;
        foreach (GunData gunData in Progress.Instance.PlayerData.GunDatas)
        {
            if (gunData.Name == name)
            {
                if (!gunData.IsOpened)
                {
                    BuyButton.gameObject.SetActive(true);

                    GunLevelPriceText.gameObject.SetActive(true);

                    Debug.Log(gunData.GunLevel);

                    int price = GunPrices[name][gunData.GunLevel + 1].price;

                    GunLevelPriceText.text = price.ToString();
                }
                else
                {
                    if (Progress.Instance.PlayerData.ActiveGun != name)
                    {
                        EquipButton.gameObject.SetActive(true);
                    }

                    GunLevelText.gameObject.SetActive(true);

                    GunLevelText.text = _prefix + gunData.GunLevel;

                    if (gunData.GunLevel >= GunPrices[name].Count)
                    {
                        return;
                    }

                    int price = GunPrices[name][gunData.GunLevel + 1].price;

                    GunLevelPriceText.text = price.ToString();

                    GunLevelPriceText.gameObject.SetActive(true);

                    UpgradeButton.gameObject.SetActive(true);
                }
            }
        }
    }

    [ContextMenu("Upgrade")]
    public void Upgade()
    {
        GunType name = Guns[_gun].Type;

        PlayerData playerData = Progress.Instance.PlayerData;

        foreach (GunData gunData in playerData.GunDatas)
        {
            if (gunData.Name == name)
            {
                int price = GunPrices[name][gunData.GunLevel + 1].price;

                if (playerData.Money >= price)
                {
                    playerData.Money -= price;
                    gunData.GunLevel++;
                    Progress.Instance.Save();

                    GunLevelText.text = _prefix + gunData.GunLevel;
                    MoneyText.text = Progress.Instance.PlayerData.Money.ToString();
                    GameManager.Instance.AddMoney();

                    if (gunData.GunLevel >= GunPrices[name].Count)
                    {
                        GunLevelPriceText.gameObject.SetActive(false);
                        UpgradeButton.gameObject.SetActive(false);
                        return;
                    }

                    price = GunPrices[name][gunData.GunLevel + 1].price;

                    GunLevelPriceText.text = price.ToString();
                }

                return;
            }
        }
    }

    [ContextMenu("Buy")]
    public void Buy()
    {
        GunType name = Guns[_gun].Type;

        PlayerData playerData = Progress.Instance.PlayerData;

        foreach (GunData gunData in playerData.GunDatas)
        {
            if (gunData.Name == name)
            {
                int price = GunPrices[name][gunData.GunLevel].price;

                if (playerData.Money >= price)
                {
                    playerData.Money -= price;
                    gunData.IsOpened = true;
                    gunData.GunLevel++;
                    Progress.Instance.Save();
                    BuyButton.gameObject.SetActive(false);
                    UpgradeButton.gameObject.SetActive(true);

                    GunLevelText.gameObject.SetActive(true);

                    EquipButton.gameObject.SetActive(true);

                    price = GunPrices[name][gunData.GunLevel + 1].price;

                    GunLevelText.text = _prefix + gunData.GunLevel;
                    GunLevelPriceText.text = price.ToString();
                    MoneyText.text = Progress.Instance.PlayerData.Money.ToString();
                    GameManager.Instance.AddMoney();
                }

                return;
            }
        }
    }

    public void Equip()
    {
        GunType name = Guns[_gun].Type;

        Debug.Log(name);

        Progress.Instance.PlayerData.ActiveGun = name;
        Progress.Instance.Save();
        
        Player.Instance.ActivateGun();
    }

    public void OpenPlayerShop()
    {
        PlayerData playerData = Progress.Instance.PlayerData;

        _healthSlider.value = playerData.HealthLevel;
        _staminaSlider.value = playerData.StaminaLevel;

        _healthLevelText.text = _prefix + playerData.HealthLevel;

        if (playerData.HealthLevel >= UpgradePrices[UpgradeType.Health].Count)
        {
            _healthPriceText.gameObject.SetActive(false);
            _healthUpgradeButton.gameObject.SetActive(false);
            return;
        }

        int price = UpgradePrices[UpgradeType.Health][playerData.HealthLevel + 1].price;

        _healthPriceText.text = price.ToString();

        _staminaLevelText.text = _prefix + playerData.StaminaLevel;

        if (playerData.StaminaLevel >= UpgradePrices[UpgradeType.Stamina].Count)
        {
            _staminaPriceText.gameObject.SetActive(false);
            _staminaUpgradeButton.gameObject.SetActive(false);
            return;
        }

        price = UpgradePrices[UpgradeType.Stamina][playerData.StaminaLevel + 1].price;

        _staminaPriceText.text = price.ToString();

        MoneyText.text = Progress.Instance.PlayerData.Money.ToString();
        GameManager.Instance.AddMoney();
    }

    public void HealthUpgrade()
    {
        PlayerData playerData = Progress.Instance.PlayerData;

        int price = UpgradePrices[UpgradeType.Health][playerData.HealthLevel + 1].price;

        if (playerData.Money >= price)
        {
            playerData.Money -= price;
            playerData.HealthLevel++;

            _healthLevelText.text = _prefix + playerData.HealthLevel;

            _healthSlider.value = playerData.HealthLevel;

            MoneyText.text = Progress.Instance.PlayerData.Money.ToString();
            GameManager.Instance.AddMoney();

            Progress.Instance.Save();

            if (playerData.HealthLevel >= UpgradePrices[UpgradeType.Health].Count)
            {
                _healthPriceText.gameObject.SetActive(false);
                _healthUpgradeButton.gameObject.SetActive(false);
                return;
            }

            price = UpgradePrices[UpgradeType.Health][playerData.HealthLevel + 1].price;
            _healthPriceText.text = price.ToString();
        }
    }

    public void StaminaUpgrade()
    {
        PlayerData playerData = Progress.Instance.PlayerData;

        int price = UpgradePrices[UpgradeType.Stamina][playerData.StaminaLevel + 1].price;

        if (playerData.Money >= price)
        {
            playerData.Money -= price;
            playerData.StaminaLevel++;

            _staminaLevelText.text = _prefix + playerData.StaminaLevel;

            _staminaSlider.value = playerData.StaminaLevel;

            MoneyText.text = Progress.Instance.PlayerData.Money.ToString();
            GameManager.Instance.AddMoney();

            Progress.Instance.Save();

            if (playerData.StaminaLevel >= UpgradePrices[UpgradeType.Stamina].Count)
            {
                _staminaPriceText.gameObject.SetActive(false);
                _staminaUpgradeButton.gameObject.SetActive(false);
                return;
            }

            price = UpgradePrices[UpgradeType.Stamina][playerData.StaminaLevel + 1].price;
        }
    }
}