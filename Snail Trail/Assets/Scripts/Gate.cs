using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Gate : ActivatableBoardPiece
{
    public static GameObject gatePrefab;

    private Color color;

    private MusicManager musicManager;

    private void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }

    public static Gate Create(Board board, Cell[,] cells, int col, int row, Color color)
    {
        if (gatePrefab == null)
        {
            gatePrefab = Resources.Load("Prefabs/Gate") as GameObject;
        }
        Gate gate = Instantiate(gatePrefab, board.transform, false).GetComponent<Gate>();
        gate.InitializeBoardPiece(board, cells, col, row, false);
        gate.color = color;
        gate.GetComponent<SpriteRenderer>().color = color;
        gate.activated = false;
        return gate;
    }

    public override void Activate(GameObject activator)
    {
        if (activator.GetComponent<Switch>() != null)
        {
            activated = !activated;
            canBeSteppedOn = !canBeSteppedOn;

            if (activated)
            {
                GetComponent<SpriteRenderer>().sprite = null;
                musicManager.PlaySFX("press button");
            }
            else
            {
                musicManager.PlaySFX("release button");
                GetComponent<SpriteRenderer>().sprite = gatePrefab.GetComponent<SpriteRenderer>().sprite;
                if (cells[col, row].GetOccupant())
                {
                    GameObject occupant = cells[col, row].GetOccupant().gameObject;
                    cells[col, row].SetOccupant(null);
                    occupant.GetComponent<MovableBoardPiece>().Die();
                }
            }
        }
    }
}
