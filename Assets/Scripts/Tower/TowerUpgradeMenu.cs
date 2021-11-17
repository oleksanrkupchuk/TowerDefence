using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TowerUpgradeMenu : MonoBehaviour {
    private readonly string _increaseDamage = "increaseDamage";
    private readonly string _sell = "sell";
    private readonly string _increaseRange = "increaseRange";

    public Dictionary<string, int> ability = new Dictionary<string, int>();

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

    [Header("Game Object Upgrade")]
    [SerializeField]
    private GameObject _increaseDamageObject;
    [SerializeField]
    private GameObject _sellObject;
    [SerializeField]
    private GameObject _increaseRangeObject;

    [Header("Text")]
    [SerializeField]
    private TextMeshProUGUI _textMeshProValueUpgrade;
    [SerializeField]
    private GameObject _textGameObject;

    [Header("Parametrs")]
    [SerializeField]
    private float _distance;
    [SerializeField]
    private float _time;
    private Vector3 _textDeafaultPosition;
    [SerializeField]
    private float _waitTime;

    [Header("Scripts")]
    [SerializeField]
    private Upgrade _damageUpgradeScript;
    [SerializeField]
    private Upgrade _rangeUpgradeScript;

    private void Awake() {
        InitializationAbility();
        _textDeafaultPosition = _textGameObject.transform.position;
    }

    private void InitializationAbility() {
        ability[_increaseDamage] = 0;
        ability[_sell] = 0;
        ability[_increaseRange] = 0;
    }

    public void EnableTowerUpgradeIcon(bool isActive) {
        _increaseDamageObject.SetActive(isActive);
        _sellObject.SetActive(isActive);
        _increaseRangeObject.SetActive(isActive);
    }

    public void SubscribleButtonOnEvent(Tower tower) {
        _buttonIncreaseDamage.onClick.AddListener(() => { IncreaseDamage(tower); });
        _buttonIncreaseRange.onClick.AddListener(() => { IncreaseRange(tower); });
        _buttonSell.onClick.AddListener(() => { SellTower(tower); });
    }

    private void IncreaseDamage(Tower tower) {
        if (GameManager.Instance.Coin >= _damageUpgradeScript.Price) {
            GameManager.Instance.SubstractCoin(_damageUpgradeScript.Price);
            _damageUpgradeScript.IncreasePrice();
            tower.IncreaseDamage();
            ability[_increaseDamage] += 1;
            float amount = tower.Bullet.Damage;
            EnableTextAndStartAnimation(amount);
            CheckOnDeactivateButton(_damageUpgradeScript, _buttonIncreaseDamage, _increaseDamage);
        }
    }

    private void IncreaseRange(Tower tower) {
        if (GameManager.Instance.Coin >= _rangeUpgradeScript.Price) {
            GameManager.Instance.SubstractCoin(_rangeUpgradeScript.Price);
            _rangeUpgradeScript.IncreasePrice();
            tower.IncreaseRange(_additionRangeRadius);
            ability[_increaseRange] += 1;
            float amount = tower.RangeAttack;
            EnableTextAndStartAnimation(amount);
            CheckOnDeactivateButton(_rangeUpgradeScript, _buttonIncreaseRange, _increaseRange);
        }
    }

    private void SellTower(Tower tower) {
        int price = tower.Price / 2;
        GameManager.Instance.AddCoin(price);
        GameManager.Instance.UpdateAmountCoin();
        tower.placeForTower.gameObject.tag = Tags.placeForTower;
        tower.placeForTower.gameObject.GetComponent<Collider2D>().enabled = true;
        EnableTextAndStartAnimation(price);
        TowerManager.Instance.towersList.Remove(tower);
        Destroy(tower.gameObject, _waitTime);
    }

    private void EnableTextAndStartAnimation(float amount) {
        StartCoroutine(ShowTextValue(amount, _waitTime));
        StartCoroutine(MoveText());
    }

    private IEnumerator ShowTextValue(float amount, float waitTime) {
        _textMeshProValueUpgrade.enabled = true;
        _textMeshProValueUpgrade.text = "" + amount;
        yield return new WaitForSeconds(waitTime);
    }

    private IEnumerator MoveText() {
        //print("text default = " + _textGameObject.transform.localPosition.y);
        //print("text distance = " + _textGameObject.transform.localPosition.y + _distance);
        LeanTween.moveLocalY(_textGameObject, _textGameObject.transform.localPosition.y + _distance, _time);
        yield return new WaitForSeconds(_time);
        _textGameObject.transform.position = _textDeafaultPosition;
        _textMeshProValueUpgrade.enabled = false;
    }

    private void CheckOnDeactivateButton(Upgrade upgrate, Button button, string nameAbility) {
        if (ability[nameAbility] >= _numberOfImprovements) {
            upgrate.DisableChildObject();
            button.interactable = false;
            print("max number upgrade");
        }
    }
}
