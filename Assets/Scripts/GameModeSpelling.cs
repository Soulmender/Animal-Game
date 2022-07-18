using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameModeSpelling : MonoBehaviour
{
    LevelManager lm;

    public Button SubmitButton;
    public Image displayAnimal;
    public TMP_InputField inputField;

    List<Animal> animals = new List<Animal>();
    int animalIndex = -1;

    private void Awake()
    {
        lm = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        animals = new List<Animal>(lm.animals);
        animals.Shuffle();

        SetupNextAnimal();
    }

    private void SetupNextAnimal()
    {
        if(animalIndex == animals.Count - 1)
        {
            lm.AddTrophy();
        }
        animalIndex = (animalIndex+1) % animals.Count;
        inputField.text = "";
        displayAnimal.sprite = animals[animalIndex].GetSprite();
    }

    public void OnSubmitButtonPressed()
    {
        if (CheckSpelling())
        {
            lm.AddCheckmark(animalIndex);
            AddPoints();
            SetupNextAnimal();
        }
        else
        {
            WrongSpelling();
        }
    }

    private void AddPoints()
    {
        lm.AddPoints(30);
    }

    private void WrongSpelling()
    {
        Debug.Log("Wrong!");
    }

    private bool CheckSpelling()
    {
        return animals[animalIndex].GetSpellingEnglish().ToLower() == inputField.text.ToLower();
    }
}
