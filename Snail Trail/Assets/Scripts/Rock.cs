using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : BoardPiece
{
    public static GameObject rockPrefab;

    public static Rock Create(Board board, Cell[,] cells, int col, int row)
    {
        if (rockPrefab == null)
        {
            rockPrefab = Resources.Load("Prefabs/Rock") as GameObject;
        }

        Rock rock = Instantiate(rockPrefab, board.transform, false).GetComponent<Rock>();
        rock.InitializeBoardPiece(board, cells, col, row, false);
        return rock;
    }
}