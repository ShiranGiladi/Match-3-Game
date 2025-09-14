using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static event Action<int> onMatchMade;

    public static void MatchMade(int matchSize)
    {
        onMatchMade?.Invoke(matchSize);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
