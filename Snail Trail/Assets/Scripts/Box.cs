using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MovableBoardPiece
{
    public static GameObject boxPrefab;

    public static Box Create(Board board, Cell[,] cells, int col, int row)
    {
        if (boxPrefab == null)
        {
            boxPrefab = Resources.Load("Prefabs/Box") as GameObject;
        }

        Box box = Instantiate(boxPrefab, board.transform, false).GetComponent<Box>();
        box.InitializeBoardPiece(board, cells, col, row, false);
        return box;
    }

    public override bool CanMove()
    {
        return true;
    }

    public override void Move(int dCol, int dRow)
    {
        base.Move(dCol, dRow);

        musicManager.PlaySFX("box move");

        StartCoroutine(MoveIncrement(cells[col + dCol, row + dRow].transform));
        col += dCol;
        row += dRow;

        cells[col, row].SetType(0);
    }

    private IEnumerator Fade()
    {
        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        Color c = sp.color;
        float alpha = c.a;

        float currentTime = Time.realtimeSinceStartup;
        while (alpha > 0.01)
        {
            float now = Time.realtimeSinceStartup;
            float deltaTime = now - currentTime;
            alpha -= 0.01f* deltaTime * 100;
            sp.color = new Color(c.r, c.g, c.b, alpha);
            currentTime = now;
            yield return null;
        }

        Destroy(gameObject);
    }

    public override void Die()
    {
        musicManager.PlaySFX("destroy");
        GetComponent<ParticleSystem>().Play();
        StartCoroutine(Fade());
    }
}
