using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeMenu : MonoBehaviour {
    private GameManager _gameManager;
    private Tower _tower;
    private int _totalMoneyForUpgrade = 0;

    [Header("Button")]
    [SerializeField]
    private Button _buttonSell;

    [SerializeField]
    private float _additionRangeRadius;
    [SerializeField]
    private int _numberOfUpgrades;
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

    public void Initialization(GameManager gameManager, Tower tower) {
        _gameManager = gameManager;
        _tower = tower;
    }

    private void Start() {
        SubscribleButtonOnEvent();
        _amountText.Init();
        gameObject.SetActive(false);
    }

    public void SubscribleButtonOnEvent() {
        _damageUpgrade.Button.onClick.AddListener(() => {
            IncreaseDamage(); 
        });
        _rangeUpgrade.Button.onClick.AddListener(() => {
            IncreaseRange(); 
        });
        _buttonSell.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.SellTower);
            SellTower(); 
        });
    }

    private void IncreaseDamage() {
        if (_gameManager.Coin >= _damageUpgrade.Price) {
            SoundManager.Instance.PlaySoundEffect(SoundName.TowerUpgrade);
            _totalMoneyForUpgrade += _damageUpgrade.Price;
            _gameManager.SubstractCoin(_damageUpgrade.Price);
            _damageUpgrade.IncreasePrice();
            _tower.IncreaseDamage();
            SetValueInTextObject(_tower.increaseDamage, false);
            CheckOnDeactivateButton(_damageUpgrade, _damageUpgrade.Button);
        }
    }

    private void IncreaseRange() {
        if (_gameManager.Coin >= _rangeUpgrade.Price) {
            SoundManager.Instance.PlaySoundEffect(SoundName.TowerUpgrade);
            _totalMoneyForUpgrade += _rangeUpgrade.Price;
            _gameManager.SubstractCoin(_rangeUpgrade.Price);
            _rangeUpgrade.IncreasePrice();
            _tower.IncreaseRange(_additionRangeRadius);
            SetValueInTextObject(_additionRangeRadius, false);
            CheckOnDeactivateButton(_rangeUpgrade, _rangeUpgrade.Button);
        }
    }

    private void SellTower() {
        int price = ((_tower.Price * percentSellTower) / 100) + _totalMoneyForUpgrade;
        _gameManager.AddCoin(price);
        _tower.PlaceForTower.EnableBoxCollider();
        SetValueInTextObject(price, true);
        _tower.RemoveTowerFromList();
        _tower.DestroyTower(_timeDestroyTower);
    }

    private void SetValueInTextObject(float value, bool enableCoinIcon) {
        _amountText.gameObject.SetActive(true);
        _amountText.StopMove();
        _amountText.SetStartPosition();
        _amountText.SetTextAmount(value, enableCoinIcon);
        _amountText.MoveText();
    }

    private void CheckOnDeactivateButton(UpgradeTower upgrate, Button button) {
        upgrate.improvement++;
        if (upgrate.improvement >= _numberOfUpgrades) {
            upgrate.DisablePrice();
            button.interactable = false;
            //print("max number upgrade");
        }
    }

    public void DisableTextAmountObject() {
        _amountText.gameObject.SetActive(false);
    }
}
