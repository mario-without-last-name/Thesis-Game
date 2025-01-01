using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Reflection;
using System.Linq;

public class BottomBarController : MonoBehaviour
{

    [Header("Active Powerups")]
    [SerializeField] private GameObject groupActivePowerupSlot1;
    [SerializeField] private Image activePowerupIcon1;
    [SerializeField] private Image activepowerupButtonColor1;
    [SerializeField] private GameObject hotKeyPowerup1;
    [SerializeField] private GameObject groupCooldown1;
    [SerializeField] private TextMeshProUGUI textCooldownNumber1;
    [SerializeField] private GameObject buttonActiveSell1;
    [SerializeField] private TextMeshProUGUI textActiveSellPrice1;
    private int activeCooldown1;
    [Header("")]
    [SerializeField] private GameObject groupActivePowerupSlot2;
    [SerializeField] private Image activePowerupIcon2;
    [SerializeField] private Image activepowerupButtonColor2;
    [SerializeField] private GameObject hotKeyPowerup2;
    [SerializeField] private GameObject groupCooldown2;
    [SerializeField] private TextMeshProUGUI textCooldownNumber2;
    [SerializeField] private GameObject buttonActiveSell2;
    [SerializeField] private TextMeshProUGUI textActiveSellPrice2;
    private int activeCooldown2;
    [Header("")]
    [SerializeField] private GameObject groupActivePowerupSlot3;
    [SerializeField] private Image activePowerupIcon3;
    [SerializeField] private Image activepowerupButtonColor3;
    [SerializeField] private GameObject hotKeyPowerup3;
    [SerializeField] private GameObject groupCooldown3;
    [SerializeField] private TextMeshProUGUI textCooldownNumber3;
    [SerializeField] private GameObject groupActiveSell3;
    [SerializeField] private TextMeshProUGUI textActiveSellPrice3;
    private int activeCooldown3;
    [Header("")]
    [SerializeField] private GameObject groupActivePowerupSlot4;
    [SerializeField] private Image activePowerupIcon4;
    [SerializeField] private Image activepowerupButtonColor4;
    [SerializeField] private GameObject hotKeyPowerup4;
    [SerializeField] private GameObject groupCooldown4;
    [SerializeField] private TextMeshProUGUI textCooldownNumber4;
    [SerializeField] private GameObject buttonActiveSell4;
    [SerializeField] private TextMeshProUGUI textActiveSellPrice4;
    private int activeCooldown4;
    [Header("")]
    [SerializeField] private GameObject groupActivePowerupSlot5;
    [SerializeField] private Image activePowerupIcon5;
    [SerializeField] private Image activepowerupButtonColor5;
    [SerializeField] private GameObject hotKeyPowerup5;
    [SerializeField] private GameObject groupCooldown5;
    [SerializeField] private TextMeshProUGUI textCooldownNumber5;
    [SerializeField] private GameObject buttonActiveSell5;
    [SerializeField] private TextMeshProUGUI textActiveSellPrice5;
    private int activeCooldown5;

    [Header("Passive Powerups")]
    [SerializeField] private GameObject groupPassivePowerupSlot1;
    [SerializeField] private Image passivePowerupIcon1;
    [SerializeField] private Image passivepowerupButtonColor1;
    [SerializeField] private GameObject buttonPassiveSell1;
    [SerializeField] private TextMeshProUGUI textPassiveSellPrice1;
    [Header("")]
    [SerializeField] private GameObject groupPassivePowerupSlot2;
    [SerializeField] private Image passivePowerupIcon2;
    [SerializeField] private Image passivepowerupButtonColor2;
    [SerializeField] private GameObject buttonPassiveSell2;
    [SerializeField] private TextMeshProUGUI textPassiveSellPrice2;
    [Header("")]
    [SerializeField] private GameObject groupPassivePowerupSlot3;
    [SerializeField] private Image passivePowerupIcon3;
    [SerializeField] private Image passivepowerupButtonColor3;
    [SerializeField] private GameObject buttonPassiveSell3;
    [SerializeField] private TextMeshProUGUI textPassiveSellPrice3;
    [Header("")]
    [SerializeField] private GameObject groupPassivePowerupSlot4;
    [SerializeField] private Image passivePowerupIcon4;
    [SerializeField] private Image passivepowerupButtonColor4;
    [SerializeField] private GameObject buttonPassiveSell4;
    [SerializeField] private TextMeshProUGUI textPassiveSellPrice4;

