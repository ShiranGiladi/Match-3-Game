using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SaveData
{
    public bool[] isActive; // Store the information about whether or not the level is active
    public int[] highScores; // Store the high score for each level
    public int[] starts; // Store the amount of starts the user earn for each level

}

public class GameData : MonoBehaviour
{
    public static GameData gameData;
    public SaveData saveData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(gameData == null)
        {
            DontDestroyOnLoad(this.gameObject);
            gameData = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        Load();
    }

    public void Save()
    {
        // Create a binary formatter which can read binart files
        BinaryFormatter formatter = new BinaryFormatter();
        // Create a route fronm the program to the file
        FileStream file = File.Open(Application.persistentDataPath + "/player.data", FileMode.Open);
        // Create a copy of save data
        SaveData data = new SaveData();
        data = saveData;
        // Save the data in the file
        formatter.Serialize(file, data);
        file.Close();
        Debug.Log("Saved");
    }

    public void Load()
    {
        // Check if the save game file exists
        if (File.Exists(Application.persistentDataPath + "/player.data"))
        {
            // Create a Binary Formatter
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/player.data", FileMode.Open);
            saveData = formatter.Deserialize(file) as SaveData;
            file.Close();
            Debug.Log("Loaded");
        }
    }

    private void OnDisable()
    {
        Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
