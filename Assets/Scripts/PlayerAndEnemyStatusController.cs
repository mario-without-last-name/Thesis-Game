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

public class PlayerAndEnemyStatusController : MonoBehaviour
{

    [Header("Prefab")]
    [SerializeField] private GameObject playerPrefab; // Assign the "chessPiece" prefab in the Inspector
    [SerializeField] private GameObject enemyPrefab; // Assign the "chessPiece" prefab in the Inspector
    [SerializeField] private GameObject battleBoard; // Assign the "BattleBoard" GameObject in the Inspector

    [Header("Other Controllers")]
    [SerializeField] private SideBarController SideBarController;
    [SerializeField] private ActualXYCoordinates coordinateScript;

    private int roundNumber;
    private int currEnemiesLeft;
    private int totalEnemiesThisRound;
    private int currPlayerHealthPoint;
    private int maxPlayerHealthPoint;
    private int currGold;
    private float timeToMove;

    //private int enemiesAlreadySpawnedThisRound;
    private string whichEnemyVariantToSpawn;
    private string[,] identitiesOfPiecesInBoard;

    // Start is called before the first frame update
    void Start()
    {
        // Set several variables, and also display them in th SideBar
        roundNumber = 1;
        if (PlayerPrefs.GetString("modeDifficulty", "???") == "Easy")
        {
            currEnemiesLeft = 2;
            totalEnemiesThisRound = 2;
            currPlayerHealthPoint = 50;
            maxPlayerHealthPoint = 50;
            currGold = 30;
            timeToMove = 6.0f;
        }
        else if (PlayerPrefs.GetString("modeDifficulty", "???") == "Medium" || PlayerPrefs.GetString("modeDifficulty", "???") == "Adaptive")
        {
            currEnemiesLeft = 5;
            totalEnemiesThisRound = 5;
            currPlayerHealthPoint = 35;
            maxPlayerHealthPoint = 35;
            currGold = 15;
            timeToMove = 3.0f;
        }
        else if (PlayerPrefs.GetString("modeDifficulty", "???") == "Hard")
        {
            currEnemiesLeft = 10;
            totalEnemiesThisRound = 10;
            currPlayerHealthPoint = 20;
            maxPlayerHealthPoint = 20;
            currGold = 0;
            timeToMove = 1.5f;
        }
        SideBarController.SetSideBarRoundNumber(roundNumber);
        SideBarController.SetSideBarCurrEnemiesLeftValue(currEnemiesLeft);
        SideBarController.SetSideBarTotalEnemiesThisRoundValue(totalEnemiesThisRound);
        SideBarController.SetSideBarcurrPlayerHealthPointValue(currPlayerHealthPoint);
        SideBarController.SetSideBarmaxPlayerHealthPointValue(maxPlayerHealthPoint);
        SideBarController.SetSideBarCurrGoldValue(currGold);


        //enemiesAlreadySpawnedThisRound = 0;

        // Start with a clean empty 2d string array
        ResetTheIdentitiesOfPiecesInBoardToBlank();
    }
    public void SpawnPlayerAndEnemiesForNewRound()
    {
        // Instantiate the chessPiece prefab
        GameObject chessPieceInstance = Instantiate(playerPrefab, battleBoard.transform);

        // Instatiate the enemyPiece prefab (right now only fixed 2, but later control its amount by the round number / difficulty mode / DGB. Ex: out of 10 enemies, only 4 may exist at any tim
        // Also determine what enemy this is (variant, and string name for 2d array). Maybe need a new script / function to determine their identity and stats?
        whichEnemyVariantToSpawn = "pawnGoblin";
        GameObject enemyPieceInstance1 = Instantiate(enemyPrefab, battleBoard.transform);
        whichEnemyVariantToSpawn = "pawnSlime";
        GameObject enemyPieceInstance2 = Instantiate(enemyPrefab, battleBoard.transform);
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
        identitiesOfPiecesInBoard = new string[16, 8];
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                identitiesOfPiecesInBoard[x, y] = ""; // Setting all positions to unoccupied
            }
        }
    }

    // GETTER, SETTER
    public float GetTimeToMove()
    {
        return timeToMove;
    }

    public string GetIdentityOfPieceAtThisBoardTile(int gridX, int gridY)
    {
        return identitiesOfPiecesInBoard[gridX-1, gridY-1];
    }

    public void SetIdentityOfPieceAtThisBoardTile(int gridX, int gridY, string pieceName)
    {
        identitiesOfPiecesInBoard[gridX-1, gridY-1] = pieceName;
        //Debug.Log(identitiesOfPiecesInBoard[gridX-1, gridY-1]);
    }

    public string GetWhichEnemyVariantToSpawn()
    {
        return whichEnemyVariantToSpawn;
    }
}