    [Header("Button Images")]
    [SerializeField] private Sprite powerupButtonActiveSprite;
    [SerializeField] private Sprite powerupButtonInactiveSprite;

    [Header("Controllers")]
    [SerializeField] private MusicController musicController;
    [SerializeField] private ShopController shopController;
    [SerializeField] private BattleModeController battleModeController;
    [SerializeField] private TurnController turnController;
    [SerializeField] private PowerupsCatalogController powerupsCatalogController;
    [SerializeField] private DynamicDifficultyController dynamicDifficultyController;
    [SerializeField] private PlayerAndEnemyStatusController playerAndEnemyStatusController;

    private List<string> activePowerupsOwned = new List<string>();
    private List<string> passivePowerupsOwned = new List<string>();
    private int powerupSellingPrice;

    public void OptionToSellPowerups() // Called by BattleModeController after the victory screen
    {
        // MAKE THE SELLING PRICE UPDATE ON ITS OWN PER PURCHASE/SELL
        powerupSellingPrice = Mathf.RoundToInt(Mathf.Lerp(1f, 7f, dynamicDifficultyController.GetDynamicOutput("powerupQuality")));

        hotKeyPowerup1.SetActive(false);
        groupCooldown1.SetActive(false);
        buttonActiveSell1.SetActive(true);
        activepowerupButtonColor1.sprite = powerupButtonActiveSprite;
        textActiveSellPrice1.text = "$" + powerupSellingPrice;
        hotKeyPowerup2.SetActive(false);
        groupCooldown2.SetActive(false);
        buttonActiveSell2.SetActive(true);
        activepowerupButtonColor2.sprite = powerupButtonActiveSprite;
        textActiveSellPrice2.text = "$" + powerupSellingPrice;
        hotKeyPowerup3.SetActive(false);
        groupCooldown3.SetActive(false);
        groupActiveSell3.SetActive(true);
        activepowerupButtonColor3.sprite = powerupButtonActiveSprite;
        textActiveSellPrice3.text = "$" + powerupSellingPrice;
        hotKeyPowerup4.SetActive(false);
        groupCooldown4.SetActive(false);
        buttonActiveSell4.SetActive(true);
        activepowerupButtonColor4.sprite = powerupButtonActiveSprite;
        textActiveSellPrice4.text = "$" + powerupSellingPrice;
        hotKeyPowerup5.SetActive(false);
        groupCooldown5.SetActive(false);
        buttonActiveSell5.SetActive(true);
        activepowerupButtonColor5.sprite = powerupButtonActiveSprite;
        textActiveSellPrice5.text = "$" + powerupSellingPrice;

        buttonPassiveSell1.SetActive(true);
        passivepowerupButtonColor1.sprite = powerupButtonActiveSprite;
        textPassiveSellPrice1.text = "$" + powerupSellingPrice;
        buttonPassiveSell2.SetActive(true);
        passivepowerupButtonColor2.sprite = powerupButtonActiveSprite;
        textPassiveSellPrice2.text = "$" + powerupSellingPrice;
        buttonPassiveSell3.SetActive(true);
        passivepowerupButtonColor3.sprite = powerupButtonActiveSprite;
        textPassiveSellPrice3.text = "$" + powerupSellingPrice;
        buttonPassiveSell4.SetActive(true);
        passivepowerupButtonColor4.sprite = powerupButtonActiveSprite;
        textPassiveSellPrice4.text = "$" + powerupSellingPrice;
    }

