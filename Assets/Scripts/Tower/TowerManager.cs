using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour {
    private Vector2 cursorMousePosition;
    private RaycastHit2D _raycastHit;
    [SerializeField]
    private LayerMask _layer;
    [SerializeField]
    private LayerMask _towerLayer;

    [Header("Components")]
    [SerializeField]
    private SpriteRenderer _towerIcon;
    [SerializeField]
    private GameManager _gameManager;
    private TowerButton _towerButtonPressed;

    [SerializeField]
    private bool putTower = true;

    [Header("Colors")]
    [SerializeField]
    private Color _colorAlpha;
    [SerializeField]
    private Color _colorDeafault;

    public List<Tower> towersList = new List<Tower>();
    private Tower _tower;

    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            cursorMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            _raycastHit = Physics2D.Raycast(cursorMousePosition, Vector2.zero, _layer);

            CheckRaycastAndCallClickOnTower(_raycastHit);
        }

        if (Input.GetButtonDown("Fire2")) {
            DisbleTowerIcon();
        }

        if (_towerIcon.sprite == true) {
            FollowMouseTowerIcon();
        }
    }

    private void CheckRaycastAndCallClickOnTower(RaycastHit2D raycast) {
        //print("Name oject = " + raycast.transform.name);
        //print("Name oject = " + raycast.collider.name);

        if (raycast.transform != null) {
            //print("raycast " + raycast.transform.name);

            if (raycast.collider.CompareTag(Tags.placeForTower)) {
                SetTowerOnPlace(raycast.transform);
            }

            if (raycast.collider.CompareTag(Tags.tower) && _towerIcon.enabled == false) {
                DisableOrEnableMenuUpgrageOntower(raycast);
                DisableMenuAnotherTowers();
            }

            else if (!raycast.collider.CompareTag(Tags.tower) && !raycast.collider.CompareTag("Menu")) {
                DisableMenuTower();
            }
        }

        else {
            DisableMenuTower();
        }
    }

    public void SetTowerOnPlace(Transform placeTower) {
        if (!putTower) {
            //null коли не вибираєш башню і тицяєш на місце для башні
            putTower = true;
            int price = _towerButtonPressed.TowerObject.GetComponent<Tower>().Price;
            _gameManager.SubstractCoin(price);
            _towerIcon.color = _colorDeafault;
            GameObject tower = Instantiate(_towerButtonPressed.TowerObject, placeTower.position, Quaternion.identity);
            Tower _tower = tower.GetComponent<Tower>();
            _tower.Initialization(this, _gameManager);
            _tower.SetPlaceForTower(placeTower.gameObject);
            _tower.DisableColliderOnPlaceForTower();
            DisbleTowerIcon();
        }

        else {
            //Debug.Log(hit2D.transform.name);
        }
    }

    private void DisableOrEnableMenuUpgrageOntower(RaycastHit2D raycast) {
        _tower = raycast.transform.GetComponent<Tower>();

        if (_tower.IsActiveCanvas()) {
            _tower.DisableCanvas();
            _tower.DisableLineRenderer();
        }
        else if(!_tower.IsActiveCanvas()) {
            _tower.EnableCanvas();
            _tower.EnableLineRenderer();
        }
    }

    private void DisableMenuAnotherTowers() {
        for (int i = 0; i < towersList.Count; i++) {
            if (towersList[i] != _tower && _tower != null) {
                towersList[i].DisableLineRenderer();
                towersList[i].DisableCanvas();
            }
        }
    }

    private void DisableMenuTower() {
        if (_tower != null) {
            _tower.DisableLineRenderer();
            _tower.DisableCanvas();
        }
    }

    public void DisbleTowerIcon() {
        _towerIcon.enabled = false;
    }

    public void SubtractMoney(TowerButton towerButton) {
        if (_gameManager.Coin >= towerButton.TowerObject.GetComponent<Tower>().Price) {
            SelectedTower(towerButton);
        }
        else {
            print("Not enough money");
        }
    }

    public void SelectedTower(TowerButton towerButton) {
        _towerButtonPressed = towerButton;
        _towerIcon.color = _colorAlpha;
        EnableSprite(_towerButtonPressed.TowerSprite);
        putTower = false;
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

    public void PutTowers() {
        putTower = false;
    }
}
