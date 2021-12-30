using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Board board;
    private bool redTurn;
    private Snail redSnail;
    private Snail blueSnail;
    private bool canSwitchSnail;

    private Transition transition;
    private MusicManager musicManager;

    private void Awake()
    {
        redTurn = true;
        canSwitchSnail = true;
    }

    private void Start()
    {
        redSnail = board.GetRedSnail();
        blueSnail = board.GetBlueSnail();
        transition = FindObjectOfType<Transition>();
        musicManager = FindObjectOfType<MusicManager>();
    }

    private void Update()
    {
        if ((redSnail == null || blueSnail == null || redSnail.GetAtGoal() || blueSnail.GetAtGoal()) && canSwitchSnail)
        {
            redTurn = !redTurn;
            canSwitchSnail = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canSwitchSnail)
        {
            redTurn = !redTurn;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            musicManager.PlaySFX("cannot move");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transition.ChangeScene(SceneManager.GetActiveScene().name);
        }

        Snail snail = redTurn ? redSnail : blueSnail;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            board.MoveRequest(snail, -1, 0);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            board.MoveRequest(snail, 1, 0);
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            board.MoveRequest(snail, 0, -1);
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            board.MoveRequest(snail, 0, 1);
        }
    }

    public bool GetRedTurn()
    {
        return redTurn;
    }
}
