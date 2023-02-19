using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public Slider musicSlider;
    public Slider sfxSlider;
    [SerializeField] private GameObject confirmPopup;
    [SerializeField] private TextMeshProUGUI timerText;

    [HideInInspector] public int currentResolutionIndex;

    private Resolution[] resolutions;
    private AudioManager _audioManager;
    private int oldResolutionIndex;
    private bool mustConfirm;

    public float timer;

    private void Start()
    {
        _audioManager = GameManager.instance.GetComponent<AudioManager>();

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        options.Reverse();
        Array.Reverse(resolutions);

        resolutionDropdown.AddOptions(options);

        LoadData();
        ConfirmSelection();
    }
    private void ChangeResolutionValue(int newResolutionIndex)
    {
        resolutionDropdown.value = newResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        oldResolutionIndex = currentResolutionIndex;
        currentResolutionIndex = resolutionIndex;
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        mustConfirm = true;
        StartCoroutine(Confimation());
    }

    public void ConfirmSelection()
    {
        mustConfirm = false;
        confirmPopup.SetActive(false);
    }

    public void SetOldResolution()
    {
        ConfirmSelection();
        ChangeResolutionValue(oldResolutionIndex);
        ConfirmSelection();
    }

    private void LoadResolution(string resolution)
    {
        for (int i = 0; i < resolutionDropdown.options.Count; i++)
        {
            if (resolutionDropdown.options[i].text == resolution)
            {
                ChangeResolutionValue(i);
                ConfirmSelection();
                return;
            }
        }

        ChangeResolutionValue(currentResolutionIndex);
        ConfirmSelection();
    }

    public void SetMusicVolume(float volume)
    {
        _audioManager.masterMixer.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        _audioManager.masterMixer.SetFloat("sfxVolume", volume);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SaveData()
    {
        SettingsData data = new SettingsData(this);
        SaveSystem.SaveData(data, "settings");
    }

    public void LoadData()
    {
        SettingsData data = SaveSystem.LoadData("settings") as SettingsData;

        if (data == null)
            return;

        LoadResolution(data.resolution);
        fullscreenToggle.isOn = data.fullscreen;
        musicSlider.value = data.musicVolume;
        sfxSlider.value = data.sfxVolume;
    }

    IEnumerator Confimation()
    {
        confirmPopup.SetActive(true);
        for (int i = 15; i >= 0; i--)
        {
            if (!mustConfirm)
            {
                confirmPopup.SetActive(false);
                yield break;
            }
            timerText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }
        SetOldResolution();
    }
}
