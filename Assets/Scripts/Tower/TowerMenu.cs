using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerMenu : MonoBehaviour
{
    [SerializeField]
    private Button _buttonIncreaseDamage;
    [SerializeField]
    private Button _buttonSell;
    [SerializeField]
    private Button _buttonIncreaseRange;

    [SerializeField]
    private Tower _tower;

    [SerializeField]
    private float _additionRangeRadius;
    [SerializeField]
    private int _numberOfImprovements;

    private readonly string _increaseDamage = "increaseDamage";
    private readonly string _sell = "sell";
    private readonly string _increaseRange = "increaseRange";

    public Dictionary<string, int> ability = new Dictionary<string, int>();

    [SerializeField]
    private Image _imageIncreaseDamage;
    [SerializeField]
    private Image _imageSell;
    [SerializeField]
    private Image _imageIncreaseRange;

    private void Awake() {
        InitializationAbility();
    }

    private void InitializationAbility() {
        ability[_increaseDamage] = 0;
        ability[_sell] = 0;
        ability[_increaseRange] = 0;
    }

    private void Start()
    {
        InitializationButtonUpgrade();
    }

    public void EnableTowerUpgradeIcon() {
        _imageIncreaseDamage.enabled = !_imageIncreaseDamage.enabled;
        _imageSell.enabled = !_imageSell.enabled;
        _imageIncreaseRange.enabled = !_imageIncreaseRange.enabled;
    }

    private void InitializationButtonUpgrade() {
        _buttonIncreaseDamage.onClick.AddListener(() => IncreaseDamage());
        _buttonIncreaseRange.onClick.AddListener(() => IncreaseRange());
        _buttonSell.onClick.AddListener(() => SellTower());
    }

    private void IncreaseRange() {
        if(ability[_increaseDamage] < _numberOfImprovements) {
            _tower.IncreaseRange(_additionRangeRadius);
            ability[_increaseDamage] += 1;
        }
        else {
            print("max number upgrade");
        }
    }

    private void IncreaseDamage() {
        if (ability[_increaseDamage] < _numberOfImprovements) {
            _tower.IncreaseDamage();
            ability[_increaseDamage] += 1;
        }
        else {
            print("max number upgrade");
        }
    }

    private void SellTower() {
        int price = _tower.Price / 2;
        GameManager.Instance.AddCoin(price);
        GameManager.Instance.UpdateCoin();
        _tower.placeForTower.gameObject.tag = Tags.placeForTower;
        Destroy(_tower.gameObject);
    }
}
