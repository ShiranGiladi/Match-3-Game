using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BlankGoal
{
    public int numberNeeded;
    public int numberCollected;
    public Sprite goalSprite;
    public string matchValue;
}

public class GoalManager : MonoBehaviour
{
    public BlankGoal[] levelGoals;
    public List<GoalPanel> currGoals = new List<GoalPanel>();
    public GameObject goalPrefab;
    public GameObject goalIntroParent;
    public GameObject goalLevelParent;
    public TextMeshProUGUI levelNumberText;
    public TextMeshProUGUI GoalRequirementText;
    private Board board;
    private EndGameManager endGameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        board = FindFirstObjectByType<Board>();
        endGameManager = FindFirstObjectByType<EndGameManager>();
        GetGoals();
        SetupGoals();
    }

    void GetGoals()
    {
        if (board != null)
        {
            if(board.world != null && board.world.levels[board.level] != null)
            {
                levelGoals = board.world.levels[board.level].levelGoals;
                for(int i = 0; i < levelGoals.Length; i++)
                {
                    levelGoals[i].numberCollected = 0;
                }
            }
        }
    }

    void SetupGoals()
    {
        if (board != null)
        {
            levelNumberText.text = "";
            int levelText = board.level + 1;
            levelNumberText.text = "Level " + levelText;
            Debug.Log(levelNumberText.text);
            Debug.Log(levelText);

            EndLevelRequirements requirements = board.world.levels[board.level].endLevelRequirements;
            if (requirements.levelType == LevelType.Moves)
            {
                GoalRequirementText.text = requirements.counterValue + " moves";
            }
            else
            {
                GoalRequirementText.text = requirements.counterValue + " seconds";
            }

            for (int i = 0; i < levelGoals.Length; i++)
            {
                // Create a new Goal Panel at the goalIntroParent
                GameObject goalIntro = Instantiate(goalPrefab, goalIntroParent.transform);
                // Set its background to light orange
                Image introImage = goalIntro.GetComponent<Image>();
                introImage.color = new Color(0.937f, 0.706f, 0.412f, 0.471f);

                // Set the image and the text of the goal
                GoalPanel introPanel = goalIntro.GetComponent<GoalPanel>();
                introPanel.thisSprite = levelGoals[i].goalSprite;
                introPanel.thisString = "" + levelGoals[i].numberNeeded;

                // Create a new Goal Panel at the goalLevelParent
                GameObject goal = Instantiate(goalPrefab, goalLevelParent.transform);
                // Set the image and the text of the goal
                GoalPanel panel = goal.GetComponent<GoalPanel>();
                currGoals.Add(panel);
                panel.thisSprite = levelGoals[i].goalSprite;
                panel.thisString = "0/" + levelGoals[i].numberNeeded;
            }
        }
    }

    public void UpdateGoals()
    {
        int goalsCompleted = 0;
        for(int i=0; i<levelGoals.Length; i++)
        {
            currGoals[i].thisText.text = "" + levelGoals[i].numberCollected + "/" + levelGoals[i].numberNeeded;
            if(levelGoals[i].numberCollected >= levelGoals[i].numberNeeded)
            {
                goalsCompleted++;
                //currGoals[i].thisText.color = Color.green;
                currGoals[i].DoneMark.gameObject.SetActive(true);
            }
        }

        if(goalsCompleted >= levelGoals.Length)
        {
            if(endGameManager != null)
            {
                endGameManager.WinGame();
            }
        }
    }

    public void CompareGoal(string goalToCompare) 
    {
        for (int i = 0; i < levelGoals.Length; i++)
        {
            if(goalToCompare == levelGoals[i].matchValue)
            {
                levelGoals[i].numberCollected++;
            }
        }
    }
}
