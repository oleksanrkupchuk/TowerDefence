using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using System.Collections.Generic;

public class Glossary : BaseMenu {
    [SerializeField]
    private Button _closeButton;
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private EnemyCart _enemyCart;
    [SerializeField]
    private EnemyCartData[] _enemyCartData;

    public int AmountEnemyCartData { get => _enemyCartData.Length; }


    private List<string> _tableNameEnemy = new List<string> {
        "Minotaur_Brown_Name",
        "Minotaur_Grey_Name",
        "Minotaur_Green_Name",
        "Satyr_Doctor_Name",
        "Satyr_Runner_Name",
    };

    private List<string> _tableDescriptionEnemy = new List<string> {
        "Minotaur_Brown_Description",
        "Minotaur_Grey_Description",
        "Minotaur_Green_Description",
        "Satyr_Doctor_Description",
        "Satyr_Runner_Description",
    };

    void Start() {
        SpawnEnemyCart();
        SubscriptionButtons();
    }

    private void InitEnemyCartData() {
        CartEnemies _cartEnemies = SaveAndLoadEnemyCart.LoadEnemyCart();

        for (int i = 0; i < _enemyCartData.Length; i++) {
            _enemyCartData[i].unlockEnemy = _cartEnemies.unlocksEnemies[i];
        }
    }

    private void SpawnEnemyCart() {
        for (int i = 0; i < _enemyCartData.Length; i++) {
            EnemyCart _enemyCartObject = Instantiate(_enemyCart);

            LocalizedString _localizationStringName = new LocalizedString { 
                TableReference = "Glossary", TableEntryReference = _tableNameEnemy[i] };

            LocalizedString _localizationStringDescription = new LocalizedString {
                TableReference = "Glossary",
                TableEntryReference = _tableDescriptionEnemy[i]
            };

            _enemyCartObject.Init(_enemyCartData[i], _localizationStringName, _localizationStringDescription);

            _enemyCartObject.transform.SetParent(_content);
            _enemyCartObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void SubscriptionButtons() {
        _closeButton.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            DisableAndEnableGameObject(ThisGameObject, enableObject);
        });
    }
}
