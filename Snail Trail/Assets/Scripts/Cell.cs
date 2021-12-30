using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private static Sprite defaultSprite; //type 0
    private static GameObject redSlime; //type 1
    private static GameObject blueSlime; //type 2
    private static GameObject acid; //type 3

    private Color defaultColor;
    private SpriteRenderer sr;
    private BoardPiece occupant;
    private ActivatableBoardPiece activatable;
    private GameObject currentSlime;
    private int type;

    private bool isRedGoal;
    private bool isBlueGoal;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        redSlime = Resources.Load("Prefabs/Red Slime") as GameObject;
        blueSlime = Resources.Load("Prefabs/Blue Slime") as GameObject;
        acid = Resources.Load("Prefabs/Acid") as GameObject;
        defaultSprite = (Resources.Load("Prefabs/Cell") as GameObject).GetComponent<SpriteRenderer>().sprite;
    }

    public void SetOccupant(BoardPiece go)
    {
        occupant = go;
    }

    public BoardPiece GetOccupant()
    {
        return occupant;
    }

    public void SetActivatable(ActivatableBoardPiece a)
    {
        activatable = a;
    }

    public ActivatableBoardPiece GetActivatable()
    {
        return activatable;
    }

    public void SetDefaultColor(Color c)
    {
        defaultColor = c;
        sr.color = c;
    }

    public int ReturnType()
    {
        return type;
    }

    public void SetType(int n)
    {
        GameObject whichSlime = null;
        if (n == 0)
        {
            sr.color = defaultColor;
            if (currentSlime != null)
            {
                Destroy(currentSlime);
            }
            whichSlime = null;
        }
        else if (n == 1 && type != 1)
        {
            if (currentSlime != null)
            {
                Destroy(currentSlime);
            }
            whichSlime = redSlime;
        }
        else if (n == 2 && type != 2)
        {
            if (currentSlime != null)
            {
                Destroy(currentSlime);
            }
            whichSlime = blueSlime;
        }
        else if (n == 3 && type != 3)
        {
            if (currentSlime != null)
            {
                Destroy(currentSlime);
            }
            whichSlime = acid;
        }

        if (n > 0 && whichSlime != null)
        {
            currentSlime = Instantiate(whichSlime, transform, false);
            currentSlime.transform.position = transform.position;
        }

        type = n;
    }

    public void SetIsRedGoal()
    {
        isRedGoal = true;
    }

    public void SetIsBlueGoal()
    {
        isBlueGoal = true;
    }

    public bool GetIsRedGoal()
    {
        return isRedGoal;
    }

    public bool GetIsBlueGoal()
    {
        return isBlueGoal;
    }
}
