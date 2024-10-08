using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualXYCoordinatesController : MonoBehaviour
{
    //private int widthOfBattleBoard = 1565;
    //private int heightOfBoard = 785;

    // For the UI Image Coordinates on the Canvas
    private float widthOfTileUIImage = 97.8125f;
    private float heightOfTileUIImage = 98.125f;
    private float battleBoardXOffsetUIImage = 134.0825f;
    private float battleBoardYOffsetUIImage = 105.3815f;

    // For the Sprite Renderer Coordinates on the Main Camera
    private float widthOfTile = 0.90533f;
    private float heightOfTile = 0.906f;
    private float battleBoardXOffset = 1.238f;
    private float battleBoardYOffset = 0.973f;

    public bool IsThisStillInsideTheBoard(int gridX, int gridY)
    {
        if (gridX < 1 || gridX > 16 || gridY < 1 || gridY > 8)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public float GetTileWidth()
    {
        return widthOfTile;
    }

    public float GetTileHeight()
    {
        return heightOfTile;
    }

    public float GetActualXCoordinate(int gridX)
    {
        if (gridX < 1 || gridX > 16)
        {
            Debug.LogWarning("x is outside of board: " + gridX);
        }
        return ((float)gridX - 8.5f) * widthOfTile + battleBoardXOffset;
    }

    public float GetActualYCoordinate(int gridY)
    {
        if (gridY < 1 || gridY > 8)
        {
            Debug.LogWarning("y is outside of board: " + gridY);
        }
        return ((float)gridY - 4.5f) * heightOfTile + battleBoardYOffset;
    }

    public float GetActualXCoordinateUIImage(int gridX)
    {
        if (gridX < 1 || gridX > 16)
        {
            Debug.LogWarning("x is outside of board: " + gridX);
        }
        return ((float)gridX - 8.5f) * widthOfTileUIImage + battleBoardXOffsetUIImage;
    }

    public float GetActualYCoordinateUIImage(int gridY)
    {
        if (gridY < 1 || gridY > 8)
        {
            Debug.LogWarning("y is outside of board: " + gridY);
        }
        return ((float)gridY - 4.5f) * heightOfTileUIImage + battleBoardYOffsetUIImage;
    }



    public float EuclideanDistance(int x1, int y1, int x2, int y2)
    {
        int dx = x2 - x1;
        int dy = y2 - y1;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }
}
