using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MenuBase
{
    [Header("Buttons Lose Menu")]
    [SerializeField]
    private Button _start;
    [SerializeField]
    private Button _settings;
    [SerializeField]
    private Button _quit;

    [Header("Objects")]
    [SerializeField]
    private GameObject _settingsObject;
    [SerializeField]
    private GameObject _menuSelectLevelObject;

    void Start()
    {
        SubscriptionButtons();
    }

    private void SubscriptionButtons() {
        _start.onClick.AddListener(() => { 
            DisableAndEnableGameObject(ThisGameObject, _menuSelectLevelObject);
            SetEnableObject(ThisGameObject, _menuSelectLevelObject);
        });
        _settings.onClick.AddListener(() => {
            DisableAndEnableGameObject(ThisGameObject, _settingsObject);
            SetEnableObject(ThisGameObject, _settingsObject);
        });
        _quit.onClick.AddListener(() => { QuitApplication(); });
    }
}