    public void PowerupsAreReadyForBattle() // Called by BattleModeController during the get ready screen
    {
        activeCooldown1 = 0;
        hotKeyPowerup1.SetActive(true);
        groupCooldown1.SetActive(false);
        activepowerupButtonColor1.sprite = powerupButtonActiveSprite;
        buttonActiveSell1.SetActive(false);
        activeCooldown2 = 0;
        hotKeyPowerup2.SetActive(true);
        groupCooldown2.SetActive(false);
        activepowerupButtonColor2.sprite = powerupButtonActiveSprite;
        buttonActiveSell2.SetActive(false);
        activeCooldown3 = 0;
        hotKeyPowerup3.SetActive(true);
        groupCooldown3.SetActive(false);
        activepowerupButtonColor3.sprite = powerupButtonActiveSprite;
        groupActiveSell3.SetActive(false);
        activeCooldown4 = 0;
        hotKeyPowerup4.SetActive(true);
        groupCooldown4.SetActive(false);
        activepowerupButtonColor4.sprite = powerupButtonActiveSprite;
        buttonActiveSell4.SetActive(false);
        activeCooldown5 = 0;
        hotKeyPowerup5.SetActive(true);
        groupCooldown5.SetActive(false);
        activepowerupButtonColor5.sprite = powerupButtonActiveSprite;
        buttonActiveSell5.SetActive(false);

        buttonPassiveSell1.SetActive(false);
        passivepowerupButtonColor1.sprite = powerupButtonInactiveSprite;
        buttonPassiveSell2.SetActive(false);
        passivepowerupButtonColor2.sprite = powerupButtonInactiveSprite;
        buttonPassiveSell3.SetActive(false);
        passivepowerupButtonColor3.sprite = powerupButtonInactiveSprite;
        buttonPassiveSell4.SetActive(false);
        passivepowerupButtonColor4.sprite = powerupButtonInactiveSprite;
    }

    private void Start()
    {
        UpdateActivePowerupDisplayOrder();
        UpdatePassivePowerupDisplayOrder();
    }

    void Update() // Players can type on their keyboard to activate an active powerup ...
    {
        if (Input.GetKeyDown(KeyCode.Q)) { ActivePowerupWasClickedOrTyped(1); }
        if (Input.GetKeyDown(KeyCode.W)) { ActivePowerupWasClickedOrTyped(2); }
        if (Input.GetKeyDown(KeyCode.E)) { ActivePowerupWasClickedOrTyped(3); }
        if (Input.GetKeyDown(KeyCode.R)) { ActivePowerupWasClickedOrTyped(4); }
        if (Input.GetKeyDown(KeyCode.T)) { ActivePowerupWasClickedOrTyped(5); }
    }

