using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class DebuffIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private DebuffCartData _debuffCartData;

    [SerializeField]
    private Image _icon;
    [SerializeField]
    private LocalizeStringEvent _descriptionLocalize;
    [SerializeField]
    private GameObject _toolTipe;
    [SerializeField]
    private LayoutElement _layoutElement;
    [SerializeField]
    private RectTransform _recttransform;

    public float Width { get => _recttransform.sizeDelta.x; }

    public void Init(DebuffCartData debuffCartData, LocalizedString tableDescription) {
        _debuffCartData = debuffCartData;
        SetIcon();
        CheckWidthTextAndSetActiveLayoutElement();
        DisableToolTipe();
        _descriptionLocalize.StringReference = tableDescription;
    }

    private void SetIcon() {
        _icon.sprite = _debuffCartData.icon;
    }

    private void CheckWidthTextAndSetActiveLayoutElement() {
        //if(_debuffCartData.description.Length < 20) {
        //    _layoutElement.enabled = false;
        //}
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
