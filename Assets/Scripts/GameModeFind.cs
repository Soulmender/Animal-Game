using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeFind : MonoBehaviour
{
    LevelManager lm;
    AudioSource audioSource;

    public Button EnglishPlayButton;
    public List<GameObject> animalSlots;

    List<Animal> animals = new List<Animal>();
    int animalIndex = -1;

    private void Awake()
    {
        lm = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        audioSource = GameObject.Find("Level Manager").GetComponent<AudioSource>();
        animals = new List<Animal>(lm.animals);

        SetBoard();
    }

    void SetBoard()
    {
        List<Animal> selection = SelectFalseAnimals();
        selection.Add(SetTheCorrectAnimal());
        selection.Shuffle();

        DisplayAnimals(selection);
        SetAnimalButtons(selection);
        EnglishPlayButton.onClick.AddListener(PlayAnimalSoundEnglish);
    }

    private void SetAnimalButtons(List<Animal> selection)
    {
        for (int i = 0; i < selection.Count; i++)
        {
            animalSlots[i].GetComponent<Button>().onClick.RemoveAllListeners();
            if (selection[i] == animals[animalIndex])
            {
                //animalSlots[i].gameObject.SetActive(true);
                animalSlots[i].GetComponent<Button>().onClick.AddListener(CorrectAnimalPressed);
            }
            else
            {
                //animalSlots[i].gameObject.SetActive(false);
            }
        }
    }

    private void CorrectAnimalPressed()
    {
        lm.AddCheckmark(animalIndex);
        AddPoints();
        if (animalIndex == animals.Count - 1)
        {
            lm.AddTrophy();
            return;
        }
        SetBoard();
        
    }

    private void AddPoints()
    {
        lm.AddPoints(20);
    }

    private void DisplayAnimals(List<Animal> selection)
    {
        for (int i = 0; i < selection.Count; i++)
        {
            animalSlots[i].GetComponent<Image>().sprite = selection[i].GetSprite();
        }
    }

    private Animal SetTheCorrectAnimal()
    {
        animalIndex++;
        if(animalIndex >= animals.Count)
        {
            animalIndex = 0;
        }
        return animals[animalIndex];
    }

    private List<Animal> SelectFalseAnimals()
    {
        List<Animal> shuffledAnimals = new List<Animal>(animals);
        shuffledAnimals.RemoveAt(animalIndex + 1);

        shuffledAnimals.Shuffle();

        List<Animal> shortList = new List<Animal>();
        for (int i = 0; i < shuffledAnimals.Count; i++)
        {
            shortList.Add(shuffledAnimals[i]);
        }
        return shortList;
    }

    public void PlayAnimalSoundEnglish()
    {
        audioSource.clip = animals[animalIndex].GetEnglish();
        audioSource.Play();
    }
}
