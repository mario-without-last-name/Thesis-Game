using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Reflection;
using UnityEditor.U2D.Aseprite;
using System.Linq;
using DG.Tweening;
using System.ComponentModel.Design;
using static UnityEngine.EventSystems.EventTrigger;
using Unity.VisualScripting;

public class CodeForPrefabEnemy : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject groupCanvasUI;
    [SerializeField] private GameObject groupMoveTiles;
    [SerializeField] private GameObject groupAttackTiles;
    [SerializeField] private GameObject groupHitpoints;
    [SerializeField] private GameObject yellowGlow;
    [SerializeField] private GameObject redGlow;
    [SerializeField] private SpriteRenderer piecePicture;
    [Header("Specific Components of Those Objects")]
    [SerializeField] private Transform thisPieceTransform;
    [SerializeField] private RectTransform canvasUITransform;
    [SerializeField] private TextMeshProUGUI textDamageOrHealTaken;
    [SerializeField] private Image imageGreenHealthBar;
    [Header("Tiles")]
    [SerializeField] private GameObject[] moveTiles;
    [SerializeField] private GameObject[] attackTiles;

    private ActualXYCoordinatesController actualXYCoordinates;
    private PlayerAndEnemyStatusController playerAndEnemyStatusController;
    private BestiaryController bestiaryController;
    private MusicController musicController;
    private CodeForPrefabPlayer codeForPrefabPlayer;
    private TurnController turnController;

    private GameObject prefabPlayerObject;

    private int currGridX;
    private int currGridY;
    private string thisEnemyVariant;
    private int[][] moveTilesToSpawn;
    private int[][] attackTilesToSpawn;
    private int[][] tilesThatTheEnemyWillMoveTo; // copy of moveTilesToSpawn

    private int maxHealth;
    private int currHealth;
    private int enemyDamage;
    private int moveDelay;
    private int currDelayLeft;

    private bool alreadyKnockbackedFromDamage;
    private bool cannotAttackNextTurn;
    private bool alreadyMoved;
    private float lowestEuclideanDistance;
    private int yellowTileIndexWithLowestEuclideanDistance;

    private int[][] knockbackDirections1Tile = new int[][] { new int[] { 1, 1}, new int[] { 1, 0}, new int[] { 1,-1}, new int[] { 0,-1}, new int[] {-1,-1}, new int[] {-1, 0}, new int[] {-1, 1}, new int[] { 0, 1} };
    private int[][] knockbackDirections2Tile = new int[][] { new int[] { 2, 2}, new int[] { 2, 1}, new int[] { 2, 0}, new int[] { 2,-1}, new int[] { 2,-2}, new int[] { 1,-2}, new int[] { 0,-2}, new int[] {-1,-2}, new int[] {-2,-2}, new int[] {-2,-1}, new int[] {-2, 0}, new int[] {-2, 1}, new int[] {-2, 2}, new int[] {-1, 2}, new int[] { 0, 2}, new int[] { 1, 2} };
    private int[][] knockbackDirections3Tile = new int[][] { new int[] { 3, 3}, new int[] { 3, 2}, new int[] { 3, 1}, new int[] { 3, 0}, new int[] { 3,-1}, new int[] { 3,-2}, new int[] { 3,-3}, new int[] { 2,-3}, new int[] { 1,-3}, new int[] { 0,-3}, new int[] {-1,-3}, new int[] {-2,-3}, new int[] {-3,-3}, new int[] {-3,-2}, new int[] {-3,-1}, new int[] {-3, 0}, new int[] {-3, 1}, new int[] {-3, 2}, new int[] {-3, 3}, new int[] {-2, 3}, new int[] {-1, 3}, new int[] { 0, 3}, new int[] { 1, 3}, new int[] { 2, 3} };
    private int[][] allPossibleBoardTileLocations = new int[][] {
        new int[] { 1, 1}, new int[] { 2, 1}, new int[] { 3, 1}, new int[] { 4, 1}, new int[] { 5, 1}, new int[] { 6, 1}, new int[] { 7, 1}, new int[] { 8, 1}, new int[] { 9, 1}, new int[] { 10, 1}, new int[] { 11, 1}, new int[] { 12, 1}, new int[] { 13, 1}, new int[] { 14, 1}, new int[] { 15, 1}, new int[] { 16, 1},
        new int[] { 1, 2}, new int[] { 2, 2}, new int[] { 3, 2}, new int[] { 4, 2}, new int[] { 5, 2}, new int[] { 6, 2}, new int[] { 7, 2}, new int[] { 8, 2}, new int[] { 9, 2}, new int[] { 10, 2}, new int[] { 11, 2}, new int[] { 12, 2}, new int[] { 13, 2}, new int[] { 14, 2}, new int[] { 15, 2}, new int[] { 16, 2},
        new int[] { 1, 3}, new int[] { 2, 3}, new int[] { 3, 3}, new int[] { 4, 3}, new int[] { 5, 3}, new int[] { 6, 3}, new int[] { 7, 3}, new int[] { 8, 3}, new int[] { 9, 3}, new int[] { 10, 3}, new int[] { 11, 3}, new int[] { 12, 3}, new int[] { 13, 3}, new int[] { 14, 3}, new int[] { 15, 3}, new int[] { 16, 3},
        new int[] { 1, 4}, new int[] { 2, 4}, new int[] { 3, 4}, new int[] { 4, 4}, new int[] { 5, 4}, new int[] { 6, 4}, new int[] { 7, 4}, new int[] { 8, 4}, new int[] { 9, 4}, new int[] { 10, 4}, new int[] { 11, 4}, new int[] { 12, 4}, new int[] { 13, 4}, new int[] { 14, 4}, new int[] { 15, 4}, new int[] { 16, 4},
        new int[] { 1, 5}, new int[] { 2, 5}, new int[] { 3, 5}, new int[] { 4, 5}, new int[] { 5, 5}, new int[] { 6, 5}, new int[] { 7, 5}, new int[] { 8, 5}, new int[] { 9, 5}, new int[] { 10, 5}, new int[] { 11, 5}, new int[] { 12, 5}, new int[] { 13, 5}, new int[] { 14, 5}, new int[] { 15, 5}, new int[] { 16, 5},
        new int[] { 1, 6}, new int[] { 2, 6}, new int[] { 3, 6}, new int[] { 4, 6}, new int[] { 5, 6}, new int[] { 6, 6}, new int[] { 7, 6}, new int[] { 8, 6}, new int[] { 9, 6}, new int[] { 10, 6}, new int[] { 11, 6}, new int[] { 12, 6}, new int[] { 13, 6}, new int[] { 14, 6}, new int[] { 15, 6}, new int[] { 16, 6},
        new int[] { 1, 7}, new int[] { 2, 7}, new int[] { 3, 7}, new int[] { 4, 7}, new int[] { 5, 7}, new int[] { 6, 7}, new int[] { 7, 7}, new int[] { 8, 7}, new int[] { 9, 7}, new int[] { 10, 7}, new int[] { 11, 7}, new int[] { 12, 7}, new int[] { 13, 7}, new int[] { 14, 7}, new int[] { 15, 7}, new int[] { 16, 7},
        new int[] { 1, 8}, new int[] { 2, 8}, new int[] { 3, 8}, new int[] { 4, 8}, new int[] { 5, 8}, new int[] { 6, 8}, new int[] { 7, 8}, new int[] { 8, 8}, new int[] { 9, 8}, new int[] { 10, 8}, new int[] { 11, 8}, new int[] { 12, 8}, new int[] { 13, 8}, new int[] { 14, 8}, new int[] { 15, 8}, new int[] { 16, 8}
    };


    void Awake()
    {
        GameObject actualXYCoordinatesObject = GameObject.FindGameObjectWithTag("tagForActualXYCoordinates");
        actualXYCoordinates = actualXYCoordinatesObject.GetComponent<ActualXYCoordinatesController>();
        GameObject playerAndEnemyStatusControllerObject = GameObject.FindGameObjectWithTag("tagForPlayerAndEnemyStatusController");
        playerAndEnemyStatusController = playerAndEnemyStatusControllerObject.GetComponent<PlayerAndEnemyStatusController>();
        GameObject bestiaryControllerObject = GameObject.FindGameObjectWithTag("tagForBestiaryController");
        bestiaryController = bestiaryControllerObject.GetComponent<BestiaryController>();
        GameObject musicControllerObject = GameObject.FindGameObjectWithTag("tagForMusicController");
        musicController = musicControllerObject.GetComponent<MusicController>();
        GameObject turnControllerObject = GameObject.FindGameObjectWithTag("tagForTurnController");
        turnController = turnControllerObject.GetComponent<TurnController>();
        // Must know where the player is.
        prefabPlayerObject = GameObject.FindGameObjectWithTag("tagForPrefabPlayer");
        codeForPrefabPlayer = prefabPlayerObject.GetComponent<CodeForPrefabPlayer>();

        // Find out what enemy variant this piece should be as controlled by the playerAndEnemyStatusController
        thisEnemyVariant = playerAndEnemyStatusController.GetWhichEnemyVariantToSpawn();

        // Based on the enemy variant, determine its sprite, health, damage, and delay
        piecePicture.sprite = bestiaryController.GetEnemySprite(thisEnemyVariant);
        int [] pieceHealthDamageDelay = bestiaryController.GetHealthAttackDelay(thisEnemyVariant);
        maxHealth = pieceHealthDamageDelay[0];
        currHealth = maxHealth;
        enemyDamage = pieceHealthDamageDelay[1];
        moveDelay = pieceHealthDamageDelay[2];
        currDelayLeft = moveDelay;
        cannotAttackNextTurn = false;

        // Ensure that it does not spawn on the same as another enemy or nearby he player (min 4 tiles away)
        (int playerCurrGridX, int playerCurrGridY) = codeForPrefabPlayer.GetPlayerCurrGridXAndCurrGridY();
        Randomize2DArray(allPossibleBoardTileLocations);
        foreach (int[] tile in allPossibleBoardTileLocations)
        {
            currGridX = tile[0];
            currGridY = tile[1];
            if (playerAndEnemyStatusController.GetIdentityOfPieceAtThisBoardTile(currGridX, currGridY) == null && (Math.Abs(playerCurrGridX - currGridX) > 3 || Math.Abs(playerCurrGridY - currGridY) > 3))
            {
                break;
            }
        }
        thisPieceTransform = GetComponent<Transform>();
        thisPieceTransform.position = new Vector3(actualXYCoordinates.GetActualXCoordinate(currGridX), actualXYCoordinates.GetActualYCoordinate(currGridY), transform.position.z);
        playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, gameObject);

        canvasUITransform = groupCanvasUI.GetComponent<RectTransform>();
        canvasUITransform.anchoredPosition = new Vector2(actualXYCoordinates.GetActualXCoordinateUIImage(currGridX), actualXYCoordinates.GetActualYCoordinateUIImage(currGridY));

        // Based on the enemy variant, determine the move tiles and attack tiles it has
        DeactivateAllMoveTiles();
        moveTilesToSpawn = bestiaryController.GetEnemyMoveTiles(thisEnemyVariant);
        ActivateSpecificMoveTiles(); // LATER, MAKE ITS VISIBILITY DEPENDANT ON VISUAL HINTS DGB
        tilesThatTheEnemyWillMoveTo = moveTilesToSpawn;
        DeactivateAllAttackTiles();
        attackTilesToSpawn = bestiaryController.GetEnemyAttackTiles(thisEnemyVariant);
        ActivateSpecificAttackTiles();

        if (currDelayLeft == 1) { ShowMoveTiles(); yellowGlow.SetActive(true); }
        else { HideMoveTiles(); yellowGlow.SetActive(false); }
        redGlow.SetActive(false);
        groupAttackTiles.SetActive(true);

        // Randomize the knockback tiles when the player lands on this enemy, and also the tiles the enemy will move to when their move delay is 0
        Randomize2DArray(knockbackDirections1Tile);
        Randomize2DArray(knockbackDirections2Tile);
        Randomize2DArray(knockbackDirections3Tile);
        Randomize2DArray(tilesThatTheEnemyWillMoveTo);
    }

    public void DeactivateAllMoveTiles()
    {
        for (int i = 0; i < moveTiles.Length; i++)
        {
            moveTiles[i].SetActive(false);
        }
    }

    public void ActivateSpecificMoveTiles()
    {
        foreach (int[] offset in moveTilesToSpawn)
        {
            int gridXOffset = offset[0];
            int gridYOffset = offset[1];

            if (actualXYCoordinates.IsThisStillInsideTheBoard(currGridX + gridXOffset, currGridY + gridYOffset))
            {
                int index = gridXOffset + 3 + (gridYOffset + 3) * 7;
                moveTiles[index].SetActive(true);
            }
        }
    }

    public void DeactivateAllAttackTiles()
    {
        for (int i = 0; i < attackTiles.Length; i++)
        {
            attackTiles[i].SetActive(false);
        }
    }

    public void ActivateSpecificAttackTiles()
    {
        foreach (int[] offset in attackTilesToSpawn)
        {
            int gridXOffset = offset[0];
            int gridYOffset = offset[1];

            if (actualXYCoordinates.IsThisStillInsideTheBoard(currGridX + gridXOffset, currGridY + gridYOffset))
            {
                int index = gridXOffset + 3 + (gridYOffset + 3) * 7;
                attackTiles[index].SetActive(true);
            }
        }
    }

    public void ThisEnemyTakesDamage(int damageTaken, bool isKnockback)
    { // QUESTION: IF THE ATTACK HAS NO KNOCKBACK? WILL THE DELAY ALSO INCREASE  AND ALSO CANNOT ATTACK NEXT TURN? - I think no?
        currHealth = currHealth - damageTaken;
        if (currHealth <= 0)
        {
            //Debug.Log("You just killed a " + thisEnemyVariant);
            musicController.PlayBuySellSoundEffectSource();
            playerAndEnemyStatusController.AnEnemyWasKilledAndEarnGold(2);
            Destroy(gameObject);
        }
        else
        {
            //Debug.Log("You dealt " + damageTaken + " damage on a " + thisEnemyVariant);
            Invoke(nameof(CallForEnemyTurn), 0.5f); // THIS MIGHT BE PROBLEMATIC WITH POWEUPS THAT ATTACK MULTIPLE ENEMIES AT ONCE
            musicController.PlayDamageSoundEffectSource();
            groupHitpoints.SetActive(true);
            imageGreenHealthBar.fillAmount = (float)currHealth / (float)maxHealth;
            textDamageOrHealTaken.text = "-" + damageTaken;
            Invoke(nameof(HideHealthBar), 0.5f);
            // We cannot just setActive the entire move and attack tile groups. It must change depending how close they are to the board's edges

            if (isKnockback)
            {
                currDelayLeft += 1;
                DeactivateAllMoveTiles();
                Invoke(nameof(ActivateSpecificMoveTiles), 0.5f);
                DeactivateAllAttackTiles();
                Invoke(nameof(ActivateSpecificAttackTiles), 0.5f);
                cannotAttackNextTurn = true;
                alreadyKnockbackedFromDamage = false;
                CheckForNearbyEmtpyTilesToMoveToWhenKnockedbacked(knockbackDirections1Tile);
                if (!alreadyKnockbackedFromDamage) { CheckForNearbyEmtpyTilesToMoveToWhenKnockedbacked(knockbackDirections2Tile); }
                if (!alreadyKnockbackedFromDamage) { CheckForNearbyEmtpyTilesToMoveToWhenKnockedbacked(knockbackDirections3Tile); }
            }
        }
    }

    public void CheckForNearbyEmtpyTilesToMoveToWhenKnockedbacked(int[][] knockbackDirections)
    {
        alreadyMoved = false;
        foreach (int[] direction in knockbackDirections)
        {
            int gridXOffset = direction[0];
            int gridYOffset = direction[1];
            if (actualXYCoordinates.IsThisStillInsideTheBoard(currGridX + gridXOffset, currGridY + gridYOffset) && playerAndEnemyStatusController.GetIdentityOfPieceAtThisBoardTile(currGridX + gridXOffset, currGridY + gridYOffset) == null)
            {
                alreadyKnockbackedFromDamage = true;
                alreadyMoved = true;
                playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, null);
                playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX + gridXOffset, currGridY + gridYOffset, gameObject);
                thisPieceTransform.DOMove(new Vector3(actualXYCoordinates.GetActualXCoordinate(currGridX + gridXOffset), actualXYCoordinates.GetActualYCoordinate(currGridY + gridYOffset), transform.position.z), 0.5f);
                canvasUITransform.DOAnchorPos(new Vector2(actualXYCoordinates.GetActualXCoordinateUIImage(currGridX + gridXOffset), actualXYCoordinates.GetActualYCoordinateUIImage(currGridY + gridYOffset)), 0.5f);
                // Randomize the 2D knockback arrays again
                if (knockbackDirections.Length == 8) { Randomize2DArray(knockbackDirections1Tile); }
                else if (knockbackDirections.Length == 16) { Randomize2DArray(knockbackDirections2Tile); }
                else if (knockbackDirections.Length == 24) { Randomize2DArray(knockbackDirections3Tile); }
                currGridX += gridXOffset;
                currGridY += gridYOffset;
                break;
            }
        }
        if (!alreadyMoved) { Debug.LogError("This enemy cannot be knockbacked to anywhere"); } // But we make sure there is not more than 14 enemies, so this shouldn't ever happen
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

    public void ActivateEnemyTurnForRedTiles()
    {
        bool playerWasInThisEnemysAttackTile = false;

        if (cannotAttackNextTurn) // If this enemy was just knockedbacked, it cannot attack player this turn with its red tiles
        {
            cannotAttackNextTurn = false;
        }
        else
        {
            foreach (int[] offset in attackTilesToSpawn)
            {
                int gridXOffset = offset[0];
                int gridYOffset = offset[1];

                if (actualXYCoordinates.IsThisStillInsideTheBoard(currGridX + gridXOffset, currGridY + gridYOffset) && playerAndEnemyStatusController.GetIdentityOfPieceAtThisBoardTile(currGridX + gridXOffset, currGridY + gridYOffset) == prefabPlayerObject)
                {
                    playerWasInThisEnemysAttackTile = true;
                    redGlow.SetActive(true);
                    musicController.PlayDamageSoundEffectSource();
                    break;
                }
            }
        }

        if (playerWasInThisEnemysAttackTile) {
            codeForPrefabPlayer.PlayerTakesDamage(enemyDamage, false);
            Invoke(nameof(CallForNextEnemysTurnOneByOneForRedTiles), 0.5f);
        }
        else { CallForNextEnemysTurnOneByOneForRedTiles(); }
    }

    public void ActivateEnemyTurnForYellowTiles()
    {
        redGlow.SetActive(false);

        currDelayLeft -= 1;
        if (currDelayLeft == 1)
        {
            ShowMoveTiles();
            yellowGlow.SetActive(true);
            CallForNextEnemysTurnOneByOneForYellowTiles();
        }
        else if (currDelayLeft == 0)
        {
            HideMoveTiles();
            yellowGlow.SetActive(false);
            DeactivateAllMoveTiles();
            Invoke(nameof(ActivateSpecificMoveTiles), 0.5f);
            DeactivateAllAttackTiles();
            Invoke(nameof(ActivateSpecificAttackTiles), 0.5f);
            EnemyMovesToOneOfItsYellowTiles(); // The enemy will now move to one of its yellow tiles
            currDelayLeft = moveDelay;

            if (currDelayLeft == 1) // Specific only for enemies whose move Delay is max just 1
            {
                ShowMoveTiles();
                yellowGlow.SetActive(true);
            }
        }
        else
        {
            CallForNextEnemysTurnOneByOneForYellowTiles();
        }
    }

    public void EnemyMovesToOneOfItsYellowTiles()
    { // EASY = random / always avoids hitting player when possible      MEDIUM = goes to the direction of the player             HARD = goes to player (and lands on them if possible) but otherwise move to a tile without one of the player's move tiles?

        // EASY ==============================================================================
        //foreach (int[] direction in tilesThatTheEnemyWillMoveTo)
        //{
        //    int gridXOffset = direction[0];
        //    int gridYOffset = direction[1];
        //    if (actualXYCoordinates.IsThisStillInsideTheBoard(currGridX + gridXOffset, currGridY + gridYOffset) && playerAndEnemyStatusController.GetIdentityOfPieceAtThisBoardTile(currGridX + gridXOffset, currGridY + gridYOffset) == null)
        //    {
        //        alreadyKnockbackedFromDamage = true;
        //        playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, null);
        //        playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX + gridXOffset, currGridY + gridYOffset, gameObject);
        //        thisPieceTransform.DOMove(new Vector3(actualXYCoordinates.GetActualXCoordinate(currGridX + gridXOffset), actualXYCoordinates.GetActualYCoordinate(currGridY + gridYOffset), transform.position.z), 0.5f);
        //        canvasUITransform.DOAnchorPos(new Vector2(actualXYCoordinates.GetActualXCoordinateUIImage(currGridX + gridXOffset), actualXYCoordinates.GetActualYCoordinateUIImage(currGridY + gridYOffset)), 0.5f);
        //        // Randomize the 2D enemy movement arrays again
        //        Randomize2DArray(tilesThatTheEnemyWillMoveTo);
        //        currGridX += gridXOffset;
        //        currGridY += gridYOffset;
        //        break;
        //    }
        //}

        // MEDIUM ==============================================================================
        // Check first if player exist in one of the yellow tiles. If yes, then move there and attack player + cause them to knockback. If not, then move to any random tiles (or the one closest to the player)
        // Maybe we can do that? Calculate the euclidean distance from a yellow tile to the player then pick the smallest one?
        alreadyMoved = false;
        lowestEuclideanDistance = 99;
        yellowTileIndexWithLowestEuclideanDistance = -1;
        for (int i = 0; i < tilesThatTheEnemyWillMoveTo.Length; i++)
            {
            int gridXOffset = tilesThatTheEnemyWillMoveTo[i][0];
            int gridYOffset = tilesThatTheEnemyWillMoveTo[i][1];
            // MAKE SURE IT IS NOT NULL FIRST, ONLY THEN YOU CAN CHECK IF IT HAS THE PLAYER OR ENEMY SCRIPT COMPONENTS
            if (actualXYCoordinates.IsThisStillInsideTheBoard(currGridX + gridXOffset, currGridY + gridYOffset))
            {
                GameObject currentGameObjectAtThisTile = playerAndEnemyStatusController.GetIdentityOfPieceAtThisBoardTile(currGridX + gridXOffset, currGridY + gridYOffset);
                if (currentGameObjectAtThisTile != null && currentGameObjectAtThisTile.GetComponent<CodeForPrefabPlayer>())
                {
                    lowestEuclideanDistance = 0;
                    yellowTileIndexWithLowestEuclideanDistance = i;
                }
                else if (currentGameObjectAtThisTile == null)
                {
                    (int playerCurrGridX, int playerCurrGridY) = codeForPrefabPlayer.GetPlayerCurrGridXAndCurrGridY();
                    int xDifferenceToPlayer = (currGridX + gridXOffset) - playerCurrGridX;
                    int yDifferenceToPlayer = (currGridY + gridYOffset) - playerCurrGridY;
                    float currentEuclideanDistance = Mathf.Sqrt(xDifferenceToPlayer * xDifferenceToPlayer + yDifferenceToPlayer * yDifferenceToPlayer);
                    if (currentEuclideanDistance < lowestEuclideanDistance)
                    {
                        lowestEuclideanDistance = currentEuclideanDistance;
                        yellowTileIndexWithLowestEuclideanDistance = i;
                    }
                }
            }
        }

        if (yellowTileIndexWithLowestEuclideanDistance > -1)
        {
            int gridXOffset = tilesThatTheEnemyWillMoveTo[yellowTileIndexWithLowestEuclideanDistance][0];
            int gridYOffset = tilesThatTheEnemyWillMoveTo[yellowTileIndexWithLowestEuclideanDistance][1];
            alreadyMoved = true;
            musicController.PlayMoveSoundEffectSource();

            playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, null);
            thisPieceTransform.DOMove(new Vector3(actualXYCoordinates.GetActualXCoordinate(currGridX + gridXOffset), actualXYCoordinates.GetActualYCoordinate(currGridY + gridYOffset), transform.position.z), 0.5f);
            canvasUITransform.DOAnchorPos(new Vector2(actualXYCoordinates.GetActualXCoordinateUIImage(currGridX + gridXOffset), actualXYCoordinates.GetActualYCoordinateUIImage(currGridY + gridYOffset)), 0.5f);
            // Randomize the 2D enemy movement arrays again
            Randomize2DArray(tilesThatTheEnemyWillMoveTo);
            currGridX += gridXOffset;
            currGridY += gridYOffset;
            Invoke(nameof(MoveToThisTileAndAttackPlayerIfItExistsHere), 0.5f);
        }

        if (!alreadyMoved) { Debug.Log(thisEnemyVariant + " cannot move to any one of its yellow tiles"); }
    }

    public void MoveToThisTileAndAttackPlayerIfItExistsHere() // Unlike the MoveToThisTileAndAttackEnemyIfItExistsHere() function from the prefab player, the enemy already knows the player is here. no need to check again
    { // Must differetiate for the easy / medium / hard difficulties?
        if (lowestEuclideanDistance == 0) // will attatck player
        {
            codeForPrefabPlayer.PlayerTakesDamage(enemyDamage, true);
            playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, gameObject);
            Invoke(nameof(CallForNextEnemysTurnOneByOneForYellowTiles), 0.5f); // Unlike the one in MoveToThisTileAndAttackEnemyIfItExistsHere(), wecan place the function to call the next turn here.
        }
        else
        {
            playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, gameObject);
            CallForNextEnemysTurnOneByOneForYellowTiles();
        }
    }

    public void CallForEnemyTurn()
    {
        turnController.EnemyTurn();
    }

    public void CallForNextEnemysTurnOneByOneForRedTiles()
    {
        turnController.NextEnemysTurnOneByOneForRedTiles();
    }

    public void CallForNextEnemysTurnOneByOneForYellowTiles()
    {
        turnController.NextEnemysTurnOneByOneForYellowTiles();
    }



    // Warning: THESE 4 FUNCTIONS ONLY CONTROL IF THE MOVEMENT AND ATTACK TILES ARE SHOWN OR HIDDEN, NOT CHANGE WHICH SPECIFIC TILES ARE ACTIVATED. NOTE THAT THEY CANNOT BE CHANGED WHEN INACTIVE
    public void HideMoveTiles()
    {
        groupMoveTiles.SetActive(false);
    }

    public void ShowMoveTiles()
    {
        groupMoveTiles.SetActive(true);
    }

    public void HideAttackTiles()
    {
        groupAttackTiles.SetActive(false);
    }

    public void ShowAttackTiles()
    {
        groupAttackTiles.SetActive(true);
    }




    //public void SayHello()
    //{
    //    Debug.Log(thisEnemyVariant + " is saying hello");
    //}
}