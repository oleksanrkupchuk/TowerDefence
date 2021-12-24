using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmSettings : MenuBase
{
    [SerializeField]
    private TextMeshProUGUI _confirmTitle;

    [SerializeField]
    private GameObject _confirmSettingsObject;
    [SerializeField]
    private Button _yes;
    [SerializeField]
    private Button _no;

    public GameObject ConfirmSettingsObject { get => _confirmSettingsObject; }
    public Button Yes { get => _yes; }
    public Button No { get => _no; }

    private void OnEnable() {
        SetConfirmTitle();
    }

    private void SetConfirmTitle() {
        _confirmTitle.text = SettingsMenuText.confirmSettingTitle;
    }
}
