using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class DebuffIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image _icon;
    [SerializeField]
    private LocalizeStringEvent _descriptionLocalize;
    [SerializeField]
    private GameObject _toolTipe;

    public void Init(DebuffCartData debuffCartData, LocalizedString tableDescription) {
        _icon.sprite = debuffCartData.icon;
        DisableToolTipe();
        _descriptionLocalize.StringReference = tableDescription;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        EnableToolTipe();
    }

    public void OnPointerExit(PointerEventData eventData) {
        DisableToolTipe();
    }

    private void EnableToolTipe() {
        _toolTipe.SetActive(true);
    }

    private void DisableToolTipe() {
        _toolTipe.SetActive(false);
    }
}
