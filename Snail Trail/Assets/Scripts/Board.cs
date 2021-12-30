using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Board : MonoBehaviour
{
    public float xPos; //center of board
    public float yPos; //center of board
    public int numCols; //left to right, 0 -> (n - 1)
    public int numRows; //top to bottom, 0 -> (n - 1)
    public List<Vector2Int> holes; //[x, y] OR [col, row]
    public List<Vector2Int> acid; //[x, y] OR [col, row]
    public List<Vector2Int> boxes; //[x, y] OR [col, row]
    public List<Vector2Int> rocks; //[x, y] OR [col, row]
    public List<Vector2Int> switches; //[x, y] OR [col, row] **Corresponding switches/gates/colors must be at the same index**
    public List<Vector2Int> gates; //[x, y] OR [col, row] **Corresponding switches/gates/colors must be at the same index**
    public List<Color> switchAndGateColors; //**Corresponding switches/gates/colors must be at the same index**
    public Vector2Int redStart; //[x, y] OR [col, row]
    public Vector2Int blueStart; //[x, y] OR [col, row]
    public Vector2Int redEnd; //[x, y] OR [col, row]
    public Vector2Int blueEnd; //[x, y] OR [col, row]
    public Color cellColor1;
    public Color cellColor2;
    public GameObject cellPrefab;
    public GameObject redGoalPrefab;
    public GameObject blueGoalPrefab;
    public GameObject flowerPrefab;

    private Snail redSnail;
    private Snail blueSnail;
    private Cell[,] cells; //[x, y] OR [col, row]
    private bool moveInProgress;

    private bool won;
    private MusicManager musicManager;
    private Transition transition;

    private void Awake()
    {
        won = false;
        moveInProgress = false;
        CreateBoard();
    }

    private void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
        transition = FindObjectOfType<Transition>();
    }

    private void Update()
    {
        if (redSnail.GetAtGoal() && blueSnail.GetAtGoal())
        {
            if (!won)
            {
                WinGame();
            }
            won = true;
        }
    }

    public bool MoveRequest(MovableBoardPiece movable, int dCol, int dRow)
    {
        if (moveInProgress) return false;
        if (won) return false;
        if (!movable.CanMove()) return false;

        Vector2Int position = new Vector2Int(movable.GetCol(), movable.GetRow());
        if (!CellExists(new Vector2Int(position.x + dCol, position.y + dRow)))
        {
            musicManager.PlaySFX("cannot move");
            return false;
        }

        if (movable.GetComponent<Snail>() != null)
        {
            int snailType = movable.GetComponent<Snail>().ReturnType();
            int cellType = cells[position.x + dCol, position.y + dRow].ReturnType();
            if (cellType == 3 || (cellType != snailType && cellType != 0))
            {
                musicManager.PlaySFX("cannot move");
                return false;
            }

            if ((cells[position.x + dCol, position.y + dRow].GetIsRedGoal() && movable.GetComponent<Snail>().GetIsRed()) ||
                (cells[position.x + dCol, position.y + dRow].GetIsBlueGoal() && !movable.GetComponent<Snail>().GetIsRed()))
            {
                movable.Move(dCol, dRow);
                cells[position.x + dCol, position.y + dRow].SetOccupant(new GameObject().AddComponent<Goal>()); //fixed bug where boxes could be pushed into completed goal with other snail
                return true;
            }
        }

        BoardPiece occupant = cells[position.x + dCol, position.y + dRow].GetOccupant();
        ActivatableBoardPiece activatable = cells[position.x + dCol, position.y + dRow].GetActivatable();
        if (occupant != null && (activatable == null || activatable.GetCanBeSteppedOn()))
        {
            if (occupant.GetComponent<MovableBoardPiece>() != null)
            {
                if (MoveRequest(occupant.GetComponent<MovableBoardPiece>(), dCol, dRow))
                {
                    movable.Move(dCol, dRow);
                    return true;
                }
            }
            else
            {
                musicManager.PlaySFX("cannot move");
                return false;
            }
        }
        else if (activatable == null || activatable.GetCanBeSteppedOn())
        {
            movable.Move(dCol, dRow);
            return true;
        }
        musicManager.PlaySFX("cannot move");
        return false;
    }

    public Snail GetRedSnail()
    {
        return redSnail;
    }

    public Snail GetBlueSnail()
    {
        return blueSnail;
    }

    public void MoveInProgress(bool b)
    {
        moveInProgress = b;
    }

    private void CreateBoard()
    {
        transform.position = new Vector2(xPos, yPos);
        CreateCells();
        CreateGoals();
        CreateSnails();
        CreateBoxes();
        CreateRocks();
        CreateSwitchesAndGates();
    }

    private void CreateCells()
    {
        cells = new Cell[numCols, numRows];
        Renderer cellRenderer = cellPrefab.GetComponentInChildren<Renderer>();

        float cellWidth = cellRenderer.bounds.size.x;
        float cellHeight = cellRenderer.bounds.size.y;
        float startingXPos = -((numCols / 2) * cellWidth) + (numCols % 2 == 0 ? (cellWidth / 2) : 0);
        float startingYPos = ((numRows / 2) * cellHeight) - (numRows % 2 == 0 ? (cellHeight / 2) : 0);

        Renderer flowerRenderer = flowerPrefab.GetComponentInChildren<Renderer>();
        float flowerWidth = flowerRenderer.bounds.size.x;
        float flowerHeight = flowerRenderer.bounds.size.y;

        for (int x = 0; x < numCols; x++)
        {
            for (int y = 0; y < numRows; y++)
            {
                Vector2Int position = new Vector2Int(x, y);
                if (!holes.Contains(position))
                {
                    GameObject go = Instantiate(cellPrefab, transform, false);
                    Cell c = go.GetComponent<Cell>();
                    go.transform.position += (Vector3)(new Vector2(startingXPos + cellWidth * x, startingYPos - cellHeight * y));
                    go.name = "Cell: [" + x + ", " + y + "]";
                    if ((x + y) % 2 == 0)
                    {
                        c.SetDefaultColor(cellColor1);
                    }
                    else
                    {
                        c.SetDefaultColor(cellColor2);
                    }
                    cells[x, y] = c;

                    if (acid.Contains(position))
                    {
                        cells[x, y].SetType(3);
                    }
                    else
                    {
                        cells[x, y].SetType(0);
                    }
                    bool isEnd = false;
                    if (new Vector2Int(redEnd.x, redEnd.y) == new Vector2Int(x, y))
                    {
                        cells[x, y].SetIsRedGoal();
                        isEnd = true;
                    }
                    else if (new Vector2Int(blueEnd.x, blueEnd.y) == new Vector2Int(x, y))
                    {
                        cells[x, y].SetIsBlueGoal();
                        isEnd = true;
                    }

                    if (!isEnd)
                    {
                        if (Random.Range(0, 9) == 0)
                        {
                            Instantiate(flowerPrefab, cells[x, y].transform, false);
                            float offsetX = cellWidth * 0.5f - flowerWidth/2f;
                            float offsetY = cellHeight * 0.5f - flowerHeight/2f;
                            
                            flowerPrefab.transform.position = (Vector3)new Vector2(Random.Range(-offsetX, offsetX), Random.Range(-offsetY, offsetY));
                        }
                    }
                } 
            }
        }
    }

    private void CreateGoals()
    {
        Goal redGoal = Goal.Create(true, this, cells, redEnd.x, redEnd.y);
        cells[redEnd.x, redEnd.y].SetOccupant(redGoal);

        Goal blueGoal = Goal.Create(false, this, cells, blueEnd.x, blueEnd.y);
        cells[blueEnd.x, blueEnd.y].SetOccupant(blueGoal);
    }

    private void CreateSnails()
    {
        redSnail = Snail.Create(true, Color.red, this, cells, redStart.x, redStart.y);
        Cell redStartCell = cells[redStart.x, redStart.y];
        redStartCell.SetOccupant(redSnail);
        redStartCell.SetType(1);

        blueSnail = Snail.Create(false, Color.blue, this, cells, blueStart.x, blueStart.y);
        Cell blueStartCell = cells[blueStart.x, blueStart.y];
        blueStartCell.SetOccupant(blueSnail);
        blueStartCell.SetType(2);
    }

    private void CreateBoxes()
    {
        foreach (Vector2Int coords in boxes)
        {
            Box box = Box.Create(this, cells, coords.x, coords.y);
            cells[coords.x, coords.y].SetOccupant(box);
        }
    }

    private void CreateRocks()
    {
        foreach (Vector2Int coords in rocks)
        {
            Rock rock = Rock.Create(this, cells, coords.x, coords.y);
            cells[coords.x, coords.y].SetOccupant(rock);
        }
    }

    private void CreateSwitchesAndGates()
    {
        for (int i = 0; i < switches.Count; i++)
        {
            Gate gate = Gate.Create(this, cells, gates[i].x, gates[i].y, switchAndGateColors[i]);
            Switch s = Switch.Create(this, cells, switches[i].x, switches[i].y, switchAndGateColors[i], new List<ActivatableBoardPiece>() { gate });
            cells[gates[i].x, gates[i].y].SetActivatable(gate);
            cells[switches[i].x, switches[i].y].SetActivatable(s);
        }
    }

    private bool CellExists(Vector2Int cell)
    {
        return (cell.x >= 0 && cell.x < numCols && cell.y >= 0 && cell.y < numRows) && cells[cell.x, cell.y] != null;
    }

    private void WinGame()
    {
        musicManager.PlaySFX("win");
        StartCoroutine(NextLevel(1f));
    }

    private IEnumerator NextLevel(float secs)
    {
        yield return new WaitForSeconds(secs);
        if (SceneManager.GetActiveScene().name != "Test Level")
        {
            int currentLevel = int.Parse(SceneManager.GetActiveScene().name.Substring(6));
            if (currentLevel == 16)
            {
                transition.ChangeScene("Win");
            }
            transition.ChangeScene("Level " + (currentLevel + 1));
            LevelManager lm = FindObjectOfType<LevelManager>();
            if (lm.GetLevel() == currentLevel)
            {
                FindObjectOfType<LevelManager>().IncreaseLevel();
            }
        }
    }
}
