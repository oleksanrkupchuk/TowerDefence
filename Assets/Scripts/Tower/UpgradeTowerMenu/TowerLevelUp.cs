using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerLevelUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField]
    private GameObject _rangeNextLevel;
    [SerializeField]
    private Tower _tower;
    [SerializeField]
    private TowerUpgradeMenu _towerUpgradeMenu;

    public void OnPointerEnter(PointerEventData eventData) {
        _rangeNextLevel.SetActive(true);
        float _diameter = _tower.RangeAttack * 2;
        _rangeNextLevel.transform.localScale = new Vector2(_diameter + (2 * _towerUpgradeMenu.AdditionRangeRadius), _diameter + (2 * _towerUpgradeMenu.AdditionRangeRadius));
    }

    public void OnPointerExit(PointerEventData eventData) {
        _rangeNextLevel.SetActive(false);
    }
}
