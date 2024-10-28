using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Reflection;

public class BottomBarController : MonoBehaviour
{

    [Header("Active Powerups")]
    [SerializeField] private GameObject groupActivePowerupSlot1;
    [SerializeField] private Image activePowerupIcon1;
    [SerializeField] private GameObject hotKeyPowerup1;
    [SerializeField] private GameObject groupCooldown1;
    [SerializeField] private TextMeshProUGUI textCooldownNumber1;
    [SerializeField] private GameObject groupActiveSell1;
    [SerializeField] private TextMeshProUGUI textActiveSellPrice1;
    [Header("")]
    [SerializeField] private GameObject groupActivePowerupSlot2;
    [SerializeField] private Image activePowerupIcon2;
    [SerializeField] private GameObject hotKeyPowerup2;
    [SerializeField] private GameObject groupCooldown2;
    [SerializeField] private TextMeshProUGUI textCooldownNumber2;
    [SerializeField] private GameObject groupActiveSell2;
    [SerializeField] private TextMeshProUGUI textActiveSellPrice2;
    [Header("")]
    [SerializeField] private GameObject groupActivePowerupSlot3;
    [SerializeField] private Image activePowerupIcon3;
    [SerializeField] private GameObject hotKeyPowerup3;
    [SerializeField] private GameObject groupCooldown3;
    [SerializeField] private TextMeshProUGUI textCooldownNumber3;
    [SerializeField] private GameObject groupActiveSell3;
    [SerializeField] private TextMeshProUGUI textActiveSellPrice3;
    [Header("")]
    [SerializeField] private GameObject groupActivePowerupSlot4;
    [SerializeField] private Image activePowerupIcon4;
    [SerializeField] private GameObject hotKeyPowerup4;
    [SerializeField] private GameObject groupCooldown4;
    [SerializeField] private TextMeshProUGUI textCooldownNumber4;
    [SerializeField] private GameObject groupActiveSell4;
    [SerializeField] private TextMeshProUGUI textActiveSellPrice4;
    [Header("")]
    [SerializeField] private GameObject groupActivePowerupSlot5;
    [SerializeField] private Image activePowerupIcon5;
    [SerializeField] private GameObject hotKeyPowerup5;
    [SerializeField] private GameObject groupCooldown5;
    [SerializeField] private TextMeshProUGUI textCooldownNumber5;
    [SerializeField] private GameObject groupActiveSell5;
    [SerializeField] private TextMeshProUGUI textActiveSellPrice5;

    [Header("Passive Powerups")]
    [SerializeField] private GameObject groupPassivePowerupSlot1;
    [SerializeField] private Image passivePowerupIcon1;
    [SerializeField] private GameObject groupPassiveSell1;
    [SerializeField] private TextMeshProUGUI textPassiveSellPrice1;
    [Header("")]
    [SerializeField] private GameObject groupPassivePowerupSlot2;
    [SerializeField] private Image passivePowerupIcon2;
    [SerializeField] private GameObject groupPassiveSell2;
    [SerializeField] private TextMeshProUGUI textPassiveSellPrice2;
    [Header("")]
    [SerializeField] private GameObject groupPassivePowerupSlot3;
    [SerializeField] private Image passivePowerupIcon3;
    [SerializeField] private GameObject groupPassiveSell3;
    [SerializeField] private TextMeshProUGUI textPassiveSellPrice3;
    [Header("")]
    [SerializeField] private GameObject groupPassivePowerupSlot4;
    [SerializeField] private Image passivePowerupIcon4;
    [SerializeField] private GameObject groupPassiveSell4;
    [SerializeField] private TextMeshProUGUI textPassiveSellPrice4;

    [Header("Controllers")]
    [SerializeField] private MusicController musicController;
    [SerializeField] private ShopController shopController;
    [SerializeField] private BattleModeController battleModeController;
    [SerializeField] private TurnController turnController;

    private List<string> activePowerupsOwned = new List<string>();
    private List<string> passivePowerupsOwned = new List<string>();

