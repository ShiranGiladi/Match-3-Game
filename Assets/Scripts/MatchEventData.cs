using System.Text.RegularExpressions;
using UnityEngine;

public class MatchEventData
{
    public int matchLength;
    public string tagName;

    public MatchEventData(int matchLength, string tagName)
    {
        this.matchLength = matchLength;
        this.tagName = tagName;
    }
}
