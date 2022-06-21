using UnityEngine;
using UnityEngine.UI;

public class GameInformation : BaseMenu {
    [SerializeField]
    private Button _backButton;
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private EnemyCart _enemyCart;
    [SerializeField]
    private EnemyCartData[] _enemyCartData;

    public int AmountEnemyCartData { get => _enemyCartData.Length; }

    void Start() {
        //InitEnemyCartData();
        SpawnEnemyCart();
        SubscriptionButtons();
        DisableGameObject(gameObject);
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
            _enemyCartObject.Init(_enemyCartData[i]);

            _enemyCartObject.transform.SetParent(_content);
            _enemyCartObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void SubscriptionButtons() {
        _backButton.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            DisableAndEnableGameObject(ThisGameObject, enableObject);
        });
    }
}
