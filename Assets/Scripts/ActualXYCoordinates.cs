using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualXYCoordinates : MonoBehaviour
{
    //private int widthOfBattleBoard = 1565;
    //private int heightOfBoard = 785;
    private float widthOfTile = 97.8125f;
    private float heightOfTile = 98.125f;

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
            Debug.LogWarning("x is outside of board");
        }
        return ((float)gridX - 8.5f) * widthOfTile;
    }

    public float GetActualYCoordinate(int gridY)
    {
        if (gridY < 1 || gridY > 8)
        {
            Debug.LogWarning("y is outside of board");
        }
        return ((float)gridY - 4.5f) * heightOfTile;
    }
}
