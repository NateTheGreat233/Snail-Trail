using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Snail : MovableBoardPiece
{
    public static GameObject redSnailPrefab;
    public static GameObject blueSnailPrefab;
    public bool isRed;
    private bool atGoal;
    private bool dead;
    private bool facingRight;

    private Transition transition;

    public static Snail Create(bool isRed, Color trailColor, Board board, Cell[,] cells, int col, int row)
    {
        if (redSnailPrefab == null)
        {
            redSnailPrefab = Resources.Load("Prefabs/Red Snail") as GameObject;
            blueSnailPrefab = Resources.Load("Prefabs/Blue Snail") as GameObject;
        }

        GameObject snailPrefab = isRed ? redSnailPrefab : blueSnailPrefab;
        Snail snail = Instantiate(snailPrefab, board.transform, false).GetComponent<Snail>();
        snail.transition = FindObjectOfType<Transition>();
        snail.InitializeBoardPiece(board, cells, col, row, false);
        snail.atGoal = false;
        snail.dead = false;
        snail.facingRight = true;
        return snail;
    }

    public override bool CanMove()
    {
        return !atGoal && !dead;
    }

    public override void Move(int dCol, int dRow)
    {
        base.Move(dCol, dRow);
        musicManager.PlaySFX("move");

        if ((facingRight && dCol < 0) || (!facingRight && dCol > 0))
        {
            transform.eulerAngles += new Vector3(0, 1, 0) * 180;
            facingRight = !facingRight;
        }

        StartCoroutine(MoveIncrement(cells[col + dCol, row + dRow].transform));
        col += dCol;
        row += dRow;

        if (isRed)
        {
            if (cells[col, row].GetIsRedGoal())
            {
                Win();
                return;
            }
        }
        else
        {
            if (cells[col, row].GetIsBlueGoal())
            {
                Win();
                return;
            }
        }
    }

    protected override IEnumerator MoveIncrement(Transform position)
    {    
        board.MoveInProgress(true);

        Vector2 currentPos = transform.position;
        Vector2 endPos = position.position;
        Vector2 increment = (endPos - currentPos) / 50f;

        float currentTime = Time.realtimeSinceStartup;
        
        while (Vector3.Dot(((Vector3)endPos - transform.position), endPos - currentPos) > 0)
        {
            float now = Time.realtimeSinceStartup;
            float deltaTime = now - currentTime;
            transform.position += (Vector3)increment * deltaTime * 150f;
            currentTime = now;
            yield return null;
        }

        transform.position = endPos;
        board.MoveInProgress(false);

        int type = isRed ? 1 : 2;
        cells[col, row].SetType(type);
    }

    public bool GetIsRed()
    {
        return isRed;
    }

    public int ReturnType()
    {
        return isRed ? 1 : 2;
    }

    public bool GetAtGoal()
    {
        return atGoal;
    }

    private void Win()
    {
        atGoal = true;
        musicManager.PlaySFX("flag");
        GetComponent<Animator>().SetBool("AtGoal", true);
        StartCoroutine(Fade(false));
    }

    private IEnumerator Fade(bool restart)
    {
        if (restart) dead = true;

        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        Color c = sp.color;
        float alpha = c.a;

        float currentTime = Time.realtimeSinceStartup;
        while (alpha > 0.01)
        {
            float now = Time.realtimeSinceStartup;
            float deltaTime = now - currentTime;
            alpha -= 0.01f*deltaTime*100;
            sp.color = new Color(c.r, c.g, c.b, alpha);
            currentTime = now;
            yield return null;
        }

        if (restart)
        {
            transition.ChangeScene(SceneManager.GetActiveScene().name);
        }

        Destroy(gameObject);
    }

    public override void Die()
    {
        musicManager.PlaySFX("lose");
        GetComponent<ParticleSystem>().Play();
        StartCoroutine(Fade(true));
    }
}
