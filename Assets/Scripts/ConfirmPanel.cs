using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfirmPanel : MonoBehaviour
{
    public string levelToLoad;
    public Image[] stars;
    public int level;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActivateStars();
    }

    void ActivateStars()
    {
        // ***************** CHANGE THAT METHOD AFTER CREATING THE BINARY FILE !!!!!!!!!!!!! *****************
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].enabled = false;
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
