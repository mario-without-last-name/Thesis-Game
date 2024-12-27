using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Reflection;
using Unity.VisualScripting;

public class ShopController : MonoBehaviour
{
    [Header("Reroll")]
    [SerializeField] private GameObject groupRerollButton;
    [SerializeField] private Image rerollButtonColor;
    [SerializeField] private TextMeshProUGUI rerollText;
    int RerollPrice;

    [Header("Powerup Choices")]
    string powerupIdentity1;
    string powerupType1;
    [SerializeField] private GameObject groupPowerupSale1;
    [SerializeField] private Image powerupIcon1;
    [SerializeField] private Image powerupPriceButtonColor1;
    [SerializeField] private TextMeshProUGUI powerupTextPrice1;
    private int powerupPrice1;
    [SerializeField] private TextMeshProUGUI powerupName1;
    [SerializeField] private TextMeshProUGUI powerupStats1;
    [SerializeField] private TextMeshProUGUI powerupDescription1;
    [SerializeField] private GameObject groupTextPurchased1;
    [SerializeField] private TextMeshProUGUI textPurchased1;
    private bool canBuyPowerup1;
    private bool haveBoughtPowerupThisRound1;

    [Header("")]
    string powerupIdentity2;
    string powerupType2;
    [SerializeField] private GameObject groupPowerupSale2;
    [SerializeField] private Image powerupIcon2;
    [SerializeField] private Image powerupPriceButtonColor2;
    [SerializeField] private TextMeshProUGUI powerupTextPrice2;
    private int powerupPrice2;
    [SerializeField] private TextMeshProUGUI powerupName2;
    [SerializeField] private TextMeshProUGUI powerupStats2;
    [SerializeField] private TextMeshProUGUI powerupDescription2;
    [SerializeField] private GameObject groupTextPurchased2;
    [SerializeField] private TextMeshProUGUI textPurchased2;
    private bool canBuyPowerup2;
    private bool haveBoughtPowerupThisRound2;

    [Header("")]
    string powerupIdentity3;
    string powerupType3;
    [SerializeField] private GameObject groupPowerupSale3;
    [SerializeField] private Image powerupIcon3;
    [SerializeField] private Image powerupPriceButtonColor3;
    [SerializeField] private TextMeshProUGUI powerupTextPrice3;
    private int powerupPrice3;
    [SerializeField] private TextMeshProUGUI powerupName3;
    [SerializeField] private TextMeshProUGUI powerupStats3;
    [SerializeField] private TextMeshProUGUI powerupDescription3;
    [SerializeField] private GameObject groupTextPurchased3;
    [SerializeField] private TextMeshProUGUI textPurchased3;
    private bool canBuyPowerup3;
    private bool haveBoughtPowerupThisRound3;

    [Header("")]
    string powerupIdentity4;
    string powerupType4;
    [SerializeField] private GameObject groupPowerupSale4;
    [SerializeField] private Image powerupIcon4;
    [SerializeField] private Image powerupPriceButtonColor4;
    [SerializeField] private TextMeshProUGUI powerupTextPrice4;
    private int powerupPrice4;
    [SerializeField] private TextMeshProUGUI powerupName4;
    [SerializeField] private TextMeshProUGUI powerupStats4;
    [SerializeField] private TextMeshProUGUI powerupDescription4;
    [SerializeField] private GameObject groupTextPurchased4;
    [SerializeField] private TextMeshProUGUI textPurchased4;
    private bool canBuyPowerup4;
    private bool haveBoughtPowerupThisRound4;

    [Header("Button Images")]
    [SerializeField] private Sprite rerollButtonActiveSprite;
    [SerializeField] private Sprite rerollButtonInactiveSprite;
    [SerializeField] private Sprite powerupPriceButtonActiveSprite;
    [SerializeField] private Sprite powerupPriceButtonInactiveSprite;

    [Header("Controllers")]
    [SerializeField] private MusicController musicController;
    [SerializeField] private BottomBarController bottomBarController;
    [SerializeField] private PlayerAndEnemyStatusController playerAndEnemyStatusController;
    [SerializeField] private PowerupsCatalogController powerupsCatalogController;
    [SerializeField] private DynamicDifficultyController dynamicDifficultyController;


    private int currGold;
    private string selectedDifficulty;
    private int totalGoldSpentSoFar;

    // Start is called before the first frame update
    void Start()
    {
        selectedDifficulty = PlayerPrefs.GetString("modeDifficulty", "Adaptive");
        ResetRerollPrice();
        totalGoldSpentSoFar = 0;
    }

