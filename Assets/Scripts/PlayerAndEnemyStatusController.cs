using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using UnityEditor;
//using System.Xml;
//using Unity.VisualScripting;
//using static UnityEditor.FilePathAttribute;

public class PlayerAndEnemyStatusController : MonoBehaviour
{

    [Header("Prefab")]
    //[SerializeField] private GameObject playerPrefabOldImageUI; // Assign the "chessPiece" prefab in the Inspector
    //[SerializeField] private GameObject enemyPrefabOldImageUI; // Assign the "chessPiece" prefab in the Inspector
    //[SerializeField] private GameObject battleBoardOldImageUI; // Assign the "battleBoard" GameObject in the Inspector

    [SerializeField] private GameObject playerPrefab; // Assign the "chessPiece" prefab in the Inspector
    [SerializeField] private GameObject enemyPrefab; // Assign the "chessPiece" prefab in the Inspector
    [SerializeField] private GameObject battleBoard; // Assign the "battleBoard" GameObject in the Inspector

    [Header("8x16 Move Tiles")]
    [SerializeField] GameObject full8x16MoveTiles;
    [SerializeField] private GameObject[] individual8x16MoveTiles;

    [Header("Controllers")]
    [SerializeField] private TurnController turnController;
    [SerializeField] private SideBarController sideBarController;
    [SerializeField] private ActualXYCoordinatesController actualXYCoordinates;
    [SerializeField] private BattleModeController battleModeController;
    [SerializeField] private BestiaryController bestiaryController;
    [SerializeField] private DynamicDifficultyController dynamicDifficultyController;
    [SerializeField] private GenerateStatisticsController generateStatisticsController;
    [SerializeField] private BottomBarController bottomBarController;
    [SerializeField] private PowerupsCatalogController powerupsCatalogController;

    private int roundNumber;
    private int killCount;
    private int totalMoveCount;
    private int moveCountThisRound;
    private int currEnemiesLeftThisRound;
    private int totalEnemiesThisRound;
    private int maxNoOfEnemiesAtAnyPoint;
    private int enemyPointsToAllocate;
    private int numberOfTimesEnemiesTookDamageThisRound;
    private int gracePeriodBeforeReducingBonusGold;
    private int numberOfTimesToDecreaseBonusGold;
    private int currPlayerHealthPoint;
    private int maxPlayerHealthPoint;
    private int currGold;
    private int goldGainedSoFar;
    private int bonusGoldAtEndOfRound;
    //private float timeToMove;
    private int playerDirectContactDamage;
    private int playerBonusDamage;
    private int[,] playerMoveTiles;
    CodeForPrefabPlayer codeForPrefabPlayer;
    private bool hasAnEnemyAlreadyBeenDamagedThisTurn;
    private bool hasPlayerAlreadyBeenDamagedThisTurn;
    private int turnsPassedWithoutUsingActivePowerupThatIsOffCooldown;

    //private int enemiesAlreadySpawnedThisRound;
    private string[] enemyVariantsToBeSpawnedThisRound;
    private int enemiesSpawnedSoFarThisRound;
    private string whichEnemyVariantToSpawn;
    private GameObject[,] identitiesOfPiecesInBoard; // Make an array of game objects
    private int[][] individual8x16MoveTilesToSpawn = new int[][] { new int[] { 1, 1}, new int[] { 1, 0}, new int[] { 1,-1}, new int[] { 0,-1}, new int[] {-1,-1}, new int[] {-1, 0}, new int[] {-1, 1}, new int[] { 0, 1}, new int[] { 2, 2}, new int[] { 2, 1}, new int[] { 2, 0}, new int[] { 2,-1}, new int[] { 2,-2}, new int[] { 1,-2}, new int[] { 0,-2}, new int[] {-1,-2}, new int[] {-2,-2}, new int[] {-2,-1}, new int[] {-2, 0}, new int[] {-2, 1}, new int[] {-2, 2}, new int[] {-1, 2}, new int[] { 0, 2}, new int[] { 1, 2}, new int[] { 3, 3}, new int[] { 3, 2}, new int[] { 3, 1}, new int[] { 3, 0}, new int[] { 3,-1}, new int[] { 3,-2}, new int[] { 3,-3}, new int[] { 2,-3}, new int[] { 1,-3}, new int[] { 0,-3}, new int[] {-1,-3}, new int[] {-2,-3}, new int[] {-3,-3}, new int[] {-3,-2}, new int[] {-3,-1}, new int[] {-3, 0}, new int[] {-3, 1}, new int[] {-3, 2}, new int[] {-3, 3}, new int[] {-2, 3}, new int[] {-1, 3}, new int[] { 0, 3}, new int[] { 1, 3}, new int[] { 2, 3}};
    private int[][] individual8x16MoveTilesToAvoid = new int[][] { new int[] { 1, 1}, new int[] { 1, 0}, new int[] { 1,-1}, new int[] { 0,-1}, new int[] {-1,-1}, new int[] {-1, 0}, new int[] {-1, 1}, new int[] { 0, 1}, new int[] { 2, 2}, new int[] { 2, 1}, new int[] { 2, 0}, new int[] { 2,-1}, new int[] { 2,-2}, new int[] { 1,-2}, new int[] { 0,-2}, new int[] {-1,-2}, new int[] {-2,-2}, new int[] {-2,-1}, new int[] {-2, 0}, new int[] {-2, 1}, new int[] {-2, 2}, new int[] {-1, 2}, new int[] { 0, 2}, new int[] { 1, 2}, new int[] { 3, 3}, new int[] { 3, 2}, new int[] { 3, 1}, new int[] { 3, 0}, new int[] { 3,-1}, new int[] { 3,-2}, new int[] { 3,-3}, new int[] { 2,-3}, new int[] { 1,-3}, new int[] { 0,-3}, new int[] {-1,-3}, new int[] {-2,-3}, new int[] {-3,-3}, new int[] {-3,-2}, new int[] {-3,-1}, new int[] {-3, 0}, new int[] {-3, 1}, new int[] {-3, 2}, new int[] {-3, 3}, new int[] {-2, 3}, new int[] {-1, 3}, new int[] { 0, 3}, new int[] { 1, 3}, new int[] { 2, 3}};
    private string currentActivePowerupIdentity = "";

