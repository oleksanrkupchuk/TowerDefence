using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ShopMenu : BaseMenu {
    private Ability _currentAbility;
    private int _currentCountStars;
    private bool[] _abilityPurchased = new bool[7];
    private List<Ability> _ability = new List<Ability>();

    [Header("Buttons Lose Menu")]
    [SerializeField]
    private Button _back;
    [SerializeField]
    private Button _buy;
    [SerializeField]
    private Button _yes;
    [SerializeField]
    private Button _no;
    [SerializeField]
    private Button _ok;

    [Header("Windows")]
    [SerializeField]
    private GameObject _confirmBuyAbilityWindow;
    [SerializeField]
    private GameObject _notEnoughMoneyWindow;

    [SerializeField]
    private GameObject _abilityPrefab;
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private Image _detailAbilityIcon;
    [SerializeField]
    private TextMeshProUGUI _detailAbilityDescription;
    [SerializeField]
    private TextMeshProUGUI _starsText;
    [SerializeField]
    private List<AbilityData> _abilityData = new List<AbilityData>();

    private void Start() {
        LoadStars();
        DisableConfirmBuyAbilityWindow();
        DisablEnotEnoughMoneyWindow();
        SubscriptionButtons();
        LoadAbility();
        SpawnAbility();

        _detailAbilityIcon.sprite = _abilityData[0].icon;
        _detailAbilityDescription.text = _abilityData[0].description;

        if (_currentAbility.Data.isPurchased) {
            DisableBuyButton();
        }
    }

    private void LoadStars() {
        if (SaveSystemStars.IsExistsSaveStarsFile()) {
            StarsData _starsData = SaveSystemStars.LoadStars();
            _currentCountStars = _starsData.stars;
            _starsText.text = "" + _currentCountStars;
        }
    }

    private void LoadAbility() {
        print("fire exist = " + AbilitySaveSystem.IsExistsSaveAbilityFile());
        if (AbilitySaveSystem.IsExistsSaveAbilityFile()) {
            AbilityPurchase _abilityPurchase = AbilitySaveSystem.LoadAbility();
            _abilityPurchased = _abilityPurchase.isPurchased;
            for (int i = 0; i < _abilityPurchased.Length; i++) {
                print($"isPurchase {i} =" + _abilityPurchased[i]);
            }
        }
    }

    private void SpawnAbility() {
        for (int index = 0; index < _abilityData.Count; index++) {
            GameObject _abilityObject = Instantiate(_abilityPrefab);
            _abilityObject.transform.SetParent(_content);
            _abilityObject.transform.localScale = new Vector3(1f, 1f, 1f);

            _abilityData[index].isPurchased = _abilityPurchased[index];
            _abilityData[index].index = index;

            Ability _abilityScript = _abilityObject.GetComponent<Ability>();
            _abilityScript.InitializationAbility(_abilityData[index], this);
            _ability.Add(_abilityScript);
        }

        _currentAbility = _ability[0];
    }

    private void SubscriptionButtons() {
        _back.onClick.AddListener(() => {
            DisableAndEnableGameObject(ThisGameObject, enableObject);
        });

        _buy.onClick.AddListener(() => {
            CheckMoneyAndEnableAbilityOrNotEnoughMoneyWindow();
        });

        _yes.onClick.AddListener(() => {
            SubstractPrice();
            SaveStars();
            SavePurchaseAbility();
            DisableConfirmBuyAbilityWindow();
        });

        _no.onClick.AddListener(() => {
            DisableConfirmBuyAbilityWindow();
        });

        _ok.onClick.AddListener(() => {
            DisablEnotEnoughMoneyWindow();
        });
    }

    private void CheckMoneyAndEnableAbilityOrNotEnoughMoneyWindow() {
        if(_currentCountStars < _currentAbility.Data.price) {
            EnableNotEnoughMoneyWindow();
        }
        else {
            EnableConfirmBuyAbilityWindow();
        }
    }

    private void EnableConfirmBuyAbilityWindow() {
        _confirmBuyAbilityWindow.SetActive(true);
    }

    private void DisableConfirmBuyAbilityWindow() {
        _confirmBuyAbilityWindow.SetActive(false);
    }

    private void EnableNotEnoughMoneyWindow() {
        _notEnoughMoneyWindow.SetActive(true);
    }

    private void DisablEnotEnoughMoneyWindow() {
        _notEnoughMoneyWindow.SetActive(false);
    }

    private void EnableBuyButton() {
        _buy.interactable = true;
    }

    private void DisableBuyButton() {
        _buy.interactable = false;
    }

    private void SubstractPrice() {
        _currentCountStars -= _currentAbility.Data.price;
        _starsText.text = "" + _currentCountStars;
    }

    private void SaveStars() {
        SaveSystemStars.SaveStars(_currentCountStars);
    }

    private void SavePurchaseAbility() {
        print("current ability index = " + _currentAbility.Data.index);
        _currentAbility.Data.isPurchased = true;
        int indexAbility = _currentAbility.Data.index;
        AbilitySaveSystem.SaveAbility(indexAbility, _currentAbility.Data.isPurchased);
        _currentAbility.CheckForPurchasedAbilityAndSetIcon(_abilityData[indexAbility]);
    }

    public void SetCurrentAbility(Ability ability) {
        _currentAbility = ability;
    }

    public void CheckPurchasedAbilityAndEnableOrDisableBuyButton() {
        if (_currentAbility.Data.isPurchased) {
            DisableBuyButton();
        }
        else {
            EnableBuyButton();
        }
    }

    public void SetDetailsAbilityIcon(Sprite icon) {
        _detailAbilityIcon.sprite = icon;
    }

    public void SetDetailsAbilityDescription(string description) {
        _detailAbilityDescription.text = description;
    }
}
