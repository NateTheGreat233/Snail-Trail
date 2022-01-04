using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    public Player player;

    private void Update()
    {
        Camera.main.backgroundColor = player.GetRedTurn() ? new Color(1, 0.6f, 0.6f) : new Color(0.6f, 0.6f, 1);
    }
}
