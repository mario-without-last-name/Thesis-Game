using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UIElements;

public class PowerupsCatalogController : MonoBehaviour
{

    [Header("Powerup Images")]
    [Header("Active")]
    [SerializeField] private Sprite imageKnife;
    [SerializeField] private Sprite imageSpear;
    [SerializeField] private Sprite imageHatchet;
    [SerializeField] private Sprite imageSlingshot;
    [SerializeField] private Sprite imageSniper;
    [SerializeField] private Sprite imageLightningBolt;
    [SerializeField] private Sprite imageBomb;
    [SerializeField] private Sprite imageFireball;
    [SerializeField] private Sprite imageArrowVolley;
    [SerializeField] private Sprite imageAcidRain;
    [SerializeField] private Sprite imageAxe;
    [SerializeField] private Sprite imageSpikedClub;
    [SerializeField] private Sprite imageWhip;
    [SerializeField] private Sprite imageTeleport;
    [SerializeField] private Sprite imageDodge;
    [Header("Passive")]
    [SerializeField] private Sprite imageLightArmor;
    [SerializeField] private Sprite imageHeavyArmor;
    [SerializeField] private Sprite imageDiamondArmor;
    [SerializeField] private Sprite imageGroundPound;
    [SerializeField] private Sprite imageInnerHealing;
    [SerializeField] private Sprite imageVampiric;
    [SerializeField] private Sprite imageWellDeservedRest;
    [SerializeField] private Sprite imageBloodlust;
    [SerializeField] private Sprite imageMercenaryTools;
    [SerializeField] private Sprite imagePickpocket;
    [SerializeField] private Sprite imageFlexibleMovementI;
    [SerializeField] private Sprite imageFlexibleMovementII;
    [SerializeField] private Sprite imageFlexibleMovementIII;
    [SerializeField] private Sprite imageFlexibleMovementIV;
    [SerializeField] private Sprite imageFlexibleMovementV;
    [SerializeField] private Sprite imageFlexibleMovementVI;
    [SerializeField] private Sprite imageFlexibleMovementVII;
    [Header("Stat Buff")]
    [SerializeField] private Sprite imageSmallSnack;
    [SerializeField] private Sprite imageLunchBreak;
    [SerializeField] private Sprite imageTavernBuffet;
    [SerializeField] private Sprite imageHardenedFists;
    [SerializeField] private Sprite imageIronFists;
    [SerializeField] private Sprite imageDiamondFists;
    [SerializeField] private Sprite imageToughenedHeart;
    [SerializeField] private Sprite imageUnbreakableHeart;
    [SerializeField] private Sprite imageWeaponProficiency;
    [SerializeField] private Sprite imageWeaponMastery;
    [Header("Missing")]
    [SerializeField] private Sprite missing;
    [Header("")]
    [Header("Controllers")]
    [SerializeField] private BottomBarController bottomBarController;
    [SerializeField] private PlayerAndEnemyStatusController playerAndEnemyStatusController;
    [SerializeField] private DynamicDifficultyController dynamicDifficultyController;

    private List<string> allPowerupIdentitiesList;
    private string selectedDifficulty;

    /*    
    Powerup Ideas:






    Stat Buff [Permanent Buffs]

    [$?-?] Small Snack: heal 5-12 health
    [$?-?] Lunch Break: Heal 15-30 health
    [$?-?] Tavern Buffet: Heal to full health
    [$?-?] Battlecry: Increase all of your damage by ? For the next round
    [$?-?] Hardened Fists: Permanently Increases your direct contact damage by 2-4
    [$?-?] Iron Fists: Permanently Increases your direct contact damage by 4-7
    [$?-?] Diamond Fists: Increases your direct contact damage by 7-10
    [$?-?] Toughened Heart: Permanently increases your max health by 4-8 And heal 4-8 health
    [$?-?] Unbreakable Heart: Permanently increases your max health by 8-15 And heal 8-15 health
    [$?-?] Weapon Proficiency: Permanently increase all of your damage by 1-3
    [$?-?] 1 More Chance: when health reaches 0, heal ? more hp
    [$?-?] Pickpocket: Gain $1-3 more per enemy killed
    */

