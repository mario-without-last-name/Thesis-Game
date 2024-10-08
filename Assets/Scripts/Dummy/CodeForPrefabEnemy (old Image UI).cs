using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Reflection;
using UnityEditor.U2D.Aseprite;

public class CodeForPrefabEnemyOldImageUI : MonoBehaviour
{
    [Header("Tiles")]
    [SerializeField] private GameObject[] moveTiles;
    [SerializeField] private GameObject[] attackTiles;

    private ActualXYCoordinatesController CoordinateScript;
    private PlayerAndEnemyStatusController PlayerAndEnemyStatusController;
    private BestiaryController BestiaryController;
    private int currGridX;
    private int currGridY;

    private string thisEnemyVariant;
    private int[][] moveTilesToSpawn;
    private int[][] attackTilesToSpawn;

    void Awake()
    {
        GameObject CoordinateScriptObject = GameObject.FindGameObjectWithTag("tagForActualXYCoordinates");
        CoordinateScript = CoordinateScriptObject.GetComponent<ActualXYCoordinatesController>();
        GameObject PlayerAndEnemyStatusControllerObject = GameObject.FindGameObjectWithTag("tagForPlayerAndEnemyStatusController");
        PlayerAndEnemyStatusController = PlayerAndEnemyStatusControllerObject.GetComponent<PlayerAndEnemyStatusController>();
        GameObject BestiaryControllerObject = GameObject.FindGameObjectWithTag("tagForBestiaryController");
        BestiaryController = BestiaryControllerObject.GetComponent<BestiaryController>();

        // Find out what enemy variant this piece should be as controlled by the playerAndenemyStatusController
        thisEnemyVariant = PlayerAndEnemyStatusController.GetWhichEnemyVariantToSpawn();

        // Based on the enemy variant, determine its sprite.
        Transform childTransform = transform.Find("ImageChessPiece");
        Image imageComponent = childTransform.GetComponent<Image>();
        imageComponent.sprite = BestiaryController.GetEnemySprite(thisEnemyVariant);

        // Ensure that it does not spawn on the same tile as the player (or 4 tiles around it), or other enemies
        currGridX = UnityEngine.Random.Range(1, 17);
        currGridY = UnityEngine.Random.Range(1, 9);
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(CoordinateScript.GetActualXCoordinate(currGridX), CoordinateScript.GetActualYCoordinate(currGridY));
        PlayerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, gameObject);

        // Based on the enemy variant, determine the move tiles and attack tiles it has
        DeactivateMoveTiles();
        moveTilesToSpawn = BestiaryController.GetEnemyMoveTiles(thisEnemyVariant);
        ActivateMoveTiles(moveTilesToSpawn); // YOU MUST MAKE IT SO THAT AN ENEMY's MOVEMENT TILES DEPEND ON THIER VARIANT, COOLDOWN, visual hint DGB etc.
        DeactivateAttackTiles();
        attackTilesToSpawn = BestiaryController.GetEnemyAttackTiles(thisEnemyVariant);
        ActivateAttackTiles(attackTilesToSpawn);

        // Based on the enemy variant, determine its stats as defined by the bestiaryController

    }

    // ALSO MAKE THE ONE FOR ENEMY ATTACK TILES

    public void DeactivateMoveTiles()
    {
        for (int i = 0; i < moveTiles.Length; i++)
        {
            moveTiles[i].SetActive(false);
        }
    }

    public void ActivateMoveTiles(int[][] offsets)
    {
        foreach (int[] offset in offsets)
        {
            int gridXOffset = offset[0];
            int gridYOffset = offset[1];

            if (CoordinateScript.IsThisStillInsideTheBoard(currGridX + gridXOffset, currGridY + gridYOffset))
            {
                int index = gridXOffset + 3 + (gridYOffset + 3) * 7;
                moveTiles[index].SetActive(true);
            }
        }
    }

    public void DeactivateAttackTiles()
    {
        for (int i = 0; i < attackTiles.Length; i++)
        {
            attackTiles[i].SetActive(false);
        }
    }

    public void ActivateAttackTiles(int[][] offsets)
    {
        foreach (int[] offset in offsets)
        {
            int gridXOffset = offset[0];
            int gridYOffset = offset[1];

            if (CoordinateScript.IsThisStillInsideTheBoard(currGridX + gridXOffset, currGridY + gridYOffset))
            {
                int index = gridXOffset + 3 + (gridYOffset + 3) * 7;
                attackTiles[index].SetActive(true);
            }
        }
    }
}