[System.Serializable]
public class SettingsData 
{
    public int indexResolution;
    public bool fullScreenToggle;

    public SettingsData(SettingsMenu settingsMenu) {
        indexResolution = settingsMenu.GetIndexScreenResolutionDropDown;
        fullScreenToggle = settingsMenu.GetFullScreenTogle;
    }
}
