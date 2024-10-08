using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Reflection;

public class ShopController : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private MusicController musicController;

    [Header("Reroll")]
    [SerializeField] private GameObject groupRerollButton;
    [SerializeField] private Image RerollButtonColor;
    [SerializeField] private TextMeshProUGUI RerollText;
    [SerializeField] int RerollPrice;

    [Header("Powerup Choices")]
    [SerializeField] private GameObject groupPowerupSale1;
    [SerializeField] private Image powerupIcon1;
    [SerializeField] private Image powerupPriceButtonColor1;
    [SerializeField] private TextMeshProUGUI powerupTextPrice1;
    [SerializeField] private TextMeshProUGUI powerupName1;
    [SerializeField] private TextMeshProUGUI powerupStats1;
    [SerializeField] private TextMeshProUGUI powerupDescription1;
    [SerializeField] private GameObject groupTextPurchased1;
    [SerializeField] private TextMeshProUGUI textPurchased1;
    [Header("")]
    [SerializeField] private GameObject groupPowerupSale2;
    [SerializeField] private Image powerupIcon2;
    [SerializeField] private Image powerupPriceButtonColor2;
    [SerializeField] private TextMeshProUGUI powerupTextPrice2;
    [SerializeField] private TextMeshProUGUI powerupName2;
    [SerializeField] private TextMeshProUGUI powerupStats2;
    [SerializeField] private TextMeshProUGUI powerupDescription2;
    [SerializeField] private GameObject groupTextPurchased2;
    [SerializeField] private TextMeshProUGUI textPurchased2;
    [Header("")]
    [SerializeField] private GameObject groupPowerupSale3;
    [SerializeField] private Image powerupIcon3;
    [SerializeField] private Image powerupPriceButtonColor3;
    [SerializeField] private TextMeshProUGUI powerupTextPrice3;
    [SerializeField] private TextMeshProUGUI powerupName3;
    [SerializeField] private TextMeshProUGUI powerupStats3;
    [SerializeField] private TextMeshProUGUI powerupDescription3;
    [SerializeField] private GameObject groupTextPurchased3;
    [SerializeField] private TextMeshProUGUI textPurchased3;
    [Header("")]
    [SerializeField] private GameObject groupPowerupSale4;
    [SerializeField] private Image powerupIcon4;
    [SerializeField] private Image powerupPriceButtonColor4;
    [SerializeField] private TextMeshProUGUI powerupTextPrice4;
    [SerializeField] private TextMeshProUGUI powerupName4;
    [SerializeField] private TextMeshProUGUI powerupStats4;
    [SerializeField] private TextMeshProUGUI powerupDescription4;
    [SerializeField] private GameObject groupTextPurchased4;
    [SerializeField] private TextMeshProUGUI textPurchased4;

    [Header("OtherControllers")]
    [SerializeField] private BottomBarController BottomBarController;

    // Start is called before the first frame update
    void Start()
    {
        // Reset reroll price. Affected by difficuly too?
        RefreshShopPowerupOptions();
    }

    public void RerollButtonClicked()
    {
        musicController.PlayBuySellSoundEffectSource();
        //Increase price of reroll
        RefreshShopPowerupOptions();
    }

    public void RefreshShopPowerupOptions()
    {
        groupPowerupSale1.SetActive(true);
        groupPowerupSale2.SetActive(true);
        groupPowerupSale3.SetActive(true);
        groupPowerupSale4.SetActive(true);
        groupTextPurchased1.SetActive(false);
        groupTextPurchased2.SetActive(false);
        groupTextPurchased3.SetActive(false);
        groupTextPurchased4.SetActive(false);
    }

    public void PurchasedPowerup1()
    {
        musicController.PlayBuySellSoundEffectSource();
        groupPowerupSale1.SetActive(false);
        groupTextPurchased1.SetActive(true);
    }

    public void PurchasedPowerup2()
    {
        musicController.PlayBuySellSoundEffectSource();
        groupPowerupSale2.SetActive(false);
        groupTextPurchased2.SetActive(true);
    }

    public void PurchasedPowerup3()
    {
        musicController.PlayBuySellSoundEffectSource();
        groupPowerupSale3.SetActive(false);
        groupTextPurchased3.SetActive(true);
    }

    public void PurchasedPowerup4()
    {
        musicController.PlayBuySellSoundEffectSource();
        groupPowerupSale4.SetActive(false);
        groupTextPurchased4.SetActive(true);
    }

    // Must understand how to...
    // Distribute Powerups Selections, change stats depending on difficulty mode, manage bottom bar powerup, change to sell mode, etc.
    // Not enough gold? Then don't do anything
}
