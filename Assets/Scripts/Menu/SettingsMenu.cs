using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MenuBase
{
    [Header("Buttons Pause Menu")]
    [SerializeField]
    private Button _back;

    private void Start() {
        SubscriptionButtons();
    }

    private void SubscriptionButtons() {
        _back.onClick.AddListener(() => { DisableAndEnableGameObject(ThisGameObject, enableObject); });
    }
}
