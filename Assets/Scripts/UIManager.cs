using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject dishPanel;  // Panel to show coompleted dish
    public Image dishImage;
    public TextMeshProUGUI dishNameTMP;

    public EventManager events;

    // Makes sure there's only one copy of UIManager
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);  // If there's another object it deletes it
    }

    public void Start()
    {
        EventManager.onMatchMade += OnMatchMade;
    }
    void OnDestroy()
    {
        EventManager.onMatchMade -= OnMatchMade;
    }
    // Call this when a level is completed
    public void ShowDishPanel(Dish dish)
    {
        dishImage.sprite = dish.dishSprite;
        dishNameTMP.text = dish.dishName;
        dishPanel.SetActive(true);
    }

    // Connected to NextButton
    public void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentIndex + 1 < SceneManager.sceneCountInBuildSettings) // Checks the current scene
            SceneManager.LoadScene(currentIndex + 1);  // If there's another scene it loads it
        else
            SceneManager.LoadScene("MainMenu");  // Gets back to the main menu or end scene
    }

    public void OnMatchMade(int MatchSize)
    {
        
    }

}
