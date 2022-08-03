using UnityEngine;

public class GameMenu : BaseMenu
{
    [Header("Objects")]
    [SerializeField]
    private GameObject _backgroundGameMenu;
    [SerializeField]
    private PauseMenu _pauseMenu;
    [SerializeField]
    private GameObject _loseMenu;
    [SerializeField]
    private GameObject _settingsMenu;
    [SerializeField]
    private WinMenu _winMenu;

    [Header("Scripts")]
    public GameManager gameManager;

    public WinMenu WinMenu { get => _winMenu; }

    private void OnEnable() {
        _pauseMenu.SetGameManager(gameManager);
        DisableBackgroundGameMenu();
        DisableLoseMenu();
        DisablePauseMenu();
        DisableSettingsMenu();
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
        _pauseMenu.gameObject.SetActive(isActive);
    }

    public void DisablePauseMenu() {
        _pauseMenu.gameObject.SetActive(false);
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
        _winMenu.gameObject.SetActive(false);
    }

    public void EnableWinMenuAndSetDeafaultSpeedTime() {
        StartTime();
        _winMenu.gameObject.SetActive(true);
    }
}
