using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    Slider masterVolumeSlider;
    [SerializeField]
    Slider speachVolumeSlider;
    [SerializeField]
    Toggle fulscreenToggle;
    [SerializeField]
    TMP_Dropdown resolutionsDropdown;
    //[SerializeField]
    //float masterVolume;
    //[SerializeField]
    //float tempMasterVolume;
    //[SerializeField]
    //float speachVolume;
    //[SerializeField]
    //float tempSpeachVolume;
    [SerializeField]
    AudioMixer mainMixer;

    

    Resolution[] availableResolutions;

    private void Awake()
    {
        Load();
        availableResolutions = Screen.resolutions;
        SetResolutionOptions();

    }

    void SetResolutionOptions()
    {
        resolutionsDropdown.ClearOptions();
        int currentResolution = 0;

        List<string> options = new List<string>();
        for (int i = 0; i < availableResolutions.Length; i++)
        {
            string option = availableResolutions[i].width + "x" + availableResolutions[i].height;
            options.Add(option);

            if(availableResolutions[i].width == Screen.currentResolution.width && availableResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResolution;
        resolutionsDropdown.RefreshShownValue();
    }

    public void OnResolutionValueChange(int resolutionIndex)
    {
        Resolution resolution = availableResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool fullscreen)
    {
        Screen.fullScreen= fullscreen;
    }

    public void SetGraphicsQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("graphicsQuality", qualityIndex);
    }

    public void OnMasterVolumeSliderValueChange(float sliderValue)
    {
        //tempMasterVolume = masterVolumeSlider.value;
        mainMixer.SetFloat("MasterVolume", sliderValue);
    }

    public void OnSpeachVolumeSliderValueChange(float sliderValue)
    {
        //tempSpeachVolume = speachVolumeSlider.value;
        mainMixer.SetFloat("SpeachVolume", sliderValue);
    }

    public void Save()
    {
        //masterVolume = tempMasterVolume;
        PlayerPrefs.SetFloat("masterVolume", masterVolumeSlider.value);
        //AudioListener.volume = masterVolume;

        //speachVolume = tempSpeachVolume;
        PlayerPrefs.SetFloat("speachVolume", speachVolumeSlider.value);
    }

    private void Load()
    {
        float masterVolume = PlayerPrefs.GetFloat("masterVolume");
        //tempMasterVolume = masterVolume;
        masterVolumeSlider.value = masterVolume;
        mainMixer.SetFloat("MasterVolume", masterVolume);

        float speachVolume = PlayerPrefs.GetFloat("speachVolume");
        //tempSpeachVolume = speachVolume;
        speachVolumeSlider.value = speachVolume;
        mainMixer.SetFloat("SpeachVolume", speachVolume);
    }


}
