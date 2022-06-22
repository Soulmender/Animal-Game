using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Animal", menuName = "ScriptableObjects/Animals", order = 1)]
public class Animal : ScriptableObject
{
    [SerializeField]
    AudioClip soundEnglish;
    [SerializeField]
    AudioClip soundFinnish;
    [SerializeField]
    Sprite sprite;
    [SerializeField]
    string spellingEnglish;
    [SerializeField]
    int basePoints;

    public AudioClip GetEnglish()
    {
        return soundEnglish;
    }

    public AudioClip GetFinnish()
    {
        return soundFinnish;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public string GetSpellingEnglish()
    {
        return spellingEnglish;
    }

    public int GetBasePoints()
    {
        return basePoints;
    }
}
