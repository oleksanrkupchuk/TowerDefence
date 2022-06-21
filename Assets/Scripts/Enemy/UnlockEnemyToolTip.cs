using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnlockEnemyToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField]
    private Text _description;

    private void Awake() {
        _description.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _description.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        _description.gameObject.SetActive(false);
    }
}
