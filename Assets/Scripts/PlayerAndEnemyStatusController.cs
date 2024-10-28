using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using UnityEditor;
using System.Xml;
using Unity.VisualScripting;

public class PlayerAndEnemyStatusController : MonoBehaviour
{

    [Header("Prefab")]
    //[SerializeField] private GameObject playerPrefabOldImageUI; // Assign the "chessPiece" prefab in the Inspector
    //[SerializeField] private GameObject enemyPrefabOldImageUI; // Assign the "chessPiece" prefab in the Inspector
    //[SerializeField] private GameObject battleBoardOldImageUI; // Assign the "battleBoard" GameObject in the Inspector

    [SerializeField] private GameObject playerPrefab; // Assign the "chessPiece" prefab in the Inspector
    [SerializeField] private GameObject enemyPrefab; // Assign the "chessPiece" prefab in the Inspector
    [SerializeField] private GameObject battleBoard; // Assign the "battleBoard" GameObject in the Inspector

    [Header("Controllers")]
    [SerializeField] private TurnController turnController;
    [SerializeField] private SideBarController sideBarController;
    [SerializeField] private ActualXYCoordinatesController coordinateScript;
    [SerializeField] private BattleModeController battleModeController;
    [SerializeField] private BestiaryController bestiaryController;
    [SerializeField] private DynamicDifficultyController dynamicDifficultyController;
    [SerializeField] private GenerateStatisticsController generateStatisticsController;

    private int roundNumber;
    private int killCount;
    private int totalMoveCount;
    private int moveCountThisRound;
    private int currEnemiesLeft;
    private int totalEnemiesThisRound;
    private int maxNoOfEnemiesAtAnyPoint;
    private int enemyPointsToAllocate;
    private int numberOfTimesEnemiesTookDamageThisRound;
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

    //private int enemiesAlreadySpawnedThisRound;
    private string[] enemyVariantsToBeSpawnedThisRound;
    private int enemiesSpawnedSoFarThisRound;
    private string whichEnemyVariantToSpawn;
    private GameObject[,] identitiesOfPiecesInBoard; // Make an array of game objects

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
        currPlayerHealthPoint = 50;
        maxPlayerHealthPoint = 50;
        currGold = 0;
        goldGainedSoFar = 0;
        //if (PlayerPrefs.GetString("modeDifficulty", "???") == "Easy")
        //{
        //    timeToMove = 6.0f;
        //}
        //else if (PlayerPrefs.GetString("modeDifficulty", "???") == "Medium" || PlayerPrefs.GetString("modeDifficulty", "???") == "Adaptive")
        //{
        //    timeToMove = 3.0f;
        //}
        //else if (PlayerPrefs.GetString("modeDifficulty", "???") == "Hard")
        //{
        //    timeToMove = 1.5f;
        //}
        sideBarController.SetSideBarRoundNumber(roundNumber);
        sideBarController.SetSideBarCurrPlayerHealthPointValue(currPlayerHealthPoint);
        sideBarController.SetSideBarMaxPlayerHealthPointValue(maxPlayerHealthPoint);
        sideBarController.SetSideBarCurrGoldValue(currGold);

        playerDirectContactDamage = 5; // The damage the player deals upon DirectContact on another enemy
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
        enemyPointsToAllocate = (int)Math.Round((double)((difficultyIndex * 2 + 1) + roundNumber * (difficultyIndex * 2 + 1)));
        //if (PlayerPrefs.GetString("modeDifficulty", "???") == "Easy")
        //{
        //    maxNoOfEnemiesAtAnyPoint = Math.Min(14, (int)Math.Round((double)(2 + roundNumber * 0.333)));
        //    enemyPointsToAllocate = (int)Math.Round((double)(1 + roundNumber * 1));
        //}
        //else if (PlayerPrefs.GetString("modeDifficulty", "???") == "Medium")
        //{
        //    maxNoOfEnemiesAtAnyPoint = Math.Min(14, (int)Math.Round((double)(3 + roundNumber * 0.66666)));
        //    enemyPointsToAllocate = (int)Math.Round((double)(2 + roundNumber * 2));
        //}
        //else if (PlayerPrefs.GetString("modeDifficulty", "???") == "Hard")
        //{
        //    maxNoOfEnemiesAtAnyPoint = Math.Min(14, (int)Math.Round((double)(4 + roundNumber * 1)));
        //    enemyPointsToAllocate = (int)Math.Round((double)(3 + roundNumber * 3));
        //}
        //else if (PlayerPrefs.GetString("modeDifficulty", "???") == "Adaptive")
        //{
        //    maxNoOfEnemiesAtAnyPoint = Math.Min(14, (int)Math.Round((double)( (difficultyIndex * 2 + 2) + roundNumber * (difficultyIndex * 0.33333 + 0.33333))));
        //    enemyPointsToAllocate = (int)Math.Round((double)( (difficultyIndex * 2 + 1) + roundNumber * (difficultyIndex * 2 + 1) ));
        //}
        //else
        //{
        //    maxNoOfEnemiesAtAnyPoint = 1;
        //    enemyPointsToAllocate = 1;
        //    Debug.LogWarning("Unkown mode difficulty");
        //}