    private void Start()
    {
        selectedDifficulty = PlayerPrefs.GetString("modeDifficulty", "???"); // CHANGE THIS, MUST NOT START WITH CAPITAL LETTER
        allPowerupIdentitiesList = new List<string> {
            // All active powerups
            //"active-knife", "active-spear", "active-hatchet", "active-slingshot", "active-sniper",
            //"active-lightningBolt", "active-bomb", "active-fireball", "active-arrowVolley",
            //"active-acidRain", "active-axe", "active-spikedClub", "active-whip",
            //"active-eleport", "active-dodge",

            // All passive powerups
            //"passive-lightArmor", "passive-heavyArmor", "passive-groundPond", "passive-innerHealing",
            //"passive-vampiric", "passive-wellDeservedRest", "passive-bloodlust",
            //"passive-mercenaryTools", "passive-pickpocket", "passive-flexibleMovementI", "passive-flexibleMovementII",
            //"passive-flexibleMovementIII", "passive-flexibleMovementIV", "passive-flexibleMovementV",
            //"passive-flexibleMovementVI", "passive-flexibleMovementVII",

            // All stat buff powerups
            "statBuff-smallSnack", "statBuff-lunchBreak", "statBuff-tavernBuffet",
            "statBuff-hardenedFists", "statBuff-ironFists", "statBuff-diamondFists",
            "statBuff-toughenedHeart", "statBuff-unbreakableHeart",
            "statBuff-weaponProficiency", "statBuff-weaponMastery"
        };
        RandomizePowerupsList();
    }

    public void RandomizePowerupsList()
    {
        var randomizedList = allPowerupIdentitiesList.OrderBy(x => UnityEngine.Random.value).ToList();
        for (int i = 0; i < randomizedList.Count; i++)
        {
            allPowerupIdentitiesList[i] = randomizedList[i];
        }
    }

    public List<string> GetFirstFourPowerups()
    {
        RandomizePowerupsList();
        // Ensure there are at least 4 elements, or return fewer if the list has less than 4.
        int count = Mathf.Min(4, allPowerupIdentitiesList.Count);
        return allPowerupIdentitiesList.Take(count).ToList();
    }

    public void RemovePowerupsFromList(string powerup) // When Equipped by player. Also Increase DGB input for powerup usage a bit? Reduce per round end
    {
        allPowerupIdentitiesList.Remove(powerup);
    }

    public void AddPowerupsToList(string powerup) // When sold by player. Also Increase DGB input for powerup usage a bit?  Reduce per round end
    {
        allPowerupIdentitiesList.Add(powerup);
    }

