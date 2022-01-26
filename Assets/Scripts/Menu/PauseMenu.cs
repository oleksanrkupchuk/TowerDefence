using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : BaseMenu
{
    private GameManager _gameManager;

    [Header("Buttons Pause Menu")]
    [SerializeField]
    private Button _continue;
    [SerializeField]
    private Button _home;
    [SerializeField]
    private Button _settings;

    [Header("Objects")]
    [SerializeField]
    private GameObject _settingsObject;
    [SerializeField]
    private GameObject _background;

    private void Start() {
        SubscriptionButtons();
    }

    public void SetGameManager(GameManager gameManager) {
        _gameManager = gameManager;
    }

    private void SubscriptionButtons() {
        _continue.onClick.AddListener(() => {
            DisableGameObject(ThisGameObject);
            DisableGameObject(_background);
            StartTime();
            _gameManager.GameUnpause();
        });
        _settings.onClick.AddListener(() => { 
            DisableAndEnableGameObject(ThisGameObject, _settingsObject); 
            SetEnableObject(ThisGameObject, _settingsObject); 
        });
        _home.onClick.AddListener(() => { LoadGameLevel(IndexMainMenu); });
    }
}