    public void ResetRerollPrice()
    {
        RerollPrice = 4;
        rerollText.text = "Reroll ($" + RerollPrice + ")";
    }

    public void RerollButtonClicked() // Called by the reroll UI button
    {
        if (currGold >= RerollPrice && !(haveBoughtPowerupThisRound1 && haveBoughtPowerupThisRound2 && haveBoughtPowerupThisRound3 && haveBoughtPowerupThisRound4))
        {
            musicController.PlayBuySellSoundEffectSource();
            currGold -= RerollPrice;
            playerAndEnemyStatusController.SetChangeInCurrGold(-1 * RerollPrice);
            totalGoldSpentSoFar += RerollPrice;
            RerollPrice += 2;
            //rerollText.text = "Reroll ($" + RerollPrice + ")";
            RerollShopPowerupOptions();
        }
    }

    public void ActivateAndRerollShopPowerupOptions() //  Called after winning a round
    {
        groupPowerupSale1.SetActive(true);
        groupTextPurchased1.SetActive(false);
        haveBoughtPowerupThisRound1 = false;
        groupPowerupSale2.SetActive(true);
        groupTextPurchased2.SetActive(false);
        haveBoughtPowerupThisRound2 = false;
        groupPowerupSale3.SetActive(true);
        groupTextPurchased3.SetActive(false);
        haveBoughtPowerupThisRound3 = false;
        groupPowerupSale4.SetActive(true);
        groupTextPurchased4.SetActive(false);
        haveBoughtPowerupThisRound4 = false;
        RerollShopPowerupOptions();
    }

    public void RerollShopPowerupOptions() // called after winning a round or clicking on the reroll button. CANNOT BUY ACTIVE / PASSIVE powerup IF ALREADY FULL
    {

        float difficultyIndex = dynamicDifficultyController.GetDynamicOutput("powerupQuality");
        currGold = playerAndEnemyStatusController.GetCurrGold();
        List<string> firstFourPowerupIdentities = powerupsCatalogController.GetFirstFourPowerups(); // randomize 4 powerup options
        powerupIdentity1  = firstFourPowerupIdentities[0];
        powerupIdentity2 = firstFourPowerupIdentities[1];
        powerupIdentity3 = firstFourPowerupIdentities[2];
        powerupIdentity4 = firstFourPowerupIdentities[3];

        if (groupPowerupSale1.activeSelf) // is the setActive of the groupPowerupSale1 == true? then that powerup location (of total 4) in the shop has not been purchased this round, so you can reroll it. But if it has been bought, don't reroll it, so players cannot buy more than 4 poweups per round.
        {
            Sprite spriteOfPowerup1 = null;
            SetupPowerup(difficultyIndex, powerupIdentity1, ref powerupType1, ref spriteOfPowerup1, ref powerupPrice1, ref powerupTextPrice1, ref powerupName1, ref powerupStats1, ref powerupDescription1, ref textPurchased1);
            powerupIcon1.sprite = spriteOfPowerup1;
        }

        if (groupPowerupSale2.activeSelf)
        {
            Sprite spriteOfPowerup2 = null;
            SetupPowerup(difficultyIndex, powerupIdentity2, ref powerupType2, ref spriteOfPowerup2, ref powerupPrice2, ref powerupTextPrice2, ref powerupName2, ref powerupStats2, ref powerupDescription2, ref textPurchased2);
            powerupIcon2.sprite = spriteOfPowerup2;
        }

        if (groupPowerupSale3.activeSelf)
        {
            Sprite spriteOfPowerup3 = null;
            SetupPowerup(difficultyIndex, powerupIdentity3, ref powerupType3, ref spriteOfPowerup3, ref powerupPrice3, ref powerupTextPrice3, ref powerupName3, ref powerupStats3, ref powerupDescription3, ref textPurchased3);
            powerupIcon3.sprite = spriteOfPowerup3;
        }

        if (groupPowerupSale4.activeSelf)
        {
            Sprite spriteOfPowerup4 = null;
            SetupPowerup(difficultyIndex, powerupIdentity4, ref powerupType4, ref spriteOfPowerup4, ref powerupPrice4, ref powerupTextPrice4, ref powerupName4, ref powerupStats4, ref powerupDescription4, ref textPurchased4);
            powerupIcon4.sprite = spriteOfPowerup4;
        }

        DetermineIfEachPurchaseButtonCanBeClicked();
    }

