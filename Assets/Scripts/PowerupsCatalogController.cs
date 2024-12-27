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
    [SerializeField] private Sprite imageFlexibleMovementVIII;
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
    [SerializeField] private Sprite imageMissing;
    [Header("")]
    [Header("Controllers")]
    [SerializeField] private BottomBarController bottomBarController;
    [SerializeField] private PlayerAndEnemyStatusController playerAndEnemyStatusController;
    [SerializeField] private DynamicDifficultyController dynamicDifficultyController;

    private List<string> allPowerupIdentitiesList;
    private string selectedDifficulty;

    private void Start()
    {
        selectedDifficulty = PlayerPrefs.GetString("modeDifficulty", "Adaptive");
        allPowerupIdentitiesList = new List<string> {
            // All active powerups
            "active-knife", "active-spear", "active-hatchet", "active-slingshot", "active-sniper",
            "active-lightningBolt", "active-bomb", "active-fireball", "active-arrowVolley",
            "active-acidRain", "active-axe", "active-spikedClub", "active-whip",
            "active-teleport", "active-dodge",

            // All passive powerups
            "passive-lightArmor", "passive-heavyArmor", "passive-diamondArmor",
            "passive-innerHealing", "passive-vampiric", "passive-wellDeservedRest", "passive-bloodlust",
            //"passive-groundPound", // hard to code
            "passive-mercenaryTools", "passive-pickpocket",
            "passive-flexibleMovementI", "passive-flexibleMovementII", "passive-flexibleMovementIII", "passive-flexibleMovementIV", "passive-flexibleMovementV", "passive-flexibleMovementVI", "passive-flexibleMovementVII", "passive-flexibleMovementVIII",

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
    public void AddPowerupsToList(string powerup) // When sold by player
    {
        if (!allPowerupIdentitiesList.Contains(powerup)) { allPowerupIdentitiesList.Add(powerup); }
        else { Debug.LogWarning("powerup to be added back to powerups list already exists: " + powerup); }
    }

    public void RemovePowerupsFromList(string powerup) // When bought by player.
    {
        if (allPowerupIdentitiesList.Contains(powerup)) { allPowerupIdentitiesList.Remove(powerup); }
        else { Debug.LogWarning("powerup to be removed from powerups list does not exist: " + powerup); }
    }


    public (string, string, string, Sprite, string, string, (int, int), (int, int), (int, int), (int, int), (int, int)) GetPowerupInfo(string powerupIdentity)
    { // Powerup Identity, Powerup Type,  Name, Image, Short Description, Long Description,
      // Easy / Hard Price, Easy / Hard Cooldown, Easy / Hard Number1(heal or damage), Easy / Hard Number2(attack area radius), Easy / Hard Number3(???) 

        // ACTIVE POWERUPS ================================================================
        // [Up to 5 and can be sold, press Q / W / E / R / T key pads ( or click on them too ? ) , can only use 1 per turn, each has a cooldown, clicking on them resets the move timer]
        // ================================================================================
        // [$12-28] [2-4 cd]  Knife: Attack an enemy 1 tile adjacent to you, dealing 5-3 damage
        //                        Effect: Reset Move Timer, Show new tiles, deal damage to enemy, end player turn
        // [$24-48] [3-5 cd]  Spear: Attack an enemy up to 2 tiles adjacent to you, dealing 8-5 damage
        //                        Effect: Reset Move Timer, Show new tiles, deal damage to enemy, end player turn
        // [$20-40] [4-6 cd]  Hatchet: Attack an enemy up to 3 tiles adjacent to you, dealing 7-4 damage
        //                        Effect: Reset Move Timer, Show new tiles, deal damage to enemy, end player turn
        // [$32-60] [5-7 cd]  Slingshot: Click a tile further than 3 tiles from you, that enemy takes 6-3 damage
        //                        Effect: Reset Move Timer, Show new tiles, deal damage to enemy, end player turn
        // [$48-72] [6-8 cd]  Sniper: Click a tile further than 3 tiles from you, that enemy takes 10-7 damage
        //                        Effect: Reset Move Timer, Show new tiles, deal damage to enemy, end player turn
        // [$40-64] [6-8 cd]  Lightning Bolt: Click a tile further than 3 tiles from you, that enemy takes 12-8 damage
        //                        Effect: Reset Move Timer, Show new tiles, deal damage to enemy, end player turn
        // [$60-88] [7-9 cd]  Bomb: Click a tile further than 3 tiles from you. All pieces within 3x3 area of it take 6-4 damage
        //                        Effect: Reset Move Timer, Show new tiles, deal damage to enemy, end player turn
        // [$72-100][8-10 cd] Fireball: Click a tile further than 3 tiles from you. All pieces within 5x5 area of it take 10-7 damage
        //                        Effect: Reset Move Timer, Show new tiles, deal damage to enemy, end player turn
        // [$80-112][9-11 cd] Arrow Volley: Click a tile further than 3 tiles from you. All pieces within 7x7 area of it take 8-5 damage
        //                        Effect: Reset Move Timer, Show new tiles, deal damage to enemy, end player turn
        // [$28-48] [7-9 cd]  Acid Rain: Deal 6-4 damage to all enemies
        //                        Effect: Reset Move Timer, Show new tiles, deal damage to enemy, end player turn
        // [$28-48] [5-7 cd]  Axe: Deal 8-5 damage to all enemies 1 tile adjacent to you
        //                        Effect: deal damage to enemy, end player turn
        // [$36-56] [6-8 cd]  Spiked Club: Deal 10-6 damage to all enemies up to 2 tiles adjacent to you
        //                        Effect: deal damage to enemy, end player turn
        // [$48-72] [7-9 cd]  Whip: Deal 12-8 damage to all enemies up to 3 tiles adjacent to you
        //                        Effect: deal damage to enemy, end player turn
        // [$40-64] [6-8 cd]  Teleport: Go to any unoccupied tile
        //                        Effect: Reset Move Timer, Show new tiles, move there, end player turn
        // [$32-56] [5-7 cd]  Dodge: Take no damage this turn
        //                        Effect: make all damage 0 this turn

        if       (powerupIdentity == "active-knife"){
            return (powerupIdentity, "active", "Knife", imageKnife, "[Active, cooldown: {7}]", "<line-height=130%>Attack an enemy 1 tile adjacent to you, dealing {8} damage",
                    (12, 28), (2, 4), (5, 3), (1, 1), (-1, -1));
        }else if (powerupIdentity == "active-spear"){
            return (powerupIdentity, "active", "Spear", imageSpear, "[Active, cooldown: {7}]", "<line-height=130%>Attack an enemy up to 2 tiles adjacent to you, dealing {8} damage",
                    (24, 48), (3, 5), (8, 5), (1, 1), (-1, -1));
        }else if (powerupIdentity == "active-hatchet"){
            return (powerupIdentity, "active", "Hatchet", imageHatchet, "[Active, cooldown: {7}]", "<line-height=130%>Attack an enemy up to 3 tiles adjacent to you, dealing {8} damage",
                    (20, 40), (4, 6), (7, 4), (1, 1), (-1, -1));
        }else if (powerupIdentity == "active-slingshot"){
            return (powerupIdentity, "active", "Slingshot", imageSlingshot, "[Active, cooldown: {7}]", "<line-height=130%>Click a tile further than 3 tiles from you, that enemy takes {8} damage",
                    (32, 60), (5, 7), (6, 3), (1, 1), (-1, -1));
        }else if (powerupIdentity == "active-sniper"){
            return (powerupIdentity, "active", "Sniper", imageSniper, "[Active, cooldown: {7}]", "<line-height=130%>Click a tile further than 3 tiles from you, that enemy takes {8} damage",
                    (48, 72), (6, 8), (10, 7), (1, 1), (-1, -1));
        }else if (powerupIdentity == "active-lightningBolt"){
            return (powerupIdentity, "active", "Lightning Bolt", imageLightningBolt, "[Active, cooldown: {7}]", "<line-height=130%>Click a tile further than 3 tiles from you, that enemy takes {8} damage",
                    (40, 64), (6, 8), (12, 8), (1, 1), (-1, -1));
        }else if (powerupIdentity == "active-bomb"){
            return (powerupIdentity, "active", "Bomb", imageBomb, "[Active, cooldown: {7}]", "<line-height=130%>Click a tile further than 3 tiles from you. All pieces within 3x3 area of it take {8} damage",
                    (60, 88), (7, 9), (6, 4), (3, 3), (-1, -1));
        }else if (powerupIdentity == "active-fireball"){
            return (powerupIdentity, "active", "Fireball", imageFireball, "[Active, cooldown: {7}]", "<line-height=130%>Click a tile further than 3 tiles from you. All pieces within 5x5 area of it take {8} damage",
                    (72, 100), (8, 10), (10, 7), (5, 5), (-1, -1));
        }else if (powerupIdentity == "active-arrowVolley"){
            return (powerupIdentity, "active", "Arrow Volley", imageArrowVolley, "[Active, cooldown: {7}]", "<line-height=130%>Click a tile further than 3 tiles from you. All pieces within 7x7 area of it take {8} damage",
                    (80, 112), (9, 11), (8, 5), (7, 7), (-1, -1));
        }else if (powerupIdentity == "active-acidRain"){
            return (powerupIdentity, "active", "Acid Rain", imageAcidRain, "[Active, cooldown: {7}]", "<line-height=130%>Deal {8} damage to all enemies",
                    (28, 40), (7, 9), (6, 4), (1, 1), (-1, -1));
        }else if (powerupIdentity == "active-axe"){
            return (powerupIdentity, "active", "Axe", imageAxe, "[Active, cooldown: {7}]", "<line-height=130%>Deal {8} damage to all enemies 1 tile adjacent to you",
                    (28, 48), (5, 7), (8, 5), (3, 3), (-1, -1));
        }else if (powerupIdentity == "active-spikedClub"){
            return (powerupIdentity, "active", "Spiked Club", imageSpikedClub, "[Active, cooldown: {7}]", "<line-height=130%>Deal {8} damage to all enemies up to 2 tiles adjacent to you",
                    (36, 56), (6, 8), (10, 6), (5, 5), (-1, -1));
        }else if (powerupIdentity == "active-whip"){
            return (powerupIdentity, "active", "Whip", imageWhip, "[Active, cooldown: {7}]", "<line-height=130%>Deal {8} damage to all enemies up to 3 tiles adjacent to you",
                    (48, 72), (7, 9), (12, 8), (7, 7), (-1, -1));
        }else if (powerupIdentity == "active-teleport"){
            return (powerupIdentity, "active", "Teleport", imageTeleport, "[Active, cooldown: {7}]", "<line-height=130%>Go to any unoccupied tile",
                    (40, 64), (6, 8), (-1, -1), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "active-dodge"){
            return (powerupIdentity, "active", "Dodge", imageDodge, "[Active, cooldown: {7}]", "<line-height=130%>Take no damage this turn",
                    (32, 56), (5, 7), (-1, -1), (-1, -1), (-1, -1));
        }

        // PASSIVE POWERUPS ===============================================================
        // [Up to 4 and can be sold]
        // ================================================================================ // SEPERATE BETWEEN RED AREA VS YELLOW AREA DAMAGE?
        //[$12-24]  Light Armor: Take 3 - 1 less damage [min 1] from any source
        //[$20-36]  Heavy Armor: Take 4 - 2 less damage [min 1] from any source
        //[$37-48]  Diamond Armor: Take 6 - 3 less damage [min 1] from any source
        //[$16-32]  Inner Healing: Heal 2 - 1 Health every 3 turns (max 5 times per round)
        //[$32-48]  Vampiric: Heal 3 - 1 health for every enemy killed\
        //[$20-40]  Well-Deserved Rest: Heal 10 - 5 health after every round
        //[$28-44]  Bloodlust: Increase all your damage by 3 - 1 per damage taken. Resets per round
        //[$24-40]  Ground Pound: After moving, all adjacent enemies take 1 - 3 damage
        //[$40-60]  Mercenary Tools: Lose $1 per turn, Doubles damage dealt, Halves damage taken
        //[$20-40]  Pickpocket: Gain $3 - 1 more per enemy killed
        //[$32-64]  Flexible Movement I: You can jump to four new tiles per turn: 1 tile orthogonally from you   // FOR THESE ONES, MAKE THE BUTTONS ALSO ACTIVATE A FUNCTION (IN INSPECTOR) TO INCREASE THE DGB POWERUP USAGE INPUT, EVEN MORE IF IT DAMAGES AN ENEMY (optional)
        //[$32-64]  Flexible Movement II: You can jump to four new tiles per turn: 1 tile diagonally from you
        //[$32-64]  Flexible Movement III: You can jump to four new tiles per turn: 2 tiles orthogonally from you
        //[$32-64]  Flexible Movement IV: You can jump to four new tiles per turn: 2 tiles diagonally from you
        //[$32-64]  Flexible Movement V: You can jump to four new tiles per turn: 3 tiles orthogonally from you
        //[$32-64]  Flexible Movement VI: You can jump to four new tiles per turn: 3 tiles diagonally from you
        //[$48-96]  Flexible Movement VII: You can jump to eight new tiles per turn: 3 tils orthogonally + 1 tile perpendicular
        //[$48-96]  Flexible Movement VIII: You can jump to eight new tiles per turn: 3 tils orthogonally + 2 tiles perpendicular

        else if (powerupIdentity == "passive-lightArmor"){
            return (powerupIdentity, "passive", "Light Armor", imageLightArmor, "[Passive]", "<line-height=130%>Take {8} less damage [min 1] from any source",
                    (12, 24), (-1, -1), (3, 1), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "passive-heavyArmor"){
            return (powerupIdentity, "passive", "Heavy Armor", imageHeavyArmor, "[Passive]", "<line-height=130%>Take {8} less damage [min 1] from any source",
                    (20, 36), (-1, -1), (4, 2), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "passive-diamondArmor"){
            return (powerupIdentity, "passive", "Diamond Armor", imageDiamondArmor, "[Passive]", "<line-height=130%>Take {8} less damage [min 1] from any source",
                    (37, 48), (-1, -1), (6, 3), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "passive-innerHealing"){
            return (powerupIdentity, "passive", "Inner Healing", imageInnerHealing, "[Passive]", "<line-height=130%>Heal {8} health every 3 turns (max 5 times per round)",
                    (16, 32), (-1, -1), (2, 1), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "passive-vampiric"){
            return (powerupIdentity, "passive", "Vampiric", imageVampiric, "[Passive]", "<line-height=130%>Heal {8} health for every enemy killed",
                    (32, 48), (-1, -1), (3, 1), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "passive-wellDeservedRest"){
            return (powerupIdentity, "passive", "Well-Deserved Rest", imageWellDeservedRest, "[Passive]", "<line-height=130%>Heal {8} health after every round",
                    (20, 40), (-1, -1), (10, 5), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "passive-bloodlust"){
            return (powerupIdentity, "passive", "Bloodlust", imageBloodlust, "[Passive]", "<line-height=130%>Increase all your damage by {8} per damage taken. Resets per round",
                    (28, 44), (-1, -1), (3, 1), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "passive-groundPound"){
            return (powerupIdentity, "passive", "Ground Pound", imageGroundPound, "[Passive]", "<line-height=130%>After moving, all adjacent enemies take {8} damage",
                    (24, 40), (-1, -1), (1, 3), (3, 3), (-1, -1));
        }else if (powerupIdentity == "passive-mercenaryTools"){
            return (powerupIdentity, "passive", "Mercenary Tools", imageMercenaryTools, "[Passive]", "<line-height=130%>Lose $1 per turn, doubles damage dealt, halves damage taken",
                    (40, 60), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "passive-pickpocket"){
            return (powerupIdentity, "passive", "Pickpocket", imagePickpocket, "[Passive]", "<line-height=130%>Gain {8} more gold per enemy killed",
                    (20, 40), (-1, -1), (3, 1), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "passive-flexibleMovementI"){
            return (powerupIdentity, "passive", "Flexible Movement I", imageFlexibleMovementI, "[Passive]", "<line-height=130%>4 new tiles to move: 1 tile orthogonally",
                    (32, 64), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "passive-flexibleMovementII"){
            return (powerupIdentity, "passive", "Flexible Movement II", imageFlexibleMovementII, "[Passive]", "<line-height=130%>4 new tiles to move: 1 tile diagonally",
                    (32, 64), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "passive-flexibleMovementIII"){
            return (powerupIdentity, "passive", "Flexible Movement III", imageFlexibleMovementIII, "[Passive]", "<line-height=130%>4 new tiles to move: 2 tiles orthogonally",
                    (32, 64), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "passive-flexibleMovementIV"){
            return (powerupIdentity, "passive", "Flexible Movement IV", imageFlexibleMovementIV, "[Passive]", "<line-height=130%>4 new tiles to move: 2 tiles diagonally",
                    (32, 64), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "passive-flexibleMovementV"){
            return (powerupIdentity, "passive", "Flexible Movement V", imageFlexibleMovementV, "[Passive]", "<line-height=130%>4 new tiles to move: 3 tiles orthogonally",
                    (32, 64), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "passive-flexibleMovementVI"){
            return (powerupIdentity, "passive", "Flexible Movement VI", imageFlexibleMovementVI, "[Passive]", "<line-height=130%>4 new tiles to move: 3 tiles diagonally",
                    (32, 64), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "passive-flexibleMovementVII"){
            return (powerupIdentity, "passive", "Flexible Movement VII", imageFlexibleMovementVII, "[Passive]", "<line-height=130%>8 new tiles to move: 3 tiles orthogonally then 1 tile perpendicular",
                    (48, 96), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "passive-flexibleMovementVIII"){
            return (powerupIdentity, "passive", "Flexible Movement VIII", imageFlexibleMovementVIII, "[Passive]", "<line-height=130%>8 new tiles to move: 3 tiles orthogonally then 2 tiles perpendicular",
                    (48, 96), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }

        // STAT BUFF POWEUPS ===============================================================
        // [Permanent Buffs or one-time use]
        // Increase DGB Input Powerup Usage a bit upon purchase. 
        // =================================================================================

        //[$10-16]  Small Snack: Heal 5 - 12 health // MAYBE REDUCE DGB INPUT POWERUP USAGE IF BUY HEALTH BUT IS ARLREADY MAXED?
        //[$18-28]  Lunch Break: Heal 15 - 30 health
        //[$30-46]  Tavern Buffet: Heal to full health
        //[$12-20]  Hardened Fists: Permanently Increases your direct contact damage by 2 - 4
        //[$20-34]  Iron Fists: Permanently Increases your direct contact damage by 4 - 7
        //[$34-52]  Diamond Fists: Increases your direct contact damage by 7 - 10
        //[$12-24]  Toughened Heart: Permanently increases your max health by 4 - 8 And heal 4 - 8 health
        //[$24-36]  Unbreakable Heart: Permanently increases your max health by 8 - 15 And heal 8 - 15 health
        //[$12-24]  Weapon Proficiency: Permanently increase all damage you deal by 1 - 2
        //[$24-36]  Weapon Mastery: Permanently increase all damage you deal by 3 - 4


        else if (powerupIdentity == "statBuff-smallSnack"){
            return (powerupIdentity, "statBuff", "Small Snack", imageSmallSnack, "[Stat Buff]", "<line-height=130%>Heal {8} health",
                    (10, 16), (-1, -1), (12, 5), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "statBuff-lunchBreak"){
            return (powerupIdentity, "statBuff", "LunchBreak", imageLunchBreak, "[Stat Buff]", "<line-height=130%>Heal {8} health",
                    (18, 28), (-1, -1), (30, 15), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "statBuff-tavernBuffet"){
            return (powerupIdentity, "statBuff", "Tavern Buffet", imageTavernBuffet, "[Stat Buff]", "<line-height=130%>Heal to full health",
                    (30, 46), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "statBuff-hardenedFists"){
            return (powerupIdentity, "statBuff", "Hardened Fists", imageHardenedFists, "[Stat Buff]", "<line-height=130%>Permanently increases your direct contact damage by {8}",
                    (12, 20), (-1, -1), (4, 2), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "statBuff-ironFists"){
            return (powerupIdentity, "statBuff", "Iron Fists", imageIronFists, "[Stat Buff]", "<line-height=130%>Permanently increases your direct contact damage by {8}",
                    (20, 34), (-1, -1), (7, 4), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "statBuff-diamondFists"){
            return (powerupIdentity, "statBuff", "Diamond Fists", imageDiamondFists, "[Stat Buff]", "<line-height=130%>Increases your direct contact damage by {8}",
                    (34, 52), (-1, -1), (10, 7), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "statBuff-toughenedHeart"){
            return (powerupIdentity, "statBuff", "Toughened Heart", imageToughenedHeart, "[Stat Buff]", "<line-height=130%>Permanently increases your max health by {8} and heal {9} health",
                    (12, 24), (-1, -1), (8, 4), (8, 4), (-1, -1));
        }else if (powerupIdentity == "statBuff-unbreakableHeart"){
            return (powerupIdentity, "statBuff", "Unbreakable Heart", imageUnbreakableHeart, "[Stat Buff]", "<line-height=130%>Permanently increases your max health by {8} and heal {9} health",
                    (24, 36), (-1, -1), (15, 8), (15, 8), (-1, -1));
        }else if (powerupIdentity == "statBuff-weaponProficiency"){
            return (powerupIdentity, "statBuff", "Weapon Proficiency", imageWeaponProficiency, "[Stat Buff]", "<line-height=130%>Permanently increase all damage you deal by {8}",
                    (12, 24), (-1, -1), (2, 1), (-1, -1), (-1, -1));
        }else if (powerupIdentity == "statBuff-weaponMastery"){
            return (powerupIdentity, "statBuff", "Weapon Mastery", imageWeaponMastery, "[Stat Buff]", "<line-height=130%>Permanently increase all damage you deal by {8}",
                    (24, 36), (-1, -1), (4, 3), (-1, -1), (-1, -1));
        }



        else{
            Debug.LogWarning("Unknown Powerup Name:" + powerupIdentity);
            return (powerupIdentity, "???", "Unkown Powerup Name!!!", imageMissing, "??????????", "??????????",
                    (-1, -1), (-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }
    }




    public Sprite GetPowerupSprite(string powerupIdentity)
    {
        if      (powerupIdentity == "active-knife")                { return imageKnife; }
        else if (powerupIdentity == "active-spear")                { return imageSpear; }
        else if (powerupIdentity == "active-hatchet")              { return imageHatchet; }
        else if (powerupIdentity == "active-slingshot")            { return imageSlingshot; }
        else if (powerupIdentity == "active-sniper")               { return imageSniper; }
        else if (powerupIdentity == "active-lightningBolt")        { return imageLightningBolt; }
        else if (powerupIdentity == "active-bomb")                 { return imageBomb; }
        else if (powerupIdentity == "active-fireball")             { return imageFireball; }
        else if (powerupIdentity == "active-arrowVolley")          { return imageArrowVolley; }
        else if (powerupIdentity == "active-acidRain")             { return imageAcidRain; }
        else if (powerupIdentity == "active-axe")                  { return imageAxe; }
        else if (powerupIdentity == "active-spikedClub")           { return imageSpikedClub; }
        else if (powerupIdentity == "active-whip")                 { return imageWhip; }
        else if (powerupIdentity == "active-teleport")             { return imageTeleport; }
        else if (powerupIdentity == "active-dodge")                { return imageDodge; }

        else if (powerupIdentity == "passive-lightArmor")          { return imageLightArmor; }
        else if (powerupIdentity == "passive-heavyArmor")          { return imageHeavyArmor; }
        else if (powerupIdentity == "passive-diamondArmor")        { return imageDiamondArmor; }
        else if (powerupIdentity == "passive-innerHealing")        { return imageInnerHealing; }
        else if (powerupIdentity == "passive-vampiric")            { return imageVampiric; }
        else if (powerupIdentity == "passive-wellDeservedRest")    { return imageWellDeservedRest; }
        else if (powerupIdentity == "passive-bloodlust")           { return imageBloodlust; }
        else if (powerupIdentity == "passive-groundPound")         { return imageGroundPound; }
        else if (powerupIdentity == "passive-mercenaryTools")      { return imageMercenaryTools; }
        else if (powerupIdentity == "passive-pickpocket")          { return imagePickpocket; }
        else if (powerupIdentity == "passive-flexibleMovementI")   { return imageFlexibleMovementI; }
        else if (powerupIdentity == "passive-flexibleMovementII")  { return imageFlexibleMovementII; }
        else if (powerupIdentity == "passive-flexibleMovementIII") { return imageFlexibleMovementIII; }
        else if (powerupIdentity == "passive-flexibleMovementIV")  { return imageFlexibleMovementIV; }
        else if (powerupIdentity == "passive-flexibleMovementV")   { return imageFlexibleMovementV; }
        else if (powerupIdentity == "passive-flexibleMovementVI")  { return imageFlexibleMovementVI; }
        else if (powerupIdentity == "passive-flexibleMovementVII") { return imageFlexibleMovementVII; }
        else if (powerupIdentity == "passive-flexibleMovementVIII"){ return imageFlexibleMovementVIII; }

        else if (powerupIdentity == "statBuff-smallSnack")         { return imageSmallSnack; }
        else if (powerupIdentity == "statBuff-lunchBreak")         { return imageLunchBreak; }
        else if (powerupIdentity == "statBuff-tavernBuffet")       { return imageTavernBuffet; }
        else if (powerupIdentity == "statBuff-hardenedFists")      { return imageHardenedFists; }
        else if (powerupIdentity == "statBuff-ironFists")          { return imageIronFists; }
        else if (powerupIdentity == "statBuff-diamondFists")       { return imageDiamondFists; }
        else if (powerupIdentity == "statBuff-toughenedHeart")     { return imageToughenedHeart; }
        else if (powerupIdentity == "statBuff-unbreakableHeart")   { return imageUnbreakableHeart; }
        else if (powerupIdentity == "statBuff-weaponProficiency")  { return imageWeaponProficiency; }
        else if (powerupIdentity == "statBuff-weaponMastery")      { return imageWeaponMastery; }

        else                                                       { return imageMissing; }
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

    public int ActivateThisActivePowerup(string powerupIdentity, string powerupAction) // It's not really "activate" but more of getting an information value from an active powerup
    {                                                                                  // called when clicked in bottombar controller or for certain damaging powerups in playerandenemystatus controller
        int cooldownEasy = GetPowerupInfo(powerupIdentity).Item8.Item1;
        int cooldownHard = GetPowerupInfo(powerupIdentity).Item8.Item2;
        int valueEasy = GetPowerupInfo(powerupIdentity).Item9.Item1;
        int valueHard = GetPowerupInfo(powerupIdentity).Item9.Item2;
        int radius = GetPowerupInfo(powerupIdentity).Item10.Item1;
        float difficultyIndex = dynamicDifficultyController.GetDynamicOutput("powerupQuality");
        int cooldownReturn = Mathf.RoundToInt(cooldownEasy + (cooldownHard - cooldownEasy) * difficultyIndex); // the return value if powerupAction == "cooldown"
        int valueReturn = Mathf.RoundToInt(valueEasy + (valueHard - valueEasy) * difficultyIndex); // the return value if powerupAction == "value"

        if (powerupAction == "cooldown") { return cooldownReturn; }
        else if (powerupAction == "value") { return valueReturn; }
        else if (powerupAction == "radius") { return radius; }
        else { Debug.LogWarning("powerupAction unidentified:" + powerupAction); }

        ////                                                                Called at:
        //if      (powerupIdentity == "active-knife")                { } // all of these at bottombar controller?
        //else if (powerupIdentity == "active-spear")                { }
        //else if (powerupIdentity == "active-hatchet")              { }
        //else if (powerupIdentity == "active-slingshot")            { }
        //else if (powerupIdentity == "active-sniper")               { }
        //else if (powerupIdentity == "active-lightningBolt")        { }
        //else if (powerupIdentity == "active-bomb")                 { }
        //else if (powerupIdentity == "active-fireball")             { }
        //else if (powerupIdentity == "active-arrowVolley")          { }
        //else if (powerupIdentity == "active-acidRain")             { }
        //else if (powerupIdentity == "active-axe")                  { }
        //else if (powerupIdentity == "active-spikedClub")           { }
        //else if (powerupIdentity == "active-whip")                 { }
        //else if (powerupIdentity == "active-teleport")             { }
        //else if (powerupIdentity == "active-dodge")                { }

        //Debug.LogWarning("Canot Find Active Powerup of identity:" + powerupIdentity);
        return -1;
    }

    private int bloodlustBuff = 0;
    public int ActivateThisPassivePowerup(string powerupIdentity, string powerupAction)
    {
        int valueEasy = GetPowerupInfo(powerupIdentity).Item9.Item1;
        int valueHard = GetPowerupInfo(powerupIdentity).Item9.Item2;
        float difficultyIndex = dynamicDifficultyController.GetDynamicOutput("powerupQuality");
        int valueReturn = Mathf.RoundToInt(valueEasy + (valueHard - valueEasy) * difficultyIndex);

        // the return -1 here just means that these powerups don't give a DGB value.          Called at:
        if      (powerupIdentity == "passive-lightArmor")          { return valueReturn; } // CodeForPrefabPlayer.PlayerTakesDamage
        else if (powerupIdentity == "passive-heavyArmor")          { return valueReturn; } // CodeForPrefabPlayer.PlayerTakesDamage
        else if (powerupIdentity == "passive-diamondArmor")        { return valueReturn; } // CodeForPrefabPlayer.PlayerTakesDamage
        else if (powerupIdentity == "passive-innerHealing")        { return valueReturn; } // TurnController.PlayerTurn
        else if (powerupIdentity == "passive-vampiric")            { return valueReturn; } // PlayerAndEnemyStatusController.AnEnemyWasKilledAndEarnGold
        else if (powerupIdentity == "passive-wellDeservedRest")    { return valueReturn; } // PlayerAndEnemyStatusController.AnEnemyWasKilledAndEarnGold
        else if (powerupIdentity == "passive-bloodlust")           {                       
            if (powerupAction == "reset") { bloodlustBuff = 0; return -1;}                 //     PlayerAndEnemyStatusController.AnEnemyWasKilledAndEarnGold
            else if (powerupAction == "buff") { bloodlustBuff += valueReturn; return -1;}  //     CodeForPrefabPlayer.PlayerTakesDamage
            else { return bloodlustBuff; }                                                 //     CodeForPrefabEnemy.ThisEnemyTakesDamage
        }
        //else if (powerupIdentity == "passive-groundPound")         { return valueReturn; }
        else if (powerupIdentity == "passive-mercenaryTools")      { return -1; }          // CodeForPrefabPlayer.PlayerTakesDamage , CodeForPrefabEnemy.ThisEnemyTakesDamage , TurnController.PlayerTurn
        else if (powerupIdentity == "passive-pickpocket")          { return valueReturn; } // PlayerAndEnemyStatusController.AnEnemyWasKilledAndEarnGold
        else if (powerupIdentity == "passive-flexibleMovementI")   { return -1; }          // CodeForPrefabPlayer.PlayerCanMoveNow
        else if (powerupIdentity == "passive-flexibleMovementII")  { return -1; }          // CodeForPrefabPlayer.PlayerCanMoveNow
        else if (powerupIdentity == "passive-flexibleMovementIII") { return -1; }          // CodeForPrefabPlayer.PlayerCanMoveNow
        else if (powerupIdentity == "passive-flexibleMovementIV")  { return -1; }          // CodeForPrefabPlayer.PlayerCanMoveNow
        else if (powerupIdentity == "passive-flexibleMovementV")   { return -1; }          // CodeForPrefabPlayer.PlayerCanMoveNow
        else if (powerupIdentity == "passive-flexibleMovementVI")  { return -1; }          // CodeForPrefabPlayer.PlayerCanMoveNow
        else if (powerupIdentity == "passive-flexibleMovementVII") { return -1; }          // CodeForPrefabPlayer.PlayerCanMoveNow
        else if (powerupIdentity == "passive-flexibleMovementVIII"){ return -1; }          // CodeForPrefabPlayer.PlayerCanMoveNow

        Debug.LogWarning("Cannot Find Passive Powerup of identity:" + powerupIdentity);
        return -1;
    }

    public void ActivateThisStatBuffPoweup(string powerupIdentity)
    {
        float difficultyIndex = dynamicDifficultyController.GetDynamicOutput("powerupQuality");
        var powerupInfo = GetPowerupInfo(powerupIdentity);
        (int easyNumberA, int hardNumberA) = powerupInfo.Item9;
        (int easyNumberB, int hardNumberB) = powerupInfo.Item10;
        int numberA = Mathf.RoundToInt(easyNumberA + (hardNumberA - easyNumberA) * difficultyIndex);
        int numberB = Mathf.RoundToInt(easyNumberB + (hardNumberB - easyNumberB) * difficultyIndex);

        // Player buy a powerup -> slightly increase powerup usage DGB input
        dynamicDifficultyController.SetDynamicInputChange("powerupUsage", +0.1f, false);

        // Reduce DGB Input powerup usage if player overheals past max health with healing stat buff powerups
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
