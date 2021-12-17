using UnityEngine;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _backgroundGameMenu;
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private GameObject _loseMenu;
    [SerializeField]
    private GameObject _settingsMenu;
    [SerializeField]
    private GameObject _winMenu;

    private void OnEnable() {
        DisableBackgroundGameMenu();
        DisableLoseMenu();
        DisablePauseMenu();
        DisableSettingsMenu();
        DisableWinMenu();
    }

    public void SetActiveBackgroundGameMenu(bool isActive) {
        _backgroundGameMenu.SetActive(isActive);
    }

    public void DisableBackgroundGameMenu() {
        _backgroundGameMenu.SetActive(false);
    }

    public void EnableBackgroundGameMenu() {
        _backgroundGameMenu.SetActive(true);
    }

    public void SetActiveDisablePauseMenu(bool isActive) {
        _pauseMenu.SetActive(isActive);
    }

    public void DisablePauseMenu() {
        _pauseMenu.SetActive(false);
    }

    public void DisableLoseMenu() {
        _loseMenu.SetActive(false);
    }

    public void EnableLoseMenu() {
        _loseMenu.SetActive(true);
    }

    public void DisableSettingsMenu() {
        _settingsMenu.SetActive(false);
    }

    public void DisableWinMenu() {
        _winMenu.SetActive(false);
    }

    public void EnableWinMenu() {
        _winMenu.SetActive(true);
    }
}
