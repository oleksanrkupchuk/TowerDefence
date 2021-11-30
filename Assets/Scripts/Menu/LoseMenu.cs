using UnityEngine;
using UnityEngine.UI;

public class LoseMenu : MenuBase
{
    [Header("Buttons Lose Menu")]
    [SerializeField]
    private Button _restart; 
    [SerializeField]
    private Button _settings;
    [SerializeField]
    private Button _home;

    [Header("Level index")]
    [SerializeField]
    private int _indexMainMenu;
    [SerializeField]
    private int _indexCurrentLevel;

    [Header("Objects")]
    [SerializeField]
    private GameObject _settingsObject;

    private void Start()
    {
        SubscriptionButtons();
    }

    private void SubscriptionButtons() {
        _restart.onClick.AddListener(() => { LoadGameLevel(_indexCurrentLevel); });
        _settings.onClick.AddListener(() => { 
            DisableAndEnableGameObject(ThisGameObject, _settingsObject); 
            SetEnableObject(ThisGameObject, _settingsObject);
        });
        _home.onClick.AddListener(() => { LoadGameLevel(_indexMainMenu); });
    }
}
