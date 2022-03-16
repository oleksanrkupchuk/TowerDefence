using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class SettingsMenu : BaseMenu {
    private SaveSoundData _soundData;

    [Header("Buttons Settings Menu")]
    [SerializeField]
    private Button _back;
    [SerializeField]
    private Button _applySettings;

    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private TMP_Dropdown _screenResolutionDropDown;
    [SerializeField]
    private TextMeshProUGUI _screenResolutionDropDownLabel;
    [SerializeField]
    private Toggle _fullScreenToggle;
    [SerializeField]
    private TextMeshProUGUI _soundVolumeText;

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

    private void OnEnable() {
        InitScreenResolutionDropdown();
        //print("enable settings");
    }

    private void Start() {
        InitSound();
        DisableConfirmSettings();
        SubscriptionButtons();
    }

    private void InitSound() {
        _soundData = SaveSystemSettings.LoadSound();
        _slider.value = _soundData.volume / 100;
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
        SettingsData settingsData = SaveSystemSettings.LoadSettings();

        _screenResolutionDropDown.value = settingsData.indexResolution;
        _fullScreenToggle.isOn = settingsData.fullScreenToggle;

        Screen.SetResolution(_resolutions[settingsData.indexResolution].weidth,
                _resolutions[settingsData.indexResolution].height, _fullScreenToggle.isOn);
        ChangeScreenResolution?.Invoke(_resolutions[settingsData.indexResolution].weidth);
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

    private void DisableConfirmSettings() {
        _confirmSettings.ConfirmSettingsObject.SetActive(false);
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
        float _volume = _slider.value * 100;
        SaveSystemSettings.SaveSoundData(_volume);
    }

    private void Update() {
        SetSoundsVolume();
    }

    private void SetSoundsVolume() {
        SoundManager.Instance.SetSoundsVolume(_slider.value);
        float _value = _slider.value * 100;
        _soundVolumeText.text = "" + _value.ToString("0");
    }
}
