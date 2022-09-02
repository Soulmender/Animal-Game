using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    Slider masterVolumeSlider;
    [SerializeField]
    Slider speachVolumeSlider;
    [SerializeField]
    float masterVolume;
    [SerializeField]
    float tempMasterVolume;
    [SerializeField]
    float speachVolume;
    [SerializeField]
    float tempSpeachVolume;

    private void Awake()
    {
        Load();
    }

    public void OnMasterVolumeSliderValueChange()
    {
        tempMasterVolume = masterVolumeSlider.value;        
    }

    public void OnSpeachVolumeSliderValueChange()
    {
        tempSpeachVolume = speachVolumeSlider.value;
    }

    public void Save()
    {
        masterVolume = tempMasterVolume;
        PlayerPrefs.SetFloat("masterVolume", tempMasterVolume);
        AudioListener.volume = masterVolume;

        speachVolume = tempSpeachVolume;
        PlayerPrefs.SetFloat("speachVolume", speachVolume);
    }

    private void Load()
    {
        masterVolume = PlayerPrefs.GetFloat("masterVolume");
        tempMasterVolume = masterVolume;
        masterVolumeSlider.value = masterVolume;

        speachVolume = PlayerPrefs.GetFloat("speachVolume");
        tempSpeachVolume = speachVolume;
        speachVolumeSlider.value = speachVolume;
    }


}
