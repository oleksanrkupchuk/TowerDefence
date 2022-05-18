[System.Serializable]
public class SettingsData 
{
    public int indexResolution;
    public bool fullScreenToggle;
    public float soundVolume;

    public SettingsData(SettingsMenu settingsMenu) {
        indexResolution = settingsMenu.GetIndexScreenResolutionDropDown;
        fullScreenToggle = settingsMenu.GetFullScreenTogle;
        soundVolume = settingsMenu.SoundVolume;
    }

    public SettingsData() {

    }
}
