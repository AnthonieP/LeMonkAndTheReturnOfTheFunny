using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    [Header("Objects")]
    public GameObject menuObj;
    public GameObject optionsObj;
    [Header("Audio")]
    public AudioMixer audioMixer;
    [Header("Resolutions")]
    Resolution[] resolutions;
    [Header("Buttons")]
    public Slider soundEffectsSlider;
    public Slider musicSlider;
    public Toggle fullscreenToggle;
    public Dropdown qualityDropdown;
    public Dropdown resolutionDropdown;

    private void Start()
    {
        if (menuObj.active)
        {
            Time.timeScale = 0.00001f;
        }
    }

    private void Update()
    {
        //Esc button
        if (Input.GetButtonDown("Cancel"))
        {
            StartButton();
        }
    }

    public void StartButton()
    {
        menuObj.SetActive(!menuObj.active);

        if (menuObj.active)
        {
            Time.timeScale = 0.00001f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void OptionsButton()
    {
        optionsObj.SetActive(!optionsObj.active);
    }

    public void SetSoundEffects(float volume)
    {
        audioMixer.SetFloat("Effects", volume);
        soundEffectsSlider.value = volume;
    }

    public void SetMusic(float volume)
    {
        audioMixer.SetFloat("Music", volume);
        musicSlider.value = volume;
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
        qualityDropdown.value = quality;
        qualityDropdown.RefreshShownValue();
    }

    public void SetResolution(int res)
    {
        Screen.SetResolution(resolutions[res].width, resolutions[res].height, Screen.fullScreen);
        resolutionDropdown.value = res;
    }

    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        fullscreenToggle.isOn = fullscreen;
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
