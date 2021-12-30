using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : BoardPiece
{
    public static GameObject redGoalPrefab;
    public static GameObject blueGoalPrefab;
    public bool isRed;

    public static Goal Create(bool isRed, Board board, Cell[,] cells, int col, int row)
    {
        GameObject goalPrefab = isRed ? Resources.Load("Prefabs/Red Goal") as GameObject : Resources.Load("Prefabs/Blue Goal") as GameObject;

        Goal goal = Instantiate(goalPrefab, board.transform, false).GetComponent<Goal>();
        goal.InitializeBoardPiece(board, cells, col, row, false);
        return goal;
    }
}