    public void ActivePowerupWasClickedOrTyped(int activePowerupIndex) // ... or players can just click the button (SO IT'S CALLED BY CLICKING THE ACTIVE POWERUP BUTTONS)
    {
        // You cannot activate a powerup if not in fight mode and it is not the player's turn.
        if (battleModeController.GetCurrentBattleMode() != "Fight" || !turnController.getCanPlayerSelectActivePowerup() ) { return; }

        // POWERUPS ARE OFF COOLDOWN, AND ONLY WHEN WE ARE IN BATTLE MODE (Player's turn), AND NO OTHER ACTIVE POWERUPS ARE SELECTED (like the ones where you must click another tile to attack)
        if      (activePowerupIndex == 1 && activePowerupsOwned.Count >= 1 && activeCooldown1 <= 0)
        {
            musicController.PlayClickSoundEffect();
            //Debug.Log("Activate active powerup 1:  " + activePowerupsOwned[0]);
            activeCooldown1 = powerupsCatalogController.ActivateThisActivePowerup(activePowerupsOwned[0], "cooldown");
            textCooldownNumber1.text = "" + activeCooldown1;
            groupCooldown1.SetActive(true);
            ActivePowerupsCannotBeClickedNow();
            turnController.PlayerTurnButActivePowerupWasActivated(activePowerupsOwned[0]);
        }
        else if (activePowerupIndex == 2 && activePowerupsOwned.Count >= 2 && activeCooldown2 <= 0)
        {
            musicController.PlayClickSoundEffect();
            //Debug.Log("Activate active powerup 2:  " + activePowerupsOwned[1]);
            activeCooldown2 = powerupsCatalogController.ActivateThisActivePowerup(activePowerupsOwned[1], "cooldown");
            textCooldownNumber2.text = "" + activeCooldown2;
            groupCooldown2.SetActive(true);
            ActivePowerupsCannotBeClickedNow();
            turnController.PlayerTurnButActivePowerupWasActivated(activePowerupsOwned[1]);
        }
        else if (activePowerupIndex == 3 && activePowerupsOwned.Count >= 3 && activeCooldown3 <= 0)
        {
            musicController.PlayClickSoundEffect();
            //Debug.Log("Activate active powerup 3:  " + activePowerupsOwned[2]);
            activeCooldown3 = powerupsCatalogController.ActivateThisActivePowerup(activePowerupsOwned[2], "cooldown");
            textCooldownNumber3.text = "" + activeCooldown3;
            groupCooldown3.SetActive(true);
            ActivePowerupsCannotBeClickedNow();
            turnController.PlayerTurnButActivePowerupWasActivated(activePowerupsOwned[2]);
        }
        else if (activePowerupIndex == 4 && activePowerupsOwned.Count >= 4 && activeCooldown4 <= 0)
        {
            musicController.PlayClickSoundEffect();
            //Debug.Log("Activate active powerup 4:  " + activePowerupsOwned[3]);
            activeCooldown4 = powerupsCatalogController.ActivateThisActivePowerup(activePowerupsOwned[3], "cooldown");
            textCooldownNumber4.text = "" + activeCooldown4;
            groupCooldown4.SetActive(true);
            ActivePowerupsCannotBeClickedNow();
            turnController.PlayerTurnButActivePowerupWasActivated(activePowerupsOwned[3]);
        }
        else if (activePowerupIndex == 5 && activePowerupsOwned.Count >= 5 && activeCooldown5 <= 0)
        {
            musicController.PlayClickSoundEffect();
            //Debug.Log("Activate active powerup 5:  " + activePowerupsOwned[4]);
            activeCooldown5 = powerupsCatalogController.ActivateThisActivePowerup(activePowerupsOwned[4], "cooldown");
            textCooldownNumber5.text = "" + activeCooldown5;
            groupCooldown5.SetActive(true);
            ActivePowerupsCannotBeClickedNow();
            turnController.PlayerTurnButActivePowerupWasActivated(activePowerupsOwned[4]);
        }
    }

    public void ActivePowerupsReduceCooldownBy1AndMaybeCanBeClickedNow() // Called by TurnController
    {
        activeCooldown1 -= 1;
        textCooldownNumber1.text = "" + activeCooldown1;
        if (activeCooldown1 <= 0)
        {
            hotKeyPowerup1.SetActive(true);
            groupCooldown1.SetActive(false);
            activepowerupButtonColor1.sprite = powerupButtonActiveSprite;
        }

        activeCooldown2 -= 1;
        textCooldownNumber2.text = "" + activeCooldown2;
        if (activeCooldown2 <= 0)
        {
            hotKeyPowerup2.SetActive(true);
            groupCooldown2.SetActive(false);
            activepowerupButtonColor2.sprite = powerupButtonActiveSprite;
        }

        activeCooldown3 -= 1;
        textCooldownNumber3.text = "" + activeCooldown3;
        if (activeCooldown3 <= 0)
        {
            hotKeyPowerup3.SetActive(true);
            groupCooldown3.SetActive(false);
            activepowerupButtonColor3.sprite = powerupButtonActiveSprite;
        }

        activeCooldown4 -= 1;
        textCooldownNumber4.text = "" + activeCooldown4;
        if (activeCooldown4 <= 0)
        {
            hotKeyPowerup4.SetActive(true);
            groupCooldown4.SetActive(false);
            activepowerupButtonColor4.sprite = powerupButtonActiveSprite;
        }

        activeCooldown5 -= 1;
        textCooldownNumber5.text = "" + activeCooldown5;
        if (activeCooldown5 <= 0)
        {
            hotKeyPowerup5.SetActive(true);
            groupCooldown5.SetActive(false);
            activepowerupButtonColor5.sprite = powerupButtonActiveSprite;
        }
    }

