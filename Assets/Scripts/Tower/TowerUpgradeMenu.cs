using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeMenu : MonoBehaviour {
    private GameManager _gameManager;
    private Tower _tower;
    private readonly string _increaseDamage = "increaseDamage";
    private readonly string _sell = "sell";
    private readonly string _increaseRange = "increaseRange";
    private TowerMenuAmountText _towerMenuAmountText = null;
    private Dictionary<string, int> _ability = new Dictionary<string, int>();

    [Header("Button")]
    [SerializeField]
    private Button _buttonIncreaseDamage;
    [SerializeField]
    private Button _buttonSell;
    [SerializeField]
    private Button _buttonIncreaseRange;

    [SerializeField]
    private float _additionRangeRadius;
    [SerializeField]
    private int _numberOfImprovements;
    [SerializeField]
    private GameObject _amountText;
    [SerializeField]
    private Transform _canvas;
    [SerializeField]
    private Bullet _bullet;

    [Header("Game Object Upgrade")]
    [SerializeField]
    private GameObject _increaseDamageObject;
    [SerializeField]
    private GameObject _sellObject;
    [SerializeField]
    private GameObject _increaseRangeObject;

    [Header("Parametrs")]
    [SerializeField]
    private float _timeDestroyTower;

    [Header("Scripts")]
    [SerializeField]
    private UpgradeTower _damageUpgradeScript;
    [SerializeField]
    private UpgradeTower _rangeUpgradeScript;

    public TowerMenuAmountText TowerMenuAmountText { get => _towerMenuAmountText; }

    public void Initialization(GameManager gameManager, Tower tower) {
        _gameManager = gameManager;
        _tower = tower;
    }

    private void Awake() {
        InitializationAbility();
    }

    private void InitializationAbility() {
        _ability[_increaseDamage] = 0;
        _ability[_sell] = 0;
        _ability[_increaseRange] = 0;
    }

    public void EnableTowerUpgradeIcon(bool isActive) {
        _increaseDamageObject.SetActive(isActive);
        _sellObject.SetActive(isActive);
        _increaseRangeObject.SetActive(isActive);
    }

    private void Start() {
        SubscribleButtonOnEvent();
    }

    public void SubscribleButtonOnEvent() {
        _buttonIncreaseDamage.onClick.AddListener(() => { IncreaseDamage(); });
        _buttonIncreaseRange.onClick.AddListener(() => { IncreaseRange(); });
        _buttonSell.onClick.AddListener(() => { SellTower(); });
    }

    private void IncreaseDamage() {
        if (_gameManager.Coin >= _damageUpgradeScript.Price) {
            //print("click button damage");
            _gameManager.SubstractCoin(_damageUpgradeScript.Price);
            _damageUpgradeScript.IncreasePrice();
            _tower.IncreaseDamage();
            _ability[_increaseDamage] += 1;
            float amount = _tower.Damage;
            SpawnTextAmount(amount);
            CheckOnDeactivateButton(_damageUpgradeScript, _buttonIncreaseDamage, _increaseDamage);
        }
    }

    private void IncreaseRange() {
        if (_gameManager.Coin >= _rangeUpgradeScript.Price) {
            //print("click button range");
            _gameManager.SubstractCoin(_rangeUpgradeScript.Price);
            _rangeUpgradeScript.IncreasePrice();
            _tower.IncreaseRange(_additionRangeRadius);
            _ability[_increaseRange] += 1;
            float amount = _tower.RangeAttack;
            SpawnTextAmount(amount);
            CheckOnDeactivateButton(_rangeUpgradeScript, _buttonIncreaseRange, _increaseRange);
        }
    }

    private void SellTower() {
        int price = _tower.Price / 2;
        _gameManager.AddCoin(price);
        _tower.EnableColliderOnPlaceForTower();
        SpawnTextAmount(price);
        _tower.RemoveTowerFromList();
        _tower.DestroyTower(_timeDestroyTower);
    }

    private void SpawnTextAmount(float amount) {
        if(_towerMenuAmountText != null) {
            Destroy(_towerMenuAmountText.gameObject);
        }
        GameObject amountObject = Instantiate(_amountText);
        amountObject.transform.SetParent(_canvas.transform);
        TowerMenuAmountText towerMenuAmountText = amountObject.GetComponent<TowerMenuAmountText>();
        _towerMenuAmountText = towerMenuAmountText;
        towerMenuAmountText.InitStartPosition(_tower.transform);
        towerMenuAmountText.SetTextAmount(amount);
    }

    private void CheckOnDeactivateButton(UpgradeTower upgrate, Button button, string nameAbility) {
        if (_ability[nameAbility] >= _numberOfImprovements) {
            upgrate.NotIntractable();
            button.interactable = false;
            //print("max number upgrade");
        }
    }
}
