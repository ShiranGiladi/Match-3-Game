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
                                if(!currMatches.Contains(leftDot))
                                    currMatches.Add(leftDot);
                                leftDot.GetComponent<Dot>().isMatched = true;
                                if (!currMatches.Contains(rightDot))
                                    currMatches.Add(rightDot);
                                rightDot.GetComponent<Dot>().isMatched = true;
                                if (!currMatches.Contains(currDot))
                                    currMatches.Add(currDot);
                                currDot.GetComponent<Dot>().isMatched = true;
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
                                if (!currMatches.Contains(upDot))
                                    currMatches.Add(upDot);
                                upDot.GetComponent<Dot>().isMatched = true;
                                if (!currMatches.Contains(downDot))
                                    currMatches.Add(downDot);
                                downDot.GetComponent<Dot>().isMatched = true;
                                if (!currMatches.Contains(currDot))
                                    currMatches.Add(currDot);
                                currDot.GetComponent<Dot>().isMatched = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