    public void OptionToSellPowerups() // While in the shop
    {
        hotKeyPowerup1.SetActive(false);
        groupCooldown1.SetActive(false);
        groupActiveSell1.SetActive(true);
        hotKeyPowerup2.SetActive(false);
        groupCooldown2.SetActive(false);
        groupActiveSell2.SetActive(true);
        hotKeyPowerup3.SetActive(false);
        groupCooldown3.SetActive(false);
        groupActiveSell3.SetActive(true);
        hotKeyPowerup4.SetActive(false);
        groupCooldown4.SetActive(false);
        groupActiveSell4.SetActive(true);
        hotKeyPowerup5.SetActive(false);
        groupCooldown5.SetActive(false);
        groupActiveSell5.SetActive(true);

        groupPassiveSell1.SetActive(true);
        groupPassiveSell2.SetActive(true);
        groupPassiveSell3.SetActive(true);
        groupPassiveSell4.SetActive(true);
    }

    public void PowerupsAreReadyForBattle()
    {
        hotKeyPowerup1.SetActive(true);
        groupCooldown1.SetActive(false);
        groupActiveSell1.SetActive(false);
        hotKeyPowerup2.SetActive(true);
        groupCooldown2.SetActive(false);
        groupActiveSell2.SetActive(false);
        hotKeyPowerup3.SetActive(true);
        groupCooldown3.SetActive(false);
        groupActiveSell3.SetActive(false);
        hotKeyPowerup4.SetActive(true);
        groupCooldown4.SetActive(false);
        groupActiveSell4.SetActive(false);
        hotKeyPowerup5.SetActive(true);
        groupCooldown5.SetActive(false);
        groupActiveSell5.SetActive(false);

        groupPassiveSell1.SetActive(false);
        groupPassiveSell2.SetActive(false);
        groupPassiveSell3.SetActive(false);
        groupPassiveSell4.SetActive(false);
    }

    void Update()
    {
        // Check for key presses and log the appropriate message
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q has been pressed");
            // Activate the 1st active powerup (if it exists, and off-cooldown)
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W has been pressed");
            // Activate the 2nd active powerup (if it exists, and off-cooldown)
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E has been pressed");
            // Activate the 3rd active powerup (if it exists, and off-cooldown)
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R has been pressed");
            // Activate the 4th active powerup (if it exists, and off-cooldown)
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("T has been pressed");
            // Activate the 5th active powerup (if it exists, and off-cooldown)
        }
    }

    // MUST CHANGE THE COLOR OF THE POWERUP FRAMES
    // ACTIVE POWERUPS: MUST BE BLUE WHEN IN COOLDOWN
    // BOTH ACTIVEAND PASSIVE POWEUPS MUST BE BROWN (NOT BLUE) WHEN IN SELLING MODE.

    public int GetNumberOfOwnedActivePowerups()
    {
        return activePowerupsOwned.Count;
    }

    public void AddNewRecentlyPurchasedActivePowerup(string newPowerupIdentity)
    {
        activePowerupsOwned.Add(newPowerupIdentity);
    }

    public void SellOwnedActivePowerup(int index)
    {
        activePowerupsOwned.RemoveAt(index);
        // Add gold back, maybe slightly add powerup usage when buying this
    }

    public void resetActivePowerupDisplayOrder()
    {
        if (activePowerupsOwned.Count >= 1)
        {

        }
        if (activePowerupsOwned.Count >= 2)
        {

        }
        if (activePowerupsOwned.Count >= 3)
        {

        }
        if (activePowerupsOwned.Count >= 4)
        {

        }
        if (activePowerupsOwned.Count >= 5)
        {

        }
    }

    public int GetNumberOfOwnedPassivePowerups()
    {
        return passivePowerupsOwned.Count;
    }

    public void AddNewRecentlyPurchasedPassivePowerup(string newPowerupIdentity)
    {
        passivePowerupsOwned.Add(newPowerupIdentity);
    }

    public void SellOwnedPassivePowerup(int index)
    {
        passivePowerupsOwned.RemoveAt(index);
        // Add gold back
    }


    public void resetPassivePowerupDisplayOrder()
    {
        if (passivePowerupsOwned.Count >= 1)
        {

        }
        if (passivePowerupsOwned.Count >= 2)
        {

        }
        if (passivePowerupsOwned.Count >= 3)
        {

        }
        if (passivePowerupsOwned.Count >= 4)
        {

        }
    }



}
