using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Potion : MonoBehaviour, IDragHandler, IEndDragHandler {

    private RectTransform _rectTransform;
    private Vector3 _startPosition;

    public float healthMultiplier;
    public float damageMultiplier;

    public void Init() {
        _startPosition = transform.position;
    }

    private void Awake() {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData) {
        _rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (transform.position != _startPosition) {
            transform.position = _startPosition;
        }
    }
}
