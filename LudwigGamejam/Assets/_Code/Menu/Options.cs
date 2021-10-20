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
    public GameObject resetObj;
    public PlayerController player;
    [Header("Audio")]
    public AudioMixer audioMixer;
    [Header("GameInfo")]
    public float playTime;
    public Text playTimeText;
    public Vector3 playerSpawn;
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
            player.isPaused = true;
        }

        GetResolutions();
    }

    private void Update()
    {
        //Esc button
        if (Input.GetButtonDown("Cancel"))
        {
            StartButton();
        }

        playTime += Time.deltaTime;
    }

    void GetResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> resolutionStrings = new List<string>();

        int currentResInt = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionStrings.Add(resolutions[i].width + " x " + resolutions[i].height);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResInt = i;
            }
        }

        resolutionDropdown.AddOptions(resolutionStrings);
        resolutionDropdown.value = currentResInt;
        resolutionDropdown.RefreshShownValue();
    }

    public void StartButton()
    {
        SetTime();
        menuObj.SetActive(!menuObj.active);

        if (menuObj.active)
        {
            Time.timeScale = 0.00001f;
            player.isPaused = true;
        }
        else
        {
            StartCoroutine(ActivatePlayer(.01f * Time.timeScale));
        }

    }

    public void OptionsButton()
    {
        optionsObj.SetActive(!optionsObj.active);
        resetObj.SetActive(false);
    }

    public void RestartButton()
    {
        resetObj.SetActive(!resetObj.active);
        optionsObj.SetActive(false);
    }

    public void RestartConfirmationButton()
    {
        playTime = 0;
        player.transform.position = playerSpawn;
        player.PutOnHat(0);

        if (FindObjectOfType<CameraController>() != null)
        {
            FindObjectOfType<CameraController>().GoToPlayer();
            TrailRenderer[] trailRenderers = FindObjectsOfType<TrailRenderer>();
            for (int i = 0; i < trailRenderers.Length; i++)
            {
                trailRenderers[i].Clear();
            }
        }
    }

    public void SetTime()
    {
        float roundPlayMins = Mathf.RoundToInt((playTime / 60) - 0.5f);
        float secs = playTime - (roundPlayMins * 60);
        string minString = roundPlayMins.ToString("#");
        string secString = secs.ToString("#");
        if(roundPlayMins < 1)
        {
            minString = "0";
        }
        if (secs < 1)
        {
            secString = "00";
        }
        else if(secs < 10)
        {
            secString = "0" + secs.ToString("#");
        }


        playTimeText.text = minString + ":" + secString;
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

    IEnumerator ActivatePlayer(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 1f;
        player.isPaused = false;
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
