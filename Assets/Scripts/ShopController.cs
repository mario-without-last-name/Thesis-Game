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
    [SerializeField] private GameObject GroupRerollButton;
    [SerializeField] private Image RerollButtonColor;
    [SerializeField] private TextMeshProUGUI RerollText;
    [SerializeField] int RerollPrice;

    [Header("Powerup Choices")]
    [SerializeField] private GameObject GroupPowerupSale1;
    [SerializeField] private Image PowerupIcon1;
    [SerializeField] private Image PowerupPriceButtonColor1;
    [SerializeField] private TextMeshProUGUI PowerupTextPrice1;
    [SerializeField] private TextMeshProUGUI PowerupName1;
    [SerializeField] private TextMeshProUGUI PowerupStats1;
    [SerializeField] private TextMeshProUGUI PowerupDescription1;
    [SerializeField] private GameObject GroupTextPurchased1;
    [SerializeField] private TextMeshProUGUI TextPurchased1;
    [Header("")]
    [SerializeField] private GameObject GroupPowerupSale2;
    [SerializeField] private Image PowerupIcon2;
    [SerializeField] private Image PowerupPriceButtonColor2;
    [SerializeField] private TextMeshProUGUI PowerupTextPrice2;
    [SerializeField] private TextMeshProUGUI PowerupName2;
    [SerializeField] private TextMeshProUGUI PowerupStats2;
    [SerializeField] private TextMeshProUGUI PowerupDescription2;
    [SerializeField] private GameObject GroupTextPurchased2;
    [SerializeField] private TextMeshProUGUI TextPurchased2;
    [Header("")]
    [SerializeField] private GameObject GroupPowerupSale3;
    [SerializeField] private Image PowerupIcon3;
    [SerializeField] private Image PowerupPriceButtonColor3;
    [SerializeField] private TextMeshProUGUI PowerupTextPrice3;
    [SerializeField] private TextMeshProUGUI PowerupName3;
    [SerializeField] private TextMeshProUGUI PowerupStats3;
    [SerializeField] private TextMeshProUGUI PowerupDescription3;
    [SerializeField] private GameObject GroupTextPurchased3;
    [SerializeField] private TextMeshProUGUI TextPurchased3;
    [Header("")]
    [SerializeField] private GameObject GroupPowerupSale4;
    [SerializeField] private Image PowerupIcon4;
    [SerializeField] private Image PowerupPriceButtonColor4;
    [SerializeField] private TextMeshProUGUI PowerupTextPrice4;
    [SerializeField] private TextMeshProUGUI PowerupName4;
    [SerializeField] private TextMeshProUGUI PowerupStats4;
    [SerializeField] private TextMeshProUGUI PowerupDescription4;
    [SerializeField] private GameObject GroupTextPurchased4;
    [SerializeField] private TextMeshProUGUI TextPurchased4;

    [Header("OtherControllers")]
    [SerializeField] private BottomBarController BottomBarController;

    // Start is called before the first frame update
    void Start()
    {
        // Reset reroll price. Affected by difficuly too?
        refreshShopPowerupOptions();
    }

    public void RerollButtonClicked()
    {
        musicController.PlayBuySellSoundEffectSource();
        //Increase price of reroll
        refreshShopPowerupOptions();
    }

    public void refreshShopPowerupOptions()
    {
        GroupPowerupSale1.SetActive(true);
        GroupPowerupSale2.SetActive(true);
        GroupPowerupSale3.SetActive(true);
        GroupPowerupSale4.SetActive(true);
        GroupTextPurchased1.SetActive(false);
        GroupTextPurchased2.SetActive(false);
        GroupTextPurchased3.SetActive(false);
        GroupTextPurchased4.SetActive(false);
    }

    public void purchasedPowerup1()
    {
        musicController.PlayBuySellSoundEffectSource();
        GroupPowerupSale1.SetActive(false);
        GroupTextPurchased1.SetActive(true);
    }

    public void purchasedPowerup2()
    {
        musicController.PlayBuySellSoundEffectSource();
        GroupPowerupSale2.SetActive(false);
        GroupTextPurchased2.SetActive(true);
    }

    public void purchasedPowerup3()
    {
        musicController.PlayBuySellSoundEffectSource();
        GroupPowerupSale3.SetActive(false);
        GroupTextPurchased3.SetActive(true);
    }

    public void purchasedPowerup4()
    {
        musicController.PlayBuySellSoundEffectSource();
        GroupPowerupSale4.SetActive(false);
        GroupTextPurchased4.SetActive(true);
    }

    // Must understand how to...
    // Distribute Powerups Selections, change stats depending on difficulty mode, manage bottom bar powerup, change to sell mode, etc.
}
