using System;
using System.IO;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static event Action<MatchEventData> onMatchMade;

    public static void MatchMade(MatchEventData eventData)
    {
        onMatchMade?.Invoke(eventData);
    }

    public static event Action onGameWon;

    public static void GameWon()
    {
        onGameWon?.Invoke();
    }

    public static event Action onGameLost;

    public static void GameLost()
    {
        onGameLost?.Invoke();
    }


}
