using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject goalLevelParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupGoals();
    }

    void SetupGoals()
    {
        for(int i=0; i<levelGoals.Length; i++)
        {
            // Create a new Goal Panel at the goalLevelParent
            GameObject goal = Instantiate(goalPrefab, goalLevelParent.transform.position, Quaternion.identity);
            goal.transform.SetParent(goalLevelParent.transform);
            // Set the image and the text of the goal
            GoalPanel panel = goal.GetComponent<GoalPanel>();
            currGoals.Add(panel);
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = "0/" + levelGoals[i].numberNeeded;
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
                currGoals[i].thisText.color = Color.green;
            }
        }

        if(goalsCompleted >= levelGoals.Length)
        {
            Debug.Log("You win!");
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
