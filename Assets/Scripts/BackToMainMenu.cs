using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    public string sceneToLoad;
    private GameData gameData;
    private Board board;
    private ScoreManager scoreManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameData = FindFirstObjectByType<GameData>();
        board = FindFirstObjectByType<Board>();
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WinOK()
    {
        if(gameData != null)
        {
            // Active the next level
            gameData.saveData.isActive[board.level + 1] = true;

            // Save the score and update the highScore if needed
            int currHighScore = gameData.saveData.highScores[board.level];
            if (scoreManager.score > currHighScore)
            {
                gameData.saveData.highScores[board.level] = scoreManager.score;
            }

            // Save the stars that the user achived (according to the scoreGoals)
            int starsAchived = 0;
            for(int i=0; i<board.scoreGoals.Length; i++)
            {
                if(scoreManager.score >= board.scoreGoals[i])
                {
                    starsAchived++;
                } else {
                    break;
                }
            }

            if(starsAchived > gameData.saveData.starts[board.level])
            {
                gameData.saveData.starts[board.level] = starsAchived;
            }

            gameData.Save();
        }
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoseOK()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
