using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class MenuSelectLevel : BaseMenu {
    private List<Level> _levels = new List<Level>();
    private List<GameObject> _containers = new List<GameObject>();
    private float _stepX = 1200;
    private int _amountContainers;
    private int _numberLevel = 1;
    private int _numberContainer = 1;
    private bool _isCompleteAnimation = true;

    [Header("Buttons Pause Menu")]
    [SerializeField]
    private Button _closeButton;

    [Header("Game objects")]
    [SerializeField]
    private int _amountLevel;
    [SerializeField]
    private GameObject _containerLevel;
    [SerializeField]
    private LevelCart _levelCart;
    [SerializeField]
    private LevelLoader _levelLoader;
    [SerializeField]
    private MainMenu _mainMenu;
    [SerializeField]
    private Transform _containerForContainers;
    [SerializeField]
    private Button _forwardButton;
    [SerializeField]
    private Button _backButton;

    public int AmountLevel { get => _amountLevel; }

    private void Start() {
        LoadLevelData();
        SubscriptionButtons();
        SpawnConteiner();
        SpawnLevelInContainer();
        _backButton.gameObject.SetActive(false);
    }

    private void LoadLevelData() {
        _levels = SaveSystemLevel.LoadLevels();
    }

    private void SubscriptionButtons() {
        _closeButton.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            DisableAndEnableGameObject(ThisGameObject, enableObject);
        });
        _forwardButton.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            MoveLevelContainer(-_stepX, 1, _backButton, _forwardButton, _containers.Count);
        });
        _backButton.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            MoveLevelContainer(_stepX, -1, _forwardButton, _backButton, 1);
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stepX"></param>
    /// <param name="operatorContainer">Increased or decreased number containers</param>
    /// <param name="buttonNeedActive">This button disable when move to last or first screen, now need to activate her</param>
    /// <param name="buttonNeedDisable">This button will be disabled on first or last screen</param>
    /// <param name="countContainer">Number container on which will be disabled "back" or "forward" buttons</param>
    private void MoveLevelContainer(float stepX, int operatorContainer, Button buttonNeedActive, Button buttonNeedDisable, int countContainer) {
        if (!_isCompleteAnimation) {
            return;
        }
        _isCompleteAnimation = false;
        LeanTween.moveLocalX(_containerForContainers.gameObject, 
            _containerForContainers.transform.localPosition.x + stepX, 1)
            .setOnComplete(() => { 
                _isCompleteAnimation = true;
            });

        _numberContainer += operatorContainer;
        if (buttonNeedActive.gameObject.activeSelf == false) {
            buttonNeedActive.gameObject.SetActive(true);
        }
        if (_numberContainer == countContainer) {
            buttonNeedDisable.gameObject.SetActive(false);
        }
    }

    private void SpawnConteiner() {
        if (_amountLevel < 4) {
            _amountContainers = 1;
        }
        else if (_amountLevel % 4 == 0) {
            _amountContainers = _amountLevel / 4;
        }
        else {
            _amountContainers = (_amountLevel / 4) + 1;
        }

        for (int i = 0; i < _amountContainers; i++) {
            GameObject _conteiner = Instantiate(_containerLevel);
            _conteiner.transform.SetParent(_containerForContainers);
            _conteiner.transform.localScale = new Vector3(1f, 1f, 1f);
            _conteiner.transform.localPosition = new Vector3(i * _stepX, -55f, 0);
            _containers.Add(_conteiner);
        }
    }

    private void SpawnLevelInContainer() {
        for (int i = 0; i < _amountContainers; i++) {
            int _amountLevelCartsForContainer = 0;
            if (i == _amountContainers - 1) {
                _amountLevelCartsForContainer = _amountLevel - (4 * i);
            }
            else {
                _amountLevelCartsForContainer = 4;
            }
            SpawnLevelCarts(_amountLevelCartsForContainer, _containers[i].transform);
        }
    }

    private void SpawnLevelCarts(int amountCarts, Transform parent) {
        for (int indexLevel = 1; indexLevel <= amountCarts; indexLevel++) {
            LevelCart _levelObject = Instantiate(_levelCart, parent);
            _levelObject.CheckUnlockLevelAndSetIntractable(_levels[_numberLevel - 1].isUnlock, _levels[_numberLevel - 1].stars);
            SubscriptionLevelButton(_levelObject.Button, indexLevel);
            SetTextOnLevelButton(_levelObject.Title, _numberLevel);
            _numberLevel++;
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
