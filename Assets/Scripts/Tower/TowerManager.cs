using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : Loader<TowerManager> {
    private Vector2 cursorMousePosition;
    private RaycastHit2D _raycastHit;
    [SerializeField]
    private LayerMask _layerPlaceForTower;
    [SerializeField]
    private LayerMask _towerLayer;
    [SerializeField]
    private LayerMask _ground;

    [Header("Components")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;
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
    private TowerUpgradeMenu _towerMenu;

    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            cursorMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            _raycastHit = Physics2D.Raycast(cursorMousePosition, Vector2.zero);

            CheckRaycastAndCallClickOnTower(_raycastHit);
        }

        if (Input.GetButtonDown("Fire2")) {
            DisbleTowerIcon();
        }

        if (spriteRenderer.sprite == true) {
            FollowMouseTowerIcon();
        }
    }

    private void CheckRaycastAndCallClickOnTower(RaycastHit2D raycast) {
        if (raycast.transform != null) {
            print("raycast " + raycast.transform.name);
            if (raycast.collider.CompareTag(Tags.placeForTower)) {
                PlaceTower(raycast);
            }

            else if (raycast.collider.CompareTag(Tags.tower)) {
                ClickOnTower(raycast);
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

    /// <summary>
    /// Перевірка розміщення вежі, і встановлення нового тегу для неможливості повторного розміщення вежі на одному місці
    /// </summary>
    /// <param name="raycast"></param>
    public void PlaceTower(RaycastHit2D raycast) {
        if (!putTower) {
            //null коли не вибираєш башню і тицяєш на місце для башні
            GameManager.Instance.SubstractCoin(_towerButtonPressed.TowerObject.GetComponent<Tower>().Price);
            putTower = true;
            raycast.transform.gameObject.tag = Tags.placeForTowerFull;
            raycast.transform.gameObject.GetComponent<Collider2D>().enabled = false;
            spriteRenderer.color = _colorDeafault;
            GameObject tower = Instantiate(_towerButtonPressed.TowerObject, raycast.transform.position, Quaternion.identity);
            tower.GetComponent<Tower>().placeForTower = raycast.transform.gameObject;
            DisbleTowerIcon();
        }

        else {
            //Debug.Log(hit2D.transform.name);
        }
    }

    private void ClickOnTower(RaycastHit2D raycast) {
        _tower = raycast.transform.GetComponent<Tower>();

        _tower.EnableLineRenderer();
        _tower.EnableTowerUpgradeIcon();
    }

    private void DisableMenuAnotherTowers() {
        for (int i = 0; i < towersList.Count; i++) {
            if (towersList[i] != _tower && _tower != null) {
                towersList[i].DisableLineRenderer();
                towersList[i].DisableTowerUpgradeIcon();
            }
        }
    }

    private void DisableMenuTower() {
        if(_tower != null) {
            _tower.DisableTowerUpgradeIcon();
            _tower.DisableLineRenderer();
        }
    }

    /// <summary>
    /// Відключення спрайту вежі
    /// </summary>
    public void DisbleTowerIcon() {
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

    public void PutTower() {
        putTower = false;
    }
}
