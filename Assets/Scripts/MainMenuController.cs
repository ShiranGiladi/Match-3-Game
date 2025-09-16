using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject openScreen;
    public GameObject levelsCanvas;

    void Start()
    {
        if (BackToMainMenu.returnToLevelCanvas)
        {
            openScreen.SetActive(false);
            levelsCanvas.SetActive(true);
            BackToMainMenu.returnToLevelCanvas = false; // reset
        }
        else
        {
            openScreen.SetActive(true);
            levelsCanvas.SetActive(false);
        }
    }

    // Loads the game scene "play" is clicked
    public void StartGame()
    {
        openScreen.SetActive(false);
        levelsCanvas.SetActive(true);
    }

    // Returns the open screen "Home" is clicked
    public void BackToOpenScreen()
    {
        openScreen.SetActive(true);
        levelsCanvas.SetActive(false);
    }
}