    public void DetermineIfEachPurchaseButtonCanBeClicked() // After a purchase / reroll / SELL POWERUP, does the player have enough money (or active/passive inventory slots) to buy this powerup? If yes, orange box + white text, else, blue box + red text
    {
        //Debug.Log(currGold + "," + RerollPrice);
        if      (currGold < RerollPrice)                                                                                                   { rerollButtonColor.sprite = rerollButtonInactiveSprite; rerollText.color = Color.red;   rerollText.text = "Reroll ($" + RerollPrice + ")"; }
        else if (haveBoughtPowerupThisRound1 && haveBoughtPowerupThisRound2 && haveBoughtPowerupThisRound3 && haveBoughtPowerupThisRound4) { rerollButtonColor.sprite = rerollButtonInactiveSprite; rerollText.color = Color.red;   rerollText.text = "All Sold Out"; }
        else                                                                                                                               { rerollButtonColor.sprite = rerollButtonActiveSprite;   rerollText.color = Color.white; rerollText.text = "Reroll ($" + RerollPrice + ")"; }

        if      (powerupType1 == "active"  && bottomBarController.GetNumberOfOwnedActivePowerups()  >= 5) { powerupPriceButtonColor1.sprite = powerupPriceButtonInactiveSprite; powerupTextPrice1.color = Color.red;   canBuyPowerup1 = false; powerupTextPrice1.text = "Full";              } // cannot buy any more active powerups
        else if (powerupType1 == "passive" && bottomBarController.GetNumberOfOwnedPassivePowerups() >= 4) { powerupPriceButtonColor1.sprite = powerupPriceButtonInactiveSprite; powerupTextPrice1.color = Color.red;   canBuyPowerup1 = false; powerupTextPrice1.text = "Full";              } // cannot buy any more passive powerups
        else if (currGold < powerupPrice1)                                                                { powerupPriceButtonColor1.sprite = powerupPriceButtonInactiveSprite; powerupTextPrice1.color = Color.red;   canBuyPowerup1 = false; powerupTextPrice1.text = "$" + powerupPrice1; } // not enough gold
        else                                                                                              { powerupPriceButtonColor1.sprite = powerupPriceButtonActiveSprite;   powerupTextPrice1.color = Color.white; canBuyPowerup1 = true;  powerupTextPrice1.text = "$" + powerupPrice1; } // can buy this

        if      (powerupType2 == "active"  && bottomBarController.GetNumberOfOwnedActivePowerups()  >= 5) { powerupPriceButtonColor2.sprite = powerupPriceButtonInactiveSprite; powerupTextPrice2.color = Color.red;   canBuyPowerup2 = false; powerupTextPrice2.text = "Full";              } // cannot buy any more active powerups
        else if (powerupType2 == "passive" && bottomBarController.GetNumberOfOwnedPassivePowerups() >= 4) { powerupPriceButtonColor2.sprite = powerupPriceButtonInactiveSprite; powerupTextPrice2.color = Color.red;   canBuyPowerup2 = false; powerupTextPrice2.text = "Full";              } // cannot buy any more passive powerups
        else if (currGold < powerupPrice2)                                                                { powerupPriceButtonColor2.sprite = powerupPriceButtonInactiveSprite; powerupTextPrice2.color = Color.red;   canBuyPowerup2 = false; powerupTextPrice2.text = "$" + powerupPrice2; } // not enough gold
        else                                                                                              { powerupPriceButtonColor2.sprite = powerupPriceButtonActiveSprite;   powerupTextPrice2.color = Color.white; canBuyPowerup2 = true;  powerupTextPrice2.text = "$" + powerupPrice2; } // can buy this

        if      (powerupType3 == "active"  && bottomBarController.GetNumberOfOwnedActivePowerups()  >= 5) { powerupPriceButtonColor3.sprite = powerupPriceButtonInactiveSprite; powerupTextPrice3.color = Color.red;   canBuyPowerup3 = false; powerupTextPrice3.text = "Full";              } // cannot buy any more active powerups
        else if (powerupType3 == "passive" && bottomBarController.GetNumberOfOwnedPassivePowerups() >= 4) { powerupPriceButtonColor3.sprite = powerupPriceButtonInactiveSprite; powerupTextPrice3.color = Color.red;   canBuyPowerup3 = false; powerupTextPrice3.text = "Full";              } // cannot buy any more passive powerups
        else if (currGold < powerupPrice3)                                                                { powerupPriceButtonColor3.sprite = powerupPriceButtonInactiveSprite; powerupTextPrice3.color = Color.red;   canBuyPowerup3 = false; powerupTextPrice3.text = "$" + powerupPrice3; } // not enough gold
        else                                                                                              { powerupPriceButtonColor3.sprite = powerupPriceButtonActiveSprite;   powerupTextPrice3.color = Color.white; canBuyPowerup3 = true;  powerupTextPrice3.text = "$" + powerupPrice3; } // can buy this

        if      (powerupType4 == "active"  && bottomBarController.GetNumberOfOwnedActivePowerups()  >= 5) { powerupPriceButtonColor4.sprite = powerupPriceButtonInactiveSprite; powerupTextPrice4.color = Color.red;   canBuyPowerup4 = false; powerupTextPrice4.text = "Full";              } // cannot buy any more active powerups
        else if (powerupType4 == "passive" && bottomBarController.GetNumberOfOwnedPassivePowerups() >= 4) { powerupPriceButtonColor4.sprite = powerupPriceButtonInactiveSprite; powerupTextPrice4.color = Color.red;   canBuyPowerup4 = false; powerupTextPrice4.text = "Full";              } // cannot buy any more passive powerups
        else if (currGold < powerupPrice4)                                                                { powerupPriceButtonColor4.sprite = powerupPriceButtonInactiveSprite; powerupTextPrice4.color = Color.red;   canBuyPowerup4 = false; powerupTextPrice4.text = "$" + powerupPrice4; } // not enough gold
        else                                                                                              { powerupPriceButtonColor4.sprite = powerupPriceButtonActiveSprite;   powerupTextPrice4.color = Color.white; canBuyPowerup4 = true;  powerupTextPrice4.text = "$" + powerupPrice4; } // can buy this
    }

