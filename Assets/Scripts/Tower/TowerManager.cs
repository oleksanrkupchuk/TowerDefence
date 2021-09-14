using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : Loader<TowerManager> {
    private Vector2 cursorMousePosition;
    private RaycastHit2D _raycastHitGetPlaceForTower;
    private RaycastHit2D _raycastHitGetTower;
    [SerializeField]
    private LayerMask _layerPlaceForTower;
    [SerializeField]
    private LayerMask _towerLayer;

    [Header("Components")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private TowerButton _towerButtonPressed;

    [SerializeField]
    private bool putTower = false;

    [Header("Colors")]
    [SerializeField]
    private Color _colorAlpha;
    [SerializeField]
    private Color _colorDeafault;

    public List<Tower> _towers = new List<Tower>();
    private Tower _tower;
    private TowerMenu _towerMenu;

    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            cursorMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            _raycastHitGetPlaceForTower = Physics2D.Raycast(cursorMousePosition, Vector2.zero, 100f, _layerPlaceForTower);
            _raycastHitGetTower = Physics2D.Raycast(cursorMousePosition, Vector2.zero, 100f, _towerLayer);


            CheckRaycastAndCallClickOnTower();

            if (_raycastHitGetPlaceForTower.transform != null) {
                PlaceTower(_raycastHitGetPlaceForTower);
            }
        }

        if (Input.GetButtonDown("Fire2")) {
            DisbleSprite();
        }

        if (spriteRenderer.sprite == true) {
            FollowMouseTowerIcon();
        }
    }

    private void CheckRaycastAndCallClickOnTower() {
        if (_raycastHitGetTower.transform != null) {
            ClickOnTower(_raycastHitGetTower);
        }
        else {
            for (int i = 0; i < _towers.Count; i++) {
                _towers[i].EnableLineRenderer(false);
                _towers[i].EnableTowerMenu(false);
            }
        }
    }

    private void ClickOnTower(RaycastHit2D raycast) {
        if (raycast.transform.CompareTag(Tags.tower)) {
            Debug.Log(raycast.transform.name);
            _tower = raycast.transform.GetComponent<Tower>();

            //_tower.EnableLineRenderer();
            //_tower.EnableTowerMenu();

            _tower.EnableLineRenderer(true);
            _tower.EnableTowerMenu(true);

            for (int i = 0; i < _towers.Count; i++) {
                if (_towers[i] != _tower) {
                    _towers[i].EnableLineRenderer(false);
                    _towers[i].EnableTowerMenu(false);
                }
            }
        }
    }

    /// <summary>
    /// Перевірка розміщення вежі, і встановлення нового тегу для неможливості повторного розміщення вежі на одному місці
    /// </summary>
    /// <param name="hit2D"></param>
    public void PlaceTower(RaycastHit2D hit2D) {
        if (hit2D.transform.gameObject.CompareTag(Tags.placePositionForTower) && !putTower) {
            Debug.Log("tags = " + hit2D.transform.name);
            GameManager.Instance.SubstractCoin(_towerButtonPressed.TowerObject.GetComponent<Tower>().Price);
            putTower = true;
            hit2D.transform.gameObject.tag = Tags.placeForTowerFull;
            spriteRenderer.color = _colorDeafault;
            Instantiate(_towerButtonPressed.TowerObject, _raycastHitGetPlaceForTower.transform.position, Quaternion.identity);
            DisbleSprite();
        }

        else {
            Debug.Log(hit2D.transform.name);
        }
    }

    /// <summary>
    /// Відключення спрайту вежі
    /// </summary>
    public void DisbleSprite() {
        spriteRenderer.enabled = false;
    }

    public void CheckMoney(TowerButton towerButton) {
        if (GameManager.Instance.Coin >= towerButton.TowerObject.GetComponent<Tower>().Price) {
            SelectedTower(towerButton);
        }
        else {
            print("Not enough money");
        }
    }

    /// <summary>
    /// Вибір вежі при натиснені на кнопку
    /// </summary>
    /// <param name="towerButton"></param>
    public void SelectedTower(TowerButton towerButton) {
        _towerButtonPressed = towerButton;
        spriteRenderer.color = _colorAlpha;
        EnableSprite(_towerButtonPressed.TowerSprite);
        putTower = false;
        //Debug.Log("tower = " + towerButtonPressed.gameObject.name);
    }

    /// <summary>
    /// Включення спрайту вежі
    /// </summary>
    /// <param name="sprite"></param>
    public void EnableSprite(Sprite sprite) {
        if (spriteRenderer != null) {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = 100;
        }

        else {
            //Debug.LogWarning("The variable spriteRenderer of TowerManager has not been assigned.");
        }
    }

    //Слідкування спрайта вежі за мишкою. Потрібно вказувати вісь z тому що по стандарту вона має значення -10 і таким спрайт відображатиметься за картою
    public void FollowMouseTowerIcon() {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 10);
    }
}
