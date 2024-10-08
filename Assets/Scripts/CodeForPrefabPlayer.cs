using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Reflection;

using DG.Tweening;
using System.Linq;

public class CodeForPrefabPlayer : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject groupCanvasUI;
    [SerializeField] private GameObject circleTimer;
    [SerializeField] private GameObject groupMoveTiles;
    [SerializeField] private GameObject groupHitpoints;
    [SerializeField] private GameObject goldEarned;
    [Header("Specific Components of Those Objects")]
    [SerializeField] private Transform thisPieceTransform;
    [SerializeField] private RectTransform canvasUITransform;
    [SerializeField] private Image imageCircleTimer;
    [SerializeField] private TextMeshProUGUI textDamageOrHealTaken;
    [SerializeField] private Image imageGreenHealthBar;
    [SerializeField] private TextMeshProUGUI textGoldEarned;
    [Header("Move Tiles")]
    [SerializeField] private GameObject[] moveTiles;

    private ActualXYCoordinatesController actualXYCoordinates;
    private PlayerAndEnemyStatusController playerAndEnemyStatusController;
    private MusicController musicController;
    private TurnController turnController;

    private bool isMoveTimeLlimitActive; // to control the Update() for controlling the timer
    private float elapsedTime;

    private float timeLimit;
    private int currGridX;
    private int currGridY;

    // The PlayerAndStatusController is the one who will remember the player's stats, and values are fetched from there, since the player prefab instance can be destroyed
    private int maxHealth;
    private int currHealth;
    private int playerLandingDamage;
    private bool alreadyKnockbackedFromDamage;

    private int[][] knockbackDirections1Tile = new int[][] { new int[] { 1, 1 }, new int[] { 1, 0 }, new int[] { 1, -1 }, new int[] { 0, -1 }, new int[] { -1, -1 }, new int[] { -1, 0 }, new int[] { -1, 1 }, new int[] { 0, 1 } };
    private int[][] knockbackDirections2Tile = new int[][] { new int[] { 2, 2 }, new int[] { 2, 1 }, new int[] { 2, 0 }, new int[] { 2, -1 }, new int[] { 2, -2 }, new int[] { 1, -2 }, new int[] { 0, -2 }, new int[] { -1, -2 }, new int[] { -2, -2 }, new int[] { -2, -1 }, new int[] { -2, 0 }, new int[] { -2, 1 }, new int[] { -2, 2 }, new int[] { -1, 2 }, new int[] { 0, 2 }, new int[] { 1, 2 } };
    private int[][] knockbackDirections3Tile = new int[][] { new int[] { 3, 3 }, new int[] { 3, 2 }, new int[] { 3, 1 }, new int[] { 3, 0 }, new int[] { 3, -1 }, new int[] { 3, -2 }, new int[] { 3, -3 }, new int[] { 2, -3 }, new int[] { 1, -3 }, new int[] { 0, -3 }, new int[] { -1, -3 }, new int[] { -2, -3 }, new int[] { -3, -3 }, new int[] { -3, -2 }, new int[] { -3, -1 }, new int[] { -3, 0 }, new int[] { -3, 1 }, new int[] { -3, 2 }, new int[] { -3, 3 }, new int[] { -2, 3 }, new int[] { -1, 3 }, new int[] { 0, 3 }, new int[] { 1, 3 }, new int[] { 2, 3 } };

    //private Coroutine fillCoroutine; // coroutine is possible, but more complicated than just using update()

    void Awake()
    {
        GameObject CoordinateScriptObject = GameObject.FindGameObjectWithTag("tagForActualXYCoordinates");
        actualXYCoordinates = CoordinateScriptObject.GetComponent<ActualXYCoordinatesController>();
        GameObject PlayerAndEnemyStatusControllerObject = GameObject.FindGameObjectWithTag("tagForPlayerAndEnemyStatusController");
        playerAndEnemyStatusController = PlayerAndEnemyStatusControllerObject.GetComponent<PlayerAndEnemyStatusController>();
        GameObject musicControllerObject = GameObject.FindGameObjectWithTag("tagForMusicController");
        musicController = musicControllerObject.GetComponent<MusicController>();
        GameObject turnControllerObject = GameObject.FindGameObjectWithTag("tagForTurnController");
        turnController = turnControllerObject.GetComponent<TurnController>();

        currGridX = UnityEngine.Random.Range(1, 17);
        currGridY = UnityEngine.Random.Range(1, 9);

        maxHealth = playerAndEnemyStatusController.GetPlayerMaxHealth();
        currHealth = playerAndEnemyStatusController.GetPlayerCurrHealth();
        playerLandingDamage = playerAndEnemyStatusController.GetPlayerLandingDamage();

        thisPieceTransform.position = new Vector3(actualXYCoordinates.GetActualXCoordinate(currGridX), actualXYCoordinates.GetActualYCoordinate(currGridY), transform.position.z);
        canvasUITransform.anchoredPosition = new Vector2(actualXYCoordinates.GetActualXCoordinateUIImage(currGridX), actualXYCoordinates.GetActualYCoordinateUIImage(currGridY));
        playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, gameObject); // "gameObject" alraedy refers to THIS Prefab gameobject itself
        isMoveTimeLlimitActive = false;
        DeactivateMoveTiles();
        groupHitpoints.SetActive(false);
        goldEarned.SetActive(false);

        // Randomize the knockback tiles when an enemy lands on the player
        Randomize2DArray(knockbackDirections1Tile);
        Randomize2DArray(knockbackDirections2Tile);
        Randomize2DArray(knockbackDirections3Tile);
    }

    public void PlayerCanMoveNow()
    {
        circleTimer.SetActive(true);
        groupMoveTiles.SetActive(true);

        StartTimer(playerAndEnemyStatusController.GetTimeToMove()); // Set the timer to the move time limit as efined in playerAndEnemyStatusController
        // The Standard 8 move tiles for a knight. OPTIONAL: Maybe add more depending on move-related powerups?
        ActivateMoveTiles(2, 1);
        ActivateMoveTiles(1, 2);
        ActivateMoveTiles(-2, 1);
        ActivateMoveTiles(-1, 2);
        ActivateMoveTiles(2, -1);
        ActivateMoveTiles(1, -2);
        ActivateMoveTiles(-2, -1);
        ActivateMoveTiles(-1, -2);
    }

    public void PlayerMoveTileWasClicked(int xAndYOffset) // Activated by clicking one of the player's move tiles. The xAndYOffest variable is the x and y offset values combined into 1 number
    {
        circleTimer.SetActive(false);
        groupMoveTiles.SetActive(false);

        musicController.PlayMoveSoundEffectSource();
        playerAndEnemyStatusController.SetMoveCountIncreaseBy1();
        playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, null);
        isMoveTimeLlimitActive = false;
        int gridXOffset = ((xAndYOffset - (xAndYOffset % 10)) / 10) - 4;
        int gridYOffset = (xAndYOffset % 10) - 4;
        DeactivateMoveTiles();
        thisPieceTransform.DOMove(new Vector3(actualXYCoordinates.GetActualXCoordinate(currGridX + gridXOffset), actualXYCoordinates.GetActualYCoordinate(currGridY + gridYOffset), transform.position.z), 0.5f);
        canvasUITransform.DOAnchorPos(new Vector2(actualXYCoordinates.GetActualXCoordinateUIImage(currGridX + gridXOffset), actualXYCoordinates.GetActualYCoordinateUIImage(currGridY + gridYOffset)), 0.5f);
        //canvasUITransform.anchoredPosition = new Vector2(actualXYCoordinates.GetActualXCoordinateUIImage(currGridX + xOffset), actualXYCoordinates.GetActualYCoordinateUIImage(currGridY + yOffset));
        //tilesTransform.anchoredPosition = new Vector2(actualXYCoordinates.GetActualXCoordinateUIImage(currGridX + xOffset), actualXYCoordinates.GetActualYCoordinateUIImage(currGridY + yOffset));
        currGridX += gridXOffset;
        currGridY += gridYOffset;

        Invoke(nameof(MoveToThisTileAndAttackEnemyIfItExistsHere), 0.5f);
    }

    public void MoveToThisTileAndAttackEnemyIfItExistsHere()
    {
        GameObject enemyAtThisTile = playerAndEnemyStatusController.GetIdentityOfPieceAtThisBoardTile(currGridX, currGridY);
        if (enemyAtThisTile != null) // If not null, no doubt that this is an enemy, so just call the enemy script. Alternatively use:    if (enemyAtThisTile.GetComponent<CodeForPrefabEnemy>() != null)
        {
            enemyAtThisTile.GetComponent<CodeForPrefabEnemy>().ThisEnemyTakesDamage(playerLandingDamage, true);
            playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, gameObject);
            //Invoke(nameof(CallForEnemyTurn), 0.5f); // Don't call it here. Otherwise when the last enemy is killed, it will call for enemy turn, but no enemies so immeadiately call player turn, but the player prefab is already destroyed since we are at the victory screen, leading to an error due to calling a function from a non-existent prefab.
        }   
        else
        {
            playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, gameObject);
            CallForEnemyTurn();
        }
    }

    public void PlayerTakesDamage(int damageTaken, bool isKnockback)
    {
        //Debug.Log("Player takes " + damageTaken + " damage");
        currHealth = playerAndEnemyStatusController.GetPlayerCurrHealth();

        currHealth = currHealth - damageTaken;
        if (currHealth <= 0)
        {
            musicController.PlayDamageSoundEffectSource();
            playerAndEnemyStatusController.SetPlayerCurrHealth(0); // Which will eventually also trigger the game over screen
        }
        else
        {
            playerAndEnemyStatusController.SetPlayerCurrHealth(currHealth);
            musicController.PlayDamageSoundEffectSource();
            groupHitpoints.SetActive(true);
            imageGreenHealthBar.fillAmount = (float)currHealth / (float)maxHealth;
            textDamageOrHealTaken.text = "-" + damageTaken;
            Invoke(nameof(HideHealthBar), 0.5f);
            DeactivateMoveTiles();
            //Invoke(nameof(ActivateMoveTiles), 0.5f); // Not required as after the PlayerTakesDamage function is called by the enemy, it will eventually go back to the player's turn

            if (isKnockback)
            {
                alreadyKnockbackedFromDamage = false;
                CheckForNearbyEmtpyTilesToMoveToWhenKnockedbacked(knockbackDirections1Tile);
                if (!alreadyKnockbackedFromDamage) { CheckForNearbyEmtpyTilesToMoveToWhenKnockedbacked(knockbackDirections2Tile); }
                if (!alreadyKnockbackedFromDamage) { CheckForNearbyEmtpyTilesToMoveToWhenKnockedbacked(knockbackDirections3Tile); }
            }
        }
    }

    public void CheckForNearbyEmtpyTilesToMoveToWhenKnockedbacked(int[][] knockbackDirections)
    {
        foreach (int[] direction in knockbackDirections)
        {
            int gridXOffset = direction[0];
            int gridYOffset = direction[1];
            if (actualXYCoordinates.IsThisStillInsideTheBoard(currGridX + gridXOffset, currGridY + gridYOffset))
            {
                if (playerAndEnemyStatusController.GetIdentityOfPieceAtThisBoardTile(currGridX + gridXOffset, currGridY + gridYOffset) == null)
                {
                    alreadyKnockbackedFromDamage = true;
                    playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, null);
                    playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX + gridXOffset, currGridY + gridYOffset, gameObject);
                    thisPieceTransform.DOMove(new Vector3(actualXYCoordinates.GetActualXCoordinate(currGridX + gridXOffset), actualXYCoordinates.GetActualYCoordinate(currGridY + gridYOffset), transform.position.z), 0.5f);
                    canvasUITransform.DOAnchorPos(new Vector2(actualXYCoordinates.GetActualXCoordinateUIImage(currGridX + gridXOffset), actualXYCoordinates.GetActualYCoordinateUIImage(currGridY + gridYOffset)), 0.5f);
                    // Randomize the 2D knockback arrays again
                    if (knockbackDirections.Length == 8)       { Randomize2DArray(knockbackDirections1Tile); }
                    else if (knockbackDirections.Length == 16) { Randomize2DArray(knockbackDirections2Tile); }
                    else if (knockbackDirections.Length == 24) { Randomize2DArray(knockbackDirections3Tile); }
                    currGridX += gridXOffset;
                    currGridY += gridYOffset;
                    break;
                }
            }
        }
    }

    public void Randomize2DArray(int[][] array)
    {
        var list = array.OrderBy(x => UnityEngine.Random.value).ToList();
        for (int i = 0; i < list.Count; i++)
        {
            array[i] = list[i];
        }
    }

    public void HideHealthBar()
    {
        groupHitpoints.SetActive(false);
    }

    public void ShowGoldEarned(int goldAmountEanred)
    {
        goldEarned.SetActive(true);
        textGoldEarned.text = "+$" + goldAmountEanred;
        Invoke(nameof(HideGoldEarned), 0.5f);
    }
    public void HideGoldEarned()
    {
        goldEarned.SetActive(false);
    }

    public void ChangePlayerLocationToCurrentTile() // NOT YET CALLED
    {
        playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, gameObject);
    }

    // Add a function if player is damaged by enemy and moves somewhere

    public void DeactivateMoveTiles()
    {
        // Loop through all tiles and deactivate them
        for (int i = 0; i < moveTiles.Length; i++)
        {
            moveTiles[i].SetActive(false);
        }
    }

    public void ActivateMoveTiles(int gridXOffset, int gridYOffset)
    {
        if (actualXYCoordinates.IsThisStillInsideTheBoard(currGridX + gridXOffset, currGridY + gridYOffset))
        {
            int index = gridXOffset + 3 + (gridYOffset + 3) * 7;
            moveTiles[index].SetActive(true);
        }
    }

    public void CallForEnemyTurn()
    {
        turnController.EnemyTurn();
    }

    public void StartTimer(float x)
    {
        isMoveTimeLlimitActive = true;
        elapsedTime = 0f;
        timeLimit = x;
    }

    void Update() // For the circle timer
    {
        if (isMoveTimeLlimitActive && elapsedTime < timeLimit)
        {
            elapsedTime += Time.deltaTime;
            imageCircleTimer.fillAmount = Mathf.Lerp(1f, 0f, elapsedTime / timeLimit);
        }
        else if (isMoveTimeLlimitActive && elapsedTime >= timeLimit)
        {
            DeactivateMoveTiles();
            playerAndEnemyStatusController.SetMoveCountIncreaseBy1();
            isMoveTimeLlimitActive = false;
            CallForEnemyTurn();
        }
    }

    //public void sayHello()
    //{
    //    Debug.Log("You have clicked on player");
    //}




    // ====================================================
    // GETTER, SETTER

    public (int, int) GetPlayerCurrGridXAndCurrGridY()
    {
        return (currGridX, currGridY);
    }
}