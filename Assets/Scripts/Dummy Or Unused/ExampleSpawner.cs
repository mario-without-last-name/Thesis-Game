using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class ExampleSpawner : MonoBehaviour
{
    public GameObject chessPiecePrefab; // Assign the "chessPiece" prefab in the Inspector
    public GameObject battleBoard; // Assign the "BattleBoard" GameObject in the Inspector
    public Sprite newImageSprite; // Drag and drop the new image Sprite in the Inspector
    public ActualXYCoordinatesController coordinateScript; // The script translating tile XY index into actual unity XY coordinates
    public ExampleCircleTimer exampleCircleTimer; // Temporary

    public void ExampleSpawnPlayerPiece()
    {
        // Instantiate the chessPiece prefab
        GameObject chessPieceInstance = Instantiate(chessPiecePrefab, battleBoard.transform);

        // Set the position relative to the BattleBoard
        RectTransform rectTransform = chessPieceInstance.GetComponent<RectTransform>();
        int gridX = UnityEngine.Random.Range(1, 17);
        int gridY = UnityEngine.Random.Range(1, 9);
        rectTransform.anchoredPosition = new Vector2(coordinateScript.GetActualXCoordinate(gridX), coordinateScript.GetActualYCoordinate(gridY));
        rectTransform.sizeDelta = new Vector2(coordinateScript.GetTileWidth(), coordinateScript.GetTileHeight());

        // Make the circle timer around the player
        exampleCircleTimer.ExampleSpawnCircleTimer(gridX, gridY);

        // Find the child Image UI component and change its sprite
        Transform childImageTransform = chessPieceInstance.transform.Find("ImageChessPiece");
        Image childImage = childImageTransform.GetComponent<Image>();
        childImage.sprite = newImageSprite;
    }
}