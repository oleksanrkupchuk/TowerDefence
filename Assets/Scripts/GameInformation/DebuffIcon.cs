using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DebuffIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private DebuffCartData _debuffCartData;

    [SerializeField]
    private Image _icon;
    [SerializeField]
    private TextMeshProUGUI _title;
    [SerializeField]
    private TextMeshProUGUI _description;
    [SerializeField]
    private GameObject _toolTipe;
    [SerializeField]
    private LayoutElement _layoutElement;
    [SerializeField]
    private RectTransform _recttransform;

    public float Width { get => _recttransform.sizeDelta.x; }

    public void Init(DebuffCartData debuffCartData) {
        _debuffCartData = debuffCartData;
        SetIcon();
        SetTitleAndDescription();
        CheckWidthTextAndSetActiveLayoutElement();
        DisableToolTipe();
    }

    private void SetIcon() {
        _icon.sprite = _debuffCartData.icon;
    }

    private void SetTitleAndDescription() {
        _title.text = _debuffCartData.title;
        _description.text = _debuffCartData.description;
    }

    private void CheckWidthTextAndSetActiveLayoutElement() {
        if(_debuffCartData.description.Length < 20) {
            _layoutElement.enabled = false;
        }
    }

    private void DisableToolTipe() {
        _toolTipe.SetActive(false);
    }

    private void EnableToolTipe() {
        _toolTipe.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        //print("name = " + gameObject.name + " enter");
        EnableToolTipe();
    }

    public void OnPointerExit(PointerEventData eventData) {
        //print("name = " + gameObject.name + " exit");
        DisableToolTipe();
    }

}
