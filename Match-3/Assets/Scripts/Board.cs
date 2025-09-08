using NUnit.Framework;
using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public enum GameState {
    wait,
    move
}

public class Board : MonoBehaviour
{
    public GameState currState = GameState.move;
    public int width;
    public int height;
    public int offset; // For the pieces to slide into the screen
    public GameObject tilePrefab;
    public GameObject[] dotsPrefab;
    private BackgroundTile[,] tiles;
    public GameObject[,] dots;
    private FindMatches findMatches;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        findMatches = FindFirstObjectByType<FindMatches>();
        tiles = new BackgroundTile[width, height];
        dots = new GameObject[width, height];
        SetUp();
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
            Destroy(dots[column, row]);
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
        yield return new WaitForSeconds(.4f);
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
        yield return new WaitForSeconds(.5f);
        while(MathcesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        yield return new WaitForSeconds(.5f);
        currState = GameState.move;
    }
}
