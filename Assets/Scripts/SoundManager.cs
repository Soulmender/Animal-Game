using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    Slider masterVolumeSlider;
    [SerializeField]
    float masterVolume;
    [SerializeField]
    float tempMasterVolume;

    private void Awake()
    {
        Load();
    }

    public void OnSliderValueChange()
    {
        tempMasterVolume = masterVolumeSlider.value;
    }

    public void Save()
    {
        masterVolume = tempMasterVolume;
        PlayerPrefs.SetFloat("masterVolume", tempMasterVolume);
    }

    private void Load()
    {
        masterVolume = PlayerPrefs.GetFloat("masterVolume");
        masterVolumeSlider.value = masterVolume;
    }


}
