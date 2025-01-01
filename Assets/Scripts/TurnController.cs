using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] PlayerAndEnemyStatusController playerAndEnemyStatusController;
    [SerializeField] DynamicDifficultyController dynamicDifficultyController;
    [SerializeField] BottomBarController bottomBarController;
    [SerializeField] PowerupsCatalogController powerupsCatalogController;

    private GameObject prefabPlayer;
    private GameObject prefabPEnemy;
    private CodeForPrefabPlayer codeForPrefabPlayer;
    private CodeForPrefabEnemy codeForPrefabEnemy;

    private GameObject[] allEnemiesAliveRightNow;
    private int currentEnemyIndex;
    private string selectedDifficulty;
    private bool canPlayerSelectActivePowerup = false;

    public void RememberNewlySpawnedPlayerForNewRound()
    {
        // So I don't have to find the player prefab all the time. It's always just 1 anyway.
        prefabPlayer = GameObject.FindWithTag("tagForPrefabPlayer");
        codeForPrefabPlayer = prefabPlayer.GetComponent<CodeForPrefabPlayer>();
        selectedDifficulty = PlayerPrefs.GetString("modeDifficulty", "Adaptive");
    }

    public void PlayerTurn()
    {
        // ACTIVE POWERUP: dodge, teleport
        if (!playerAndEnemyStatusController.GetHasPlayerAlreadyBeenDamagedThisTurn() && playerAndEnemyStatusController.GetCurrentActivePowerupIdentity() == "active-dodge") { dynamicDifficultyController.SetDynamicInputChange("powerupUsage", -0.05f, false); }
        else if(!playerAndEnemyStatusController.GetHasPlayerAlreadyBeenDamagedThisTurn() && playerAndEnemyStatusController.GetCurrentActivePowerupIdentity() == "active-teleport") { dynamicDifficultyController.SetDynamicInputChange("powerupUsage", +0.05f, false); }
        // ACTIVE POWERUP: all except dodge and teleport
        if (!playerAndEnemyStatusController.GetHasAnEnemyAlreadyBeenDamagedThisTurn() && playerAndEnemyStatusController.GetCurrentActivePowerupIdentity() != "") { dynamicDifficultyController.SetDynamicInputChange("powerupUsage", -0.005f, false); }

        // Check if player has not been using an active powerup off-cooldown for multiple turns in a row
        if (playerAndEnemyStatusController.GetCurrentActivePowerupIdentity() != "") { playerAndEnemyStatusController.SetTurnsPassedWithoutUsingActivePowerupThatIsOffCooldown(0); }
        if (playerAndEnemyStatusController.GetTurnsPassedWithoutUsingActivePowerupThatIsOffCooldown() >= 2) { dynamicDifficultyController.SetDynamicInputChange("powerupUsage", -0.15f, false); Debug.Log("You have not played an active powerup for 2+ turns now"); }
        if (bottomBarController.GetIsAnyActivePowerupOffCooldown()) { playerAndEnemyStatusController.SetTurnsPassedWithoutUsingActivePowerupThatIsOffCooldown(playerAndEnemyStatusController.GetTurnsPassedWithoutUsingActivePowerupThatIsOffCooldown() + 1); }



        // Click on a move tile = move there and maybe damage + knockback enemy + add 1 to their delay counter, and maybe kill for gold
        // Or a powerup where you don't have to click on a tile.
        // Timer's up = skip to enemy's turn
        // Enemy left in round = 0, win and go to sho
        playerAndEnemyStatusController.SetMoveCountIncreaseBy1AndMaybeDeductBonusGold();
        playerAndEnemyStatusController.SetCurrentActivePowerupIdentity(""); // Reset, so no active powerup is in effect now
        playerAndEnemyStatusController.SetHasAnEnemyAlreadyBeenDamagedThisTurn(false);
        playerAndEnemyStatusController.SetHasPlayerAlreadyBeenDamagedThisTurn(false);

        // player's turn to move
        // PASSIVE POWERUP
        if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-mercenaryTools")) { playerAndEnemyStatusController.SetChangeInCurrGold(-1); }
        int turnsSoFar = playerAndEnemyStatusController.GetMoveCountThisRound();
        if (bottomBarController.CheckIfThisPassivePowerUpIsOwned("passive-innerHealing") && (turnsSoFar % 3 == 0) && (turnsSoFar <= 15))
        {
            codeForPrefabPlayer.PlayerTakesDamageOrHealing(powerupsCatalogController.ActivateThisPassivePowerup("passive-innerHealing", ""), false); ; Invoke(nameof(CallPlayerCanMoveNow), 0.5f);
        }
        else { CallPlayerCanMoveNow(); }
    }


    public void CallPlayerCanMoveNow() // Built into a seperate function, just in case this needs to be called 0.5 seconds later due to healing powerup
    {
        // Player can choose one of their active powerups (if off cooldown)
        canPlayerSelectActivePowerup = true;
        bottomBarController.ActivePowerupsReduceCooldownBy1AndMaybeCanBeClickedNow();
        codeForPrefabPlayer.PlayerCanMoveNow();
    }

    public void PlayerIsMovingToATile()
    {
        // Player cannot choose one of their active powerups anymore
        canPlayerSelectActivePowerup = false;
        bottomBarController.MakeActivePowerupSquaresBlueTemporarilyBecauseItsNotPlayerTurn();
        // turn off the 8x16 move tiles from active powerups
        playerAndEnemyStatusController.SetActiveFull8x16MoveTiles(false);
    }

    public void PlayerTurnButActivePowerupWasActivated(string powerupIdentity)
    {
        // Player cannot choose one of their active powerups anymore
        canPlayerSelectActivePowerup = false;
        bottomBarController.MakeActivePowerupSquaresBlueTemporarilyBecauseItsNotPlayerTurn();
        // OTHER THAN THE 1 CODE ABOVE, EVERYTHING ELSE MUST BE CALLED AT PLAYER AND ENEMY STATUS CONTROLLER
        playerAndEnemyStatusController.AnActivePowerupWasActivated(powerupIdentity);        
    }

    public void AllEnemiesTurn()
    {
        // Player cannot choose one of their active powerups anymore
        canPlayerSelectActivePowerup = false;
        bottomBarController.MakeActivePowerupSquaresBlueTemporarilyBecauseItsNotPlayerTurn();
        // turn off the 8x16 move tiles from active powerups
        playerAndEnemyStatusController.SetActiveFull8x16MoveTiles(false);

        // For every enemy... reduce delay counter by 1
        // If counter = 1, show yellow tiles
        // If coutner = 0, move the enemy, maybe damage + knockback player, set counter back to original
        // Player hp = 0, game over

        currentEnemyIndex = -1;
        NextEnemysTurnOneByOneForRedTiles();
    }

    public void NextEnemysTurnOneByOneForRedTiles() // First, call every enemy one by one to damage the player if they are in one of their red tiles
    {
        allEnemiesAliveRightNow = GameObject.FindGameObjectsWithTag("tagForPrefabEnemy");
        currentEnemyIndex += 1;

        if (currentEnemyIndex >= allEnemiesAliveRightNow.Length)
        {
            currentEnemyIndex = -1;
            NextEnemysTurnOneByOneForYellowTiles(); // Meaning all enemies have scanned their red tiles to damage player
        }
        else
        {
            CodeForPrefabEnemy codeForPrefabEnemy = allEnemiesAliveRightNow[currentEnemyIndex].GetComponent<CodeForPrefabEnemy>();
            codeForPrefabEnemy.ActivateEnemyTurnForRedTiles();
        }
    }

    public void NextEnemysTurnOneByOneForYellowTiles() // Then, call every enemy one by one to lose patience / delay, or move to one of their yellow tiles( if available, and maybe damage + knockback player too, one by one
    {
        allEnemiesAliveRightNow = GameObject.FindGameObjectsWithTag("tagForPrefabEnemy");
        currentEnemyIndex += 1;

        if (currentEnemyIndex >= allEnemiesAliveRightNow.Length)
        {
            if (selectedDifficulty == "Adaptive")
            {
                dynamicDifficultyController.PrintAndLogPerTurnAllDGBInputAndOutputIndex();
                playerAndEnemyStatusController.PrintAndLogPerTurnHealthKillsPointsGold();
            }
            Invoke(nameof(PlayerTurn), 0.5f); // Meaning all enemies have taken their turn moving / reduce delay by 1
        }
        else
        {
            CodeForPrefabEnemy codeForPrefabEnemy = allEnemiesAliveRightNow[currentEnemyIndex].GetComponent<CodeForPrefabEnemy>();
            codeForPrefabEnemy.ActivateEnemyTurnForYellowTiles();
        }
    }

    // ====================================================
    // GETTER, SETTER

    public bool getCanPlayerSelectActivePowerup()
    {
        return canPlayerSelectActivePowerup;
    }

    public void setCanPlayerSelectActivePowerup(bool newBool)
    {
        canPlayerSelectActivePowerup = newBool;
    }
}
