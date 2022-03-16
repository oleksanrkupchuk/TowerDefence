using UnityEngine;
using UnityEngine.UI;

public class MainMenu : BaseMenu
{
    private SettingsMenu _settingsMenu;

    [Header("Buttons Lose Menu")]
    [SerializeField]
    private Button _start;
    [SerializeField]
    private Button _shop;
    [SerializeField]
    private Button _gameInformationButton;
    [SerializeField]
    private Button _settings;
    [SerializeField]
    private Button _quit;

    [Header("Objects")]
    [SerializeField]
    private GameObject _shopObject;
    [SerializeField]
    private GameInformation _gameInformation;
    [SerializeField]
    private GameObject _settingsObject;
    [SerializeField]
    private GameObject _menuSelectLevelObject;

    private void Start()
    {
        _settingsMenu = _settingsObject.GetComponent<SettingsMenu>();
        _settingsMenu.CheckSaveSettingsAndLoad();

        SubscriptionButtons();
    }

    private void SubscriptionButtons() {
        _start.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            DisableAndEnableGameObject(ThisGameObject, _menuSelectLevelObject);
            SetEnableObject(ThisGameObject, _menuSelectLevelObject);
        });
        _shop.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            DisableAndEnableGameObject(ThisGameObject, _shopObject);
            SetEnableObject(ThisGameObject, _shopObject);
        });
        _gameInformationButton.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            DisableAndEnableGameObject(ThisGameObject, _gameInformation.gameObject);
            SetEnableObject(ThisGameObject, _gameInformation.gameObject);
        });
        _settings.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            DisableAndEnableGameObject(ThisGameObject, _settingsObject);
            SetEnableObject(ThisGameObject, _settingsObject);
        });
        _quit.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick); 
            QuitApplication(); 
        });
    }
}
