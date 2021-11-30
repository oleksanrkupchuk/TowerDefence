using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MenuBase
{
    [Header("Buttons Pause Menu")]
    [SerializeField]
    private Button _continue;
    [SerializeField]
    private Button _home;
    [SerializeField]
    private Button _settings;

    [Header("Level index")]
    [SerializeField]
    private int _indexMainMenu;

    [Header("Objects")]
    [SerializeField]
    private GameObject _settingsObject;
    [SerializeField]
    private GameObject _background;
    [SerializeField]
    private GameManager _gameManager;

    private void Start() {
        SubscriptionButtons();
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
        _home.onClick.AddListener(() => { LoadGameLevel(_indexMainMenu); });
    }
}
