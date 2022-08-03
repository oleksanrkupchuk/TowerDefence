using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeMenu : MonoBehaviour {
    private GameManager _gameManager;
    private Tower _tower;
    private int _totalMoneyForUpgrade = 0;
    private int _priceUpgrade;
    private int _currentUpgrade = 0;
    private RectTransform _levelUpButtonRect;
    private RectTransform _sellButtonRect;

    [Header("Button")]
    [SerializeField]
    private Button _levelUpButton;
    [SerializeField]
    private Button _sellButton;

    [Header("Parametrs")]
    [SerializeField]
    private float _additionRangeRadius;
    [SerializeField]
    private int _numberOfUpgrades;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private GameObject _range;
    [SerializeField]
    private GameObject _rangeLevelUp;
    [SerializeField]
    private float _timeDestroyTower;
    [SerializeField]
    private Text _priceUpgradeText;

    public int percentSellTower;

    public float AdditionRangeRadius { get => _additionRangeRadius; }

    public void Init(GameManager gameManager, Tower tower, Camera camera) {
        _gameManager = gameManager;
        _tower = tower;
        _canvas.worldCamera = camera;
        _priceUpgrade = _tower.Price * 80 / 100;
        _priceUpgradeText.text = _priceUpgrade.ToString();
        _levelUpButtonRect = _levelUpButton.GetComponent<RectTransform>();
        _sellButtonRect = _sellButton.GetComponent<RectTransform>();
        UpdateButtonPosition();
    }

    private void Start() {
        SubscribleButtonOnEvent();
        gameObject.SetActive(false);
        _range.transform.localScale = new Vector2(_tower.RangeAttack * 2, _tower.RangeAttack * 2);
    }

    public void SubscribleButtonOnEvent() {
        _levelUpButton.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.SellTower);
            TowerLevelUp();
        });
        _sellButton.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.SellTower);
            SellTower();
        });
    }

    private void TowerLevelUp() {
        if (_gameManager.Coins >= _priceUpgrade) {
            SoundManager.Instance.PlaySoundEffect(SoundName.TowerUpgrade);
            _totalMoneyForUpgrade += _priceUpgrade;
            _priceUpgrade += _priceUpgrade * 30 / 100;
            _priceUpgradeText.text = _priceUpgrade.ToString();
            _gameManager.SubstractCoin(_priceUpgrade);
            _tower.IncreaseDamage();
            _tower.IncreaseRange(_additionRangeRadius);

            _range.transform.localScale = new Vector2(_tower.RangeAttack * 2, _tower.RangeAttack * 2);
            _rangeLevelUp.transform.localScale = new Vector2((_tower.RangeAttack * 2) + (_additionRangeRadius * 2), (_tower.RangeAttack * 2) + (_additionRangeRadius * 2));

            UpdateButtonPosition();
            CheckOnDeactivateButton();
        }
    }

    private void UpdateButtonPosition() {
        _levelUpButtonRect.anchoredPosition = new Vector2(0f, _tower.RangeAttack);
        _sellButtonRect.anchoredPosition = new Vector2(0f, -_tower.RangeAttack);
    }

    private void CheckOnDeactivateButton() {
        _currentUpgrade++;
        if (_currentUpgrade == _numberOfUpgrades) {
            _levelUpButton.gameObject.SetActive(false);
        }
    }

    private void SellTower() {
        int price = ((_tower.Price * percentSellTower) / 100) + _totalMoneyForUpgrade;
        _gameManager.AddCoin(price);
        _tower.PlaceForTower.EnableBoxCollider();
        _tower.RemoveTowerFromList();
        _tower.DestroyTower(_timeDestroyTower);
    }
}