        enemyVariantsToBeSpawnedThisRound = bestiaryController.DecideWhatEnemiesToSpawnThisRound(enemyPointsToAllocate);
        currEnemiesLeft = enemyVariantsToBeSpawnedThisRound.Length;
        totalEnemiesThisRound = currEnemiesLeft;

        sideBarController.SetSideBarCurrEnemiesLeftValue(currEnemiesLeft);
        sideBarController.SetSideBarTotalEnemiesThisRoundValue(totalEnemiesThisRound);

        enemiesSpawnedSoFarThisRound = 0;
        while (enemiesSpawnedSoFarThisRound < totalEnemiesThisRound && enemiesSpawnedSoFarThisRound < maxNoOfEnemiesAtAnyPoint)
        {
            whichEnemyVariantToSpawn = enemyVariantsToBeSpawnedThisRound[enemiesSpawnedSoFarThisRound];
            Instantiate(enemyPrefab, battleBoard.transform);
            enemiesSpawnedSoFarThisRound += 1;
        }

        //The faster the player finishes a level (depending on enemyPointsToAllocate)
        bonusGoldAtEndOfRound = enemyPointsToAllocate * 2; // Initial max reward or clearing levels faster
        numberOfTimesEnemiesTookDamageThisRound = 0;

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
        currEnemiesLeft -= 1;
        sideBarController.SetSideBarCurrEnemiesLeftValue(currEnemiesLeft);
        SetKillCountIncreaseBy1();

        SetCurrGold(currGold + goldEarned);
        codeForPrefabPlayer.ShowGoldEarned(goldEarned);

        if (currEnemiesLeft == 0) // All enemies killed, victory for this round
        {
            // at end of each round, update the logs of both per turn and per round
            PrintAndLogPerTurnHealthKillsPointsGold();
            dynamicDifficultyController.PrintAndLogPerTurnAllDGBInputAndOutputIndex();
            PrintAndLogPerRoundHealthKillsPointsGoldMoves();
            dynamicDifficultyController.PrintAndLogPerRoundAllDGBInputAndOutputIndex();

            // CHANGE THE BONUS GOLD CALCULATION. WHENEVER AN ENEMY IS DAMAGED, THE BONUS GOLD REDUCTION IS HALTED FOR 2 TURNS
            // MAYBE ALSO CHANGE IT SO THAT IT IS INDEPENDENT OF THE enemyPointsToAllocate. PERHAPS MAKE IT DEPENDENT ON ROUND NUMBER, THEN THE MORE THE enemyPointsToAllocate, THEN THE SLOWE IT IS REDUCED
            float changeToDynamicInputIndexTimeThinkingAndStepsTaken = Mathf.Clamp((float)((bonusGoldAtEndOfRound - moveCountThisRound * 0.25 + numberOfTimesEnemiesTookDamageThisRound * 0.5) / 10), -0.3f, 0.3f);
            dynamicDifficultyController.SetDynamicInputChange("TimeThinkingAndStepsTaken", changeToDynamicInputIndexTimeThinkingAndStepsTaken, false);
            bonusGoldAtEndOfRound = Mathf.Max((int)(bonusGoldAtEndOfRound - moveCountThisRound * 0.25 + numberOfTimesEnemiesTookDamageThisRound * 0.5), 0);
            battleModeController.SetTextBonusGold(bonusGoldAtEndOfRound);
            SetCurrGold(currGold + bonusGoldAtEndOfRound);
            Invoke(nameof(VictoryBattle), 0.5f);
        }
        else
        {
            if (enemiesSpawnedSoFarThisRound < totalEnemiesThisRound) // If there are still more enemies to spawn this round, replace the dead enemy with a new one
            {
                whichEnemyVariantToSpawn = enemyVariantsToBeSpawnedThisRound[enemiesSpawnedSoFarThisRound];
                Instantiate(enemyPrefab, battleBoard.transform);
                enemiesSpawnedSoFarThisRound += 1;
            }

            Invoke(nameof(CallForEnemyTurn), 0.5f);
        }

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

    public void SetTotalMoveCountIncreaseBy1()
    {
        totalMoveCount += 1;
    }

    public int GetMoveCountThisRound()
    {
        return moveCountThisRound;
    }

    public void SetMoveCountThisRoundIncreaseBy1()
    {
        moveCountThisRound += 1;
    }

    public int GetCurrGold()
    {
        return currGold;
    }

    public void SetCurrGold(int newGoldValue)
    {
        currGold = newGoldValue;
        goldGainedSoFar += currGold;
        sideBarController.SetSideBarCurrGoldValue(currGold);
    }

    public void SetNumberOfTimesEnemiesTookDamageThisRoundIncreaseBy1()
    {
        numberOfTimesEnemiesTookDamageThisRound += 1;
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



}
