using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class LevelSelectItemUI : MonoBehaviour
{
    [SerializeField]
    int levelItemIndex;
    [SerializeField]
    int trophyCount;
    [SerializeField]
    TextMeshProUGUI levelName;
    [SerializeField]
    Image bronzeTrophy;
    [SerializeField]
    Image silverTrophy;
    [SerializeField]
    Image goldTrophy;

    GameManager gm;

    private void Awake()
    {
        levelName = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        UpdateUI();
    }

    private void UpdateUI()
    {
        levelName.text = "Level " + levelItemIndex.ToString();
        if(trophyCount > 2)
        {
            goldTrophy.gameObject.SetActive(true);
        }
        if (trophyCount > 1)
        {
            silverTrophy.gameObject.SetActive(true);
        }
        if (trophyCount > 0)
        {
            bronzeTrophy.gameObject.SetActive(true);
        }
    }

    public void SetLevelIndex(int index)
    {
        levelItemIndex = index;
        UpdateUI();
    }

    public void SetTrophyCount(int trophies)
    {
        trophyCount = trophies;
        UpdateUI();
    }

    public void SetupLevelItem(int index, int trophies)
    {
        levelItemIndex = index;
        trophyCount = trophies;
        gm = GameManager.Instance;
        UpdateUI();
    }


    public void OnLevelSelectItemPressed()
    {
        gm.PlayScene(levelItemIndex);
    }
}
