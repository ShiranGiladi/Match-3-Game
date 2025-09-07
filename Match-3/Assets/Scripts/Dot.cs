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
    private Board board;
    private GameObject otherDot;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Vector2 tempPosition; // Hold the target position that the dot should move to
    public float swipeAngle = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //board = FindObjectOfType<Board>();
        board = FindFirstObjectByType<Board>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        column = targetX;
        row = targetY;
        prevColumn = column;
        prevRow = row;
    }

    // Update is called once per frame
    void Update()
    {
        FindMatches();
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
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        } else {
            // Directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.dots[column, row] = this.gameObject;
        }

        if(Mathf.Abs(targetY - transform.position.y) > .1)
        {
            // Move towards the target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else {
            // Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.dots[column, row] = this.gameObject;
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
            }
            otherDot = null;
        }
    }

    private void OnMouseDown()
    {
        startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        endTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ClaculateAngle();
    }

    void ClaculateAngle()
    {
        swipeAngle = Mathf.Atan2(endTouchPosition.y - startTouchPosition.y, endTouchPosition.x - startTouchPosition.x) * 180 / Mathf.PI;
        SetPiecesNewPosition();
    }

    void SetPiecesNewPosition()
    {   
        if(swipeAngle > -45 &&  swipeAngle <= 45 && column < board.width - 1) { // Right Swipe
            otherDot = board.dots[column + 1, row];
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
        } else if(swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1) { // Up Swipe
            otherDot = board.dots[column, row + 1];
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        } else if((swipeAngle > 135 || swipeAngle <= -135) && column > 0) { // Left Swipe
            otherDot = board.dots[column - 1, row];
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        } else if(swipeAngle < -45 && swipeAngle >= -135 && row > 0) { // Down Swipe
            otherDot = board.dots[column, row - 1];
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }

        StartCoroutine(CheckMoveCo());
    }

    void FindMatches()
    {
        // Horizontal matches
        if(column > 0 && column < board.width - 1)
        {
            GameObject leftDot1 = board.dots[column - 1, row];
            GameObject rightDot1 = board.dots[column + 1, row];
            if(leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
            {
                leftDot1.GetComponent<Dot>().isMatched = true;
                rightDot1.GetComponent<Dot>().isMatched = true;
                isMatched = true;
            }
        }

        // Vertical matches
        if(row > 0 && row < board.height - 1)
        {
            GameObject upDot1 = board.dots[column, row + 1];
            GameObject downDot1 = board.dots[column, row - 1];
            if(upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag)
            {
                upDot1.GetComponent<Dot>().isMatched = true;
                downDot1.GetComponent<Dot>().isMatched = true;
                isMatched = true;
            }
        }
    }
}
