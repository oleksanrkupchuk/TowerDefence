using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    private int _countTower = 0;
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
    private Collider2D _collider;

    public List<Tower> towersList = new List<Tower>();
    public GameManager gameManager;
    public Camera mainCamera;

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
            print("raycast " + raycast.transform.name);

            if (raycast.transform.gameObject.TryGetComponent(out PlaceForTower placeForTower)) {
                SetTowerOnPlace(placeForTower);
            }

            if (raycast.collider.CompareTag(Tags.tower) && _towerIcon.enabled == false) {
                DisableOrEnableMenuUpgrageOntower(raycast);
                DisableMenuAnotherTowers();
            }

            else if (raycast.collider.CompareTag(Tags.tower) && _towerIcon.enabled == true) {
                SoundManager.Instance.PlaySoundEffect(SoundName.ErrorSetTower);
            }

            else if (!raycast.collider.CompareTag("Menu")) {
                DisableMenuTower();
                _towerButtonPressed = null;
                _collider.enabled = false;
                DisbleTowerIcon();
            }
        }

        else {
            //print("transform NULL");
            _towerButtonPressed = null;
            DisbleTowerIcon();
            DisableMenuTower();
        }
    }

    public void SetTowerOnPlace(PlaceForTower placeForTower) {
        if(_towerButtonPressed == null) {
            return;
        }

        //null коли не вибираєш башню і тицяєш на місце для башні
        _collider.enabled = false;
        int price = _towerButtonPressed.Tower.Price;
        gameManager.SubstractCoin(price);
        placeForTower.DisableIlluminationIcon();

        Tower tower = Instantiate(_towerButtonPressed.Tower, placeForTower.transform.position, Quaternion.identity);
        tower.Init(this, gameManager, mainCamera);
        tower.name = tower.name + " " + _countTower;
        _countTower++;
        tower.SetPlaceForTower(placeForTower);
        placeForTower.DisableBoxCollider();
        DisbleTowerIcon();
        _towerButtonPressed = null;
        _collider.enabled = false;
    }

    private void DisableOrEnableMenuUpgrageOntower(RaycastHit2D raycast) {
        _tower = raycast.transform.GetComponent<Tower>();

        if (_tower.IsActiveUpgradeMenu()) {
            _tower.DisableUpgradeMenu();
        }
        else if (!_tower.IsActiveUpgradeMenu()) {
            _tower.EnableUpgradeMenu();
        }
    }

    private void DisableMenuAnotherTowers() {
        for (int i = 0; i < towersList.Count; i++) {
            if (towersList[i] != _tower && _tower != null) {
                towersList[i].DisableUpgradeMenu();
            }
        }
    }

    private void DisableMenuTower() {
        if (_tower != null) {
            _tower.DisableUpgradeMenu();
        }
    }

    public void DisbleTowerIcon() {
        _towerIcon.enabled = false;
    }

    public void SetSelectedTower(TowerButton towerButton) {
        _towerButtonPressed = towerButton;
        EnableSprite(_towerButtonPressed.Sprite);
        _collider.enabled = true;
        //Debug.Log("tower = " + towerButtonPressed.gameObject.name);
    }

    public void EnableSprite(Sprite sprite) {
        if (_towerIcon != null) {
            _towerIcon.enabled = true;
            _towerIcon.sprite = sprite;
            _towerIcon.sortingOrder = 201;
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
