using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeMenu : MonoBehaviour {
    private GameManager _gameManager;
    private Tower _tower;
    private TowerMenuAmountText _towerMenuAmountTextBuffer = null;

    [Header("Button")]
    [SerializeField]
    private Button _buttonSell;

    [SerializeField]
    private float _additionRangeRadius;
    [SerializeField]
    private int _numberOfImprovements;
    [SerializeField]
    private TowerMenuAmountText _amountText;
    [SerializeField]
    private Transform _canvas;

    [Header("Parametrs")]
    [SerializeField]
    private float _timeDestroyTower;

    [Header("Scripts")]
    [SerializeField]
    private UpgradeTower _damageUpgrade;
    [SerializeField]
    private UpgradeTower _rangeUpgrade;
    [SerializeField]
    private GameObject _sellObject;

    public int percentSellTower;
    public TowerMenuAmountText TowerMenuAmountText { get => _towerMenuAmountTextBuffer; }

    public void Initialization(GameManager gameManager, Tower tower) {
        _gameManager = gameManager;
        _tower = tower;
    }

    private void Start() {
        SubscribleButtonOnEvent();
    }

    public void SubscribleButtonOnEvent() {
        _damageUpgrade.Button.onClick.AddListener(() => { IncreaseDamage(); });
        _rangeUpgrade.Button.onClick.AddListener(() => { IncreaseRange(); });
        _buttonSell.onClick.AddListener(() => { SellTower(); });
    }

    private void IncreaseDamage() {
        if (_gameManager.Coin >= _damageUpgrade.Price) {
            //print("click button damage");
            _gameManager.SubstractCoin(_damageUpgrade.Price);
            _damageUpgrade.IncreasePrice();
            _tower.IncreaseDamage();
            float amount = _tower.Damage;
            SpawnTextAmount(amount);
            CheckOnDeactivateButton(_damageUpgrade, _damageUpgrade.Button);
        }
    }

    private void IncreaseRange() {
        if (_gameManager.Coin >= _rangeUpgrade.Price) {
            //print("click button range");
            _gameManager.SubstractCoin(_rangeUpgrade.Price);
            _rangeUpgrade.IncreasePrice();
            _tower.IncreaseRange(_additionRangeRadius);
            float amount = _tower.RangeAttack;
            SpawnTextAmount(amount);
            CheckOnDeactivateButton(_rangeUpgrade, _rangeUpgrade.Button);
        }
    }

    private void SellTower() {
        int price = (_tower.Price * percentSellTower) / 100;
        _gameManager.AddCoin(price);
        _tower.PlaceForTower.EnableBoxCollider();
        SpawnTextAmount(price);
        _tower.RemoveTowerFromList();
        _tower.DestroyTower(_timeDestroyTower);
    }

    private void SpawnTextAmount(float amount) {
        if(_towerMenuAmountTextBuffer != null) {
            Destroy(_towerMenuAmountTextBuffer.gameObject);
        }
        TowerMenuAmountText amountText = Instantiate(_amountText);
        amountText.transform.SetParent(_canvas.transform);
        _towerMenuAmountTextBuffer = amountText;
        amountText.InitStartPosition(_tower.transform);
        amountText.SetTextAmount(amount);
    }

    private void CheckOnDeactivateButton(UpgradeTower upgrate, Button button) {
        upgrate.improvement++;
        if (upgrate.improvement >= _numberOfImprovements) {
            upgrate.DisablePrice();
            button.interactable = false;
            //print("max number upgrade");
        }
    }
}
