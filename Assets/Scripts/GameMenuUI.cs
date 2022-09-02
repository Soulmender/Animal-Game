using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class GameMenuUI : MonoBehaviour
{
    [SerializeField]
    GameObject PlayerItemPrefab;
    [SerializeField]
    Transform PlayerItemContent;
    [SerializeField]
    TMP_InputField PlayerNameInput;
    [SerializeField]
    TextMeshProUGUI PlayerNameDisplay;
    [SerializeField]
    TextMeshProUGUI PlayerScoreDisplay;
    [SerializeField]
    GameObject LevelContainer;
    [SerializeField]
    GameObject LvlSelectItemPrefab;


    List<GameObject> PlayerItems;
    List<GameObject> levelItems;

    private void Awake()
    {
        PlayerItems = new List<GameObject>();
        levelItems = new List<GameObject>();
    }


    // Start is called before the first frame update
    void Start()
    {
        GenerateLevelItems();
        GeneratePlayerItems();
        UpdateUI();
    }

    private void GeneratePlayerItems()
    {
        ClearPlayerItems();

        PlayerCollection pc = GameManager.Instance.AllPlayers;
        for (int i = 0; i < pc.players.Count; i++)
        {
            GameObject tempPlayerItem = Instantiate(PlayerItemPrefab, PlayerItemContent);
            TextMeshProUGUI[] texts =  tempPlayerItem.GetComponentsInChildren<TextMeshProUGUI>();
            texts[1].text = pc.players[i].playerName;
            texts[0].text = "Score: " + pc.players[i].playerScore;
            int index = i;
            tempPlayerItem.GetComponent<Button>().onClick.AddListener(() => { SetPlayerName(pc.players[index].playerName); });
        }
    }

    private void ClearPlayerItems()
    {
        for (int i = PlayerItems.Count - 1; i >= 0; i--)
        {
            Destroy(PlayerItems[i]);
        }

        PlayerItems.Clear();
    }

    public void UpdateLevelItems()
    {
        DestroyLevelItems();
        GenerateLevelItems();
    }

    private void DestroyLevelItems()
    {
        for (int i = levelItems.Count - 1; i >= 0; i--)
        {
            Destroy(levelItems[i]);
        }
        levelItems.Clear();
    }

    private void GenerateLevelItems()
    {
        for (int i = 0; i < GameManager.Instance.NumberOfLevels; i++)
        {
            GameObject lvlSelectItem = Instantiate(LvlSelectItemPrefab, LevelContainer.transform);
            lvlSelectItem.GetComponent<LevelSelectItemUI>().SetupLevelItem(i + 1, GameManager.Instance.GetTrophiesAtLevel(i+1));
            levelItems.Add(lvlSelectItem);
        }
    }

    void SetPlayerName(string name)
    {
        PlayerNameInput.text = name;
    }


    public void OnPlayerNameSubmitted()
    {
        GameManager.Instance.PlayerName = PlayerNameInput.text;
        UpdateUI();
    }

    public void UpdateUI()
    {
        PlayerNameDisplay.text = GameManager.Instance.PlayerName;
        PlayerScoreDisplay.text = GameManager.Instance.PlayerScore.ToString();
        UpdateLevelItems();
    }
}
