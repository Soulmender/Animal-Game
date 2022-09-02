using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelManager : MonoBehaviour
{
    public int LevelIndex = 0;
    public int noTrophies = 0;
    public string playerName;
    public int score;

    public List<Transform> trophies;
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI scoreText;

    public Transform ScorePanel;
    public GameObject ScoreBoxPrefab;
    List<GameObject> scoreBoxes;

    public List<GameObject> gameModes;


    public List<Animal> animals = new List<Animal>();
    GameManager gm;

    AudioSource audioPlayer;
    

    private void Awake()
    {
        GetPlayerInfo();
        UpdateUI();
        StartANewGameMode();
        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.volume = PlayerPrefs.GetFloat("speachVolume");
    }

    private void GetPlayerInfo()
    {
        gm = GameManager.Instance;
        if(gm == null)
        {
            Debug.LogError("No GameManager has been found.");
        }
        else
        {
            playerName = gm.PlayerName;
            score = gm.PlayerScore;
            noTrophies = gm.GetTrophiesAtLevel(LevelIndex);
        }
    }

    private void StartANewGameMode()
    {
        if(noTrophies >= gameModes.Count)
        {
            Debug.LogError("Too many trophies for this level. Or too few Game modes");
            CompleteLevel();
        }
        else
        {
            gameModes[noTrophies].SetActive(true);
        }
    }

    public void UpdateUI()
    {
        playerText.text = playerName;
        scoreText.text = score.ToString();

        DestroyCheckBoxes();
        CreateScoreBoxes();

        for (int i = 0; i < trophies.Count; i++)
        {
            trophies[i].GetChild(0).gameObject.SetActive(false);
        }

        for (int i = 0; i < noTrophies; i++)
        {
            trophies[i].GetChild(0).gameObject.SetActive(true);
        }
    }

    private void DestroyCheckBoxes()
    {
        if(scoreBoxes == null || scoreBoxes.Count == 0)
        {
            return;
        }

        for (int i = scoreBoxes.Count-1; i >= 0; i--)
        {
            Destroy(scoreBoxes[i].gameObject);
        }
        scoreBoxes = new List<GameObject>();
    }

    void CreateScoreBoxes()
    {
        scoreBoxes = new List<GameObject>();
        for (int i = 0; i < animals.Count; i++)
        {
            scoreBoxes.Add(Instantiate(ScoreBoxPrefab, ScorePanel));
            scoreBoxes[i].transform.Find("CheckMark").gameObject.SetActive(false);
            scoreBoxes[i].transform.Find("CrossMark").gameObject.SetActive(false);
        }
    }

    public void AddCheckmark(int index)
    {
        scoreBoxes[index].transform.Find("CheckMark").gameObject.SetActive(true);
    }

    public void AddTrophy()
    {
        gameModes[noTrophies].SetActive(false);

        noTrophies++;
        if(noTrophies >= trophies.Count)
        {
            noTrophies = trophies.Count;
            CompleteLevel();
        }

        if(noTrophies < trophies.Count)
        {
            StartANewGameMode();
        }
        
        UpdateUI();
    }

    public void AddPoints(int points)
    {
        int pastPoints;
        if(Int32.TryParse(scoreText.text,out pastPoints)){
            scoreText.text = (pastPoints + points).ToString();
        }
        else
        {
            Debug.LogWarning("Score wasn't a number!");
        }
    }


    private void CompleteLevel()
    {
        //Pass on new total points to the level manager, update the trophy count]
        int points;
        if (Int32.TryParse(scoreText.text, out points))
        {
            scoreText.text = (points).ToString();
        }
        else
        {
            Debug.LogWarning("Score wasn't a number!");
        }

        GameManager.Instance.UpdateLevelInfo(LevelIndex, noTrophies, points);
    }
}
