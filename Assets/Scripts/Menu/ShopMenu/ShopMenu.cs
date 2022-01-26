using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ShopMenu : BaseMenu {
    private Ability _currentAbility;
    private int _currentCountStars;
    private bool[] _abilityPurchased = new bool[7];

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
    private GameObject _confirmBuyAbilityWindow;

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
    private List<AbilityData> _ability = new List<AbilityData>();

    private void Start() {
        LoadStars();
        DisableConfirmBuyAbilityWindow();
        SubscriptionButtons();
        SpawnAbility();
        _detailAbilityIcon.sprite = _ability[0].icon;
        _detailAbilityDescription.text = _ability[0].description;
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
        LoadAbility();

        for (int index = 0; index < _ability.Count; index++) {
            GameObject _abilityObject = Instantiate(_abilityPrefab);
            _abilityObject.transform.SetParent(_content);
            _abilityObject.transform.localScale = new Vector3(1f, 1f, 1f);

            _ability[index].isPurchased = _abilityPurchased[index];
            _ability[index].index = index;

            Ability _abilityScript = _abilityObject.GetComponent<Ability>();
            _abilityScript.InitializationAbility(_ability[index], this);
        }
    }

    private void SubscriptionButtons() {
        _back.onClick.AddListener(() => {
            DisableAndEnableGameObject(ThisGameObject, enableObject);
        });

        _buy.onClick.AddListener(() => {
            EnableConfirmBuyAbilityWindow();
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
    }

    private void EnableConfirmBuyAbilityWindow() {
        _confirmBuyAbilityWindow.SetActive(true);
    }

    private void DisableConfirmBuyAbilityWindow() {
        _confirmBuyAbilityWindow.SetActive(false);
    }

    private void SubstractPrice() {
        _currentCountStars -= _currentAbility.AbilityData.price;
        _starsText.text = "" + _currentCountStars;
    }

    private void SaveStars() {
        SaveSystemStars.SaveStars(_currentCountStars);
    }

    private void SavePurchaseAbility() {
        print("current ability index = " + _currentAbility.AbilityData.index);
        _currentAbility.AbilityData.isPurchased = true;
        int indexAbility = _currentAbility.AbilityData.index;
        AbilitySaveSystem.SaveAbility(indexAbility, _currentAbility.AbilityData.isPurchased);
        _currentAbility.CheckForPurchasedAbilityAndSetIcon(_ability[indexAbility]);
    }

    public void SetCurrentAbility(Ability ability) {
        _currentAbility = ability;
    }

    public void SetDetailsAbilityIcon(Sprite icon) {
        _detailAbilityIcon.sprite = icon;
    }

    public void SetDetailsAbilityDescription(string description) {
        _detailAbilityDescription.text = description;
    }
}
