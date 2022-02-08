﻿using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour {

    private Tower _tower;
    private Vector2 cursorMousePosition;
    private RaycastHit2D _raycastHit;
    [SerializeField]
    private TowerButton _towerButtonPressed;

    [SerializeField]
    private LayerMask _layer;

    [Header("Components")]
    [SerializeField]
    private SpriteRenderer _towerIcon;
    [SerializeField]
    private GameManager _gameManager;
    [SerializeField]
    private Collider2D _collider;

    public List<Tower> towersList = new List<Tower>();

    public TowerButton TowerButtonPressed { get => _towerButtonPressed; }

    private void Start() {
        _collider.enabled = false;
    }

    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            cursorMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            _raycastHit = Physics2D.Raycast(cursorMousePosition, Vector2.zero, _layer);

            CheckRaycastAndCallClickOnTower(_raycastHit);
        }

        if (_towerIcon.sprite == true) {
            FollowMouseTowerIcon();
        }
    }

    private void CheckRaycastAndCallClickOnTower(RaycastHit2D raycast) {
        if (raycast.transform != null) {
            //print("raycast " + raycast.transform.name);

            if(raycast.transform.gameObject.TryGetComponent(out PlaceForTower placeForTower)) {
                SetTowerOnPlace(placeForTower);
            }

            if (raycast.collider.CompareTag(Tags.tower) && _towerIcon.enabled == false) {
                DisableOrEnableMenuUpgrageOntower(raycast);
                DisableMenuAnotherTowers();
            }

            else if (!raycast.collider.CompareTag("Menu")) {
                DisableMenuTower();
            }
        }

        else {
            //print("transform NULL");
            DisableMenuTower();
        }
    }

    public void SetTowerOnPlace(PlaceForTower placeForTower) {
        if (_towerButtonPressed != null) {
            //null коли не вибираєш башню і тицяєш на місце для башні
            _collider.enabled = false;
            int price = _towerButtonPressed.TowerScript.Price;
            _gameManager.SubstractCoin(price);
            placeForTower.DisableIlluminationIcon();

            Tower tower = Instantiate(_towerButtonPressed.TowerScript, placeForTower.transform.position, Quaternion.identity);
            tower.Initialization(this, _gameManager);
            tower.SetPlaceForTower(placeForTower);
            placeForTower.DisableBoxCollider();
            DisbleTowerIcon();
            _towerButtonPressed = null;
        }

        else {
            //Debug.Log(hit2D.transform.name);
        }
    }

    private void DisableOrEnableMenuUpgrageOntower(RaycastHit2D raycast) {
        _tower = raycast.transform.GetComponent<Tower>();

        if (_tower.IsActiveUpgradeMenu()) {
            _tower.DisableUpgradeMenu();
            _tower.DisableLineRenderer();
        }
        else if (!_tower.IsActiveUpgradeMenu()) {
            _tower.EnableUpgradeMenu();
            _tower.EnableLineRenderer();
        }
    }

    private void DisableMenuAnotherTowers() {
        for (int i = 0; i < towersList.Count; i++) {
            if (towersList[i] != _tower && _tower != null) {
                towersList[i].DisableLineRenderer();
                towersList[i].DisableUpgradeMenu();
            }
        }
    }

    private void DisableMenuTower() {
        if (_tower != null) {
            _tower.DisableLineRenderer();
            _tower.DisableUpgradeMenu();
            if (_tower.TowerUpgradeMenu.TowerMenuAmountText != null) {
                _tower.TowerUpgradeMenu.TowerMenuAmountText.Destroy();
            }
        }
    }

    public void DisbleTowerIcon() {
        _towerIcon.enabled = false;
    }

    public void SelectedTower(TowerButton towerButton) {
        _towerButtonPressed = towerButton;
        EnableSprite(_towerButtonPressed.Sprite);
        _collider.enabled = true;
        //Debug.Log("tower = " + towerButtonPressed.gameObject.name);
    }

    public void EnableSprite(Sprite sprite) {
        if (_towerIcon != null) {
            _towerIcon.enabled = true;
            _towerIcon.sprite = sprite;
            _towerIcon.sortingOrder = 100;
        }

        else {
            //Debug.LogWarning("The variable spriteRenderer of TowerManager has not been assigned.");
        }
    }

    public void FollowMouseTowerIcon() {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 10);
    }
}
