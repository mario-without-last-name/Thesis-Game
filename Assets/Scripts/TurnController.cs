using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    private GameObject prefabPlayer;
    private CodeForPrefabPlayer codeForPrefabPlayer;
    private GameObject prefabPEnemy;
    private CodeForPrefabEnemy codeForPrefabEnemy;

    private GameObject[] allEnemiesAliveRightNow;
    private int currentEnemyIndex;

    public void RememberNewlySpawnedPlayerForNewRound()
    {
        // So I don't have to find the player prefab all the time. It's always just 1 anyway.
        prefabPlayer = GameObject.FindWithTag("tagForPrefabPlayer");
        codeForPrefabPlayer = prefabPlayer.GetComponent<CodeForPrefabPlayer>();
    }

    public void PlayerTurn()
    {
        // Click on a move tile = move there and maybe damage + knockback enemy + add 1 to their delay counter, and maybe kill for gold
        // Or a powerup where you don't have to click on a tile.
        // Timer's up = skip to enemy's turn
        // Enemy left in round = 0, win and go to shop

        codeForPrefabPlayer.PlayerCanMoveNow();
    }

    public void PlayerTurnTileSpecificPowerups()
    {
        // Click on a move tile to activate specific abilities
        // Timer's up = skip to enemy's turn
    }

    public void EnemyTurn() // Change into AllEnemiesTurn
    {
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
            Invoke(nameof(PlayerTurn), 0.5f); // Meaning all enemies have taken their turn moving / reduce delay by 1
        }
        else
        {
            CodeForPrefabEnemy codeForPrefabEnemy = allEnemiesAliveRightNow[currentEnemyIndex].GetComponent<CodeForPrefabEnemy>();
            codeForPrefabEnemy.ActivateEnemyTurnForYellowTiles();
        }
    }
}
