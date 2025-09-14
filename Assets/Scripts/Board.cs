using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public enum GameState {
    wait,
    move,
    win,
    lose,
    pause
}

public class Board : MonoBehaviour
{
    [Header("Scriptable Object Stuff")]
    public World world;
    public int level;

    public GameState currState = GameState.move;

    [Header("Board Dimensions")]
    public int width;
    public int height;
    public int offset; // For the pieces to slide into the screen

    [Header("Prefabs")]
    public GameObject tilePrefab;
    public GameObject[] dotsPrefab;
    private BackgroundTile[,] tiles;
    public GameObject[,] dots;

    private FindMatches findMatches;
    private int basePieceValue = 20;
    public int streakValue = 1;
    public int[] scoreGoals;
    private ScoreManager scoreManager;
    private GoalManager goalManager;
    public float refillDeley = 0.5f;

    private void Awake()
    {
        if(PlayerPrefs.HasKey("Current Level"))
        {
            level = PlayerPrefs.GetInt("Current Level");
        }

        if(world != null)
        {
            if (world.levels[level] != null) // Check if that level exists
            {
                width = world.levels[level].width;
                height = world.levels[level].height;
                dotsPrefab = world.levels[level].dotsPrefab;
                scoreGoals = world.levels[level].scoreGoals;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        findMatches = FindFirstObjectByType<FindMatches>();
        scoreManager = FindFirstObjectByType<ScoreManager>();
        goalManager = FindFirstObjectByType<GoalManager>();
        tiles = new BackgroundTile[width, height];
        dots = new GameObject[width, height];
        SetUp();
        currState = GameState.pause;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetUp()
    {
        for(int i=0; i<width; i++)
        {
            for(int j=0; j<height; j++)
            {
                Vector2 position = new Vector2(i, j + offset);
                GameObject backgroundTile = Instantiate(tilePrefab, position, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + i + "," + j + ")";

                int dotToUse = Random.Range(0, dotsPrefab.Length);
                int maxIterations = 0; // To prevent infinite loop
                while (MatchesAt(i, j, dotsPrefab[dotToUse]) && maxIterations < 100)
                {
                    dotToUse = Random.Range(0, dotsPrefab.Length);
                    maxIterations++;
                }
                maxIterations = 0;

                GameObject dot = Instantiate(dotsPrefab[dotToUse], position, Quaternion.identity);
                // adjust the correct position ('undo the offset')
                dot.GetComponent<Dot>().row = j;
                dot.GetComponent<Dot>().column = i;

                dot.transform.parent = this.transform;
                dot.name = "(" + i + "," + j + ")";

                dots[i, j] = dot;
            } 
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {   
        if(column > 1)
        {
            if (dots[column - 1, row].tag == piece.tag && dots[column - 2, row].tag == piece.tag) 
                return true;
        }
        if (row > 1)
        {
            if (dots[column, row - 1].tag == piece.tag && dots[column, row - 2].tag == piece.tag)
                return true;
        }
        return false;
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (dots[column, row].GetComponent<Dot>().isMatched)
        {
            findMatches.currMatches.Remove(dots[column, row]);

            if(goalManager != null)
            {
                goalManager.CompareGoal(dots[column, row].tag.ToString());
                goalManager.UpdateGoals();
            }
            Destroy(dots[column, row]);
            scoreManager.IncreaseScore(basePieceValue * streakValue);
            dots[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for(int i=0; i<width; i++)
        {
            for(int j=0; j<height; j++)
            {
                if (dots[i, j] != null)
                    DestroyMatchesAt(i, j);
            }
        }
        findMatches.currMatches.Clear();
        StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(dots[i, j] == null) {
                    nullCount++;
                } else if(nullCount > 0) { // decrease the row in the amount of null dots beneath
                    dots[i, j].GetComponent<Dot>().row -= nullCount;
                    dots[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(refillDeley * 0.5f);
        StartCoroutine(FillBoardCo());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (dots[i, j] == null)
                {
                    Vector2 position = new Vector2(i, j + offset);
                    int dotToUse = Random.Range(0, dotsPrefab.Length);

                    int maxIterations = 0; // To prevent infinite loop
                    while (MatchesAt(i, j, dotsPrefab[dotToUse]) && maxIterations < 100)
                    {
                        dotToUse = Random.Range(0, dotsPrefab.Length);
                        maxIterations++;
                    }
                    maxIterations = 0;

                    GameObject newPiece = Instantiate(dotsPrefab[dotToUse], position, Quaternion.identity);
                    // adjust the correct position ('undo the offset')
                    newPiece.GetComponent<Dot>().row = j;
                    newPiece.GetComponent<Dot>().column = i;
                    dots[i, j] = newPiece;
                }
            }
        }
    }

    private bool MathcesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (dots[i, j] != null) {
                    if(dots[i, j].GetComponent<Dot>().isMatched) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(refillDeley);

        while(MathcesOnBoard())
        {
            streakValue++;
            DestroyMatches();
            yield return new WaitForSeconds(2 * refillDeley);
        }
        yield return new WaitForSeconds(refillDeley);

        if(IsDeadLocked())
        {
            ShuffleBoard();
        }
        currState = GameState.move;
        streakValue = 1;
    }

    private void SwitchPieces(int column, int row, Vector2 direction)
    {
        // Take the second piece and save it in a holder
        GameObject holder = dots[column + (int)direction.x, row + (int)direction.y] as GameObject;
        // Switching the first piece to be the second position
        dots[column + (int)direction.x, row + (int)direction.y] = dots[column, row];
        // Set the first piece to be the second piece
        dots[column, row] = holder;
    }

    private bool ChaeckForMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(dots[i, j] != null)
                {
                    // Check if we have two pieces on the right side
                    if(i < width - 2)
                    {
                        if (dots[i + 1, j] != null && dots[i + 2, j] != null)
                        {
                            if(dots[i + 1, j].tag == dots[i, j].tag && dots[i + 2, j].tag == dots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                    // Check if we have two pieces above (up) 
                    if(j < height - 2)
                    {
                        if (dots[i, j + 1] != null && dots[i, j + 2] != null)
                        {
                            if (dots[i, j + 1].tag == dots[i, j].tag && dots[i, j + 2].tag == dots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    private bool SwitchAndCheck(int column, int row, Vector2 direction)
    {
        SwitchPieces(column, row, direction);
        if(ChaeckForMatches())
        {
            SwitchPieces(column, row, direction);
            return true;
        }
        SwitchPieces(column, row, direction);
        return false;
    }

    private bool IsDeadLocked()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (dots[i, j] != null)
                {
                    if(i < width - 1)
                    {
                        if(SwitchAndCheck(i, j, Vector2.right))
                        {
                            return false;
                        }
                    }

                    if (j < height - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.up))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    private void ShuffleBoard()
    {
        List<GameObject> newBoard = new List<GameObject>();
        // Add every piece to the list
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (dots[i, j] != null)
                {
                    newBoard.Add(dots[i, j]);
                }
            }
        }
        // Spot every piece on the board in new place
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // Pick a random number
                int pieceToUse = Random.Range(0, newBoard.Count);

                // Assign new column and row for the piece
                int maxIterations = 0; // To prevent infinite loop
                while (MatchesAt(i, j, newBoard[pieceToUse]) && maxIterations < 100)
                {
                    pieceToUse = Random.Range(0, newBoard.Count);
                    maxIterations++;
                }
                maxIterations = 0;
                Dot piece = newBoard[pieceToUse].GetComponent<Dot>();
                piece.column = i;
                piece.row = j;

                // Set the piece in the new place
                dots[i, j] = newBoard[pieceToUse];
                newBoard.Remove(newBoard[pieceToUse]);
            }
        }
        // Check if it's still deadlocked
        if(IsDeadLocked())
        {
            ShuffleBoard();
        }
    }
}
