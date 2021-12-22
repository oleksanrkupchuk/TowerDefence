using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class SettingsMenu : MenuBase
{
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
    private Toggle _fullScreenTogle;
    [SerializeField]
    private TextMeshProUGUI _soundVolumeText;

    [SerializeField]
    private List<ResolutionScreen> _resolutions = new List<ResolutionScreen>();

    [Header("Confirm settings")]
    [SerializeField]
    private ConfirmSettings _confirmSettings;

    public static event Action ChangeScreenResolution;

    private void Start() {
        DisableConfirmSettings();
        SubscriptionButtons();
        InitScreenResolutionDropdown();
    }

    private void InitScreenResolutionDropdown() {
        _screenResolutionDropDown.options.Clear();

        foreach (var resolution in _resolutions) {
            _screenResolutionDropDown.options.Add(new TMP_Dropdown.OptionData() { 
                text = resolution.weidth + " x " + resolution.height
            });
        }

        //CheckSaveScreenResolution();

        //_screenResolutionDropDown.onValueChanged.AddListener(delegate { PrintDropdownValue(_screenResolutionDropDown); });
    }

    //private void CheckSaveScreenResolution() {
    //    DefaultScreenResolution();
    //}

    //private void DefaultScreenResolution() {
    //    int oneThousandNineHundredAndTwenty = _resolutions[0].weidth;
    //    int oneThousandEighty = _resolutions[0].height;
    //    Screen.SetResolution(oneThousandNineHundredAndTwenty, oneThousandEighty, _fullScreenTogle.isOn);
    //}

    private void SubscriptionButtons() {
        _back.onClick.AddListener(() => { 
            DisableAndEnableGameObject(ThisGameObject, enableObject); 
        });
        _applySettings.onClick.AddListener(() => { 
            EnableConfirmSettings(); 
        });
        _confirmSettings.Yes.onClick.AddListener(() => { 
            SetScreenResolution(_screenResolutionDropDown);
            DisableConfirmSettings();
        });
        _confirmSettings.No.onClick.AddListener(() => {
            DisableConfirmSettings();
        });
    }

    private void SetScreenResolution(TMP_Dropdown dropdown) {
        int _index = dropdown.value;
        Screen.SetResolution(_resolutions[_index].weidth, _resolutions[_index].height, _fullScreenTogle.isOn);
        ChangeScreenResolution?.Invoke();
        print("index = " + _index);
    }

    private void EnableConfirmSettings() {
        _confirmSettings.ConfirmSettingsObject.SetActive(true);
    }

    private void DisableConfirmSettings() {
        _confirmSettings.ConfirmSettingsObject.SetActive(false);
    }

    private void Update() {
        SetSoundVolume();
    }

    private void SetSoundVolume() {
        SoundManager.Instance.SetVolume(_slider.value);
        float _value = _slider.value * 100;
        _soundVolumeText.text = "" + _value.ToString("0");
    }
}
