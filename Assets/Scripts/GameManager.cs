using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //[SerializeField]
    //PlayerData player;
    [SerializeField]
    int numberOfLevels = 3;
    [SerializeField]
    PlayerCollection allPlayers;

    string SAVE_PATH;
    
    public PlayerData player { get { return allPlayers.GetActivePlayer(); } }
    public string PlayerName { get { return player.playerName; } set { SetActivePlayer(value); } }
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

        SAVE_PATH = Path.Combine(Application.persistentDataPath, "save.json");
    }

    private void Start()
    {
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

        //Actual LOAD functionality
        //TODO: Read from disc
        if (!IsFileValid(SAVE_PATH, ".json"))
        {
            Debug.LogWarning(string.Format("File {0} does not exist or doesn't have a valid extension.", SAVE_PATH));
        }
        else
        {
            //From JSON to object
            string playersJSON = GetFileContents(SAVE_PATH);

            allPlayers = JsonUtility.FromJson<PlayerCollection>(playersJSON);

            FindObjectOfType<GameMenuUI>().UpdateUI();
        }
    }


    public void OnSavePressed()
    {
        //Actual SAVE unctionality
        //Converting to JSON
        string playersJSON = JsonUtility.ToJson(allPlayers, true);

        //TODO: Saving to Disc
        //Open or create the file.
        StreamWriter writer = new StreamWriter(SAVE_PATH, false);
        writer.Write(playersJSON);
        writer.Close();
    }
  

    public void OnQuitPressed()
    {
        Application.Quit();
    }

    public void PlayScene(int index)
    {
        SceneManager.LoadScene("Level" + index.ToString());
    }


    bool IsFileValid(string path, string extension)
    {
        bool IsValid = true;

        if (!File.Exists(path))
        {
            IsValid = false;
        }
        else if (Path.GetExtension(path).ToLower() != extension)
        {
            IsValid = false;
        }

        return IsValid;
    }

    string GetFileContents(string path)
    {
        string fileContent = string.Empty;

        using (StreamReader reader = new StreamReader(path))
        {
            fileContent = reader.ReadToEnd();
        }

        return fileContent;
    }

    void SetActivePlayer(string name)
    {
        //Find if the player exists in the old list
        //There are more clever ways of doing this, such as using Linq, but tha t requires a far more indepth knowledge than is necessary for this
        for (int i = 0; i < allPlayers.players.Count; i++)
        {
            if(allPlayers.players[i].playerName == name)
            {
                allPlayers.activePlayer = i;
                FindObjectOfType<GameMenuUI>().UpdateUI();
                return;
            }
        }

        //If we haven't found a player with matching name, create a new player and add to list and make it active;
        PlayerData newPlayer = new PlayerData();
        newPlayer.playerName = name;
        allPlayers.players.Add(newPlayer);
        allPlayers.activePlayer = allPlayers.players.Count - 1;

        //Save the game after creating a new player
        OnSavePressed();

        FindObjectOfType<GameMenuUI>().UpdateUI();
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
    public List<PlayerData> players;
    public int activePlayer;

    public PlayerCollection()
    {
        players = new List<PlayerData>();
    }

    public PlayerCollection(List<PlayerData> _players, int _activePlayer)
    {
        players = new List<PlayerData>(_players);
        activePlayer = _activePlayer;
    }

    public PlayerData GetActivePlayer()
    {
        return players[activePlayer];
    }
}