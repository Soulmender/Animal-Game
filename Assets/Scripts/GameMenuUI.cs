using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameMenuUI : MonoBehaviour
{

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


    // Start is called before the first frame update
    void Start()
    {
        GenerateLevelItems();
        UpdateUI();
    }


    private void GenerateLevelItems()
    {
        for (int i = 0; i < GameManager.Instance.NumberOfLevels; i++)
        {
            GameObject lvlSelectItem = Instantiate(LvlSelectItemPrefab, LevelContainer.transform);
            lvlSelectItem.GetComponent<LevelSelectItemUI>().SetupLevelItem(i + 1, GameManager.Instance.GetTrophiesAtLevel(i+1));

        }
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
    }
}
