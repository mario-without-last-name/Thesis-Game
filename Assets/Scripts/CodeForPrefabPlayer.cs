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
using UnityEngine.Windows;
//using System.Drawing;

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
    private DynamicDifficultyController dynamicDifficultyController;
    private BottomBarController bottomBarController;
    private PowerupsCatalogController powerupsCatalogController;

    private bool isMoveTimeLlimitActive; // to control the Update() for controlling the timer
    private float elapsedTime;
    private float timeLimit;
    private int currGridX;
    private int currGridY;

    // The PlayerAndStatusController is the one who will remember the player's stats, and values are fetched from there, since the player prefab instance can be destroyed
    private int maxHealth;
    private int currHealth;
    private int playerDirectContactDamage;
    private int playerBonusDamage;
    private bool alreadyKnockbackedFromDamage;
    //private bool stopTimerButNoChangeToDGBInputTime;

    private int[][] moveTilesToSpawn                      = new int[][] { new int[] { 1, 2}, new int[] { 2, 1}, new int[] {-1, 2}, new int[] {-2, 1}, new int[] { 1,-2}, new int[] { 2,-1}, new int[] {-1,-2}, new int[] {-2,-1} };
    private int[][] knockbackDirections1Tile              = new int[][] { new int[] { 1, 1}, new int[] { 1, 0}, new int[] { 1,-1}, new int[] { 0,-1}, new int[] {-1,-1}, new int[] {-1, 0}, new int[] {-1, 1}, new int[] { 0, 1} };
    private int[][] knockbackDirections2Tile              = new int[][] { new int[] { 2, 2}, new int[] { 2, 1}, new int[] { 2, 0}, new int[] { 2,-1}, new int[] { 2,-2}, new int[] { 1,-2}, new int[] { 0,-2}, new int[] {-1,-2}, new int[] {-2,-2}, new int[] {-2,-1}, new int[] {-2, 0}, new int[] {-2, 1}, new int[] {-2, 2}, new int[] {-1, 2}, new int[] { 0, 2}, new int[] { 1, 2} };
    private int[][] knockbackDirections3Tile              = new int[][] { new int[] { 3, 3}, new int[] { 3, 2}, new int[] { 3, 1}, new int[] { 3, 0}, new int[] { 3,-1}, new int[] { 3,-2}, new int[] { 3,-3}, new int[] { 2,-3}, new int[] { 1,-3}, new int[] { 0,-3}, new int[] {-1,-3}, new int[] {-2,-3}, new int[] {-3,-3}, new int[] {-3,-2}, new int[] {-3,-1}, new int[] {-3, 0}, new int[] {-3, 1}, new int[] {-3, 2}, new int[] {-3, 3}, new int[] {-2, 3}, new int[] {-1, 3}, new int[] { 0, 3}, new int[] { 1, 3}, new int[] { 2, 3} };
    private int[][] arrayOfTilesToAttackDependingOnRadius = new int[][] { new int[] { 0, 0} };
    //private Coroutine fillCoroutine; // coroutine is possible, but more complicated than just using update()

    void Awake()
    {
        GameObject actualXYCoordinatesObject = GameObject.FindGameObjectWithTag("tagForActualXYCoordinatesController");
        actualXYCoordinates = actualXYCoordinatesObject.GetComponent<ActualXYCoordinatesController>();
        GameObject PlayerAndEnemyStatusControllerObject = GameObject.FindGameObjectWithTag("tagForPlayerAndEnemyStatusController");
        playerAndEnemyStatusController = PlayerAndEnemyStatusControllerObject.GetComponent<PlayerAndEnemyStatusController>();
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

        currGridX = UnityEngine.Random.Range(7, 11); // Make the player spawn around the center
        currGridY = UnityEngine.Random.Range(3, 7);

        maxHealth = playerAndEnemyStatusController.GetPlayerMaxHealth(); // MAYBE LATER: THE MAX HEALTH IS INCREASED IN THE MIDDLE OF A FIGHT SO IT SHOULD BE UPDATED PER TURN?
        currHealth = playerAndEnemyStatusController.GetPlayerCurrHealth();
        playerDirectContactDamage = playerAndEnemyStatusController.GetPlayerDirectContactDamage();
        playerBonusDamage = playerAndEnemyStatusController.GetPlayerBonusDamage();

        thisPieceTransform.position = new Vector3(actualXYCoordinates.GetActualXCoordinate(currGridX), actualXYCoordinates.GetActualYCoordinate(currGridY), transform.position.z);
        canvasUITransform.anchoredPosition = new Vector2(actualXYCoordinates.GetActualXCoordinateUIImage(currGridX), actualXYCoordinates.GetActualYCoordinateUIImage(currGridY));
        playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, gameObject); // "gameObject" alraedy refers to THIS Prefab gameobject itself
        isMoveTimeLlimitActive = false;
        DeactivateMoveTiles(); //DeactivateMoveTiles(true); DeactivateMoveTiles(false);
        groupHitpoints.SetActive(false);
        goldEarned.SetActive(false);

        // Randomize the knockback tiles when an enemy lands on the player
        Randomize2DArray(knockbackDirections1Tile);
        Randomize2DArray(knockbackDirections2Tile);
        Randomize2DArray(knockbackDirections3Tile);

        //stopTimerButNoChangeToDGBInputTime = false;
    }

    public void PlayerCanMoveNow()
    {
        playerDirectContactDamage = playerAndEnemyStatusController.GetPlayerDirectContactDamage(); // maybe necessary to update per turn due to some powerups
        playerBonusDamage = playerAndEnemyStatusController.GetPlayerBonusDamage(); // maybe necessary to update per turn due to some powerups

        circleTimer.SetActive(true);
        groupMoveTiles.SetActive(true);

        //RestartTimer(playerAndEnemyStatusController.GetTimeToMove());

        float timeLimitIndex = dynamicDifficultyController.GetDynamicOutput("timeLimit");
        if (timeLimitIndex <= 0.5f) { timeLimit = Mathf.Lerp(6.0f, 3.0f, timeLimitIndex / 0.5f); }
        else { timeLimit = Mathf.Lerp(3.0f, 1.5f, (timeLimitIndex - 0.5f) / 0.5f); }
        RestartTimer();

        // Determining the player's move range. It starts with 8 standard knight movements. And more depending on the passive powrups owned
        // PASSIVE POWERUPS
        moveTilesToSpawn = new int[][] { new int[] { 1, 2}, new int[] { 2, 1}, new int[] {-1, 2}, new int[] {-2, 1}, new int[] { 1,-2}, new int[] { 2,-1}, new int[] {-1,-2}, new int[] {-2,-1} };
        List<int[]> moveTilesList = new List<int[]>(moveTilesToSpawn); // must change array form first before you can add new tiles?
        if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-flexibleMovementI"))   { moveTilesList.Add(new int[] { 1, 0}); moveTilesList.Add(new int[] { 0, 1}); moveTilesList.Add(new int[] {-1, 0}); moveTilesList.Add(new int[] { 0,-1}); }
        if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-flexibleMovementII"))  { moveTilesList.Add(new int[] { 1, 1}); moveTilesList.Add(new int[] { 1,-1}); moveTilesList.Add(new int[] {-1, 1}); moveTilesList.Add(new int[] {-1,-1}); }
        if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-flexibleMovementIII")) { moveTilesList.Add(new int[] { 2, 0}); moveTilesList.Add(new int[] { 0, 2}); moveTilesList.Add(new int[] {-2, 0}); moveTilesList.Add(new int[] { 0,-2}); }
        if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-flexibleMovementIV"))  { moveTilesList.Add(new int[] { 2, 2}); moveTilesList.Add(new int[] { 2,-2}); moveTilesList.Add(new int[] {-2, 2}); moveTilesList.Add(new int[] {-2,-2}); }
        if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-flexibleMovementV"))   { moveTilesList.Add(new int[] { 3, 0}); moveTilesList.Add(new int[] { 0, 3}); moveTilesList.Add(new int[] {-3, 0}); moveTilesList.Add(new int[] { 0,-3}); }
        if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-flexibleMovementVI"))  { moveTilesList.Add(new int[] { 3, 3}); moveTilesList.Add(new int[] { 3,-3}); moveTilesList.Add(new int[] {-3, 2}); moveTilesList.Add(new int[] {-3,-3}); }
        if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-flexibleMovementVII")) { moveTilesList.Add(new int[] { 3, 1}); moveTilesList.Add(new int[] { 3,-1}); moveTilesList.Add(new int[] { 1, 3}); moveTilesList.Add(new int[] {-1, 3}); moveTilesList.Add(new int[] {-3, 1}); moveTilesList.Add(new int[] {-3,-1}); moveTilesList.Add(new int[] { 1,-3}); moveTilesList.Add(new int[] {-1,-3}); }
        if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-flexibleMovementVIII")){ moveTilesList.Add(new int[] { 3, 2}); moveTilesList.Add(new int[] { 3,-2}); moveTilesList.Add(new int[] { 2, 3}); moveTilesList.Add(new int[] {-2, 3}); moveTilesList.Add(new int[] {-3, 2}); moveTilesList.Add(new int[] {-3,-2}); moveTilesList.Add(new int[] { 2,-3}); moveTilesList.Add(new int[] {-2,-3}); }
        moveTilesToSpawn = moveTilesList.ToArray();

        // Reveal the move tiles of the player for them to click on
        ActivateMoveTiles();  //ActivateMoveTiles(true, "");
    }

    public void PlayerMoveTileWasClicked(int xAndYOffset) // Activated by clicking one of the player's move tiles or from ACTIVE POWERUP: "teleport". The xAndYOffest variable is the x and y offset values combined into 1 number
    {
        turnController.PlayerIsMovingToATile(); // just a signal so that player cannot select active powerups anymore until it's their turn again

        circleTimer.SetActive(false);
        groupMoveTiles.SetActive(false);
        //dynamicDifficultyController.SetDynamicInputChange("damageReceivedAndDealt", +0.01f, false);
        dynamicDifficultyController.SetDynamicInputChange("TimeThinkingAndStepsTaken", 0.02f - (0.03f * elapsedTime / timeLimit), false);

        musicController.PlayMoveSoundEffectSource();
        //playerAndEnemyStatusController.SetTotalMoveCountIncreaseBy1();
        playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, null);
        isMoveTimeLlimitActive = false;
        int gridXOffset = ((xAndYOffset - (xAndYOffset % 10)) / 10) - 4;
        int gridYOffset = (xAndYOffset % 10) - 4;

        // ACTIVE POWERUP : if teleport ability was used, use a different calculation for gridXOffset and gridYOffset
        if (playerAndEnemyStatusController.getCurrentActivePowerupIdentity() == "active-teleport")
        {
            gridXOffset = xAndYOffset / 10 - currGridX;
            gridYOffset = xAndYOffset % 10 - currGridY;
        }

        DeactivateMoveTiles(); //DeactivateMoveTiles(true); DeactivateMoveTiles(false);
        thisPieceTransform.DOMove(new Vector3(actualXYCoordinates.GetActualXCoordinate(currGridX + gridXOffset), actualXYCoordinates.GetActualYCoordinate(currGridY + gridYOffset), transform.position.z), 0.5f);
        canvasUITransform.DOAnchorPos(new Vector2(actualXYCoordinates.GetActualXCoordinateUIImage(currGridX + gridXOffset), actualXYCoordinates.GetActualYCoordinateUIImage(currGridY + gridYOffset)), 0.5f);
        //canvasUITransform.anchoredPosition = new Vector2(actualXYCoordinates.GetActualXCoordinateUIImage(currGridX + xOffset), actualXYCoordinates.GetActualYCoordinateUIImage(currGridY + yOffset));
        //tilesTransform.anchoredPosition = new Vector2(actualXYCoordinates.GetActualXCoordinateUIImage(currGridX + xOffset), actualXYCoordinates.GetActualYCoordinateUIImage(currGridY + yOffset));
        currGridX += gridXOffset;
        currGridY += gridYOffset;

        Invoke(nameof(MoveToThisTileAndAttackEnemyIfItExistsHere), 0.5f);
    }

    public void MoveToThisTileAndAttackEnemyIfItExistsHere() // When this function is called, the player already moved to the new tile (visually). Only after that will it deal damage to an enemy (if exists on that tile) and change the 2d array location of the player to that index.
    {
        GameObject enemyAtThisTile = playerAndEnemyStatusController.GetIdentityOfPieceAtThisBoardTile(currGridX, currGridY);
        if (enemyAtThisTile != null) // If not null, no doubt that this is an enemy, so just call the enemy script. If one day there are other map entities other than player or enemies, then alternatively use:    enemyAtThisTile != null && enemyAtThisTile.GetComponent<CodeForPrefabEnemy>() != null
        {
            enemyAtThisTile.GetComponent<CodeForPrefabEnemy>().ThisEnemyTakesDamage(playerDirectContactDamage + playerBonusDamage, true);
            playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, gameObject);
            //Invoke(nameof(CallForEnemyTurn), 0.5f); // Don't call it here. Otherwise when the last enemy is killed, it will call for enemy turn, but no enemies so it will immeadiately call for player turn, but the player prefab is already destroyed since we are at the victory screen, leading to an error due to calling a function from a now non-existent player prefab.

        }   
        else
        {
            playerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, gameObject);
            CallForEnemyTurn();
        }
    }

    public void PlayerTakesDamageOrHealing(int damageOrHealingTaken, bool isKnockback) // Make this function also accept healing?
    {
        //currHealth = playerAndEnemyStatusController.GetPlayerCurrHealth();

        if (damageOrHealingTaken <= 0) // negative value means taking damage. 0 means using the dodge active powerup
        {
            int damageTaken = -1 * damageOrHealingTaken;
            // PASSIVE POWERUPS
            if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-lightArmor")) { damageTaken = Mathf.Max(damageTaken - powerupsCatalogController.ActivateThisPassivePowerup("passive-lightArmor",""), 1); }
            if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-heavyArmor")) { damageTaken = Mathf.Max(damageTaken - powerupsCatalogController.ActivateThisPassivePowerup("passive-heavyArmor", ""), 1); }
            if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-diamondArmor")) { damageTaken = Mathf.Max(damageTaken - powerupsCatalogController.ActivateThisPassivePowerup("passive-diamondArmor", ""), 1); }
            if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-mercenaryTools")) { damageTaken = Mathf.Max(damageTaken/2, 1); }
            if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-bloodlust")) { int dummyVariable = powerupsCatalogController.ActivateThisPassivePowerup("passive-bloodlust", "buff"); }

            //ACTIVE POWERUPS - dodge, teleport
            if (playerAndEnemyStatusController.getCurrentActivePowerupIdentity() == "active-dodge")
            {
                damageTaken = 0;
                dynamicDifficultyController.SetDynamicInputChange("powerupUsage", +0.15f, false); // use dodge when you'll get damaged otherwise = good
            }
            else if (playerAndEnemyStatusController.getCurrentActivePowerupIdentity() == "active-teleport")
            {
                dynamicDifficultyController.SetDynamicInputChange("powerupUsage", -0.15f, false); // use teleport but get damaged = bad
            }

            musicController.PlayDamageSoundEffectSource();
            currHealth -= damageTaken;
            if (damageTaken != 0 && !playerAndEnemyStatusController.getHasPlayerAlreadyBeenDamagedThisTurn()) {
                dynamicDifficultyController.SetDynamicInputChange("damageReceivedAndDealt", -0.2f, false);
                playerAndEnemyStatusController.setHasPlayerAlreadyBeenDamagedThisTurn(true);
            }
            dynamicDifficultyController.SetDynamicInputChange("healthLeft", (float)(Mathf.Max(currHealth, 100)) / 100, true);

            if (currHealth <= 0)
            {
                playerAndEnemyStatusController.SetPlayerCurrHealth(0); // Which will eventually also trigger the game over screen
            }
            else
            {
                playerAndEnemyStatusController.SetPlayerCurrHealth(currHealth);
                groupHitpoints.SetActive(true);
                imageGreenHealthBar.fillAmount = (float)currHealth / (float)maxHealth;
                textDamageOrHealTaken.color = Color.red;
                textDamageOrHealTaken.text = "-" + damageTaken;
                Invoke(nameof(HideHealthBar), 0.5f);
                DeactivateMoveTiles(); //DeactivateMoveTiles(true); DeactivateMoveTiles(false);
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
        else // positive means healing
        {
            int healingTaken = damageOrHealingTaken;
            //Debug.Log("healing:" + healingTaken);

            musicController.PlayHealSoundEffectSource();
            currHealth = Mathf.Min(currHealth + healingTaken, maxHealth);

            playerAndEnemyStatusController.SetPlayerCurrHealth(currHealth);
            groupHitpoints.SetActive(true);
            imageGreenHealthBar.fillAmount = (float)currHealth / (float)maxHealth;
            textDamageOrHealTaken.color = Color.green;
            textDamageOrHealTaken.text = "+" + healingTaken;
            Invoke(nameof(HideHealthBar), 0.5f);
            DeactivateMoveTiles(); //DeactivateMoveTiles(true); DeactivateMoveTiles(false);


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

    public void DeactivateMoveTiles()
    {
        // Loop through all tiles and deactivate them
        for (int i = 0; i < moveTiles.Length; i++)
        {
            moveTiles[i].SetActive(false);
        }   
    }

    public void ActivateMoveTiles()
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


    //public void DeactivateMoveTiles(bool isPlayer7x7MoveTiles)
    //{
    //    if (isPlayer7x7MoveTiles) // The 7x7 move tiles surrounding the player
    //    {
    //        // Loop through all tiles and deactivate them
    //        for (int i = 0; i < moveTiles.Length; i++)
    //        {
    //            moveTiles[i].SetActive(false);
    //        }
    //    }
    //    else // The 8x16 move tiles of the entire board for some active powerups
    //    {
    //        for (int i = 0; i < individual8x16MoveTiles.Length; i++)
    //        {
    //            individual8x16MoveTiles[i].SetActive(false);
    //        }
    //    }
    //}

    //public void ActivateMoveTiles(bool isPlayer7x7MoveTiles, string powerupIdentity)
    //{
    //    if (isPlayer7x7MoveTiles) // The 7x7 move tiles surrounding the player
    //    {
    //        foreach (int[] offset in moveTilesToSpawn)
    //        {
    //            int gridXOffset = offset[0];
    //            int gridYOffset = offset[1];

    //            if (actualXYCoordinates.IsThisStillInsideTheBoard(currGridX + gridXOffset, currGridY + gridYOffset))
    //            {
    //                int index = gridXOffset + 3 + (gridYOffset + 3) * 7;
    //                moveTiles[index].SetActive(true);
    //            }
    //        }
    //    }
    //    else // The 8x16 move tiles of the entire board for some active powerups
    //    {
    //        // manually change individual8x16MoveTilesToSpawn for these active powerups: knife (1 tile adjacent), spear (2 tiles adjacent), hatchet (3 tiles adjacent)
    //        // manually change individual8x16MoveTilesToAvoid for these active powerups (they're all the same tho): slingshot, sniper, lightning bolt, bomb, fireball, arrow volley
    //        foreach (int[] offset in individual8x16MoveTilesToSpawn)
    //        {
    //            // FIGURE OUT THE MATH!!!

    //            //int gridXOffset = offset[0];
    //            //int gridYOffset = offset[1];

    //            //if (actualXYCoordinates.IsThisStillInsideTheBoard(currGridX + gridXOffset, currGridY + gridYOffset))
    //            //{
    //            //    int index = gridXOffset + 3 + (gridYOffset + 3) * 7;
    //            //    moveTiles[index].SetActive(true);
    //            //}
    //        }
    //    }
    //}

    public void CallForEnemyTurn()
    {
        turnController.AllEnemiesTurn();
    }

    public void RestartTimer()
    {
        isMoveTimeLlimitActive = true;
        elapsedTime = 0f;
    }

    void Update() // For the circle timer
    {
        if (isMoveTimeLlimitActive && elapsedTime < timeLimit)
        {
            elapsedTime += Time.deltaTime;
            imageCircleTimer.fillAmount = Mathf.Lerp(1f, 0f, elapsedTime / timeLimit);
        }
        else if (isMoveTimeLlimitActive && elapsedTime >= timeLimit) // Player did not make a move before its timer ran out
        {
            //if (stopTimerButNoChangeToDGBInputTime) { stopTimerButNoChangeToDGBInputTime = false; } // but it was caused by the timer finishing due to an active powerup
            //else                                    { dynamicDifficultyController.SetDynamicInputChange("TimeThinkingAndStepsTaken", -0.02f, false); }

            dynamicDifficultyController.SetDynamicInputChange("TimeThinkingAndStepsTaken", -0.02f, false);
            DeactivateMoveTiles(); //DeactivateMoveTiles(true); DeactivateMoveTiles(false);
            //playerAndEnemyStatusController.SetTotalMoveCountIncreaseBy1();
            isMoveTimeLlimitActive = false;
            CallForEnemyTurn();
        }
    }

    public void EndMoveTimerFromActivePowerup()
    {
        //stopTimerButNoChangeToDGBInputTime = true;
        //setElapsedTime(9999);

        isMoveTimeLlimitActive = false;
        CallForEnemyTurn();
    }

    //public void sayHello()
    //{
    //    Debug.Log("You have clicked on player");
    //}




    // ACTIVE POWERUPS + ground pound PASSIVE?
    public void ActivateAreaDamageWithRadius(int xLocation, int yLocation, int damage, int radius) // Attack enemies found at a radius around a coordinate (ignore if the player is found in 1 of the tiles)
    {
        circleTimer.SetActive(false);
        dynamicDifficultyController.SetDynamicInputChange("TimeThinkingAndStepsTaken", 0.02f - (0.03f * elapsedTime / timeLimit), false);

        if      (radius == 1) { arrayOfTilesToAttackDependingOnRadius = new int[][] { new int[] { 0, 0} }; }
        else if (radius == 3) { arrayOfTilesToAttackDependingOnRadius = new int[][] { new int[] { 0, 0}, new int[] { 1, 1}, new int[] { 1, 0}, new int[] { 1,-1}, new int[] { 0,-1}, new int[] {-1,-1}, new int[] {-1, 0}, new int[] {-1, 1}, new int[] { 0, 1} }; }
        else if (radius == 5) { arrayOfTilesToAttackDependingOnRadius = new int[][] { new int[] { 0, 0}, new int[] { 1, 1}, new int[] { 1, 0}, new int[] { 1,-1}, new int[] { 0,-1}, new int[] {-1,-1}, new int[] {-1, 0}, new int[] {-1, 1}, new int[] { 0, 1}, new int[] { 2, 2}, new int[] { 2, 1}, new int[] { 2, 0}, new int[] { 2,-1}, new int[] { 2,-2}, new int[] { 1,-2}, new int[] { 0,-2}, new int[] {-1,-2}, new int[] {-2,-2}, new int[] {-2,-1}, new int[] {-2, 0}, new int[] {-2, 1}, new int[] {-2, 2}, new int[] {-1, 2}, new int[] { 0, 2}, new int[] { 1, 2} }; }
        else if (radius == 7) { arrayOfTilesToAttackDependingOnRadius = new int[][] { new int[] { 0, 0}, new int[] { 1, 1}, new int[] { 1, 0}, new int[] { 1,-1}, new int[] { 0,-1}, new int[] {-1,-1}, new int[] {-1, 0}, new int[] {-1, 1}, new int[] { 0, 1}, new int[] { 2, 2}, new int[] { 2, 1}, new int[] { 2, 0}, new int[] { 2,-1}, new int[] { 2,-2}, new int[] { 1,-2}, new int[] { 0,-2}, new int[] {-1,-2}, new int[] {-2,-2}, new int[] {-2,-1}, new int[] {-2, 0}, new int[] {-2, 1}, new int[] {-2, 2}, new int[] {-1, 2}, new int[] { 0, 2}, new int[] { 1, 2}, new int[] { 3, 3}, new int[] { 3, 2}, new int[] { 3, 1}, new int[] { 3, 0}, new int[] { 3,-1}, new int[] { 3,-2}, new int[] { 3,-3}, new int[] { 2,-3}, new int[] { 1,-3}, new int[] { 0,-3}, new int[] {-1,-3}, new int[] {-2,-3}, new int[] {-3,-3}, new int[] {-3,-2}, new int[] {-3,-1}, new int[] {-3, 0}, new int[] {-3, 1}, new int[] {-3, 2}, new int[] {-3, 3}, new int[] {-2, 3}, new int[] {-1, 3}, new int[] { 0, 3}, new int[] { 1, 3}, new int[] { 2, 3} }; }
        else                  { arrayOfTilesToAttackDependingOnRadius = new int[][] { new int[] { 0, 0} }; Debug.LogWarning("Unkown radius for active powerup attack area"); }

        // TEMPORARY: just attack one tile (no knockback) regardless of radius.
        //GameObject enemyAtThisTile = playerAndEnemyStatusController.GetIdentityOfPieceAtThisBoardTile(xLocation, yLocation);
        //if (enemyAtThisTile.GetComponent<CodeForPrefabEnemy>() != null) // must actually check if it is an enemy, must ignore player
        //{
        //    circleTimer.SetActive(false);
        //    isMoveTimeLlimitActive = false;
        //    enemyAtThisTile.GetComponent<CodeForPrefabEnemy>().ThisEnemyTakesDamage(playerDirectContactDamage + playerBonusDamage, false);
        //}
        //else // miss
        //{
        //    EndMoveTimerFromActivePowerup();
        //}

        if (playerAndEnemyStatusController.getCurrentActivePowerupIdentity() == "active-acidRain") // If acid rain, attack every tile
        {
            for (int i = 0; i < 128; i++)
            {
                GameObject enemyAtThisTile = playerAndEnemyStatusController.GetIdentityOfPieceAtThisBoardTile((i % 16) + 1, (int)(i / 16) + 1);
                if (enemyAtThisTile != null && enemyAtThisTile.GetComponent<CodeForPrefabEnemy>() != null) // must actually check if it is an enemy, skip if it is the player
                {
                    enemyAtThisTile.GetComponent<CodeForPrefabEnemy>().ThisEnemyTakesDamage(damage + playerBonusDamage, false);
                }
            }
        }
        else
        {
            foreach (int[] offset in arrayOfTilesToAttackDependingOnRadius)
            {
                int gridXOffset = offset[0];
                int gridYOffset = offset[1];

                if (actualXYCoordinates.IsThisStillInsideTheBoard(xLocation + gridXOffset, yLocation + gridYOffset))
                {
                    GameObject enemyAtThisTile = playerAndEnemyStatusController.GetIdentityOfPieceAtThisBoardTile(xLocation + gridXOffset, yLocation + gridYOffset);
                    if (enemyAtThisTile != null && enemyAtThisTile.GetComponent<CodeForPrefabEnemy>() != null) // must actually check if it is an enemy, skip if it is the player
                    {
                        enemyAtThisTile.GetComponent<CodeForPrefabEnemy>().ThisEnemyTakesDamage(damage + playerBonusDamage, false);
                    }
                }
            }
        }
        Invoke(nameof(EndMoveTimerFromActivePowerup), 0.5f);

        // make a seperate function for radius > 1 (attacking multiple tiles) to check + attack enemies for each tlie.
        // if enemy not found here, go to next tile and instantly call that seperate function.
        // If enemy exists, call the seperate function 0.5 seconds later (unless all enemies are dead).
        // Also somehow ensure that when multiple tiles are being attacked, the enemy prefab will not call the function to activate all enemy turn after taking damage
        // (could be solved by making the enemy prefab read the currentActivePowerupIdentity variable from the playerandenemystatuscontroller)
    }




    // ====================================================
    // GETTER, SETTER

    public (int, int) GetPlayerCurrGridXAndCurrGridY()
    {
        return (currGridX, currGridY);
    }

    public int[][] GetPlayerMoveTiles()
    {
        return moveTilesToSpawn;
    }

    public void setElapsedTime(int newElapsedTime)
    {
        elapsedTime = newElapsedTime;
        imageCircleTimer.fillAmount = 0;
    }
}