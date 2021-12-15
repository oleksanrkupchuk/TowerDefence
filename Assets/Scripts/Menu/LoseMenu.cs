using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoseMenu : MenuBase
{
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

    private void SubscriptionButtons() {
        _restart.onClick.AddListener(() => { LoadGameLevel(_indexCurrentLevel); });
        _settings.onClick.AddListener(() => { 
            DisableAndEnableGameObject(ThisGameObject, _settingsObject); 
            SetEnableObject(ThisGameObject, _settingsObject);
        });
        _home.onClick.AddListener(() => { LoadGameLevel(IndexMainMenu); });
    }
}
