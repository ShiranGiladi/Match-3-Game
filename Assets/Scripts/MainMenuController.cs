using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Loads the game scene "play" is clicked
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    // Returns the main menu scene when "Quit Game" is clicked
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenuController");
    }
}