    // When accessing a specific gameobject prefab in the 2d array, you can just access one of its function, like getting damaged.
    // Move a piece, move the reference to the other tile coordinates.
    // Changing the prefab being referenced in the 2d array doesnot immeadiately dictate where the prefab should be
    // For example, the object's references changes from [0,0] to [1,2], but it's position is still in the [0,0] tile
    // You still have to manually move it to the correct tile.

    // Start is called before the first frame update
    void Start()
    {
        // Set several variables, and also display them in the SideBar
        roundNumber = 1;
        currPlayerHealthPoint = 100;
        maxPlayerHealthPoint = 100;
        currGold = 10020; // TEMPORARY GOLD START XXXXXXXXXXXXXX
        goldGainedSoFar = 0;
        //if (PlayerPrefs.GetString("modeDifficulty", "Adaptive") == "Easy")
        //{
        //    timeToMove = 6.0f;
        //}
        //else if (PlayerPrefs.GetString("modeDifficulty", "Adaptive") == "Medium" || PlayerPrefs.GetString("modeDifficulty", "Adaptive") == "Adaptive")
        //{
        //    timeToMove = 3.0f;
        //}
        //else if (PlayerPrefs.GetString("modeDifficulty", "Adaptive") == "Hard")
        //{
        //    timeToMove = 1.5f;
        //}
        sideBarController.SetSideBarRoundNumber(roundNumber);
        sideBarController.SetSideBarCurrPlayerHealthPointValue(currPlayerHealthPoint);
        sideBarController.SetSideBarMaxPlayerHealthPointValue(maxPlayerHealthPoint);
        sideBarController.SetSideBarCurrGoldValue(currGold);

        playerDirectContactDamage = 10; // The damage the player deals upon DirectContact on another enemy
        playerBonusDamage = 0; // Bonus damage applied to both direct contact or from powerups

        killCount = 0;
        totalMoveCount = -1;


        //enemiesAlreadySpawnedThisRound = 0;

        // Start with a clean empty 2d string array
        ResetTheIdentitiesOfPiecesInBoardToBlank();
    }
    public void SpawnPlayerAndEnemiesForNewRound()
    {
        //// Instantiate the chessPiece prefab
        //GameObject chessPieceInstance = Instantiate(playerPrefabOldImageUI, battleBoardOldImageUI.transform);

        //// Instatiate the enemyPiece prefab (right now only fixed 2, but later control its amount by the round number / difficulty mode / DGB. Ex: out of 10 enemies, only 4 may exist at any tim
        //// Also determine what enemy this is (variant, and string name for 2d array). Maybe need a new script / function to determine their identity and stats?
        //whichEnemyVariantToSpawn = "pawnGoblin";
        //GameObject enemyPieceInstance1 = Instantiate(enemyPrefabOldImageUI, battleBoardOldImageUI.transform);
        //whichEnemyVariantToSpawn = "kingLich";
        //GameObject enemyPieceInstance2 = Instantiate(enemyPrefabOldImageUI, battleBoardOldImageUI.transform);


        // Instantiate the chessPiece prefab first before the enemies
        Instantiate(playerPrefab, battleBoard.transform);
        GameObject prefabPlayer = GameObject.FindWithTag("tagForPrefabPlayer");
        codeForPrefabPlayer = prefabPlayer.GetComponent<CodeForPrefabPlayer>();

        // Now Randomize the instantiation of enemies
        float difficultyIndex = dynamicDifficultyController.GetDynamicOutput("enemyStats");
        maxNoOfEnemiesAtAnyPoint = Math.Min(14, (int)Math.Round((double)((difficultyIndex * 2 + 2) + roundNumber * (difficultyIndex * 0.33333 + 0.33333))));
        enemyPointsToAllocate = (int)Math.Round((double)( (roundNumber + 1) * (difficultyIndex * 2 + 1)) + 0.03 * Math.Pow(roundNumber, 1 + 3 * difficultyIndex));

        enemyVariantsToBeSpawnedThisRound = bestiaryController.DecideWhatEnemiesToSpawnThisRound(enemyPointsToAllocate);
        currEnemiesLeftThisRound = enemyVariantsToBeSpawnedThisRound.Length;
        totalEnemiesThisRound = currEnemiesLeftThisRound;
        hasAnEnemyAlreadyBeenDamagedThisTurn = false;

        sideBarController.SetSideBarCurrEnemiesLeftValue(currEnemiesLeftThisRound);
        sideBarController.SetSideBarTotalEnemiesThisRoundValue(totalEnemiesThisRound);

        full8x16MoveTiles.SetActive(false);

        enemiesSpawnedSoFarThisRound = 0;
        while (enemiesSpawnedSoFarThisRound < totalEnemiesThisRound && enemiesSpawnedSoFarThisRound < maxNoOfEnemiesAtAnyPoint)
        {
            whichEnemyVariantToSpawn = enemyVariantsToBeSpawnedThisRound[enemiesSpawnedSoFarThisRound];
            Instantiate(enemyPrefab, battleBoard.transform);
            enemiesSpawnedSoFarThisRound += 1;
        }

        // Player gets more bonus gold (after clearing a round) for finishing in less moves
        bonusGoldAtEndOfRound = 4 + roundNumber * 4; // might be better to make it dependant on round number than enemyPointsToAllocate
        //bonusGoldAtEndOfRound = enemyPointsToAllocate * 2;
        numberOfTimesEnemiesTookDamageThisRound = 0;
        gracePeriodBeforeReducingBonusGold = 6; // actually supposed to be 3-move grace period, but new round starts with player's turn, which immeadiately decreases it by 1. Also give players more time to attack an enemy if they are far away before reducing their bonus gold
        numberOfTimesToDecreaseBonusGold = 0;
        turnsPassedWithoutUsingActivePowerupThatIsOffCooldown = -1; // actually supposed to be 0, but new round starts with player's turn, which immeadiately sets it to 0

        //Finally, start with the player's turn
        turnController.RememberNewlySpawnedPlayerForNewRound();
        turnController.PlayerTurn();
    }

