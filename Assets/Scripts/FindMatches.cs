using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currMatches = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        board = FindFirstObjectByType<Board>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private void AddToListAndMatch(GameObject dot)
    {
        if (!currMatches.Contains(dot))
            currMatches.Add(dot);
        dot.GetComponent<Dot>().isMatched = true;
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currDot = board.dots[i, j];
                if(currDot != null)
                {
                    // Horizontal matches
                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftDot = board.dots[i - 1, j];
                        GameObject rightDot = board.dots[i + 1, j];
                        if (leftDot != null && rightDot != null)
                        {
                            if (leftDot.tag == currDot.tag && rightDot.tag == currDot.tag)
                            {
                                AddToListAndMatch(leftDot);
                                AddToListAndMatch(rightDot);
                                AddToListAndMatch(currDot);
                            }
                        }
                    }

                    // Vertical matches
                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upDot = board.dots[i, j + 1];
                        GameObject downDot = board.dots[i, j - 1];
                        if (upDot != null && downDot != null)
                        {
                            if (upDot.tag == currDot.tag && downDot.tag == currDot.tag)
                            {
                                AddToListAndMatch(upDot);
                                AddToListAndMatch(downDot);
                                AddToListAndMatch(currDot);
                            }
                        }
                    }
                }
            }
        }
    }
}
