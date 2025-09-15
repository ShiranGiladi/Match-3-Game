using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score;
    private GameData gameData;
    private Board board;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameData = FindFirstObjectByType<GameData>();
        board = FindFirstObjectByType<Board>();
        EventManager.onMatchMade += IncreaseScore;
    }

    void OnDestroy()
    {
        EventManager.onMatchMade -= IncreaseScore;
    }
    public void IncreaseScore(int amountToIncrease)
    {
        if(gameData != null)
        {
            int highScore = gameData.saveData.highScores[board.level];
            if(highScore > score)
            {
                gameData.saveData.highScores[board.level] = score;
            }
            gameData.Save();
        }

        score += amountToIncrease;
        scoreText.text = score.ToString();
    }

}
