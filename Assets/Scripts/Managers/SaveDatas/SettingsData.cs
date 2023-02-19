[System.Serializable]
public class SettingsData
{
    public string resolution;
    public float musicVolume;
    public float sfxVolume;
    public bool fullscreen;

    public SettingsData(SettingsMenu settings)
    {
        resolution = settings.resolutionDropdown.options[settings.currentResolutionIndex].text;
        musicVolume = settings.musicSlider.value;
        sfxVolume = settings.sfxSlider.value;
        fullscreen = settings.fullscreenToggle.isOn;
    }
}
