using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;
using UnityEngine.Localization.Settings;
using System.Collections;

public class SettingsMenu : BaseMenu {
    private SaveSoundData _soundData;
    private SettingsData _settingsData;

    [Header("Buttons Settings Menu")]
    [SerializeField]
    private Button _back;
    [SerializeField]
    private Button _applySettings;

    [SerializeField]
    private Slider _sliderSound;
    [SerializeField]
    private TMP_Dropdown _screenResolutionDropDown;
    [SerializeField]
    private Dropdown _languageDropDown;
    [SerializeField]
    private Text _languageDropDownLabel;
    [SerializeField]
    private Toggle _fullScreenToggle;
    [SerializeField]
    private Text _soundVolumeText;

    [SerializeField]
    private List<ResolutionScreen> _resolutions = new List<ResolutionScreen>();

    [Header("Confirm settings")]
    [SerializeField]
    private ConfirmSettings _confirmSettings;

    public static event Action<float> ChangeScreenResolution;
    public int GetIndexScreenResolutionDropDown {
        get {
            return _screenResolutionDropDown.value;
        }
    }

    public bool GetFullScreenTogle {
        get {
            return _fullScreenToggle.isOn;
        }
    }

    public float SoundVolume { get => _sliderSound.value; }

    private void OnEnable() {
        LoadSettings();
        InitSound();
        StartCoroutine(InitLanguageDropdown());
        InitScreenResolutionDropdown();
        //print("enable settings");
    }

    private void LoadSettings() {
        _settingsData = SaveSystemSettings.LoadSettings();
    }

    private void InitSound() {
        _sliderSound.value = _settingsData.soundVolume;
    }

    private IEnumerator InitLanguageDropdown() {
        _languageDropDown.options.Clear();
        yield return LocalizationSettings.InitializationOperation;

        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++) {
            //_languageDropDown.options.Add(new Dropdown.OptionData(LocalizationSettings.AvailableLocales.Locales[i].name));
            _languageDropDown.options.Add(new Dropdown.OptionData() {
                text = LocalizationSettings.AvailableLocales.Locales[i].name
            });
        }

        //_languageDropDown.value = 0;
        _languageDropDownLabel.text = LocalizationSettings.AvailableLocales.Locales[0].name;
        LocaleSelected(0);
        _languageDropDown.onValueChanged.AddListener(LocaleSelected);
    }

    private void LocaleSelected(int index) {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        //print("language = " + LocalizationSettings.AvailableLocales.Locales[index].name);
    }

    private void InitScreenResolutionDropdown() {
        _screenResolutionDropDown.options.Clear();

        foreach (var resolution in _resolutions) {
            _screenResolutionDropDown.options.Add(new TMP_Dropdown.OptionData() {
                text = resolution.weidth + " x " + resolution.height
            });
        }

        CheckSaveSettingsAndLoad();
    }

    public void CheckSaveSettingsAndLoad() {
        if (SaveSystemSettings.IsExistsSaveSettingsFile()) {
            LoadSettingsAndSetResolution();
        }
        else {
            print("file not create");
        }
    }

    public void LoadSettingsAndSetResolution() {
        _screenResolutionDropDown.value = _settingsData.indexResolution;
        _fullScreenToggle.isOn = _settingsData.fullScreenToggle;

        Screen.SetResolution(_resolutions[_settingsData.indexResolution].weidth,
                _resolutions[_settingsData.indexResolution].height, _fullScreenToggle.isOn);
        ChangeScreenResolution?.Invoke(_resolutions[_settingsData.indexResolution].weidth);
    }

    private void Start() {
        DisableConfirmSettings();
        SubscriptionButtons();
    }

    private void DisableConfirmSettings() {
        _confirmSettings.ConfirmSettingsObject.SetActive(false);
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
            SetScreenResolution(_screenResolutionDropDown);
            SaveSettings();
            SaveVolume();
        });
        _confirmSettings.No.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            DisableConfirmSettings();
        });
    }

    private void EnableConfirmSettings() {
        _confirmSettings.ConfirmSettingsObject.SetActive(true);
    }

    private void SetScreenResolution(TMP_Dropdown dropdown) {
        int _index = dropdown.value;
        Screen.SetResolution(_resolutions[_index].weidth, _resolutions[_index].height, _fullScreenToggle.isOn);
        ChangeScreenResolution?.Invoke(_resolutions[_index].weidth);
        //print("index = " + _index);
    }

    private void SaveSettings() {
        SaveSystemSettings.SaveSettings(this);
    }

    private void SaveVolume() {
        float _volume = _sliderSound.value * 100;
        SaveSystemSettings.SaveSoundData(_volume);
    }

    private void Update() {
        SetSoundsVolume();
    }

    private void SetSoundsVolume() {
        SoundManager.Instance.SetSoundsVolume(_sliderSound.value);
        float _value = _sliderSound.value * 100;
        _soundVolumeText.text = "" + _value.ToString("0");
    }
}
