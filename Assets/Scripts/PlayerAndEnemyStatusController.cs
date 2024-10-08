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

    [Header("Other Controllers")]
    [SerializeField] private TurnController turnController;
    [SerializeField] private SideBarController sideBarController;
    [SerializeField] private ActualXYCoordinatesController coordinateScript;
    [SerializeField] private BattleModeController battleModeController;
    [SerializeField] private BestiaryController bestiaryController;

    private int roundNumber;
    private int killCount;
    private int moveCount;
    private int currEnemiesLeft;
    private int totalEnemiesThisRound;
    private int maxNoOfEnemiesAtAnyPoint; // not used yet
    private int enemyPointsToAllocate;
    private int currPlayerHealthPoint;
    private int maxPlayerHealthPoint;
    private int currGold;
    private float timeToMove;
    private int playerLandingDamage;
    private int[,] playerMoveTiles;

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
        if (PlayerPrefs.GetString("modeDifficulty", "???") == "Easy")
        {
            currPlayerHealthPoint = 50;
            maxPlayerHealthPoint = 50;
            currGold = 30;
            timeToMove = 6.0f;
        }
        else if (PlayerPrefs.GetString("modeDifficulty", "???") == "Medium" || PlayerPrefs.GetString("modeDifficulty", "???") == "Adaptive")
        {
            currPlayerHealthPoint = 35;
            maxPlayerHealthPoint = 35;
            currGold = 15;
            timeToMove = 3.0f;
        }
        else if (PlayerPrefs.GetString("modeDifficulty", "???") == "Hard")
        {
            currPlayerHealthPoint = 20;
            maxPlayerHealthPoint = 20;
            currGold = 0;
            timeToMove = 1.5f;
        }
        sideBarController.SetSideBarRoundNumber(roundNumber);
        sideBarController.SetSideBarCurrPlayerHealthPointValue(currPlayerHealthPoint);
        sideBarController.SetSideBarMaxPlayerHealthPointValue(maxPlayerHealthPoint);
        sideBarController.SetSideBarCurrGoldValue(currGold);

        playerLandingDamage = 5; // The damage the player deals upon landing on another enemy

        killCount = 0;
        moveCount = 0;


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

        // TEMPORARY - MANUAL DETERMINATION OF ENEMIES
        // Instatiate the enemyPiece prefab (but later control the amount of spawned enemies by the round number / difficulty mode / DGB. Ex: out of 10 enemies, only 4 may exist at any tim
        //if (roundNumber == 1)
        //{
        //    whichEnemyVariantToSpawn = "pawnBandit";
        //    Instantiate(enemyPrefab, battleBoard.transform);
        //    whichEnemyVariantToSpawn = "pawnSkeleton";
        //    Instantiate(enemyPrefab, battleBoard.transform);
        //    whichEnemyVariantToSpawn = "pawnSlime";
        //    Instantiate(enemyPrefab, battleBoard.transform);
        //    whichEnemyVariantToSpawn = "pawnGoblin";
        //    Instantiate(enemyPrefab, battleBoard.transform);
        //    currEnemiesLeft = 4;
        //    totalEnemiesThisRound = 4;
        //}
        //else if (roundNumber == 2)
        //{
        //    whichEnemyVariantToSpawn = "rookTroll";
        //    Instantiate(enemyPrefab, battleBoard.transform);
        //    whichEnemyVariantToSpawn = "bishopDarkElf";
        //    Instantiate(enemyPrefab, battleBoard.transform);
        //    whichEnemyVariantToSpawn = "pawnSlime";
        //    Instantiate(enemyPrefab, battleBoard.transform);
        //    currEnemiesLeft = 3;
        //    totalEnemiesThisRound = 3;
        //}
        //else if (roundNumber >= 3)
        //{
        //    whichEnemyVariantToSpawn = "knightDullahan";
        //    Instantiate(enemyPrefab, battleBoard.transform);
        //    whichEnemyVariantToSpawn = "queenGolem";
        //    Instantiate(enemyPrefab, battleBoard.transform);
        //    whichEnemyVariantToSpawn = "kingLich";
        //    Instantiate(enemyPrefab, battleBoard.transform);
        //    currEnemiesLeft = 3;
        //    totalEnemiesThisRound = 3;
        //}
        //sideBarController.SetSideBarCurrEnemiesLeftValue(currEnemiesLeft);
        //sideBarController.SetSideBarTotalEnemiesThisRoundValue(totalEnemiesThisRound);

        if (PlayerPrefs.GetString("modeDifficulty", "???") == "Easy")
        {
            maxNoOfEnemiesAtAnyPoint = (int)Math.Round((double)(2 + roundNumber * 0.33));
            enemyPointsToAllocate = (int)Math.Round((double)(1 + roundNumber));
        }
        else if (PlayerPrefs.GetString("modeDifficulty", "???") == "Medium" || PlayerPrefs.GetString("modeDifficulty", "???") == "Adaptive")
        {
            maxNoOfEnemiesAtAnyPoint = (int)Math.Round((double)(3 + roundNumber * 0.5));
            enemyPointsToAllocate = (int)Math.Round((double)(2 + roundNumber * 2));
        }
        else if (PlayerPrefs.GetString("modeDifficulty", "???") == "Hard")
        {
            maxNoOfEnemiesAtAnyPoint = (int)Math.Round((double)(5 + roundNumber * 1));
            enemyPointsToAllocate = (int)Math.Round((double)(4 + roundNumber * 3.5));
        }

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
        currEnemiesLeft -= 1;
        sideBarController.SetSideBarCurrEnemiesLeftValue(currEnemiesLeft);
        SetKillCountIncreaseBy1();

        currGold += goldEarned;
        sideBarController.SetSideBarCurrGoldValue(currGold);

        GameObject prefabPlayer = GameObject.FindWithTag("tagForPrefabPlayer");
        CodeForPrefabPlayer codeForPrefabPlayer = prefabPlayer.GetComponent<CodeForPrefabPlayer>();
        codeForPrefabPlayer.ShowGoldEarned(goldEarned);

        if (currEnemiesLeft == 0)
        {
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
        turnController.EnemyTurn();
    }




    // ====================================================
    // GETTER, SETTER
    public float GetTimeToMove()
    {
        return timeToMove;
    }

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

    public int GetPlayerLandingDamage()
    {
        return playerLandingDamage;
    }

    public void SetPlayerLandingDamage(int newLandingDamage)
    {
        playerLandingDamage = newLandingDamage;
    }

    public void SetNextRoundNumber()
    {
        roundNumber += 1;
        sideBarController.SetSideBarRoundNumber(roundNumber);
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
        sideBarController.SetSideBarCurrPlayerHealthPointValue(currPlayerHealthPoint);
        if (currPlayerHealthPoint <= 0) { PlayerDied(); }
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
    public int GetMoveCount()
    {
        return moveCount;
    }

    public void SetMoveCountIncreaseBy1()
    {
        moveCount += 1;
    }
}