    public void MakeActivePowerupSquaresBlueTemporarilyBecauseItsNotPlayerTurn() // called by turn controller
    {
        activepowerupButtonColor1.sprite = powerupButtonInactiveSprite;
        activepowerupButtonColor2.sprite = powerupButtonInactiveSprite;
        activepowerupButtonColor3.sprite = powerupButtonInactiveSprite;
        activepowerupButtonColor4.sprite = powerupButtonInactiveSprite;
        activepowerupButtonColor5.sprite = powerupButtonInactiveSprite;
    }

    public void ActivePowerupsCannotBeClickedNow()
    {
        hotKeyPowerup1.SetActive(false);
        activepowerupButtonColor1.sprite = powerupButtonInactiveSprite;
        hotKeyPowerup2.SetActive(false);
        activepowerupButtonColor2.sprite = powerupButtonInactiveSprite;
        hotKeyPowerup3.SetActive(false);
        activepowerupButtonColor3.sprite = powerupButtonInactiveSprite;
        hotKeyPowerup4.SetActive(false);
        activepowerupButtonColor4.sprite = powerupButtonInactiveSprite;
        hotKeyPowerup5.SetActive(false);
        activepowerupButtonColor5.sprite = powerupButtonInactiveSprite;
    }


    public void SellOwnedActivePowerup(int index) // called by the sell powerup buttons
    {
        powerupsCatalogController.AddPowerupsToList(activePowerupsOwned[index]);
        activePowerupsOwned.RemoveAt(index);
        playerAndEnemyStatusController.SetChangeInCurrGold(powerupSellingPrice);
        dynamicDifficultyController.SetDynamicInputChange("powerupUsage", +0.025f, false); // sell powerup
        musicController.PlayBuySellSoundEffectSource();
        UpdateActivePowerupDisplayOrder();
        shopController.DetermineIfEachPurchaseButtonCanBeClicked();
    }

    public void UpdateActivePowerupDisplayOrder()
    {
        groupActivePowerupSlot1.SetActive(false);
        groupActivePowerupSlot2.SetActive(false);
        groupActivePowerupSlot3.SetActive(false);
        groupActivePowerupSlot4.SetActive(false);
        groupActivePowerupSlot5.SetActive(false);

        if (activePowerupsOwned.Count >= 1)
        {
            groupActivePowerupSlot1.SetActive(true);
            activepowerupButtonColor1.sprite = powerupButtonActiveSprite;
            activePowerupIcon1.sprite = powerupsCatalogController.GetPowerupSprite(activePowerupsOwned[0]);
        }
        if (activePowerupsOwned.Count >= 2)
        {
            groupActivePowerupSlot2.SetActive(true);
            activepowerupButtonColor2.sprite = powerupButtonActiveSprite;
            activePowerupIcon2.sprite = powerupsCatalogController.GetPowerupSprite(activePowerupsOwned[1]);
        }
        if (activePowerupsOwned.Count >= 3)
        {
            groupActivePowerupSlot3.SetActive(true);
            activepowerupButtonColor3.sprite = powerupButtonActiveSprite;
            activePowerupIcon3.sprite = powerupsCatalogController.GetPowerupSprite(activePowerupsOwned[2]);
        }
        if (activePowerupsOwned.Count >= 4)
        {
            groupActivePowerupSlot4.SetActive(true);
            activepowerupButtonColor4.sprite = powerupButtonActiveSprite;
            activePowerupIcon4.sprite = powerupsCatalogController.GetPowerupSprite(activePowerupsOwned[3]);
        }
        if (activePowerupsOwned.Count >= 5)
        {
            groupActivePowerupSlot5.SetActive(true);
            activepowerupButtonColor5.sprite = powerupButtonActiveSprite;
            activePowerupIcon5.sprite = powerupsCatalogController.GetPowerupSprite(activePowerupsOwned[4]);
        }
    }


