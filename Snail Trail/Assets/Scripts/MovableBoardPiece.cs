using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovableBoardPiece : BoardPiece
{
    protected MusicManager musicManager;

    private void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }

    public abstract bool CanMove();
    public abstract void Die();

    public virtual void Move(int dCol, int dRow)
    {
        cells[col, row].SetOccupant(null);
        Cell c = cells[col + dCol, row + dRow];
        c.SetOccupant(this);

        if (c.GetActivatable() != null)
        {
            c.GetActivatable().Activate(gameObject);
        }

        if (cells[col, row].GetActivatable() != null)
        {
            cells[col, row].GetActivatable().Activate(gameObject);
        }
    }

    protected virtual IEnumerator MoveIncrement(Transform position)
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
    }
}
