using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class WinMenu : BaseMenu {
    private int _currentLevelIndex;
    private int _indexNextLevel;
    private int _countAllStars;
    private List<Level> _levels;

    [SerializeField]
    private LevelLoader _levelLoader;

    [Header("Buttons Win Menu")]
    [SerializeField]
    private Button _nextLevel;

    [Header("Stars")]
    [SerializeField]
    private Star[] _star;


    public int amountReceivedStarsOnCurrentLevel = 0;

    void Start() {
        _currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        _indexNextLevel = _currentLevelIndex + 1;
        //print("index level = " + (_indexNextLevel - 1));
        LoadLevels();
        SubscriptionButtons();
    }

    private void LoadLevels() {
        LevelData _starsData = SaveSystemLevel.LoadLevelData();
        _countAllStars = _starsData.stars;
        _levels = _starsData.levels;
    }

    private void SubscriptionButtons() {
        _nextLevel.onClick.AddListener(() => {
            CheckExistDataLevelsFileAndSaveLevels();
            _levelLoader.gameObject.SetActive(true);
            _levelLoader.LoadLevel(_indexNextLevel);
            //LoadGameLevel(_indexNextLevel);
        });
    }

    private void CheckExistDataLevelsFileAndSaveLevels() {
        if (amountReceivedStarsOnCurrentLevel > _levels[_currentLevelIndex - 1].stars) {
            int _differentStars = amountReceivedStarsOnCurrentLevel - _levels[_currentLevelIndex - 1].stars;
            SaveStars(_differentStars, amountReceivedStarsOnCurrentLevel);
            return;
        }

        SaveStars(amountReceivedStarsOnCurrentLevel, amountReceivedStarsOnCurrentLevel);
    }

    private void SaveStars(int amountStars, int amountStarsForCurrentLevel) {
        int _currentLevel = _currentLevelIndex - 1;
        int _nextLevel = _currentLevelIndex;
        _countAllStars += amountStars;
        _levels[_currentLevel].stars = amountStarsForCurrentLevel;
        _levels[_nextLevel].isUnlock = true;
        SaveSystemLevel.SaveLevel(_countAllStars, _levels);
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
    }
}
