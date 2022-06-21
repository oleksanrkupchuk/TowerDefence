using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
using UnityEngine.Localization.Settings;
using System.Collections;

public class SettingsMenu : BaseMenu {
    private SettingsData _settingsData;

    [Header("Buttons Settings Menu")]
    [SerializeField]
    private Button _back;
    [SerializeField]
    private Button _applySettings;
    [SerializeField]
    private Slider _sliderSound;
    [SerializeField]
    private Text _soundVolumeText;
    [SerializeField]
    private Slider _sliderEffect;
    [SerializeField]
    private Text _effectVolumeText;
    [SerializeField]
    private Dropdown _screenResolutionDropDown;
    [SerializeField]
    private Text _screenResolutionDropDownLabel;
    [SerializeField]
    private Dropdown _languageDropDown;
    [SerializeField]
    private Text _languageDropDownLabel;
    [SerializeField]
    private Toggle _fullScreenToggle;

    [SerializeField]
    private List<ResolutionScreen> _resolutions = new List<ResolutionScreen>();

    [Header("Confirm settings")]
    [SerializeField]
    private ConfirmSettings _confirmSettings;

    public static event Action<float> ChangeScreenResolution;

    private void OnEnable() {
        StartCoroutine(Init());
    }

    private IEnumerator Init() {
        LoadSettings();
        yield return StartCoroutine(LoadLanguages());
        InitLanguageDropdown();
        InitScreenResolutionDropdown();
        InitSettings();
    }

    public void LoadSettings() {
        _settingsData = SaveSystemSettings.LoadSettings();
    }

    public IEnumerator LoadLanguages() {
        yield return LocalizationSettings.InitializationOperation;
    }

    private void InitLanguageDropdown() {
        _languageDropDown.options.Clear();

        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++) {
            _languageDropDown.options.Add(new Dropdown.OptionData() {
                text = LocalizationSettings.AvailableLocales.Locales[i].name
            });
        }
    }

    private void InitScreenResolutionDropdown() {
        _screenResolutionDropDown.options.Clear();

        foreach (var resolution in _resolutions) {
            _screenResolutionDropDown.options.Add(new Dropdown.OptionData() {
                text = resolution.weidth + " x " + resolution.height
            });
        }
    }

    private void InitSettings() {
        _languageDropDownLabel.text = LocalizationSettings.AvailableLocales.Locales[_settingsData.indexLanguage].name;
        _languageDropDown.value = _settingsData.indexLanguage;

        _screenResolutionDropDownLabel.text = _resolutions[_settingsData.indexResolution].weidth + " x " + _resolutions[_settingsData.indexResolution].height;
        _screenResolutionDropDown.value = _settingsData.indexResolution;

        _sliderSound.value = _settingsData.soundVolume;
        _sliderEffect.value = _settingsData.effectVolume;
        _fullScreenToggle.isOn = _settingsData.fullScreenToggle;
    }

    private void Start() {
        SubscriptionButtons();
        DisableConfirmSettings();
    }

    private void SubscriptionButtons() {
        _back.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            DisableAndEnableGameObject(ThisGameObject, enableObject);
        });
        _applySettings.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            EnableConfirmSettings();
        });
        _confirmSettings.Yes.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            DisableConfirmSettings();
            SaveSettings();
            SetScreenResolution();
            LocaleSelected(_languageDropDown.value);
        });
        _confirmSettings.No.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            DisableConfirmSettings();
        });
    }

    private void DisableConfirmSettings() {
        _confirmSettings.gameObject.SetActive(false);
    }

    private void EnableConfirmSettings() {
        _confirmSettings.gameObject.SetActive(true);
    }

    private void SaveSettings() {
        _settingsData = new SettingsData();
        _settingsData.soundVolume = _sliderSound.value;
        _settingsData.effectVolume = _sliderEffect.value;
        _settingsData.fullScreenToggle = _fullScreenToggle.isOn;
        _settingsData.indexResolution = _screenResolutionDropDown.value;
        _settingsData.indexLanguage = _languageDropDown.value;

        SaveSystemSettings.SaveSettings(_settingsData);

        //print("-------SAVE-------");
        //print("volume = " + _settingsData.soundVolume);
        //print("resolution = " + _settingsData.indexResolution);
        //print("language = " + _settingsData.indexLanguage);
        //print("fullScreen = " + _settingsData.fullScreenToggle);
    }

    private void SetScreenResolution() {
        Screen.SetResolution(_resolutions[_screenResolutionDropDown.value].weidth,
                _resolutions[_screenResolutionDropDown.value].height, _fullScreenToggle.isOn);

        ChangeScreenResolution?.Invoke(_resolutions[_screenResolutionDropDown.value].weidth);
    }

    public void LocaleSelected(int indexLanguage) {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[indexLanguage];
    }

    private void Update() {
        SetSoundsVolume();
        SetEffectsVolume();
    }

    private void SetSoundsVolume() {
        SoundManager.Instance.SetSoundsVolume(_sliderSound.value);
        float _value = _sliderSound.value * 100;
        _soundVolumeText.text = "" + _value.ToString("0");
    }

    private void SetEffectsVolume() {
        SoundManager.Instance.SetEffectsVolume(_sliderEffect.value);
        float _value = _sliderEffect.value * 100;
        _effectVolumeText.text = "" + _value.ToString("0");
    }
}
