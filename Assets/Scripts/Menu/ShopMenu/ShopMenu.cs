using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ShopMenu : BaseMenu {
    private Ability _currentAbility;
    private int _currentCountStars;
    private AbilityPurchased _abilityPurchased;
    private List<Ability> _abilityes = new List<Ability>();
    private AbilityItem _item;

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
    private Ability _ability;
    [SerializeField]
    private ApplyingAbility _applyingAbility;
    [SerializeField]
    private Scrollbar _scrollBar;
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

    private void OnEnable() {
        _scrollBar.value = 0f;
        LoadStars();
        #region FOR TEST DATA
        LoadAbility();
        #endregion
    }

    private void LoadStars() {
        if (SaveSystemStars.IsExistsSaveStarsFile()) {
            StarsData _starsData = SaveSystemStars.LoadStars();
            _currentCountStars = _starsData.stars;
            _starsText.text = "" + _currentCountStars;
        }
    }

    private void LoadAbility() {
        print("fire ability exist = " + AbilitySaveSystem.IsExistsSaveAbilityFile());
        if (AbilitySaveSystem.IsExistsSaveAbilityFile()) {
            _abilityPurchased = AbilitySaveSystem.LoadAbility();
        }
    }

    private void Start() {
        InitAbilityData();
        SpawnAbility();
        DisableConfirmBuyAbilityWindow();
        DisablEnotEnoughMoneyWindow();
        SubscriptionButtons();

        _detailAbilityIcon.sprite = _abilityData[0].icon;
        _detailAbilityDescription.text = _abilityData[0].description;

        if (_currentAbility.Data.isPurchased) {
            DisableBuyButton();
        }

        DisableShopMenu();
    }

    private void InitAbilityData() {
        if (AbilitySaveSystem.IsExistsSaveAbilityFile()) {
            print("init ability data from file");
            for (int i = 0; i < _abilityData.Count; i++) {
                for (int j = 0; j < _abilityPurchased.abilities.Count; j++) {
                    if (_abilityData[i].type == _abilityPurchased.abilities[j].type) {
                        _abilityData[i].isPurchased = _abilityPurchased.abilities[j].isPurchased;
                    }
                }
            }
        }
        else {
            for (int i = 0; i < _abilityData.Count; i++) {
                _abilityData[i].isPurchased = false;
            }
        }
    }

    private void SpawnAbility() {
        for (int index = 0; index < _abilityData.Count; index++) {
            Ability _abilityObject = Instantiate(_ability);
            _abilityObject.transform.SetParent(_content);
            _abilityObject.transform.localScale = new Vector3(1f, 1f, 1f);

            _abilityData[index].index = index;

            _abilityObject.Init(_abilityData[index], this);
            _abilityObject.CheckForPurchasedAbilityAndSetIcon();
            _abilityes.Add(_abilityObject);
        }

        _currentAbility = _abilityes[0];
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
            SavePurchasedAbility();
            ApplyAbility();
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
        if (_currentCountStars < _currentAbility.Data.price) {
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

    private void SavePurchasedAbility() {
        _currentAbility.Data.isPurchased = true;
        if (AbilitySaveSystem.IsExistsSaveAbilityFile()) {
            _abilityPurchased = AbilitySaveSystem.LoadAbility();
            _item = new AbilityItem(_currentAbility.Data.type, _currentAbility.Data.isPurchased);
            _abilityPurchased.abilities.Add(_item);
            AbilitySaveSystem.SaveAbility(_abilityPurchased.abilities);
        }
        else {
            _item = new AbilityItem(_currentAbility.Data.type, _currentAbility.Data.isPurchased);
            AbilitySaveSystem.SaveAbility(_item);
        }
        //print("current ability index = " + _currentAbility.Data.index);
        if (AbilitySaveSystem.IsExistsSaveAbilityFile()) {
            _abilityPurchased = AbilitySaveSystem.LoadAbility();
            for (int i = 0; i < _abilityPurchased.abilities.Count; i++) {
                print("---------------------SHOP BUY-------------------------");
                print($"ability {i} type = " + _abilityPurchased.abilities[i].type + " purchased = " + _abilityPurchased.abilities[i].isPurchased);
            }
        }
        _currentAbility.CheckForPurchasedAbilityAndSetIcon();
    }

    private void ApplyAbility() {
        _applyingAbility.ApplyAbility(_currentAbility.Data.type);
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

    private void DisableShopMenu() {
        gameObject.SetActive(false);
    }
}