    public void SetupPowerup(float difficultyIndex, string powerupIdentity, ref string powerupType, ref Sprite powerupIcon, ref int powerupPrice, ref TextMeshProUGUI powerupTextPrice, ref TextMeshProUGUI powerupName, ref TextMeshProUGUI powerupStats, ref TextMeshProUGUI powerupDescription, ref TextMeshProUGUI textPurchased)
    {
        var powerupInfo = powerupsCatalogController.GetPowerupInfo(powerupIdentity);
        string typeOfPowerup = powerupInfo.Item2;
        string powerupTitle = powerupInfo.Item3;
        Sprite powerupImage = powerupInfo.Item4;
        string powerupShortDescription = powerupInfo.Item5;
        string powerupLongDescription = powerupInfo.Item6;
        (int easyPrice, int hardPrice) = powerupInfo.Item7;
        (int easyCooldown, int hardCooldown) = powerupInfo.Item8;
        (int easyNumberA, int hardNumberA) = powerupInfo.Item9;
        (int easyNumberB, int hardNumberB) = powerupInfo.Item10;
        (int easyNumberC, int hardNumberC) = powerupInfo.Item11;

        int price = Mathf.RoundToInt(easyPrice + (hardPrice - easyPrice) * difficultyIndex);
        string cooldown = selectedDifficulty == "Adaptive" ? Mathf.Min(easyCooldown,hardCooldown) + " - " + Mathf.Max(easyCooldown,hardCooldown) : Mathf.RoundToInt(easyCooldown + (hardCooldown - easyCooldown) * difficultyIndex).ToString();
        string numberA  = selectedDifficulty == "Adaptive" ? Mathf.Min(easyNumberA,hardNumberA) + " - " + Mathf.Max(easyNumberA,hardNumberA)     : Mathf.RoundToInt(easyNumberA + (hardNumberA - easyNumberA) * difficultyIndex).ToString();
        string numberB  = selectedDifficulty == "Adaptive" ? Mathf.Min(easyNumberB,hardNumberB) + " - " + Mathf.Max(easyNumberB,hardNumberB)     : Mathf.RoundToInt(easyNumberB + (hardNumberB - easyNumberB) * difficultyIndex).ToString();
        string numberC  = selectedDifficulty == "Adaptive" ? Mathf.Min(easyNumberC,hardNumberC) + " - " + Mathf.Max(easyNumberC,hardNumberC)     : Mathf.RoundToInt(easyNumberC + (hardNumberC - easyNumberC) * difficultyIndex).ToString();

        powerupShortDescription = powerupShortDescription.Replace("{7}", cooldown);
        powerupLongDescription = powerupLongDescription.Replace("{8}", numberA).Replace("{9}", numberB).Replace("{10}", numberC);

        powerupType = typeOfPowerup;
        powerupIcon = powerupImage;
        powerupPrice = price;
        powerupTextPrice.text = "$" + powerupPrice;
        powerupName.text = powerupTitle;
        powerupStats.text = powerupShortDescription;
        powerupDescription.text = powerupLongDescription;
        textPurchased.text = powerupsCatalogController.GetDescriptionAfterPurchasingAPowerup(powerupInfo.Item2);
    }

