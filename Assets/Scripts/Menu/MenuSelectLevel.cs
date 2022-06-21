using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuSelectLevel : BaseMenu {
    [SerializeField]
    private List<LevelCart> _levelCarts = new List<LevelCart>();
    private List<Level> _levels = new List<Level>();

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
    [SerializeField]
    private LevelLoader _levelLoader;

    public int AmountLevel { get => _amountLevel; }

    private void Start() {
        LoadLevelData();
        SubscriptionButtons();
        SpawnLevelContainer();
        InitStarsOnLevelCart();
        DisableGameObject(gameObject);
    }

    private void LoadLevelData() {
        _levels = SaveSystemLevel.LoadLevels();
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
            _levelObject.CheckUnlockLevelAndSetIntractable(_levels[indexLevel - 1].isUnlock);
            SubscriptionLevelButton(_levelObject.Button, indexLevel);
            SetTextOnLevelButton(_levelObject.Title, indexLevel);
            _levelCarts.Add(_levelObject);
        }
    }

    private void InitStarsOnLevelCart() {
        //print("count save levels = " + _levelData.levels.Count);
        for (int i = 0; i < _levels.Count; i++) {
            int _countStarsOnLevel = _levels[i].stars;
            _levelCarts[i].SetIconStarsFull(_countStarsOnLevel);
        }
    }

    private void SubscriptionLevelButton(Button levelButton, int indexLevel) {
        levelButton.onClick.AddListener(() => {
            _levelLoader.gameObject.SetActive(true);
            _levelLoader.LoadLevel(indexLevel);
        });
    }

    private void SetTextOnLevelButton(Text buttonText, int indexLevel) {
        buttonText.text = "Level " + indexLevel;
    }
}
