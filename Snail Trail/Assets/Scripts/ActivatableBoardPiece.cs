using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivatableBoardPiece : BoardPiece
{
    protected bool activated;

    public abstract void Activate(GameObject activator);
}
