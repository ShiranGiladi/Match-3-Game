using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum LevelType
{
    Moves,
    Time
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
        SetStatsPanel();
        youWinPanel.SetActive(true);
        board.currState = GameState.win;
        currCounterValue = 0;
        counter.text = "" + currCounterValue;
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
        SetStatsPanel();
        tryAgainPanel.SetActive(true);
        board.currState = GameState.lose;
        Debug.Log("You Lose!!");
        currCounterValue = 0;
        counter.text = "" + currCounterValue;
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayLoseSound();
        }
    }

    public void SetStatsPanel()
    {
        statsPanel.SetActive(true);
        scoreText.text = board.currState == GameState.win ? scoreManager.score.ToString() : "0";
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
}
