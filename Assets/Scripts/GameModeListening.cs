using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeListening : MonoBehaviour
{
    LevelManager lm;
    AudioSource audioSource;

    public Image displayAnimal;
    public Button EnglishPlayButton;
    public Button FinnishPlayButton;
    public Button NextButton;


    List<Animal> animals = new List<Animal>();

    int animalIndex = -1;
    int lastAnimal;

    private void Awake()
    {
        lm = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        audioSource = GameObject.Find("Level Manager").GetComponent<AudioSource>();
        animals = new List<Animal>(lm.animals);

        animals.Shuffle();

    }

    private void Start()
    {
        SetNewAnimal();
    }



    public void NextAnimal()
    {
        //Prepare for the next animal and give rewards
        lastAnimal = animalIndex;
        lm.AddPoints(10);
        lm.AddCheckmark(animalIndex);
        //Check if we've come to the last animal
        if (animalIndex == animals.Count - 1)
        {
            LevelComplete();
        }
        else
        {
            //Set a new animal
            SetNewAnimal();
        }        
    }

    /*
    public void PreviousAnimal()
    {
        if (animalIndex != 0)
        {
            animalIndex = (animalIndex - 1) % animals.Count;
        }
        else
        {
            animalIndex = animals.Count - 1;
        }


        displayAnimal.sprite = animals[animalIndex].GetSprite();
    }
    */

    void SetNewAnimal()
    {
        animalIndex = (animalIndex + 1) % animals.Count;

        displayAnimal.sprite = animals[animalIndex].GetSprite();
        ResetButtonState();
        
    }

    void ResetButtonState()
    {
        EnglishPlayButton.interactable = false;
        NextButton.interactable = false;
    }

    public void PlayAnimalSoundEnglish()
    {
        audioSource.clip = animals[animalIndex].GetEnglish();
        audioSource.Play();
        NextButton.interactable = true;
    }

    public void PlayAnimalSoundFinnish()
    {
        audioSource.clip = animals[animalIndex].GetFinnish();
        audioSource.Play();
        EnglishPlayButton.interactable = true;
    }

    void LevelComplete()
    {
        if (lm == null)
        {
            return;
        }
        else
        {
            lm.AddTrophy();
        }
    }
}
