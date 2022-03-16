using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MenuSelectLevel : BaseMenu {
    [SerializeField]
    private List<LevelCart> _levelCarts = new List<LevelCart>();
    private LevelData _levelData;

    [Header("Buttons Pause Menu")]
    [SerializeField]
    private Button _back;

    [Header("Game objects")]
    [SerializeField]
    private int _amountLevel;
    [SerializeField]
    private GameObject _containerLevel;
    [SerializeField]
    private LevelCart _levelCart;

    public int AmountLevel { get => _amountLevel; }

    private void Start() {
        LoadLevelData();
        SubscriptionButtons();
        SpawnLevelContainer();
        InitStarsOnLevelCart();
        DisableGameObject(gameObject);
    }

    private void LoadLevelData() {
        _levelData = SaveSystemLevel.LoadLevelData();
    }

    private void SubscriptionButtons() {
        _back.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            DisableAndEnableGameObject(ThisGameObject, enableObject);
        });
    }

    private void SpawnLevelContainer() {
        for (int indexLevel = 1; indexLevel <= _amountLevel; indexLevel++) {
            LevelCart _levelObject = Instantiate(_levelCart, _containerLevel.transform);
            _levelObject.CheckUnlockLevelAndSetIntractable(_levelData.levels[indexLevel - 1].isUnlock);
            SubscriptionLevelButton(_levelObject.Button, indexLevel);
            SetTextOnLevelButton(_levelObject.Text, indexLevel);
            _levelCarts.Add(_levelObject);
        }
    }

    private void InitStarsOnLevelCart() {
        //print("count save levels = " + _levelData.levels.Count);
        for (int i = 0; i < _levelData.levels.Count; i++) {
            int _countStarsOnLevel = _levelData.levels[i].stars;
            _levelCarts[i].SetIconStarsFull(_countStarsOnLevel);
        }
    }

    private void SubscriptionLevelButton(Button levelButton, int indexLevel) {
        levelButton.onClick.AddListener(() => {
            LoadGameLevel(indexLevel);
        });
    }

    private void SetTextOnLevelButton(TextMeshProUGUI buttonText, int indexLevel) {
        buttonText.text = "Level " + indexLevel;
    }
}
