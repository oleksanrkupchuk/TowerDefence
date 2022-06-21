using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoseMenu : BaseMenu
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
        _restart.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            LoadGameLevel(_indexCurrentLevel);
            StartTime();
        });
        _settings.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            DisableAndEnableGameObject(ThisGameObject, _settingsObject); 
            SetEnableObject(ThisGameObject, _settingsObject);
        });
        _home.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            LoadGameLevel(IndexMainMenu); 
        });
    }
}