    public (string,string, string, Sprite, string, string, (int, int), (int, int), (int, int), (int, int), (int, int)) GetPowerupInfo(string powerupIdentity)
    { // Powerup Identity, Name, Image, Short Description, Long Description, Easy / Hard Price, Easy / Hard Cooldown, Easy / Hard Number1(health or damage), Easy / Hard Number2(health or damage), Easy / Hard Number3(health or damage) 

        // ACTIVE POWERUPS ================================================================
        // [Up to 5 and can be sold, press 1 / 2 / 3 / 4 / 5 key pads or click on them, can only use 1 per tur, each has a cooldown, clicking on them resets the move timer]
        // ================================================================================
        // [$3-7]   [2-4 cd]  Knife: Attack an enemy 1 tile adjacent to you, dealing 5-3 damage
        // [$6-12]  [3-5 cd]  Spear: Attack an enemy up to 2 tiles adjacent to you, dealing 8-5 damage
        // [$5-10]  [4-6 cd]  Hatchet: Attack an enemy up to 3 tiles adjacent to you, dealing 7-4 damage
        // [$8-15]  [5-7 cd]  Slingshot: Click a tile further than 3 tiles from you, that enemy takes 6-3 damage
        // [$12-18] [6-8 cd]  Sniper: Click a tile further than 3 tiles from you, that enemy takes 10-7 damage
        // [$10-16] [6-8 cd]  Lightning Bolt: Click a tile further than 3 tiles from you, that enemy takes 12-8 damage
        // [$15-22] [7-9 cd]  Bomb: Click a tile further than 3 tiles from you. All pieces within 3x3 area of it take 6-4 damage
        // [$18-25] [8-10 cd] Fireball: Click a tile further than 3 tiles from you. All pieces within 5x5 area of it take 10-7 damage
        // [$20-28] [9-11 cd] Arrow Volley: Click a tile further than 3 tiles from you. All pieces within 7x7 area of it take 8-5 damage
        // [$14-20] [7-9 cd]  Acid Rain: Deal 6-4 damage to all enemies
        // [$7-12]  [5-7 cd]  Axe: Deal 8-5 damage to all enemies 1 tile adjacent to you
        // [$9-14]  [6-8 cd]  Spiked Club: Deal 10-6 damage to all enemies up to 2 tiles adjacent to you
        // [$12-18] [7-9 cd]  Whip: Deal 12-8 damage to all enemies up to 3 tiles adjacent to you
        // [$10-16] [6-8 cd]  Teleport: Go to any unoccupied tile
        // [$8-14]  [5-7 cd]  Dodge: Take no damage this turn

        if      (powerupIdentity == "active-knife")
        {
            return (powerupIdentity, "active", "Knife", imageKnife, "[Active, cooldown: {7}]", "<line-height=130%>Attack an enemy 1 tile adjacent to you, dealing {8} damage",
                    (3, 7), (2, 4), (5, 3), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "active-spear")
        {
            return (powerupIdentity, "active", "Spear", imageSpear, "[Active, cooldown: {7}]", "<line-height=130%>Attack an enemy up to 2 tiles adjacent to you, dealing {8} damage",
                    (6, 12), (3, 5), (8, 5), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "active-satchet")
        {
            return (powerupIdentity, "active", "Hatchet", imageHatchet, "[Active, cooldown: {7}]", "<line-height=130%>Choose any enemy and deal {8} damage",
                    (5, 10), (4, 6), (7, 4), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "active-slingshot")
        {
            return (powerupIdentity, "active", "Slingshot", imageSlingshot, "[Active, cooldown: {7}]", "<line-height=130%>Click a tile further than 3 tiles from you, that enemy takes {8} damage",
                    (8, 15), (5, 7), (6, 3), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "active-sniper")
        {
            return (powerupIdentity, "active", "Sniper", imageSniper, "[Active, cooldown: {7}]", "<line-height=130%>Click a tile further than 3 tiles from you, that enemy takes {8} damage",
                    (12, 18), (6, 8), (10, 7), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "active-lightningBolt")
        {
            return (powerupIdentity, "active", "Lightning Bolt", imageLightningBolt, "[Active, cooldown: {7}]", "<line-height=130%>Click a tile further than 3 tiles from you, that enemy takes {8} damage",
                    (10, 16), (6, 8), (12, 8), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "active-bomb")
        {
            return (powerupIdentity, "active", "Bomb", imageBomb, "[Active, cooldown: {7}]", "<line-height=130%>Click a tile further than 3 tiles from you. All pieces within 3x3 area of it take {8} damage",
                    (15, 22), (7, 9), (6, 4), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "active-fireball")
        {
            return (powerupIdentity, "active", "Fireball", imageFireball, "[Active, cooldown: {7}]", "<line-height=130%>Click a tile further than 3 tiles from you. All pieces within 5x5 area of it take {8} damage",
                    (18, 25), (8, 10), (10, 7), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "active-arrowVolley")
        {
            return (powerupIdentity, "active", "Arrow Volley", imageArrowVolley, "[Active, cooldown: {7}]", "<line-height=130%>Click a tile further than 3 tiles from you. All pieces within 7x7 area of it take {8} damage",
                    (20, 28), (9, 11), (8, 5), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "active-acidRain")
        {
            return (powerupIdentity, "active", "Acid Rain", imageAcidRain, "[Active, cooldown: {7}]", "<line-height=130%>Deal {8} damage to all enemies",
                    (14, 20), (7, 9), (6, 4), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "active-axe")
        {
            return (powerupIdentity, "active", "Axe", imageAxe, "[Active, cooldown: {7}]", "<line-height=130%>Deal {8} damage to all enemies 1 tile adjacent to you",
                    (7, 12), (5, 7), (8, 5), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "active-spikedlub")
        {
            return (powerupIdentity, "active", "Spiked Club", imageSpikedClub, "[Active, cooldown: {7}]", "<line-height=130%>Deal {8} damage to all enemies up to 2 tiles adjacent to you",
                    (9, 14), (6, 8), (10, 6), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "active-whip")
        {
            return (powerupIdentity, "active", "Whip", imageWhip, "[Active, cooldown: {7}]", "<line-height=130%>Deal {8} damage to all enemies up to 3 tiles adjacent to you",
                    (12, 18), (7, 9), (12, 8), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "active-teleport")
        {
            return (powerupIdentity, "active", "Teleport", imageTeleport, "[Active, cooldown: {7}]", "<line-height=130%>Go to any unoccupied tile",
                    (10, 16), (6, 8), (-1, -1), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "active-dodge")
        {
            return (powerupIdentity, "active", "Dodge", imageDodge, "[Active, cooldown: {7}]", "<line-height=130%>Take no damage this turn",
                    (8, 14), (5, 7), (-1, -1), (-1, -1), (-1, -1));
        }

        // PASSIVE POWERUPS ===============================================================
        // [Up to 4 and can be sold]
        // ================================================================================ // SEPERATE BETWEEN RED AREA VS YELLOW AREA DAMAGE?
        //[$3-6]    Light Armor: Take 3 - 1 less damage [min 1] from any source 
        //[$5-9]    Heavy Armor: Take 4 - 2 less damage [min 1] from any source
        //[$7-12]   Diamond Armor: Take 6 - 3 less damage [min 1] from any source
        //[$6-10]   Ground Pound: After moving, all adjacent enemies take 1 - 3 damage
        //[$4-8]    Inner Healing: Heal 2 - 1 Health every 3 turns (max 5 times per round)
        //[$8-12]   Vampiric: Heal 3 - 1 health for every enemy killed
        //[$5-10]   Well-Deserved Rest: Heal 10 - 5 health after every round
        //[$7-11]   Bloodlust: Gain +1 damage per damage taken. Resets per round
        //[$10-15]  Mercenary Tools: Lose $1 per turn, Doubles damage dealt, Halves damage taken
        //[$5-10]   Pickpocket: Gain $3 - 1 more per enemy killed
        //[$8-16]   Flexible Movement I: You can jump to four new tiles per turn: 1 tile orthogonally from you   // FOR THESE ONES, MAKE THE BUTTONS ALSO ACTIVATE A FUNCTION (IN INSPECTOR) TO INCREASE THE DGB POWERUP USAGE INPUT, EVEN MORE IF IT DAMAGES AN ENEMY (optional)
        //[$8-16]   Flexible Movement II: You can jump to four new tiles per turn: 1 tile diagonally from you
        //[$8-16]   Flexible Movement III: You can jump to four new tiles per turn: 2 tiles orthogonally from you
        //[$8-16]   Flexible Movement IV: You can jump to four new tiles per turn: 2 tiles diagonally from you
        //[$8-16]   Flexible Movement V: You can jump to four new tiles per turn: 3 tiles orthogonally from you
        //[$8-16]   Flexible Movement VI: You can jump to four new tiles per turn: 3 tiles diagonally from you
        //[$?12-24] Flexible Movement VII: You can jump to eight new tiles per turn: Like your 8 knight tiles but even further.

        else if (powerupIdentity == "passive-lightArmor")
        {
            return (powerupIdentity, "passive", "Light Armor", imageLightArmor, "[Passive]", "<line-height=130%>Take {8} less damage [min 1] from any source",
                    (3, 6), (-1, -1), (3, 1), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "passive-heavyArmor")
        {
            return (powerupIdentity, "passive", "Heavy Armor I", imageHeavyArmor, "[Passive]", "<line-height=130%>Take {8} less damage [min 1] from any source",
                    (5, 9), (-1, -1), (4, 2), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "passive-diamondArmor")
        {
            return (powerupIdentity, "passive", "Heavy Armor II", imageDiamondArmor, "[Passive]", "<line-height=130%>Take {8} less damage [min 1] from any source",
                    (7, 12), (-1, -1), (6, 3), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "passive-groundPond")
        {
            return (powerupIdentity, "passive", "Ground Pound", imageGroundPound, "[Passive]", "<line-height=130%>After moving, all adjacent enemies take {8} damage",
                    (6, 10), (-1, -1), (1, 3), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "passive-innerHealing")
        {
            return (powerupIdentity, "passive", "Inner Healing", imageInnerHealing, "[Passive]", "<line-height=130%>Heal {8} health every 3 turns (max 5 times per round)",
                    (4, 8), (-1, -1), (2, 1), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "passive-vampiric")
        {
            return (powerupIdentity, "passive", "Vampiric", imageVampiric, "[Passive]", "<line-height=130%>Heal {8} health for every enemy killed",
                    (8, 12), (-1, -1), (3, 1), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "passive-wellDeservedRest")
        {
            return (powerupIdentity, "passive", "Well-Deserved Rest", imageWellDeservedRest, "[Passive]", "<line-height=130%>Heal {8} health after every round",
                    (5, 10), (-1, -1), (10, 5), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "passive-bloodlust")
        {
            return (powerupIdentity, "passive", "Bloodlust", imageBloodlust, "[Passive]", "<line-height=130%>Gain 1 damage per damage taken. Resets per round",
                    (7, 11), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "passive-mercenaryTools")
        {
            return (powerupIdentity, "passive", "Mercenary Tools", imageMercenaryTools, "[Passive]", "<line-height=130%>Lose $1 per turn, doubles damage dealt, halves damage taken",
                    (10, 15), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "passive-pickpocket")
        {
            return (powerupIdentity, "passive", "Pickpocket", imagePickpocket, "[Passive]", "<line-height=130%>Gain {8} more per enemy killed",
                    (5, 10), (-1, -1), (3, 1), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "passive-flexibleMovementI")
        {
            return (powerupIdentity, "passive", "Flexible Movement I", imageFlexibleMovementI, "[Passive]", "<line-height=130%>You can jump to four new tiles per turn: 1 tile orthogonally from you",
                    (8, 16), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "passive-flexibleMovementII")
        {
            return (powerupIdentity, "passive", "Flexible Movement II", imageFlexibleMovementII, "[Passive]", "<line-height=130%>You can jump to four new tiles per turn: 1 tile diagonally from you",
                    (8, 16), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "passive-flexibleMovementIII")
        {
            return (powerupIdentity, "passive", "Flexible Movement III", imageFlexibleMovementIII, "[Passive]", "<line-height=130%>You can jump to four new tiles per turn: 2 tiles orthogonally from you",
                    (8, 16), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "passive-flexibleMovementIV")
        {
            return (powerupIdentity, "passive", "Flexible Movement IV", imageFlexibleMovementIV, "[Passive]", "<line-height=130%>You can jump to four new tiles per turn: 2 tiles diagonally from you",
                    (8, 16), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "passive-flexibleMovementV")
        {
            return (powerupIdentity, "passive", "Flexible Movement V", imageFlexibleMovementV, "[Passive]", "<line-height=130%>You can jump to four new tiles per turn: 3 tiles orthogonally from you",
                    (8, 16), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "passive-flexibleMovementVI")
        {
            return (powerupIdentity, "passive", "Flexible Movement VI", imageFlexibleMovementVI, "[Passive]", "<line-height=130%>You can jump to four new tiles per turn: 3 tiles diagonally from you",
                    (8, 16), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "passive-flexibleMovementVII")
        {
            return (powerupIdentity, "passive", "Flexible Movement VII", imageFlexibleMovementVII, "[Passive]", "<line-height=130%>You can jump to eight new tiles per turn: Like your 8 knight tiles but even further",
                    (12, 24), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }

        // STAT BUFF POWEUPS ===============================================================
        // [Permanent Buffs or one-time use]
        // Increase DGB Input Powerup Usage a bit upon purchase. 
        // =================================================================================

        //[$3-5]   Small Snack: Heal 5 - 12 health // MAYBE REDUCE DGB INPUT POWERUP USAGE IF BUY HEALTH BUT IS ARLREADY MAXED?
        //[$6-9]   Lunch Break: Heal 15 - 30 health
        //[$10-15] Tavern Buffet: Heal to full health
        //[$4-7]   Hardened Fists: Permanently Increases your direct contact damage by 2 - 4
        //[$7-11]  Iron Fists: Permanently Increases your direct contact damage by 4 - 7
        //[$11-17] Diamond Fists: Increases your direct contact damage by 7 - 10
        //[$4-8]   Toughened Heart: Permanently increases your max health by 4 - 8 And heal 4 - 8 health
        //[$8-12]  Unbreakable Heart: Permanently increases your max health by 8 - 15 And heal 8 - 15 health
        //[$4-8]   Weapon Proficiency: Permanently increase all damage you deal by 1 - 2
        //[$8-12]  Weapon Mastery: Permanently increase all damage you deal by 3 - 4


        else if (powerupIdentity == "statBuff-smallSnack")
        {
            return (powerupIdentity, "statBuff", "Small Snack", imageSmallSnack, "[Stat Buff]", "<line-height=130%>Heal {8} health",
                    (5, 8), (-1, -1), (12, 5), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "statBuff-lunchBreak")
        {
            return (powerupIdentity, "statBuff", "LunchBreak", imageLunchBreak, "[Stat Buff]", "<line-height=130%>Heal {8} health",
                    (9, 14), (-1, -1), (30, 15), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "statBuff-tavernBuffet")
        {
            return (powerupIdentity, "statBuff", "Tavern Buffet", imageTavernBuffet, "[Stat Buff]", "<line-height=130%>Heal to full health",
                    (15, 23), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "statBuff-hardenedFists")
        {
            return (powerupIdentity, "statBuff", "Hardened Fists", imageHardenedFists, "[Stat Buff]", "<line-height=130%>Permanently increases your direct contact damage by {8}",
                    (6, 10), (-1, -1), (4, 2), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "statBuff-ironFists")
        {
            return (powerupIdentity, "statBuff", "Iron Fists", imageIronFists, "[Stat Buff]", "<line-height=130%>Permanently increases your direct contact damage by {8}",
                    (10, 17), (-1, -1), (7, 4), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "statBuff-diamondFists")
        {
            return (powerupIdentity, "statBuff", "Diamond Fists", imageDiamondFists, "[Stat Buff]", "<line-height=130%>Increases your direct contact damage by {8}",
                    (17, 26), (-1, -1), (10, 7), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "statBuff-toughenedHeart")
        {
            return (powerupIdentity, "statBuff", "Toughened Heart", imageToughenedHeart, "[Stat Buff]", "<line-height=130%>Permanently increases your max health by {8} and heal {9} health",
                    (6, 12), (-1, -1), (8, 4), (8, 4), (-1, -1));
        }
        else if (powerupIdentity == "statBuff-unbreakableHeart")
        {
            return (powerupIdentity, "statBuff", "Unbreakable Heart", imageUnbreakableHeart, "[Stat Buff]", "<line-height=130%>Permanently increases your max health by {8} and heal {9} health",
                    (12, 18), (-1, -1), (15, 8), (15, 8), (-1, -1));
        }
        else if (powerupIdentity == "statBuff-weaponProficiency")
        {
            return (powerupIdentity, "statBuff", "Weapon Proficiency", imageWeaponProficiency, "[Stat Buff]", "<line-height=130%>Permanently increase all damage you deal by {8}",
                    (6, 12), (-1, -1), (2, 1), (-1, -1), (-1, -1));
        }
        else if (powerupIdentity == "statBuff-weaponMastery")
        {
            return (powerupIdentity, "statBuff", "Weapon Mastery", imageWeaponMastery, "[Stat Buff]", "<line-height=130%>Permanently increase all damage you deal by {8}",
                    (12, 18), (-1, -1), (4, 3), (-1, -1), (-1, -1));
        }



        else

        {
            return (powerupIdentity, "???", "Unkown Powerup Name!!!", missing, "??????????", "??????????",
                    (-1, -1), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }
    }


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
        else if (powerupType == "statBuff")
        {
            return "<line-height=100%>-- Purchased --\r\n<size=20><line-height=130%>Stat-buffs are either permanent or just a one-time use. They do not take up an ability slot. You can buy more than 1 of these.";
        }
        //else if (powerupType == "singleUse")
        //{
        //    return "<line-height=100%>-- Purchased --\r\n<size=20><line-height=130%>Single-use shop items only take effect once. They do not take up an ability slot.";
        //}
        else
        {
            return "Warning: unkown powerup type: " + powerupType;
        }
    }

    public void ActivateThisActivePoweup(string powerupIdentity)
    {

    }
    public void ActivateThisPassivePoweup(string powerupIdentity)
    {

    }
    public void ActivateThisStatBuffPoweup(string powerupIdentity)
    {
        float difficultyIndex = dynamicDifficultyController.GetDynamicOutput("powerupQuality");
        var powerupInfo = GetPowerupInfo(powerupIdentity);
        (int easyNumberA, int hardNumberA) = powerupInfo.Item9;
        (int easyNumberB, int hardNumberB) = powerupInfo.Item10;
        int numberA = Mathf.RoundToInt(easyNumberA + (hardNumberA - easyNumberA) * difficultyIndex);
        int numberB = Mathf.RoundToInt(easyNumberB + (hardNumberB - easyNumberB) * difficultyIndex);
        dynamicDifficultyController.SetDynamicInputChange("powerupUsage", +0.1f, false);

        if (powerupIdentity == "statBuff-smallSnack")
        {
            playerAndEnemyStatusController.SetPlayerCurrHealth(playerAndEnemyStatusController.GetPlayerCurrHealth() + numberA);
            int healthOverflow = Mathf.Max(playerAndEnemyStatusController.GetPlayerCurrHealth() + numberA - playerAndEnemyStatusController.GetPlayerMaxHealth() , 0); // Deduct player DGB input powerup usage if they wasted >= half of this powerup's healing on overheal.
            float DGBInputToDeductPowerupUsage = Mathf.Max( (healthOverflow / numberA) - 0.5f , 0) / 5;
            dynamicDifficultyController.SetDynamicInputChange("powerupUsage", - DGBInputToDeductPowerupUsage, false);
        }
        else if (powerupIdentity == "statBuff-lunchBreak")
        {
            playerAndEnemyStatusController.SetPlayerCurrHealth(playerAndEnemyStatusController.GetPlayerCurrHealth() + numberA);
            int healthOverflow = Mathf.Max(playerAndEnemyStatusController.GetPlayerCurrHealth() + numberA - playerAndEnemyStatusController.GetPlayerMaxHealth(), 0); // Deduct player DGB input powerup usage if they wasted >= half of this powerup's healing on overheal.
            float DGBInputToDeductPowerupUsage = Mathf.Max((healthOverflow / numberA) - 0.5f, 0) / 5;
            dynamicDifficultyController.SetDynamicInputChange("powerupUsage", - DGBInputToDeductPowerupUsage, false);
        }
        else if (powerupIdentity == "statBuff-tavernBuffet")
        {
            playerAndEnemyStatusController.SetPlayerCurrHealth(playerAndEnemyStatusController.GetPlayerMaxHealth());
            int healthOverflow = playerAndEnemyStatusController.GetPlayerCurrHealth(); // Deduct player DGB input powerup usage if they wasted >= half of this powerup's healing on overheal.
            float DGBInputToDeductPowerupUsage = Mathf.Max((healthOverflow / numberA) - 0.5f, 0) / 5;
            dynamicDifficultyController.SetDynamicInputChange("powerupUsage", - DGBInputToDeductPowerupUsage, false);
        }
        else if (powerupIdentity == "statBuff-hardenedFists")
        {
            playerAndEnemyStatusController.SetPlayerDirectContactDamage(playerAndEnemyStatusController.GetPlayerDirectContactDamage() + numberA);
        }
        else if (powerupIdentity == "statBuff-ironFists")
        {
            playerAndEnemyStatusController.SetPlayerDirectContactDamage(playerAndEnemyStatusController.GetPlayerDirectContactDamage() + numberA);
        }
        else if (powerupIdentity == "statBuff-diamondFists")
        {
            playerAndEnemyStatusController.SetPlayerDirectContactDamage(playerAndEnemyStatusController.GetPlayerDirectContactDamage() + numberA);
        }
        else if (powerupIdentity == "statBuff-toughenedHeart")
        {
            playerAndEnemyStatusController.SetPlayerMaxHealth(playerAndEnemyStatusController.GetPlayerMaxHealth() + numberA);
            playerAndEnemyStatusController.SetPlayerCurrHealth(playerAndEnemyStatusController.GetPlayerCurrHealth() + numberB);
        }
        else if (powerupIdentity == "statBuff-unbreakableHeart")
        {
            playerAndEnemyStatusController.SetPlayerMaxHealth(playerAndEnemyStatusController.GetPlayerMaxHealth() + numberA);
            playerAndEnemyStatusController.SetPlayerCurrHealth(playerAndEnemyStatusController.GetPlayerCurrHealth() + numberB);
        }
        else if (powerupIdentity == "statBuff-weaponProficiency")
        {
            playerAndEnemyStatusController.SetPlayerBonusDamage(playerAndEnemyStatusController.GetPlayerBonusDamage() + numberA);
        }
        else if (powerupIdentity == "statBuff-weaponMastery")
        {
            playerAndEnemyStatusController.SetPlayerBonusDamage(playerAndEnemyStatusController.GetPlayerBonusDamage() + numberA);
        }
        else
        {
            Debug.Log("Warning: unkown powerup type: " + powerupIdentity);
        }
    }

}
