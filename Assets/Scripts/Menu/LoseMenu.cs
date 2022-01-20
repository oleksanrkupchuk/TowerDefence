using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoseMenu : MenuBase
{
    private GameManager _gameManager;

    [Header("Buttons Lose Menu")]
    [SerializeField]
    private Button _restart; 
    [SerializeField]
    private Button _settings;
    [SerializeField]
    private Button _home;

    private int _indexCurrentLevel;

    [Header("Objects")]
    [SerializeField]
    private GameObject _settingsObject;

    private void Start()
    {
        _indexCurrentLevel = SceneManager.GetActiveScene().buildIndex;
        SubscriptionButtons();
    }

    public void SetGameManager(GameManager gameManager) {
        _gameManager = gameManager;
    }

    private void SubscriptionButtons() {
        _restart.onClick.AddListener(() => { 
            _gameManager.StartTime();
            LoadGameLevel(_indexCurrentLevel);
        });
        _settings.onClick.AddListener(() => { 
            DisableAndEnableGameObject(ThisGameObject, _settingsObject); 
            SetEnableObject(ThisGameObject, _settingsObject);
        });
        _home.onClick.AddListener(() => { LoadGameLevel(IndexMainMenu); });
    }
}
