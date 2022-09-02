using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [SerializeField]
    float masterVolume;
    [SerializeField]
    float tempMasterVolume;
    [SerializeField]
    float speachVolume;
    [SerializeField]
    float tempSpeachVolume;

    

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