    public void AddNewRecentlyPurchasedPassivePowerup(string newPowerupIdentity)
    {
        if (newPowerupIdentity == "passive-lightArmor" || newPowerupIdentity == "passive-heavyArmor" || newPowerupIdentity == "passive-diamondArmor" || newPowerupIdentity == "passive-innerHealing" || newPowerupIdentity == "passive-vampiric" || newPowerupIdentity == "passive-wellDeservedRest" || newPowerupIdentity == "passive-bloodlust" || newPowerupIdentity == "passive-mercenaryTools" || newPowerupIdentity == "passive-pickpocket")
        { dynamicDifficultyController.SetDynamicInputChange("powerupUsage", +0.1f, false); }
        passivePowerupsOwned.Add(newPowerupIdentity);
        UpdatePassivePowerupDisplayOrder();
    }

    public void SellOwnedPassivePowerup(int index) // called by the sell powerup buttons
    {
        powerupsCatalogController.AddPowerupsToList(passivePowerupsOwned[index]);
        passivePowerupsOwned.RemoveAt(index);
        playerAndEnemyStatusController.SetChangeInCurrGold(powerupSellingPrice);
        dynamicDifficultyController.SetDynamicInputChange("powerupUsage", +0.025f, false); // sell powerup
        musicController.PlayBuySellSoundEffectSource();
        UpdatePassivePowerupDisplayOrder();
        shopController.DetermineIfEachPurchaseButtonCanBeClicked();
    }


    public void UpdatePassivePowerupDisplayOrder()
    {
        groupPassivePowerupSlot1.SetActive(false);
        groupPassivePowerupSlot2.SetActive(false);
        groupPassivePowerupSlot3.SetActive(false);
        groupPassivePowerupSlot4.SetActive(false);

        if (passivePowerupsOwned.Count >= 1)
        {
            groupPassivePowerupSlot1.SetActive(true);
            passivepowerupButtonColor1.sprite = powerupButtonActiveSprite;
            passivePowerupIcon1.sprite = powerupsCatalogController.GetPowerupSprite(passivePowerupsOwned[0]);
        }
        if (passivePowerupsOwned.Count >= 2)
        {
            groupPassivePowerupSlot2.SetActive(true);
            passivepowerupButtonColor2.sprite = powerupButtonActiveSprite;
            passivePowerupIcon2.sprite = powerupsCatalogController.GetPowerupSprite(passivePowerupsOwned[1]);
        }
        if (passivePowerupsOwned.Count >= 3)
        {
            groupPassivePowerupSlot3.SetActive(true);
            passivepowerupButtonColor3.sprite = powerupButtonActiveSprite;
            passivePowerupIcon3.sprite = powerupsCatalogController.GetPowerupSprite(passivePowerupsOwned[2]);
        }
        if (passivePowerupsOwned.Count >= 4)
        {
            groupPassivePowerupSlot4.SetActive(true);
            passivepowerupButtonColor4.sprite = powerupButtonActiveSprite;
            passivePowerupIcon4.sprite = powerupsCatalogController.GetPowerupSprite(passivePowerupsOwned[3]);
        }
    }

    // ====================================================
    // GETTER, SETTER

    public int GetNumberOfOwnedActivePowerups()
    {
        return activePowerupsOwned.Count;
    }

    public bool CheckIfThisActivePowerUpIsOwned(string powerupIdentity)
    {
        return activePowerupsOwned.Contains(powerupIdentity);
    }

    public void AddNewRecentlyPurchasedActivePowerup(string newPowerupIdentity)
    {
        activePowerupsOwned.Add(newPowerupIdentity);
        UpdateActivePowerupDisplayOrder();
    }

    public int GetNumberOfOwnedPassivePowerups()
    {
        return passivePowerupsOwned.Count;
    }

    public bool CheckIfThisPassivePowerUpIsOwned(string powerupIdentity)
    {
        return passivePowerupsOwned.Contains(powerupIdentity);
    }

    public int GetPowerupSellPrice()
    {
        return powerupSellingPrice;
    }

    public bool GetIsAnyActivePowerupOffCooldown()
    {
        return (activePowerupsOwned.Count >= 1 && activeCooldown1 <= 0) || (activePowerupsOwned.Count >= 2 && activeCooldown2 <= 0) || (activePowerupsOwned.Count >= 3 && activeCooldown3 <= 0) || (activePowerupsOwned.Count >= 4 && activeCooldown4 <= 0) || (activePowerupsOwned.Count >= 5 && activeCooldown5 <= 0);
    }


}