    public void DestroyAllPlayerAndEnemyPrefabs()
    {
        // Find all GameObjects with the specified tags
        GameObject[] taggedPlayerAndEnemyObjects = GameObject.FindGameObjectsWithTag("tagForPrefabEnemy")
            .Concat(GameObject.FindGameObjectsWithTag("tagForPrefabPlayer"))
            .Concat(GameObject.FindGameObjectsWithTag("tagForPrefabEnemyMoveTile"))
            .Concat(GameObject.FindGameObjectsWithTag("tagForPrefabPlayerMoveTile"))
            .ToArray();

        // Destroy each GameObject
        foreach (GameObject obj in taggedPlayerAndEnemyObjects)
        {
            Destroy(obj);
        }

        // Then clear the names of the 2d array as well
        ResetTheIdentitiesOfPiecesInBoardToBlank();
    }

    public void ResetTheIdentitiesOfPiecesInBoardToBlank()
    {
        // Initialize the 2d array
        identitiesOfPiecesInBoard = new GameObject[16, 8];
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                identitiesOfPiecesInBoard[x, y] = null; // Setting all positions to unoccupied
            }
        }
    }

    public void AnEnemyWasKilledAndEarnGold(int goldEarned)
    {
        //Debug.Log(Mathf.Max((int)(bonusGoldAtEndOfRound - moveCountThisRound * 0.25 + numberOfTimesEnemiesTookDamageThisRound * 0.5), 0));
        currEnemiesLeftThisRound -= 1;
        sideBarController.SetSideBarCurrEnemiesLeftValue(currEnemiesLeftThisRound);
        SetKillCountIncreaseBy1();

        // PASSIVE POWERUP
        if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-pickpocket")) { goldEarned += powerupsCatalogController.ActivateThisPassivePowerup("passive-pickpocket", ""); }
        SetChangeInCurrGold(goldEarned);
        codeForPrefabPlayer.ShowGoldEarned(goldEarned);

        if (currEnemiesLeftThisRound == 0) // All enemies killed, victory for this round
        {
            // at end of each round, update the logs of both per turn and per round
            PrintAndLogPerTurnHealthKillsPointsGold();
            dynamicDifficultyController.PrintAndLogPerTurnAllDGBInputAndOutputIndex();
            PrintAndLogPerRoundHealthKillsPointsGoldMoves();
            dynamicDifficultyController.PrintAndLogPerRoundAllDGBInputAndOutputIndex();


            // then, based on the moves, update the DGB input TimeThinkingAndStepsTaken and award the bonus gold
            float changeToDynamicInputIndexTimeThinkingAndStepsTaken = Mathf.Lerp(-0.3f, 0.3f, Mathf.Max(bonusGoldAtEndOfRound - numberOfTimesToDecreaseBonusGold, 0) / bonusGoldAtEndOfRound);
            //float changeToDynamicInputIndexTimeThinkingAndStepsTaken = Mathf.Clamp((float)((bonusGoldAtEndOfRound - moveCountThisRound * 0.25 + numberOfTimesEnemiesTookDamageThisRound * 0.5) / 10), -0.3f, 0.3f);
            bonusGoldAtEndOfRound = Mathf.Max(bonusGoldAtEndOfRound - numberOfTimesToDecreaseBonusGold, 0);
            //bonusGoldAtEndOfRound = Mathf.Max((int)(bonusGoldAtEndOfRound - moveCountThisRound * 0.25 + numberOfTimesEnemiesTookDamageThisRound * 0.5), 0);
            dynamicDifficultyController.SetDynamicInputChange("TimeThinkingAndStepsTaken", changeToDynamicInputIndexTimeThinkingAndStepsTaken, false);
            battleModeController.SetTextBonusGold(bonusGoldAtEndOfRound);
            SetChangeInCurrGold(bonusGoldAtEndOfRound);

            //Debug.Log("Bonus Gold:" + bonusGoldAtEndOfRound + " , Potential maxx bonus gold:" + (10 + roundNumber * 5) + " , Change in DGB Input TimeThinkingAndStepsTaken:" + changeToDynamicInputIndexTimeThinkingAndStepsTaken);

            // PASSIVE POWERUP
            if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-bloodlust")) { int dummyVariable = powerupsCatalogController.ActivateThisPassivePowerup("passive-bloodlust", "reset"); }

            // Proceed to the victory screen
            // PASSIVE POWERUP
            ResetMoveCountThisRound(); // for the inner healing powerup
            if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-vampiric") && bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-wellDeservedRest")) { Invoke(nameof(PassiveVampiric), 0.5f); Invoke(nameof(PassiveWellDeservedRrest), 1.0f); Invoke(nameof(VictoryBattle), 1.5f); }
            else if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-vampiric")) { Invoke(nameof(PassiveVampiric), 0.5f); Invoke(nameof(VictoryBattle), 1.0f); }
            else if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-wellDeservedRest")){ Invoke(nameof(PassiveWellDeservedRrest), 0.5f); Invoke(nameof(VictoryBattle), 1.0f); }
            else { Invoke(nameof(VictoryBattle), 0.5f); }
            
        }
        else // there are still enemies left this round
        {
            if (enemiesSpawnedSoFarThisRound < totalEnemiesThisRound) // If there are still more enemies to spawn this round, replace the dead enemy with a new one
            {
                whichEnemyVariantToSpawn = enemyVariantsToBeSpawnedThisRound[enemiesSpawnedSoFarThisRound];
                Instantiate(enemyPrefab, battleBoard.transform); // not too far nor too close from player
                enemiesSpawnedSoFarThisRound += 1;
            }

            // PASSIVE POWERUP
            if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-vampiric")) { Invoke(nameof(PassiveVampiric), 0.5f); Invoke(nameof(CallForEnemyTurn), 1.0f); }
            
            else { if (currentActivePowerupIdentity == "") { Invoke(nameof(CallForEnemyTurn), 0.5f); } } // If player kills an enemy with an active powerup, the player prefab will call the enemy turn themselves. else, this controller will call it

        }

    }

    public void PassiveWellDeservedRrest()
    {
        codeForPrefabPlayer.PlayerTakesDamageOrHealing(powerupsCatalogController.ActivateThisPassivePowerup("passive-wellDeservedRest", ""), false);
    }

    public void PassiveVampiric()
    {
        codeForPrefabPlayer.PlayerTakesDamageOrHealing(powerupsCatalogController.ActivateThisPassivePowerup("passive-vampiric", ""), false);
    }

    public void VictoryBattle()
    {
        battleModeController.BattleModeChanger("Victory");
    }

    public void PlayerDied()
    {
        battleModeController.BattleModeChanger("GameOver");
    }

    public void CallForEnemyTurn()
    {
        turnController.AllEnemiesTurn();
    }

    public void PrintAndLogPerTurnHealthKillsPointsGold() // ENEMY POINTS GAINED NOT YET FIXED
    {
        Debug.Log("[PER TURN] Player Hp: " + currPlayerHealthPoint + " --- Kill Count: " + killCount + ", Enemy Points Gained: " + killCount + ", Gold Gained So Far: " + goldGainedSoFar + ", Currrent Gold: " + currGold);
        generateStatisticsController.LogPerTurnHealthKillsPointsGold(currPlayerHealthPoint.ToString(), killCount.ToString(), killCount.ToString(), goldGainedSoFar.ToString(), currGold.ToString());
    }

    public void PrintAndLogPerRoundHealthKillsPointsGoldMoves() // ENEMY POINTS GAINED NOT YET FIXED
    {
        Debug.Log("[===PER ROUND===] Player Hp: " + currPlayerHealthPoint + " --- Kill Count: " + killCount + ", Enemy Points Gained: " + killCount + ", Gold Gained So Far: " + goldGainedSoFar + ", Currrent Gold: " + currGold + ", Total Moves: " + totalMoveCount);
        generateStatisticsController.LogPerRoundHealthKillsPointsGoldMoves(currPlayerHealthPoint.ToString(), killCount.ToString(), killCount.ToString(), goldGainedSoFar.ToString(), currGold.ToString(), totalMoveCount.ToString());
    }


    public void AnActivePowerupWasActivated(string powerupIdentity)
    {
        //dynamicDifficultyController.SetDynamicInputChange("powerupUsage", +0.05f, false); // Probably Don't change DGB here? change it depending on how the powerup was used, like hitting or missing an enemy
        currentActivePowerupIdentity = powerupIdentity;
        codeForPrefabPlayer.DeactivateMoveTiles();

        // TEMPORARY: just skip to the enemy's turn immeadiately
        //codeForPrefabPlayer.EndMoveTimerFromActivePowerup();
        //Debug.Log("TurnController has received the active Powerup:" + powerupIdentity);

        // Click on a move tile to activate specific abilities, reset the player timer. Timer's up = skip to enemy's turn
        if (powerupIdentity == "active-knife" || powerupIdentity == "active-spear" || powerupIdentity == "active-hatchet" || powerupIdentity == "active-slingshot" || powerupIdentity == "active-sniper" || powerupIdentity == "active-lightningBolt" ||
            powerupIdentity == "active-bomb" || powerupIdentity == "active-fireball" || powerupIdentity == "active-arrowVolley")
        {
            codeForPrefabPlayer.RestartTimer();
            //codeForPrefabPlayer.DeactivateMoveTiles(true);
            //codeForPrefabPlayer.ActivateMoveTiles(false, powerupIdentity);
            full8x16MoveTiles.SetActive(true); // the 8x16 tiles can appear, but which specific tiles to appear depends
            bool trueSpawnTilesFalseAvoidTiles = false;

            if (powerupIdentity == "active-knife")
            {
                trueSpawnTilesFalseAvoidTiles = true;
                individual8x16MoveTilesToSpawn = new int[][] { new int[] { 1, 1}, new int[] { 1, 0}, new int[] { 1,-1}, new int[] { 0,-1}, new int[] {-1,-1}, new int[] {-1, 0}, new int[] {-1, 1}, new int[] { 0, 1}};
            }
            else if (powerupIdentity == "active-spear")
            {
                trueSpawnTilesFalseAvoidTiles = true;
                individual8x16MoveTilesToSpawn = new int[][] { new int[] { 1, 1}, new int[] { 1, 0}, new int[] { 1,-1}, new int[] { 0,-1}, new int[] {-1,-1}, new int[] {-1, 0}, new int[] {-1, 1}, new int[] { 0, 1}, new int[] { 2, 2}, new int[] { 2, 1}, new int[] { 2, 0}, new int[] { 2,-1}, new int[] { 2,-2}, new int[] { 1,-2}, new int[] { 0,-2}, new int[] {-1,-2}, new int[] {-2,-2}, new int[] {-2,-1}, new int[] {-2, 0}, new int[] {-2, 1}, new int[] {-2, 2}, new int[] {-1, 2}, new int[] { 0, 2}, new int[] { 1, 2} };
            }
            else if (powerupIdentity == "active-hatchet")
            {
                trueSpawnTilesFalseAvoidTiles = true;
                individual8x16MoveTilesToSpawn = new int[][] { new int[] { 1, 1}, new int[] { 1, 0}, new int[] { 1,-1}, new int[] { 0,-1}, new int[] {-1,-1}, new int[] {-1, 0}, new int[] {-1, 1}, new int[] { 0, 1}, new int[] { 2, 2}, new int[] { 2, 1}, new int[] { 2, 0}, new int[] { 2,-1}, new int[] { 2,-2}, new int[] { 1,-2}, new int[] { 0,-2}, new int[] {-1,-2}, new int[] {-2,-2}, new int[] {-2,-1}, new int[] {-2, 0}, new int[] {-2, 1}, new int[] {-2, 2}, new int[] {-1, 2}, new int[] { 0, 2}, new int[] { 1, 2}, new int[] { 3, 3}, new int[] { 3, 2}, new int[] { 3, 1}, new int[] { 3, 0}, new int[] { 3,-1}, new int[] { 3,-2}, new int[] { 3,-3}, new int[] { 2,-3}, new int[] { 1,-3}, new int[] { 0,-3}, new int[] {-1,-3}, new int[] {-2,-3}, new int[] {-3,-3}, new int[] {-3,-2}, new int[] {-3,-1}, new int[] {-3, 0}, new int[] {-3, 1}, new int[] {-3, 2}, new int[] {-3, 3}, new int[] {-2, 3}, new int[] {-1, 3}, new int[] { 0, 3}, new int[] { 1, 3}, new int[] { 2, 3}};
            }
            else if (powerupIdentity == "active-slingshot")
            {
                trueSpawnTilesFalseAvoidTiles = false;
                individual8x16MoveTilesToAvoid = new int[][] { new int[] { 0, 0}, new int[] { 1, 1}, new int[] { 1, 0}, new int[] { 1,-1}, new int[] { 0,-1}, new int[] {-1,-1}, new int[] {-1, 0}, new int[] {-1, 1}, new int[] { 0, 1}, new int[] { 2, 2}, new int[] { 2, 1}, new int[] { 2, 0}, new int[] { 2,-1}, new int[] { 2,-2}, new int[] { 1,-2}, new int[] { 0,-2}, new int[] {-1,-2}, new int[] {-2,-2}, new int[] {-2,-1}, new int[] {-2, 0}, new int[] {-2, 1}, new int[] {-2, 2}, new int[] {-1, 2}, new int[] { 0, 2}, new int[] { 1, 2}, new int[] { 3, 3}, new int[] { 3, 2}, new int[] { 3, 1}, new int[] { 3, 0}, new int[] { 3,-1}, new int[] { 3,-2}, new int[] { 3,-3}, new int[] { 2,-3}, new int[] { 1,-3}, new int[] { 0,-3}, new int[] {-1,-3}, new int[] {-2,-3}, new int[] {-3,-3}, new int[] {-3,-2}, new int[] {-3,-1}, new int[] {-3, 0}, new int[] {-3, 1}, new int[] {-3, 2}, new int[] {-3, 3}, new int[] {-2, 3}, new int[] {-1, 3}, new int[] { 0, 3}, new int[] { 1, 3}, new int[] { 2, 3}};
            }
            else if (powerupIdentity == "active-sniper")
            {
                trueSpawnTilesFalseAvoidTiles = false;
                individual8x16MoveTilesToAvoid = new int[][] { new int[] { 0, 0}, new int[] { 1, 1}, new int[] { 1, 0}, new int[] { 1,-1}, new int[] { 0,-1}, new int[] {-1,-1}, new int[] {-1, 0}, new int[] {-1, 1}, new int[] { 0, 1}, new int[] { 2, 2}, new int[] { 2, 1}, new int[] { 2, 0}, new int[] { 2,-1}, new int[] { 2,-2}, new int[] { 1,-2}, new int[] { 0,-2}, new int[] {-1,-2}, new int[] {-2,-2}, new int[] {-2,-1}, new int[] {-2, 0}, new int[] {-2, 1}, new int[] {-2, 2}, new int[] {-1, 2}, new int[] { 0, 2}, new int[] { 1, 2}, new int[] { 3, 3}, new int[] { 3, 2}, new int[] { 3, 1}, new int[] { 3, 0}, new int[] { 3,-1}, new int[] { 3,-2}, new int[] { 3,-3}, new int[] { 2,-3}, new int[] { 1,-3}, new int[] { 0,-3}, new int[] {-1,-3}, new int[] {-2,-3}, new int[] {-3,-3}, new int[] {-3,-2}, new int[] {-3,-1}, new int[] {-3, 0}, new int[] {-3, 1}, new int[] {-3, 2}, new int[] {-3, 3}, new int[] {-2, 3}, new int[] {-1, 3}, new int[] { 0, 3}, new int[] { 1, 3}, new int[] { 2, 3}};
            }
            else if (powerupIdentity == "active-lightningBolt")
            {
                trueSpawnTilesFalseAvoidTiles = false;
                individual8x16MoveTilesToAvoid = new int[][] { new int[] { 0, 0}, new int[] { 1, 1}, new int[] { 1, 0}, new int[] { 1,-1}, new int[] { 0,-1}, new int[] {-1,-1}, new int[] {-1, 0}, new int[] {-1, 1}, new int[] { 0, 1}, new int[] { 2, 2}, new int[] { 2, 1}, new int[] { 2, 0}, new int[] { 2,-1}, new int[] { 2,-2}, new int[] { 1,-2}, new int[] { 0,-2}, new int[] {-1,-2}, new int[] {-2,-2}, new int[] {-2,-1}, new int[] {-2, 0}, new int[] {-2, 1}, new int[] {-2, 2}, new int[] {-1, 2}, new int[] { 0, 2}, new int[] { 1, 2}, new int[] { 3, 3}, new int[] { 3, 2}, new int[] { 3, 1}, new int[] { 3, 0}, new int[] { 3,-1}, new int[] { 3,-2}, new int[] { 3,-3}, new int[] { 2,-3}, new int[] { 1,-3}, new int[] { 0,-3}, new int[] {-1,-3}, new int[] {-2,-3}, new int[] {-3,-3}, new int[] {-3,-2}, new int[] {-3,-1}, new int[] {-3, 0}, new int[] {-3, 1}, new int[] {-3, 2}, new int[] {-3, 3}, new int[] {-2, 3}, new int[] {-1, 3}, new int[] { 0, 3}, new int[] { 1, 3}, new int[] { 2, 3}};
            }
            else if (powerupIdentity == "active-bomb")
            {
                trueSpawnTilesFalseAvoidTiles = false;
                individual8x16MoveTilesToAvoid = new int[][] { new int[] { 0, 0}, new int[] { 1, 1}, new int[] { 1, 0}, new int[] { 1,-1}, new int[] { 0,-1}, new int[] {-1,-1}, new int[] {-1, 0}, new int[] {-1, 1}, new int[] { 0, 1}, new int[] { 2, 2}, new int[] { 2, 1}, new int[] { 2, 0}, new int[] { 2,-1}, new int[] { 2,-2}, new int[] { 1,-2}, new int[] { 0,-2}, new int[] {-1,-2}, new int[] {-2,-2}, new int[] {-2,-1}, new int[] {-2, 0}, new int[] {-2, 1}, new int[] {-2, 2}, new int[] {-1, 2}, new int[] { 0, 2}, new int[] { 1, 2}, new int[] { 3, 3}, new int[] { 3, 2}, new int[] { 3, 1}, new int[] { 3, 0}, new int[] { 3,-1}, new int[] { 3,-2}, new int[] { 3,-3}, new int[] { 2,-3}, new int[] { 1,-3}, new int[] { 0,-3}, new int[] {-1,-3}, new int[] {-2,-3}, new int[] {-3,-3}, new int[] {-3,-2}, new int[] {-3,-1}, new int[] {-3, 0}, new int[] {-3, 1}, new int[] {-3, 2}, new int[] {-3, 3}, new int[] {-2, 3}, new int[] {-1, 3}, new int[] { 0, 3}, new int[] { 1, 3}, new int[] { 2, 3}};
            }
            else if (powerupIdentity == "active-fireball")
            {
                trueSpawnTilesFalseAvoidTiles = false;
                individual8x16MoveTilesToAvoid = new int[][] { new int[] { 0, 0}, new int[] { 1, 1}, new int[] { 1, 0}, new int[] { 1,-1}, new int[] { 0,-1}, new int[] {-1,-1}, new int[] {-1, 0}, new int[] {-1, 1}, new int[] { 0, 1}, new int[] { 2, 2}, new int[] { 2, 1}, new int[] { 2, 0}, new int[] { 2,-1}, new int[] { 2,-2}, new int[] { 1,-2}, new int[] { 0,-2}, new int[] {-1,-2}, new int[] {-2,-2}, new int[] {-2,-1}, new int[] {-2, 0}, new int[] {-2, 1}, new int[] {-2, 2}, new int[] {-1, 2}, new int[] { 0, 2}, new int[] { 1, 2}, new int[] { 3, 3}, new int[] { 3, 2}, new int[] { 3, 1}, new int[] { 3, 0}, new int[] { 3,-1}, new int[] { 3,-2}, new int[] { 3,-3}, new int[] { 2,-3}, new int[] { 1,-3}, new int[] { 0,-3}, new int[] {-1,-3}, new int[] {-2,-3}, new int[] {-3,-3}, new int[] {-3,-2}, new int[] {-3,-1}, new int[] {-3, 0}, new int[] {-3, 1}, new int[] {-3, 2}, new int[] {-3, 3}, new int[] {-2, 3}, new int[] {-1, 3}, new int[] { 0, 3}, new int[] { 1, 3}, new int[] { 2, 3}};
            }
            else if (powerupIdentity == "active-arrowVolley")
            {
                trueSpawnTilesFalseAvoidTiles = false;
                individual8x16MoveTilesToAvoid = new int[][] { new int[] { 0, 0}, new int[] { 1, 1}, new int[] { 1, 0}, new int[] { 1,-1}, new int[] { 0,-1}, new int[] {-1,-1}, new int[] {-1, 0}, new int[] {-1, 1}, new int[] { 0, 1}, new int[] { 2, 2}, new int[] { 2, 1}, new int[] { 2, 0}, new int[] { 2,-1}, new int[] { 2,-2}, new int[] { 1,-2}, new int[] { 0,-2}, new int[] {-1,-2}, new int[] {-2,-2}, new int[] {-2,-1}, new int[] {-2, 0}, new int[] {-2, 1}, new int[] {-2, 2}, new int[] {-1, 2}, new int[] { 0, 2}, new int[] { 1, 2}, new int[] { 3, 3}, new int[] { 3, 2}, new int[] { 3, 1}, new int[] { 3, 0}, new int[] { 3,-1}, new int[] { 3,-2}, new int[] { 3,-3}, new int[] { 2,-3}, new int[] { 1,-3}, new int[] { 0,-3}, new int[] {-1,-3}, new int[] {-2,-3}, new int[] {-3,-3}, new int[] {-3,-2}, new int[] {-3,-1}, new int[] {-3, 0}, new int[] {-3, 1}, new int[] {-3, 2}, new int[] {-3, 3}, new int[] {-2, 3}, new int[] {-1, 3}, new int[] { 0, 3}, new int[] { 1, 3}, new int[] { 2, 3}};
            }

            // Do we spawn tiles that are near the player or spawn tiles away from player?
            if (trueSpawnTilesFalseAvoidTiles) // activate close to player
            {
                for (int i = 0; i < individual8x16MoveTiles.Length; i++)
                {
                    individual8x16MoveTiles[i].SetActive(false);
                }

                (int playerCurrGridX, int playerCurrGridY) = codeForPrefabPlayer.GetPlayerCurrGridXAndCurrGridY();
                foreach (int[] offset in individual8x16MoveTilesToSpawn)
                {
                    int gridXOffset = offset[0];
                    int gridYOffset = offset[1];

                    if (actualXYCoordinates.IsThisStillInsideTheBoard(playerCurrGridX + gridXOffset, playerCurrGridY + gridYOffset))
                    {
                        int index = (playerCurrGridX + gridXOffset - 1) + (playerCurrGridY + gridYOffset - 1) * 16;
                        individual8x16MoveTiles[index].SetActive(true);
                    }
                }
            }
            else  // activate far from player
            {
                for (int i = 0; i < individual8x16MoveTiles.Length; i++)
                {
                    individual8x16MoveTiles[i].SetActive(true);
                }

                (int playerCurrGridX, int playerCurrGridY) = codeForPrefabPlayer.GetPlayerCurrGridXAndCurrGridY();
                foreach (int[] offset in individual8x16MoveTilesToAvoid)
                {
                    int gridXOffset = offset[0];
                    int gridYOffset = offset[1];

                    if (actualXYCoordinates.IsThisStillInsideTheBoard(playerCurrGridX + gridXOffset, playerCurrGridY + gridYOffset))
                    {
                        int index = (playerCurrGridX + gridXOffset - 1) + (playerCurrGridY + gridYOffset - 1) * 16;
                        individual8x16MoveTiles[index].SetActive(false);
                    }
                }
            }



        }

        // Can just do and effect immeadiately (ex: damaging enemies around the player), then it's the enemy's turn
        // Or some other effects
        else if (powerupIdentity == "active-acidRain" || powerupIdentity == "active-axe" || powerupIdentity == "active-spikedClub" || powerupIdentity == "active-whip" )
        {
            //codeForPrefabPlayer.EndMoveTimerFromActivePowerup(); // CHANGE THIS, JUST TEMPORARY

            (int playerCurrGridX, int playerCurrGridY) = codeForPrefabPlayer.GetPlayerCurrGridXAndCurrGridY();
            codeForPrefabPlayer.ActivateAreaDamageWithRadius(playerCurrGridX, playerCurrGridY, powerupsCatalogController.ActivateThisActivePowerup(currentActivePowerupIdentity, "value"), powerupsCatalogController.ActivateThisActivePowerup(currentActivePowerupIdentity, "radius"));
        }

        else if (powerupIdentity == "active-teleport") // done - EXCEPT SOME DGB INPUT XXXXX
        {
            codeForPrefabPlayer.RestartTimer();
            full8x16MoveTiles.SetActive(true); // the 8x16 tiles can appear, but which specific tiles to appear depends
            for (int i = 0; i < individual8x16MoveTiles.Length; i++)
            {
                GameObject pieceAtThisTile = GetIdentityOfPieceAtThisBoardTile((i % 16) + 1, (int)(i / 16) + 1);
                if (pieceAtThisTile == null) { individual8x16MoveTiles[i].SetActive(true); }
                else { individual8x16MoveTiles[i].SetActive(false); }
                
            }
        }

        else if (powerupIdentity == "active-dodge") // done - EXCEPT SOME DGB INPUT XXXXX
        {
            codeForPrefabPlayer.EndMoveTimerFromActivePowerup();
        }


        else
        {
            Debug.LogWarning("Active Powerup Identity Not Found:" + powerupIdentity);
        }
    }

    public void ATileFromTheFull8x16MoveTilesWasClicked(int xAndYLocation) // called by the 128 move tile buttons for active powerups
    {
        int xLocation = (int)(xAndYLocation / 10);
        int yLocation = xAndYLocation % 10;
        int individual8x16MoveTilesIndex = (yLocation - 1) * 16 + (xLocation - 1);
        //Debug.Log("x: " + xLocation + " , y: " + yLocation + " , tile index: " + individual8x16MoveTilesIndex);
        full8x16MoveTiles.SetActive(false);

        if (currentActivePowerupIdentity == "active-teleport")
        {
            codeForPrefabPlayer.PlayerMoveTileWasClicked(xAndYLocation);
            return;
        }

        codeForPrefabPlayer.ActivateAreaDamageWithRadius(xLocation, yLocation, powerupsCatalogController.ActivateThisActivePowerup(currentActivePowerupIdentity, "value"), powerupsCatalogController.ActivateThisActivePowerup(currentActivePowerupIdentity, "radius"));

        // TEMPORARY to just move on with the enemy's turn
        //codeForPrefabPlayer.EndMoveTimerFromActivePowerup();
    }

    // ====================================================
    // GETTER, SETTER

    //public float GetTimeToMove()
    //{
    //    return timeToMove;
    //}

    public GameObject GetIdentityOfPieceAtThisBoardTile(int gridX, int gridY)
    {
        return identitiesOfPiecesInBoard[gridX-1, gridY-1];
    }

    public void SetIdentityOfPieceAtThisBoardTile(int gridX, int gridY, GameObject pieceGameObjectIdentity)
    {
        identitiesOfPiecesInBoard[gridX-1, gridY-1] = pieceGameObjectIdentity;
        //Debug.Log(identitiesOfPiecesInBoard[gridX-1, gridY-1]);
    }

    public string GetWhichEnemyVariantToSpawn()
    {
        return whichEnemyVariantToSpawn;
    }

    public void SetNextRoundNumber()
    {
        roundNumber += 1;
        sideBarController.SetSideBarRoundNumber(roundNumber);
    }

    public int GetRoundNumber()
    {
        return roundNumber;
    }

    public int GetKillCount()
    {
        return killCount;
    }

    public void SetKillCountIncreaseBy1()
    {
        killCount += 1;
    }
    public int GetTotalMoveCount()
    {
        return totalMoveCount;
    }

    //public void SetTotalMoveCountIncreaseBy1()
    //{
    //    totalMoveCount += 1;
    //}

    public int GetMoveCountThisRound()
    {
        return moveCountThisRound;
    }

    public void SetMoveCountIncreaseBy1AndMaybeDeductBonusGold()
    {
        moveCountThisRound += 1;
        totalMoveCount += 1;
        if (gracePeriodBeforeReducingBonusGold == 0) { numberOfTimesToDecreaseBonusGold += 1; }
        else { gracePeriodBeforeReducingBonusGold -= 1; }
    }

    public void ResetMoveCountThisRound()
    {
        moveCountThisRound = 0;
    }

    public int GetCurrGold()
    {
        return currGold;
    }

    public void SetChangeInCurrGold(int changeInCurrGold)
    {
        goldGainedSoFar += changeInCurrGold;
        currGold += changeInCurrGold;
        sideBarController.SetSideBarCurrGoldValue(currGold);
    }

    public void SetNumberOfTimesEnemiesTookDamageThisRoundIncreaseBy1AndResetGracePeriodBeforeReducingBonusGold() // called anytime an enemy takes damage
    {
        numberOfTimesEnemiesTookDamageThisRound += 1;
        if (currentActivePowerupIdentity == "") { gracePeriodBeforeReducingBonusGold = 3; } // Only reset grace period if player attacks an enemy with landing damage. so staying far away and spamming ranged attacks is discouraged.
    }

    public int GetPlayerMaxHealth()
    {
        return maxPlayerHealthPoint;
    }

    public void SetPlayerMaxHealth(int newPlayerMaxHealth)
    {
        maxPlayerHealthPoint = newPlayerMaxHealth;
        sideBarController.SetSideBarMaxPlayerHealthPointValue(maxPlayerHealthPoint);
    }

    public int GetPlayerCurrHealth()
    {
        return currPlayerHealthPoint;
    }

    public void SetPlayerCurrHealth(int newPlayerCurrHealth)
    {
        currPlayerHealthPoint = newPlayerCurrHealth;
        if (currPlayerHealthPoint > maxPlayerHealthPoint) { currPlayerHealthPoint = maxPlayerHealthPoint; }
        sideBarController.SetSideBarCurrPlayerHealthPointValue(currPlayerHealthPoint);
        if (currPlayerHealthPoint <= 0) { PlayerDied(); }
    }
    public int GetPlayerDirectContactDamage()
    {
        return playerDirectContactDamage;
    }

    public void SetPlayerDirectContactDamage(int newDirectContactDamage)
    {
        playerDirectContactDamage = newDirectContactDamage;
    }

    public int GetPlayerBonusDamage()
    {
        return playerBonusDamage;
    }

    public void SetPlayerBonusDamage(int newBonusDamage)
    {
        playerBonusDamage = newBonusDamage;
    }

    public void SetActiveFull8x16MoveTiles(bool newBool)
    {
        full8x16MoveTiles.SetActive(newBool);
    }

    public bool GetHasAnEnemyAlreadyBeenDamagedThisTurn()
    {
        return hasAnEnemyAlreadyBeenDamagedThisTurn;
    }

    public void SetHasAnEnemyAlreadyBeenDamagedThisTurn( bool newBool)
    {
        hasAnEnemyAlreadyBeenDamagedThisTurn = newBool;
    }

    public bool GetHasPlayerAlreadyBeenDamagedThisTurn()
    {
        return hasPlayerAlreadyBeenDamagedThisTurn;
    }

    public void SetHasPlayerAlreadyBeenDamagedThisTurn(bool newBool)
    {
        hasPlayerAlreadyBeenDamagedThisTurn = newBool;
    }

    public string GetCurrentActivePowerupIdentity()
    {
        return currentActivePowerupIdentity;
    }

    public void SetCurrentActivePowerupIdentity(string newString)
    {
        currentActivePowerupIdentity = newString;
    }

    public int GetCurrEnemiesLeftThisRound()
    {
        return currEnemiesLeftThisRound;
    }

    public int GetTurnsPassedWithoutUsingActivePowerupThatIsOffCooldown()
    {
        return turnsPassedWithoutUsingActivePowerupThatIsOffCooldown;
    }

    public void SetTurnsPassedWithoutUsingActivePowerupThatIsOffCooldown(int newInt)
    {
        turnsPassedWithoutUsingActivePowerupThatIsOffCooldown = newInt;
    }

}
