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
    [SerializeField]
    List<PlayerData> allPlayers;
    



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

        OnSavePressed();

        OnLoadPressed();
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
        //For TESTING only
        List<PlayerData> players = new List<PlayerData>();
        for (int i = 0; i < 5; i++)
        {
            PlayerData p = new PlayerData("Player " + i.ToString(), i * 250, player.trophiesAtLevel);
            p.ToJson();
            players.Add(p);
        }

        PlayerCollection pc = new PlayerCollection(players.ToArray());
        string playersJSON = JsonUtility.ToJson(pc, true);

        //Actual LOAD functionality
        //TODO: Read from disc


        //From JSON to object
        PlayerCollection pcFromJson = JsonUtility.FromJson<PlayerCollection>(playersJSON);
        allPlayers = new List<PlayerData>(pcFromJson.players);

        FindObjectOfType<GameMenuUI>().UpdateUI();
    }

    public void OnSavePressed()
    {
        //For TESTING ONLY
        List<PlayerData> players = new List<PlayerData>();
        for (int i = 0; i < 5; i++)
        {
            PlayerData p = new PlayerData("Player " + i.ToString(), i * 250, player.trophiesAtLevel);
            p.ToJson();
            players.Add(p);
        }

        //Actual SAVE unctionality
        //Converting to JSON
        PlayerCollection pc = new PlayerCollection(players.ToArray());
        string playersJSON = JsonUtility.ToJson(pc, true);
        Debug.Log(playersJSON);

        //TODO: Saving to Disc


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

[Serializable]
public class PlayerCollection
{
    public PlayerData[] players;

    public PlayerCollection()
    {
        players = new PlayerData[0];
    }

    public PlayerCollection(PlayerData[] _players)
    {
        players = _players;
    }
}