    public void PurchasedPowerup(int powerupNumber) // Called by the price UI button. CANNOT BUY ACTIVE / PASSIVE powerup IF ALREADY FULL
    {
        if (powerupNumber == 1 && canBuyPowerup1)
        {
            musicController.PlayBuySellSoundEffectSource();
            currGold -= powerupPrice1;
            playerAndEnemyStatusController.SetChangeInCurrGold(-1 * powerupPrice1);
            totalGoldSpentSoFar += powerupPrice1;
            groupPowerupSale1.SetActive(false);
            groupTextPurchased1.SetActive(true);
            haveBoughtPowerupThisRound1 = true;
            powerupsCatalogController.RemovePowerupsFromList(powerupIdentity1);
            if (powerupType1 == "active") { bottomBarController.AddNewRecentlyPurchasedActivePowerup(powerupIdentity1); }
            else if (powerupType1 == "passive") { bottomBarController.AddNewRecentlyPurchasedPassivePowerup(powerupIdentity1); }
            else if (powerupType1 == "statBuff") { powerupsCatalogController.ActivateThisStatBuffPoweup(powerupIdentity1); }

        }
        else if (powerupNumber == 2 && canBuyPowerup2)
        {
            musicController.PlayBuySellSoundEffectSource();
            currGold -= powerupPrice2;
            playerAndEnemyStatusController.SetChangeInCurrGold(-1 * powerupPrice2);
            totalGoldSpentSoFar += powerupPrice2;
            groupPowerupSale2.SetActive(false);
            groupTextPurchased2.SetActive(true);
            haveBoughtPowerupThisRound2 = true;
            powerupsCatalogController.RemovePowerupsFromList(powerupIdentity2);
            if (powerupType2 == "active") { bottomBarController.AddNewRecentlyPurchasedActivePowerup(powerupIdentity2); }
            else if (powerupType2 == "passive") { bottomBarController.AddNewRecentlyPurchasedPassivePowerup(powerupIdentity2); }
            else if (powerupType2 == "statBuff") { powerupsCatalogController.ActivateThisStatBuffPoweup(powerupIdentity2); }
        }
        else if (powerupNumber == 3 && canBuyPowerup3)
        {
            musicController.PlayBuySellSoundEffectSource();
            currGold -= powerupPrice3;
            playerAndEnemyStatusController.SetChangeInCurrGold(-1 * powerupPrice3);
            totalGoldSpentSoFar += powerupPrice3;
            groupPowerupSale3.SetActive(false);
            groupTextPurchased3.SetActive(true);
            haveBoughtPowerupThisRound3 = true;
            powerupsCatalogController.RemovePowerupsFromList(powerupIdentity3);
            if (powerupType3 == "active") { bottomBarController.AddNewRecentlyPurchasedActivePowerup(powerupIdentity3); }
            else if (powerupType3 == "passive") { bottomBarController.AddNewRecentlyPurchasedPassivePowerup(powerupIdentity3); }
            else if (powerupType3 == "statBuff") { powerupsCatalogController.ActivateThisStatBuffPoweup(powerupIdentity3); }
        }
        else if (powerupNumber == 4 && canBuyPowerup4)
        {
            musicController.PlayBuySellSoundEffectSource();
            currGold -= powerupPrice4;
            playerAndEnemyStatusController.SetChangeInCurrGold(-1 * powerupPrice4);
            totalGoldSpentSoFar += powerupPrice4;
            groupPowerupSale4.SetActive(false);
            groupTextPurchased4.SetActive(true);
            haveBoughtPowerupThisRound4 = true;
            powerupsCatalogController.RemovePowerupsFromList(powerupIdentity4);
            if (powerupType4 == "active") { bottomBarController.AddNewRecentlyPurchasedActivePowerup(powerupIdentity4); }
            else if (powerupType4 == "passive") { bottomBarController.AddNewRecentlyPurchasedPassivePowerup(powerupIdentity4); }
            else if (powerupType4 == "statBuff") { powerupsCatalogController.ActivateThisStatBuffPoweup(powerupIdentity4); }
        }

        DetermineIfEachPurchaseButtonCanBeClicked();

    }


    // GETTER SETTER

    public int GetTotalGoldSpentSoFar()
    {
        return totalGoldSpentSoFar;
    }

}
