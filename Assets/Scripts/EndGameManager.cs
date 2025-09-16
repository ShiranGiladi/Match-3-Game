using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum LevelType
{
    Moves,
    Time
}

public enum EndingState
{
    win,
    lose,
    gameOngoing
}

[System.Serializable]
public class EndLevelRequirements
{
    public LevelType levelType;
    public int counterValue;
}

public class EndGameManager : MonoBehaviour
{
    public GameObject movesLabel;
    public GameObject timeLabel;
    public GameObject youWinPanel;
    public GameObject tryAgainPanel;
    public GameObject statsPanel;
    public TextMeshProUGUI counter;
    public EndLevelRequirements requirements;
    public int currCounterValue;
    private Board board;
    private ScoreManager scoreManager;
    private float timerSeconds;
    private EndingState endingState = EndingState.gameOngoing;

    [Header("Level UI")]
    public Image[] stars;
    public TextMeshProUGUI scoreText;

    public Dish currentDish;  

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        board = FindFirstObjectByType<Board>();
        scoreManager = FindFirstObjectByType<ScoreManager>();
        SetGameType();
        SetupLevel();
    }

    void SetGameType()
    {
        if (board != null)
        {
            if (board.world.levels[board.level] != null)
            {
                requirements = board.world.levels[board.level].endLevelRequirements;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (board.currState == GameState.move)
        {
            ResolveEnd();
        }

        if (endingState != EndingState.gameOngoing) return;

        if (requirements.levelType == LevelType.Time && currCounterValue > 0)
        {
            timerSeconds -= Time.deltaTime;
            if (timerSeconds <= 0)
            {
                DecreaseCounterValue();
                timerSeconds = 1;
            }
        }  
    }

    void SetupLevel()
    {
        currCounterValue = requirements.counterValue;
        if (requirements.levelType == LevelType.Moves)
        {
            movesLabel.SetActive(true);
            timeLabel.SetActive(false);
        }
        else
        {
            timerSeconds = 1;
            movesLabel.SetActive(false);
            timeLabel.SetActive(true);
        }

        counter.text = "" + currCounterValue;
    }

    public void DecreaseCounterValue()
    {
        if (board.currState != GameState.pause)
        {
            currCounterValue--;
            counter.text = "" + currCounterValue;
            if (currCounterValue <= 0)
            {
                LoseGame();
            }
        }
    }

    public void WinGame()
    {
        endingState = EndingState.win;
    }

    private void winGamePanel()
    {
        SetStatsPanel();
        youWinPanel.SetActive(true);
        board.currState = GameState.win;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayWinSound();  // Plays the win sound
        }
        if (currentDish != null)
        {
            UIManager.Instance.ShowDishPanel(currentDish);  // Shows the dish 
        }
    }

    public void LoseGame()
    {
        endingState = EndingState.lose;
    }

    private void LoseGamePanel()
    {
        SetStatsPanel();
        tryAgainPanel.SetActive(true);
        board.currState = GameState.lose;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayLoseSound();
        }
    }

    public void SetStatsPanel()
    {
        statsPanel.SetActive(true);
        scoreText.text = scoreManager.score.ToString();
        for (int i = 0; i < board.scoreGoals.Length; i++)
        {
            if (scoreManager.score >= board.scoreGoals[i])
            {
                stars[i].enabled = true;
            }
            else {
                break;
            }
        }
    }

    private void ResolveEnd()
    {
        if (endingState == EndingState.gameOngoing) return;

        if (endingState == EndingState.win)
        {
            winGamePanel();
        }

        if(endingState == EndingState.lose)
        {
            LoseGamePanel();
        }
    }
}
