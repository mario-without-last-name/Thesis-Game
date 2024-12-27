using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Reflection;

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
    private DynamicDifficultyController dynamicDifficultyController;
    private BottomBarController bottomBarController;
    private PowerupsCatalogController powerupsCatalogController;

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
    private int goldOnKill;
    private int moveDelay;
    private int currDelayLeft;
    private int numberOfMovesThisRound;
    private int numberOfTurnsThisRound;
    private float opacityOfMoveTilesAndAttackTiles;

    private bool alreadyKnockbackedFromDamage;
    private bool cannotAttackNextTurn;
    private bool alreadyMoved;
    private float lowestEuclideanDistance;
    private int yellowTileIndexWithLowestEuclideanDistance;
    private string selectedDifficulty;

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
        GameObject actualXYCoordinatesObject = GameObject.FindGameObjectWithTag("tagForActualXYCoordinatesController");
        actualXYCoordinates = actualXYCoordinatesObject.GetComponent<ActualXYCoordinatesController>();
        GameObject playerAndEnemyStatusControllerObject = GameObject.FindGameObjectWithTag("tagForPlayerAndEnemyStatusController");
        playerAndEnemyStatusController = playerAndEnemyStatusControllerObject.GetComponent<PlayerAndEnemyStatusController>();
        GameObject bestiaryControllerObject = GameObject.FindGameObjectWithTag("tagForBestiaryController");
        bestiaryController = bestiaryControllerObject.GetComponent<BestiaryController>();
        GameObject musicControllerObject = GameObject.FindGameObjectWithTag("tagForMusicController");
        musicController = musicControllerObject.GetComponent<MusicController>();
        GameObject turnControllerObject = GameObject.FindGameObjectWithTag("tagForTurnController");
        turnController = turnControllerObject.GetComponent<TurnController>();
        GameObject dynamicDifficultyControllerObject = GameObject.FindGameObjectWithTag("tagForDynamicDifficultyController");
        dynamicDifficultyController = dynamicDifficultyControllerObject.GetComponent<DynamicDifficultyController>();
        GameObject bottomBarControllerObject = GameObject.FindGameObjectWithTag("tagForBottomBarController");
        bottomBarController = bottomBarControllerObject.GetComponent<BottomBarController>();
        GameObject powwerupsCatalogControllerObject = GameObject.FindGameObjectWithTag("tagForPowerupsCatalogController");
        powerupsCatalogController = powwerupsCatalogControllerObject.GetComponent<PowerupsCatalogController>();
        // Must know where the player is.
        prefabPlayerObject = GameObject.FindGameObjectWithTag("tagForPrefabPlayer");
        codeForPrefabPlayer = prefabPlayerObject.GetComponent<CodeForPrefabPlayer>();

        selectedDifficulty = PlayerPrefs.GetString("modeDifficulty", "Adaptive");
        numberOfMovesThisRound = 0;
        numberOfTurnsThisRound = 0;

        // Find out what enemy variant this piece should be as controlled by the playerAndEnemyStatusController
        thisEnemyVariant = playerAndEnemyStatusController.GetWhichEnemyVariantToSpawn();

        // Based on the enemy variant, determine its sprite, health, damage, and delay
        piecePicture.sprite = bestiaryController.GetEnemySprite(thisEnemyVariant);
        UpdateEnemyStats();
        currHealth = maxHealth;
        currDelayLeft = moveDelay;
        cannotAttackNextTurn = false;

        // An enemy must spawn under 3 criteria...
        (int playerCurrGridX, int playerCurrGridY) = codeForPrefabPlayer.GetPlayerCurrGridXAndCurrGridY();
        Randomize2DArray(allPossibleBoardTileLocations);
        foreach (int[] tile in allPossibleBoardTileLocations)
        {
            currGridX = tile[0];
            currGridY = tile[1];

            // 1. not on an occupied tile
            // 2. not in the 3 leftmost/rightmost columns,
            // 3. is between 4-6 tiles away from the player
            if (playerAndEnemyStatusController.GetIdentityOfPieceAtThisBoardTile(currGridX, currGridY) == null &&
                currGridX >= 4 && currGridX <= 13 &&
                ( ( Math.Abs(playerCurrGridX - currGridX) >= 4 && Math.Abs(playerCurrGridX - currGridX) <= 6 ) || ( Math.Abs(playerCurrGridY - currGridY) >= 4 && Math.Abs(playerCurrGridY - currGridY) <= 7 ) )
            )
            {
                break; // so stop looking for another coordinate, we already found one that satisfies all spawn criteria
            }
        }
        thisPieceTransform = GetComponent<Transform>();
        thisPieceTransform.position = new Vector3(actualXYCoordinates.GetActualXCoordinate(currGridX), actualXYCoordinates.GetActualYCoordinate(currGridY), transform.position.z);
        playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, gameObject);

        canvasUITransform = groupCanvasUI.GetComponent<RectTransform>();
        canvasUITransform.anchoredPosition = new Vector2(actualXYCoordinates.GetActualXCoordinateUIImage(currGridX), actualXYCoordinates.GetActualYCoordinateUIImage(currGridY));

        // Reveal the enemies' attack and move tile when spawned. After this enemy moves once, control their visibility
        //opacityOfMoveTilesAndAttackTiles = 1.0f;

        // Based on the enemy variant, determine the move tiles and attack tiles it has
        DeactivateAllMoveTiles();
        moveTilesToSpawn = bestiaryController.GetEnemyMoveTiles(thisEnemyVariant);
        ActivateSpecificMoveTiles();
        tilesThatTheEnemyWillMoveTo = moveTilesToSpawn;
        DeactivateAllAttackTiles();
        attackTilesToSpawn = bestiaryController.GetEnemyAttackTiles(thisEnemyVariant);
        ActivateSpecificAttackTiles();

        //// Determine Visibility of move tiles and attack tiles after the enemy has moved once
        //if (selectedDifficulty == "Easy") { opacityOfMoveTilesAndAttackTiles = 1.0f; }
        //else if (selectedDifficulty == "Medium") { opacityOfMoveTilesAndAttackTiles = 0.5f; }
        //else if (selectedDifficulty == "Hard") { opacityOfMoveTilesAndAttackTiles = 0.0f; }

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

    public void UpdateEnemyStats()
    {
        //int[] pieceHealthDamageDelayGold = bestiaryController.GetHealthAttackDelayGold(thisEnemyVariant);
        //highestMaxHealth = pieceHealthDamageDelayGold[0];
        //enemyDamage = pieceHealthDamageDelayGold[1];
        //moveDelay = pieceHealthDamageDelayGold[2];
        //goldOnKill = pieceHealthDamageDelayGold[3];

        float difficultyIndex = dynamicDifficultyController.GetDynamicOutput("enemyStats");
        int[] pieceHealthDamageDelayGoldMinAndMax = bestiaryController.GetHealthAttackDelayGoldMinAndMax(thisEnemyVariant);

        // Calculate stats based on difficulty index (INTERPOLATION)
        int lowesthighestMaxHealth = pieceHealthDamageDelayGoldMinAndMax[0];
        int highestMaxHealth = pieceHealthDamageDelayGoldMinAndMax[1];
        maxHealth = Mathf.RoundToInt(lowesthighestMaxHealth + (highestMaxHealth - lowesthighestMaxHealth) * difficultyIndex);
        int minEnemyDamage = pieceHealthDamageDelayGoldMinAndMax[2];
        int maxEnemyDamage = pieceHealthDamageDelayGoldMinAndMax[3];
        enemyDamage = Mathf.RoundToInt(minEnemyDamage + (maxEnemyDamage - minEnemyDamage) * difficultyIndex);
        int slowestMoveDelay = pieceHealthDamageDelayGoldMinAndMax[4];
        int fastestMoveDelay = pieceHealthDamageDelayGoldMinAndMax[5];
        moveDelay = Mathf.RoundToInt(slowestMoveDelay - (slowestMoveDelay - fastestMoveDelay) * difficultyIndex);
        goldOnKill = pieceHealthDamageDelayGoldMinAndMax[6];
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
        if (numberOfMovesThisRound == 0) { opacityOfMoveTilesAndAttackTiles = 1.0f; }
        else { opacityOfMoveTilesAndAttackTiles = Math.Max(1.0f - dynamicDifficultyController.GetDynamicOutput("visualHint") * 1.0f , 0); }
        foreach (int[] offset in moveTilesToSpawn)
        {
            int gridXOffset = offset[0];
            int gridYOffset = offset[1];

            if (actualXYCoordinates.IsThisStillInsideTheBoard(currGridX + gridXOffset, currGridY + gridYOffset))
            {
                int index = gridXOffset + 3 + (gridYOffset + 3) * 7;
                moveTiles[index].SetActive(true);
                moveTiles[index].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacityOfMoveTilesAndAttackTiles);
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
        if (numberOfTurnsThisRound == 0) { opacityOfMoveTilesAndAttackTiles = 1.0f; }
        else { opacityOfMoveTilesAndAttackTiles = Math.Max(1.0f - dynamicDifficultyController.GetDynamicOutput("visualHint") * 1.0f, 0); }
        foreach (int[] offset in attackTilesToSpawn)
        {
            int gridXOffset = offset[0];
            int gridYOffset = offset[1];

            if (actualXYCoordinates.IsThisStillInsideTheBoard(currGridX + gridXOffset, currGridY + gridYOffset))
            {
                int index = gridXOffset + 3 + (gridYOffset + 3) * 7;
                attackTiles[index].SetActive(true);
                attackTiles[index].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, opacityOfMoveTilesAndAttackTiles);
            }
        }
    }

    public void ThisEnemyTakesDamage(int damageTaken, bool isKnockback)
    { // QUESTION: IF THE ATTACK HAS NO KNOCKBACK? WILL THE DELAY ALSO INCREASE  AND ALSO CANNOT ATTACK NEXT TURN? - I think no?

        if (!playerAndEnemyStatusController.getHasAnEnemyAlreadyBeenDamagedThisTurn())
        {
            dynamicDifficultyController.SetDynamicInputChange("damageReceivedAndDealt", +0.025f, false);
            playerAndEnemyStatusController.setHasAnEnemyAlreadyBeenDamagedThisTurn(true);
        }

        if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-mercenaryTools")) { damageTaken *= 2; }
        if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-bloodlust")) { damageTaken += powerupsCatalogController.ActivateThisPassivePowerup("passive-bloodlust", ""); }

        playerAndEnemyStatusController.SetNumberOfTimesEnemiesTookDamageThisRoundIncreaseBy1AndResetGracePeriodBeforeReducingBonusGold();
        currHealth = currHealth - damageTaken;

        // enemy is dead
        if (currHealth <= 0)
        {
            //Debug.Log("You just killed a " + thisEnemyVariant);
            musicController.PlayBuySellSoundEffectSource();
            playerAndEnemyStatusController.AnEnemyWasKilledAndEarnGold(goldOnKill);
            Destroy(gameObject);
        }
        else
        {
            //Debug.Log("You dealt " + damageTaken + " damage on a " + thisEnemyVariant);

            // ACTIVE AND PASSIVE POWERUP
            // If this enemy is damaged from direct landing damage, call for all enemy turn. Else if it is damaged when player has an active powerup activated (or ground pound), just wait for the player prefab to call all enemy turn
            if (playerAndEnemyStatusController.getCurrentActivePowerupIdentity() == "") { Invoke(nameof(CallForEnemyTurn), 0.5f); }

            musicController.PlayDamageSoundEffectSource();
            groupHitpoints.SetActive(true);
            imageGreenHealthBar.fillAmount = Math.Min((float)currHealth / (float)maxHealth, 1);
            textDamageOrHealTaken.text = "-" + damageTaken;
            Invoke(nameof(HideHealthBar), 0.5f);
            // We cannot just setActive the entire move and attack tile groups. It must change depending how close they are to the board's edges
            currDelayLeft += 1;

            if (isKnockback)
            {
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

    public void ActivateEnemyTurnForRedTiles() // Check if player is in one of this enemy's red tiles at the end of their turn. If yes, damage them (no knockback)
    {
        UpdateEnemyStats(); // AT THE START OF THE ENEMY'S TURN, START UPDATING THEIR STATS.
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
            codeForPrefabPlayer.PlayerTakesDamageOrHealing(-1 * enemyDamage, false);
            Invoke(nameof(CallForNextEnemysTurnOneByOneForRedTiles), 0.5f);
        }
        else { CallForNextEnemysTurnOneByOneForRedTiles(); }
    }

    public void ActivateEnemyTurnForYellowTiles() // Enemy reduces their movement delay by 1.
    {
        UpdateEnemyStats(); // UPDATING THEIR STATS AGAIN
        DeactivateAllMoveTiles(); // REFRESH ATTACK TILES RIGHT NOW TO UPDATE THE OPACITY
        ActivateSpecificMoveTiles();
        numberOfTurnsThisRound += 1;
        redGlow.SetActive(false);

        if (numberOfTurnsThisRound == 1) // to readjust the red tile visibility after 1 turn
        {
            DeactivateAllAttackTiles();
            ActivateSpecificAttackTiles();
        }

        currDelayLeft -= 1;
        if (currDelayLeft == 1)
        {
            ShowMoveTiles();
            yellowGlow.SetActive(true);
            CallForNextEnemysTurnOneByOneForYellowTiles();
        }
        else if (currDelayLeft == 0)
        {
            numberOfMovesThisRound += 1;
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

    public void EnemyMovesToOneOfItsYellowTiles() // Move to a yellow tile when their delay counter is 0. If there is a player in the target, then damage + knockback the player. If none of the yelloe tiles point to an empty tile, then just skip this turn.
    { 
        alreadyMoved = false;
        lowestEuclideanDistance = 999;
        yellowTileIndexWithLowestEuclideanDistance = -1; 

        // For the Hard difficulty only, calculating where the player can move next turn
        (int playerCurrGridX, int playerCurrGridY) = codeForPrefabPlayer.GetPlayerCurrGridXAndCurrGridY();
        int[][] originalPlayerMoveTiles = codeForPrefabPlayer.GetPlayerMoveTiles();
        int[][] playerCanMoveToTheseMoveTilesNextTurn = new int[originalPlayerMoveTiles.Length][];
        for (int i = 0; i < originalPlayerMoveTiles.Length; i++)
        {
            playerCanMoveToTheseMoveTilesNextTurn[i] = new int[2];
            playerCanMoveToTheseMoveTilesNextTurn[i][0] = originalPlayerMoveTiles[i][0] + playerCurrGridX;
            playerCanMoveToTheseMoveTilesNextTurn[i][1] = originalPlayerMoveTiles[i][1] + playerCurrGridY;
        }

        for (int i = 0; i < tilesThatTheEnemyWillMoveTo.Length; i++)
            {
            int gridXOffset = tilesThatTheEnemyWillMoveTo[i][0];
            int gridYOffset = tilesThatTheEnemyWillMoveTo[i][1];
            float aiMovementDifficulty = 0.0f;
            aiMovementDifficulty = selectedDifficulty == "Easy" ? 0.0f : selectedDifficulty == "Medium" ? 0.5f : selectedDifficulty == "Hard" ? 1.0f : 0.5f;
            if (selectedDifficulty == "Adaptive") { aiMovementDifficulty = dynamicDifficultyController.GetDynamicOutput("enemyAI"); } 
            // If selected difficulty is adaptive, then switch between easy / easy 2 / medium / hard ai movements


            if (actualXYCoordinates.IsThisStillInsideTheBoard(currGridX + gridXOffset, currGridY + gridYOffset))
            {
                GameObject currentGameObjectAtThisTile = playerAndEnemyStatusController.GetIdentityOfPieceAtThisBoardTile(currGridX + gridXOffset, currGridY + gridYOffset);

                // EASY ===========================================================
                // Prioritize going to a tile the player can land on next turn, avoid the player when possible, else, just move randomly.
                if (aiMovementDifficulty >= 0.0f && aiMovementDifficulty < 0.25f)
                {
                    if (currentGameObjectAtThisTile != null && currentGameObjectAtThisTile.GetComponent<CodeForPrefabPlayer>())
                    {
                        float currentEuclideanDistance = 500;
                        if (currentEuclideanDistance < lowestEuclideanDistance)
                        {
                            lowestEuclideanDistance = currentEuclideanDistance;
                            yellowTileIndexWithLowestEuclideanDistance = i;
                        }
                    }
                    else if (currentGameObjectAtThisTile == null)
                    {
                        int xDifferenceToPlayer = (currGridX + gridXOffset) - playerCurrGridX;
                        int yDifferenceToPlayer = (currGridY + gridYOffset) - playerCurrGridY;

                        bool playerMayMoveToThisTileNextTurn = false;
                        for (int j = 0; j < playerCanMoveToTheseMoveTilesNextTurn.Length; j++)
                        {
                            if (currGridX + gridXOffset == playerCanMoveToTheseMoveTilesNextTurn[j][0] && currGridY + gridYOffset == playerCanMoveToTheseMoveTilesNextTurn[j][1])
                            {
                                playerMayMoveToThisTileNextTurn = true;

                                break;
                            }
                        }

                        float currentEuclideanDistance = 0;
                        if (playerMayMoveToThisTileNextTurn) { currentEuclideanDistance = UnityEngine.Random.Range(1f, 99f); }
                        else { currentEuclideanDistance = UnityEngine.Random.Range(100f, 199f); }

                        if (currentEuclideanDistance < lowestEuclideanDistance)
                        {
                            lowestEuclideanDistance = currentEuclideanDistance;
                            yellowTileIndexWithLowestEuclideanDistance = i;
                        }
                    }
                }

                // EASY 2 ===========================================================
                // Completely random
                else if (aiMovementDifficulty >= 0.25f && aiMovementDifficulty < 0.5f)
                {
                    if ( ( currentGameObjectAtThisTile != null && currentGameObjectAtThisTile.GetComponent<CodeForPrefabPlayer>() ) || currentGameObjectAtThisTile == null )
                    {
                        float currentEuclideanDistance = UnityEngine.Random.Range(1f, 99f);
                        if (currentEuclideanDistance < lowestEuclideanDistance)
                        {
                            lowestEuclideanDistance = currentEuclideanDistance;
                            yellowTileIndexWithLowestEuclideanDistance = i;
                        }
                    }
                }

                // MEDIUM ===========================================================
                // Move to the yellow tile with the lowest euclidean distance to the player
                else if (aiMovementDifficulty >= 0.5f && aiMovementDifficulty < 0.75f)
                {
                    if (currentGameObjectAtThisTile != null && currentGameObjectAtThisTile.GetComponent<CodeForPrefabPlayer>())
                    {
                        lowestEuclideanDistance = 0;
                        yellowTileIndexWithLowestEuclideanDistance = i;
                    }
                    else if (currentGameObjectAtThisTile == null)
                    {
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

                // HARD ===========================================================
                // like Medium, but avoid going to one of the tiles that the player can jump to in the next turn.
                else if (aiMovementDifficulty >= 0.75f && aiMovementDifficulty <= 1.0f)
                {
                    if (currentGameObjectAtThisTile != null && currentGameObjectAtThisTile.GetComponent<CodeForPrefabPlayer>())
                    {
                        lowestEuclideanDistance = 0;
                        yellowTileIndexWithLowestEuclideanDistance = i;
                    }
                    else if (currentGameObjectAtThisTile == null)
                    {
                        int xDifferenceToPlayer = (currGridX + gridXOffset) - playerCurrGridX;
                        int yDifferenceToPlayer = (currGridY + gridYOffset) - playerCurrGridY;

                        bool playerMayMoveToThisTileNextTurn = false;
                        for (int j = 0; j < playerCanMoveToTheseMoveTilesNextTurn.Length; j++)
                        {
                            if (currGridX + gridXOffset == playerCanMoveToTheseMoveTilesNextTurn[j][0] && currGridY + gridYOffset == playerCanMoveToTheseMoveTilesNextTurn[j][1])
                            {
                                playerMayMoveToThisTileNextTurn = true;

                                break;
                            }
                        }

                        float currentEuclideanDistance = 0;
                        if (playerMayMoveToThisTileNextTurn) { currentEuclideanDistance = Mathf.Sqrt(xDifferenceToPlayer * xDifferenceToPlayer + yDifferenceToPlayer * yDifferenceToPlayer) + 100; }
                        else { currentEuclideanDistance = Mathf.Sqrt(xDifferenceToPlayer * xDifferenceToPlayer + yDifferenceToPlayer * yDifferenceToPlayer); }

                        if (currentEuclideanDistance < lowestEuclideanDistance)
                        {
                            lowestEuclideanDistance = currentEuclideanDistance;
                            yellowTileIndexWithLowestEuclideanDistance = i;
                        }
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

        if (!alreadyMoved) {
            Debug.Log(thisEnemyVariant + " cannot move to any one of its yellow tiles");
            CallForNextEnemysTurnOneByOneForYellowTiles();
        }
    }

    public void MoveToThisTileAndAttackPlayerIfItExistsHere() // Unlike the MoveToThisTileAndAttackEnemyIfItExistsHere() function from the prefab player, the enemy already knows the player is here. no need to check again
    {
        (int playerCurrGridX, int playerCurrGridY) = codeForPrefabPlayer.GetPlayerCurrGridXAndCurrGridY();
        //if (lowestEuclideanDistance == 0) // will attatck player: THIS WILL NOT WORK FOR THE 2 EASY MODES
        if (currGridX == playerCurrGridX && currGridY == playerCurrGridY)
        {
            codeForPrefabPlayer.PlayerTakesDamageOrHealing(-1 * enemyDamage, true); // move the player elsewhere first from knockbak, before marking the enemy's current move tile to this location (even if it has moved here through DOM animation)
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
        turnController.AllEnemiesTurn();
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