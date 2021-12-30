using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : ActivatableBoardPiece
{
    public static GameObject switchPrefab;

    private List<ActivatableBoardPiece> activatables;
    private Color color;
    private bool recentlyActivated;
    private bool doubleActivated;

    public static Switch Create(Board board, Cell[,] cells, int col, int row, Color color, List<ActivatableBoardPiece> activatables)
    {
        if (switchPrefab == null)
        {
            switchPrefab = Resources.Load("Prefabs/Switch") as GameObject;
        }

        Switch s = Instantiate(switchPrefab, board.transform, false).GetComponent<Switch>();
        s.recentlyActivated = false;
        s.doubleActivated = false;
        s.InitializeBoardPiece(board, cells, col, row, true);
        s.color = color;
        s.GetComponent<SpriteRenderer>().color = color;
        s.activatables = activatables;
        return s;
    }

    public override void Activate(GameObject activator)
    {
        if (recentlyActivated)
        {
            doubleActivated = true;
            return;
        }
        recentlyActivated = true;
        StartCoroutine(WaitForDouble());
    }

    private IEnumerator WaitForDouble()
    {
        yield return new WaitForSeconds(0.1f);
        if (!doubleActivated)
        {
            foreach (ActivatableBoardPiece a in activatables)
            {
                a.Activate(gameObject);
            }
        }
        doubleActivated = false;
        recentlyActivated = false;
    }
}
