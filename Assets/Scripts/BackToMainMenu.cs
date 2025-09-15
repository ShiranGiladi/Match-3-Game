using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    public string sceneToLoad;
    private GameData gameData;
    private Board board;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameData = FindFirstObjectByType<GameData>();
        board = FindFirstObjectByType<Board>();
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
            gameData.Save();
        }
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoseOK()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
