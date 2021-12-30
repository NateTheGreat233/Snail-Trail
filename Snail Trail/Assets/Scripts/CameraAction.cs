using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    public Player player;

    private void Update()
    {
        if (player.GetRedTurn())
        {
            Camera.main.backgroundColor = new Color(1, 0.6f, 0.6f);
        }
        else
        {
            Camera.main.backgroundColor = new Color(0.6f, 0.6f, 1);
        }
    }
}
