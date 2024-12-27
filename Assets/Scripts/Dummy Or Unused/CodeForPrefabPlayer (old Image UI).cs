using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Reflection;

public class CodeForPrefabPlayerOldImageUI : MonoBehaviour
{
    /*
     * 
     * 
     * 
    //[Header("Other Controllers")]
    //private PlayerAndEnemyStatusController PlayerAndEnemyStatusController;
    //private TurnController TurnController;
    //private ActualXYCoordinates CoordinateScript;

    [Header("Move Tiles")]
    [SerializeField] private GameObject[] moveTiles;

    private ActualXYCoordinatesController CoordinateScript;
    private PlayerAndEnemyStatusController PlayerAndEnemyStatusController;

    private bool isMoveTimeLlimitActive; // to control the Update() for controlling the timer
    private Image circleTimerImage;
    private float elapsedTime;
    private float timeLimit;
    private int currGridX;
    private int currGridY;

    //private Coroutine fillCoroutine; // coroutine is possible, but more complicated than just using update()

    void Awake()
    {
        GameObject CoordinateScriptObject = GameObject.FindGameObjectWithTag("tagForActualXYCoordinates");
        CoordinateScript = CoordinateScriptObject.GetComponent<ActualXYCoordinatesController>();
        GameObject PlayerAndEnemyStatusControllerObject = GameObject.FindGameObjectWithTag("tagForPlayerAndEnemyStatusController");
        PlayerAndEnemyStatusController = PlayerAndEnemyStatusControllerObject.GetComponent<PlayerAndEnemyStatusController>();

        currGridX = UnityEngine.Random.Range(1, 17);
        currGridY = UnityEngine.Random.Range(1, 9);
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(CoordinateScript.GetActualXCoordinate(currGridX), CoordinateScript.GetActualYCoordinate(currGridY));
        PlayerAndEnemyStatusController.SetIdentityOfPieceAtThisBoardTile(currGridX, currGridY, gameObject);

        // Find the CircleTimer UI Image component(child of this prefab)
        circleTimerImage = transform.Find("CircleTimer").GetComponent<Image>();

        isMoveTimeLlimitActive = true;
        StartTimer(PlayerAndEnemyStatusController.GetTimeToMove()); // Set the timer to the move time limit as efined in PlayerAndEnemyStatusController

        DeactivateMoveTiles();

        // The Standard 8 move tiles for a knight. Maybe add more depending on move-related powerups?
        ActivateMoveTiles(2, 1);
        ActivateMoveTiles(1, 2);
        ActivateMoveTiles(-2, 1);
        ActivateMoveTiles(-1, 2);
        ActivateMoveTiles(2, -1);
        ActivateMoveTiles(1, -2);
        ActivateMoveTiles(-2, -1);
        ActivateMoveTiles(-1, -2);
    }

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
        if (CoordinateScript.IsThisStillInsideTheBoard(currGridX + gridXOffset, currGridY + gridYOffset))
        {
            int index = gridXOffset + 3 + (gridYOffset + 3) * 7;
            moveTiles[index].SetActive(true);
        }
    }

    public void StartTimer(float x)
    {
        elapsedTime = 0f;
        timeLimit = x;
    }

    void Update()
    {
        if (isMoveTimeLlimitActive && elapsedTime < timeLimit)
        {
            elapsedTime += Time.deltaTime;
            circleTimerImage.fillAmount = Mathf.Lerp(1f, 0f, elapsedTime / timeLimit);
        }
    }



    */

}