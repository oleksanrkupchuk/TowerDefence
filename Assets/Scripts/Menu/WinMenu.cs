using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class WinMenu : BaseMenu {
    private int _currentLevelIndex;
    private int _nextLevelIndex;
    private StarsData _starsData;
    private List<Level> _levels = new List<Level>();

    [SerializeField]
    private LevelLoader _levelLoader;
    [SerializeField]
    private MenuSelectLevel _menuSelectLevel;

    [Header("Buttons Win Menu")]
    [SerializeField]
    private Button _nextLevelButton;

    [Header("Stars")]
    [SerializeField]
    private Star[] _star;

    private int LastSceneIndex {
        get => SceneManager.sceneCountInBuildSettings - 2;
    }

    public int amountReceivedStarsOnCurrentLevel = 0;

    void Start() {
        //_nextLevelButton.interactable = false;
        _currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        CheckExistNextLevel();
        LoadLevels();
        SubscriptionButtons();
    }

    private void CheckExistNextLevel() {
        if (_currentLevelIndex < LastSceneIndex) {
            _nextLevelIndex = _currentLevelIndex + 1;
        }
        else {
            _nextLevelIndex = 0;
        }
    }

    private void LoadLevels() {
        _levels = SaveSystemLevel.LoadLevels();
        _starsData = SaveAndLoadStars.LoadStars();
    }

    private void SubscriptionButtons() {
        _nextLevelButton.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            CheckAmountStarsOnCurrentLevelSaveStarsAndLevel();
            StartCoroutine(WaitForChangeToggle());
            _levelLoader.gameObject.SetActive(true);
            _levelLoader.LoadLevel(0);
        });
    }

    private IEnumerator WaitForChangeToggle() {
        _menuSelectLevel.isPassLevel = true;
        yield return new WaitForSeconds(1f);
    }

    private void CheckAmountStarsOnCurrentLevelSaveStarsAndLevel() {
        if (_levels[_currentLevelIndex - 1].stars == 0) {
            SaveLevelAndStars(amountReceivedStarsOnCurrentLevel, amountReceivedStarsOnCurrentLevel);
        }
        else if (amountReceivedStarsOnCurrentLevel > _levels[_currentLevelIndex - 1].stars) {
            int _differentStars = amountReceivedStarsOnCurrentLevel - _levels[_currentLevelIndex - 1].stars;
            SaveLevelAndStars(_differentStars, amountReceivedStarsOnCurrentLevel);
        }
    }

    private void SaveLevelAndStars(int amountStars, int amountStarsForCurrentLevel) {
        _starsData.stars += amountStars;
        SaveAndLoadStars.SaveStars(_starsData);

        int _currentLevelIndexInList = _currentLevelIndex - 1;
        int _nextLevelIndexInList = _currentLevelIndex;

        _levels[_currentLevelIndexInList].stars = amountStarsForCurrentLevel;
        if (_nextLevelIndexInList < _levels.Count) {
            _levels[_nextLevelIndexInList].isUnlock = true;
        }

        SaveSystemLevel.SaveLevels(_levels);
    }

    public void StartAnimationStars() {
        StartCoroutine(StarsFillingAndBlinkAnimation());
    }

    private IEnumerator StarsFillingAndBlinkAnimation() {
        for (int i = 0; i < amountReceivedStarsOnCurrentLevel; i++) {
            _star[i].StartFillingAnimation();
            yield return new WaitUntil(() => _star[i].endFillingAnimation);
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < amountReceivedStarsOnCurrentLevel; i++) {
            _star[i].StartBlinkAnimation();
        }

        //_nextLevelButton.interactable = true;
    }
}
