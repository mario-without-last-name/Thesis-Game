using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupsCatalogController : MonoBehaviour
{

    //[Header("Objects")]
    //[SerializeField] private Image imageThrowingHatchet;
    //[SerializeField] private Image imageThrowingBomb;
    //[SerializeField] private Image imageThrowingFireball;
    //[SerializeField] private Image imageKnife;
    //[SerializeField] private Image imageSpear;
    //[SerializeField] private Image imageAxe;
    //[SerializeField] private Image imageSpikedClub;
    //[SerializeField] private Image imageSniper;
    //[SerializeField] private Image imageTeleport;
    //[SerializeField] private Image imageDodge;
    //[SerializeField] private Image imageAcidRain;
    //[SerializeField] private Image imageThrowingHatchet;
    //[SerializeField] private Image imageThrowingHatchet;
    //[SerializeField] private Image imageThrowingHatchet;
    //[SerializeField] private Image imageThrowingHatchet;
    //[SerializeField] private Image imageThrowingHatchet;
    //[SerializeField] private Image imageThrowingHatchet;
    //[SerializeField] private Image imageThrowingHatchet;
    //[SerializeField] private Image imageThrowingHatchet;
    //[SerializeField] private Image imageThrowingHatchet;
    //[SerializeField] private Image imageThrowingHatchet;

    /*    
    Powerup Ideas:

    ACTIVE [Up to 5 and can be sold, press 1/2/3/4/5 key pads or click on them, can only use 1 per tur, each has a cooldown, clicking on them resets the move timer]

    [$4-5] [?-? cd] Throwing Hatchet: Choose any enemy and deal ? damage
    [$?-?] [?-? cd] Throwing Bomb: Choose any tile. All pieces within 3x3 area of it take ? damage
    [$?-?] [?-? cd] Throwing Fireball: Choose any tile. All pieces within 5x5 area of it take ? damage
    [$?-?] [?-? cd] Knife: Choose an enemy adjacent to you, dealing ? damage
    [$?-?] [?-? cd] Spear: Choose an enemy up to 2 tiles adjacent to you, dealing ? damage
    [$?-?] [?-? cd] Axe: Deal ? damage to all enemies adjacent to you
    [$?-?] [?-? cd] Spiked Club: Deal ? damage to all enemies up to 2 tiles adjacent to you
    [$?-?] [?-? cd] Sniper: Click a tile further than 3 tiles from you, that enemy takes ? damage
    [$?-?] [?-? cd] Teleport: Go to any unoccupied tile
    [$?-?] [?-? cd] Dodge: Take no damage this turn
    [$?-?] [?-? cd] Acid Rain: Deal ? damage to all enemies

    One Time [Single Use]

    [$?-?] Small snack: heal 5-15 health
    [$?-?] Tavern Buffet: Heal 25-50 health
    [$?-?] Battlecry: Increase all of your damage by ? For the next round

    PASSIVE [Up to 4 and can be sold]

    [$?-?] Light Armor: Take 1-3 less damage [min 1] from any source 
    [$?-?] Heavy Armor: -? damage. Take 4-7 less damage [min 1] from any source
    [$?-?] Ground Pond: After moving, all adjacent enemies take ? damage
    [$?-?] Posionous Aura: all enemies take {1-2} damage per turn
    [$?-?] Riposte: Enemies who damage you will take ??? damage in return
    [$?-?] Inner Healing: Heal ? Health every ? turns
    [$?-?] Vampiric: Heal ? health for every enemy killed
    [$?-?] Bloodlust: Gain +1 damage per damage taken. Resets per round
    [$?-?] Mercenary Tools: Lose $1 per turn, Doubles damage dealt, Halves damage taken
    [$?-?] Nimble Feet: You can move 2 tiles up/down/left/right per turn


    Stat Buff [Permanent Buffs]

    [$?-?] Hardened Fists: Increases your landing damage by ?
    [$?-?] Iron Fists: Increases your landing damage by ?
    [$?-?] Toughened Heart: Increases your max health by ?
    [$?-?] Unbreakable Heart: Increases your max health by ?
    [$?-?] Weapon Proficiency: Increases all of your damage by ?
    [$?-?] 1 More Chance: when health reaches 0, heal ? more hp
    [$?-?] Pickpocket: Gain $? more per enemy killed
    */



    public void ActivateThisInstantPoweup(string powerupName)
    {

    }

    //public (string, string, string, int, int, Image) GetPowerupInfo(string powerupIdentity)
    //{ // Name, Short Description, Long Description, LowCost, HighCost, 
    //    if (powerupIdentity == "active-ThrowingHatchet")
    //    {
    //        return ("ThrowingHatchet", "???????", "Choose any enemy and deal ? damage", 1, 2);
    //    }
    //    else
    //    {
    //        return ("???", "???", "???", 999, 999);
    //    }
    //}

    // FIRST, SET UP ALL ICONS FOR POWERUPS, JUST SET THEM ALL INTO VARIABLES
    // SET THEIR IDS (that includes their powerup type codename), and all data
    // SET the text to be displayed depending on the ID. some values however (like price and number strength), set the upper and lower bounds, juts like how you controlled the enemy stats with DGB difficulty index. How many per powerup? could be varietive. Active powerups have a number for cooldown, the rest do not.
    // MANAGE IT IN BOTTOM BAR CONTROLLER.
    // PERPARE A FUNCTION TO ACTIVATE AN ACTIVE POWERUP AVAILABLE IN BOTTOM BAR CONTROLLER IF CALLED.
    // AT DIFFERENT TIMES IN A BATTLE (ex: round start, taking damage, dealing damage, it's the players turn, etc): CALL THIS CONTROLLER (AFTER BOTTOM BAR) to see if a powerup must activate at this time.
    // LATER CAN SELL POWERUPS IN SHOP

    public string GetDescriptionAfterPurchasingAPowerup(string powerupType)
    {
        if (powerupType == "active")
        {
            return "<line-height=100%>-- Purchased --\r\n<size=20><line-height=130%>Active abilities appear at the bottom-center of the screen (maximum: 5). Click on them or press their keyboard letter [Q/W/E/R/T] to activate when off-cooldown.";
        }
        else if (powerupType == "passive")
        {
            return "<line-height=100%>-- Purchased --\r\n<size=20><line-height=130%>Passive abilities appear at the bottom-right of the screen (maximum: 4). You do not have to click on them to activate.";
        }
        else if (powerupType == "singleUse")
        {
            return "<line-height=100%>-- Purchased --\r\n<size=20><line-height=130%>Single-use shop items only take effect once. They do not take up an ability slot.";
        }
        else if (powerupType == "statBuffs")
        {
            return "<line-height=100%>-- Purchased --\r\n<size=20><line-height=130%>Stat-buffs are permanent. They do not take up an ability slot.";
        }
        else
        {
            return "Warning: unkown powerup type: " + powerupType;
        }
    }
}
