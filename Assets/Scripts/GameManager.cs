using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    string playerName;
    [SerializeField]
    int playerScore;
    [SerializeField]
    int numberOfLevels = 3;
    [SerializeField]
    List<int> trophiesAtLevel;



    public string PlayerName { get { return playerName; } set { playerName = value; } }
    public int PlayerScore { get { return playerScore; } set { playerScore = value; } }
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


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            GoToMainMenu();
        }
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("Main");
    }

    public int GetTrophiesAtLevel(int lvl)
    {
        return trophiesAtLevel[lvl-1];
    }

    public void SetTrophiesAtLevel( int lvl, int trophies)
    {
        trophiesAtLevel[lvl - 1] = trophies;
    }

    public void UpdateLevelInfo(int lvl, int trophies, int score)
    {
        //Update trophies
        if(trophiesAtLevel[lvl-1] > trophies)
        {
            Debug.LogWarning("Trying to save lower trophies value than existing.");
            return;
        }
        else
        {
            trophiesAtLevel[lvl - 1] = trophies;
        }
        //Update score
        if(playerScore > score)
        {
            Debug.LogWarning("Trying to save lower score value than existing.");
            return;
        }
        else
        {
            playerScore = score;
        }

        GoToMainMenu();

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
