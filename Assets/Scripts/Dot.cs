using System.Collections;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public int column;
    public int row;
    public int prevColumn;
    public int prevRow;
    public int targetX;
    public int targetY;
    public bool isMatched = false;

    private FindMatches findMatches;
    private Board board;
    private GameObject otherDot;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Vector2 tempPosition; // Hold the target position that the dot should move to
    public float swipeAngle = 0;
    public float swipeResist = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        board = FindFirstObjectByType<Board>();
        findMatches = FindFirstObjectByType<FindMatches>();
    }

    // Update is called once per frame
    void Update()
    {
        //FindMatches();
        if(isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f, 1f, 1f, .2f); // Set to greyed color

        }

        targetX = column;
        targetY = row;

        if(Mathf.Abs(targetX - transform.position.x) > .1) {
            // Move towards the target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.dots[column, row] != this.gameObject)
            {
                board.dots[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        } else {
            // Directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
        }

        if(Mathf.Abs(targetY - transform.position.y) > .1)
        {
            // Move towards the target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.dots[column, row] != this.gameObject)
            {
                board.dots[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else {
            // Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.5f);
        if(otherDot != null)
        {
            // If neither of the dots has a match, we want to move them back
            if(!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                otherDot.GetComponent<Dot>().row = row; // Changing the other dot row to our row
                otherDot.GetComponent<Dot>().column = column;
                row = prevRow;
                column = prevColumn;
                yield return new WaitForSeconds(.5f);
                board.currState = GameState.move;
            } else { // we have at least one match
                board.DestroyMatches();
            }
            otherDot = null;
        }
        else
        {
            // If there was no other dot, reset the state
            board.currState = GameState.move;
        }
    }

    private void OnMouseDown()
    {
        if(board.currState == GameState.move)
            startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        if (board.currState == GameState.move)
        {
            endTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ClaculateAngle();
        }
    }

    void ClaculateAngle()
    {
        if(Mathf.Abs(endTouchPosition.x - startTouchPosition.x) > swipeResist 
            || Mathf.Abs(endTouchPosition.y - startTouchPosition.y) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(endTouchPosition.y - startTouchPosition.y, endTouchPosition.x - startTouchPosition.x) * 180 / Mathf.PI;
            SetPiecesNewPosition();
            board.currState = GameState.wait;
        } else {
            board.currState = GameState.move;
        }
    }

    void SetPiecesNewPosition()
    {   
        if(swipeAngle > -45 &&  swipeAngle <= 45 && column < board.width - 1) { // Right Swipe
            otherDot = board.dots[column + 1, row];
            prevColumn = column;
            prevRow = row;
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
        } else if(swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1) { // Up Swipe
            otherDot = board.dots[column, row + 1];
            prevColumn = column;
            prevRow = row;
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        } else if((swipeAngle > 135 || swipeAngle <= -135) && column > 0) { // Left Swipe
            otherDot = board.dots[column - 1, row];
            prevColumn = column;
            prevRow = row;
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        } else if(swipeAngle < -45 && swipeAngle >= -135 && row > 0) { // Down Swipe
            otherDot = board.dots[column, row - 1];
            prevColumn = column;
            prevRow = row;
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }

        StartCoroutine(CheckMoveCo());
    }
}
