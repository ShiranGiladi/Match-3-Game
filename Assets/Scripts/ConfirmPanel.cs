using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ConfirmPanel : MonoBehaviour
{
    [Header("Level Information")]
    public string levelToLoad;
    public int level;
    private GameData gameData;
    private int starsActive;
    private int highScore;

    [Header("Level UI")]
    public Image[] stars;
    public TextMeshProUGUI highScoreText;

    void OnEnable()
    {
        Debug.Log("onEnavle function");
        gameData = FindFirstObjectByType<GameData>();
        LoadData();
        ActivateStars();
        SetText();
    }

    void LoadData()
    {
        // Enable all stars
        for (int i = 0; i < 3; i++)
        {
            stars[i].enabled = false;
        }

        if (gameData != null)
        {
            starsActive = gameData.saveData.starts[level - 1];
            highScore = gameData.saveData.highScores[level - 1];
        }
    }

    void SetText()
    {
        highScoreText.text = highScore.ToString();
    }

    void ActivateStars()
    {
        for (int i = 0; i < starsActive; i++)
        {
            stars[i].enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cancel()
    {
        this.gameObject.SetActive(false);
    }

    public void Play()
    {
        PlayerPrefs.SetInt("Current Level", level - 1);
        SceneManager.LoadScene(levelToLoad);
    }
}
