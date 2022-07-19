using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    /*
    [SerializeField]
    string playerName;
    [SerializeField]
    int playerScore;
    [SerializeField]
    List<int> trophiesAtLevel;*/
    [SerializeField]
    PlayerData player;
    [SerializeField]
    int numberOfLevels = 3;
    



    public string PlayerName { get { return player.playerName; } set { player.playerName = value; } }
    public int PlayerScore { get { return player.playerScore; } set { player.playerScore = value; } }
    public int NumberOfLevels { get { return numberOfLevels; } private set { numberOfLevels = value; } }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        player = new PlayerData();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            GoToMainMenu();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnSavePressed();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnLoadPressed();
        }
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("Main");
    }

    public int GetTrophiesAtLevel(int lvl)
    {
        return player.trophiesAtLevel[lvl-1];
    }

    public void SetTrophiesAtLevel( int lvl, int trophies)
    {
        player.trophiesAtLevel[lvl - 1] = trophies;
    }

    public void UpdateLevelInfo(int lvl, int trophies, int score)
    {
        //Update trophies
        if(player.trophiesAtLevel[lvl-1] > trophies)
        {
            Debug.LogWarning("Trying to save lower trophies value than existing.");
            return;
        }
        else
        {
            player.trophiesAtLevel[lvl - 1] = trophies;
        }
        //Update score
        if(player.playerScore > score)
        {
            Debug.LogWarning("Trying to save lower score value than existing.");
            return;
        }
        else
        {
            player.playerScore = score;
        }

        GoToMainMenu();

    }

    public void OnLoadPressed()
    {
        string testData = "{ \"playerName\": \"Player ABC\", \"playerScore\": 123, \"trophiesAtLevel\": [3,2,1]}"; 
        player = PlayerData.FromJson(testData);
        FindObjectOfType<GameMenuUI>().UpdateUI();
    }

    public void OnSavePressed()
    {
        player.ToJson();
    }
  

    public void OnQuitPressed()
    {
        Application.Quit();
    }

    public void PlayScene(int index)
    {
        SceneManager.LoadScene("Level" + index.ToString());
    }
}

[Serializable]
public class PlayerData
{
    public string playerName;
    public int playerScore;
    public int[] trophiesAtLevel;

    public PlayerData()
    {
        playerName = "Player";
        playerScore = 0;
        trophiesAtLevel = new int[GameManager.Instance.NumberOfLevels];
    }

    public PlayerData(string _playerName, int _playerScore, int[] _trophiesAtLevel)
    {
        playerName = _playerName;
        playerScore = _playerScore;
        trophiesAtLevel = _trophiesAtLevel;
    }

    public string ToJson()
    {
        try
        {
            string jsonData = JsonUtility.ToJson(this, true);
            Debug.Log(jsonData);
            return jsonData;
        }
        catch
        {
            Debug.LogError("Converting to JSON failed.");
            return "{\"Error\": \"Conversion failed\"}";
        }
    }

    public static PlayerData FromJson(string jsonData)
    {
        try
        {
            return JsonUtility.FromJson<PlayerData>(jsonData);
        }
        catch
        {
            Debug.LogError("Error loading PLayerData from JSON: " + jsonData);
            return null;
        }
    }
}