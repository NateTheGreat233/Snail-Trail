using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardPiece : MonoBehaviour
{
    protected int col;
    protected int row;
    protected Board board;
    protected Cell[,] cells;
    protected bool canBeSteppedOn;

    public void InitializeBoardPiece(Board board, Cell[,] cells, int col, int row, bool canBeSteppedOn)
    {
        this.board = board;
        this.cells = cells;
        this.col = col;
        this.row = row;
        this.canBeSteppedOn = canBeSteppedOn;
        transform.position = cells[col, row].transform.position;

        if (GetComponent<ActivatableBoardPiece>() != null)
        {
            cells[col, row].SetActivatable(GetComponent<ActivatableBoardPiece>());
        }
    }

    public int GetCol()
    {
        return col;
    }

    public int GetRow()
    {
        return row;
    }

    public bool GetCanBeSteppedOn()
    {
        return canBeSteppedOn;
    }
}
