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

    [Header("Buttons Win Menu")]
    [SerializeField]
    private Button _nextLevel;

    [Header("Stars")]
    [SerializeField]
    private Star[] _star;

    private int LastSceneIndex {
        get => SceneManager.sceneCountInBuildSettings - 2;
    }

    public int amountReceivedStarsOnCurrentLevel = 0;

    void Start() {
        _currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        CheckExistNextLevel();
        //print("index level = " + (_indexNextLevel - 1));
        LoadLevels();
        SubscriptionButtons();
    }

    private void CheckExistNextLevel() {
        if(_currentLevelIndex < LastSceneIndex) {
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
        _nextLevel.onClick.AddListener(() => {
            SoundManager.Instance.PlaySoundEffect(SoundName.ButtonClick);
            CheckAmountStarsOnCurrentLevelSaveStarsAndLevel();
            _levelLoader.gameObject.SetActive(true);
            _levelLoader.LoadLevel(_nextLevelIndex);
        });
    }

    private void CheckAmountStarsOnCurrentLevelSaveStarsAndLevel() {
        if(_levels[_currentLevelIndex - 1].stars == 0) {
            SaveLevelAndStars(amountReceivedStarsOnCurrentLevel, amountReceivedStarsOnCurrentLevel);
        }
        else if(amountReceivedStarsOnCurrentLevel > _levels[_currentLevelIndex - 1].stars) {
            int _differentStars = amountReceivedStarsOnCurrentLevel - _levels[_currentLevelIndex - 1].stars;
            SaveLevelAndStars(_differentStars, amountReceivedStarsOnCurrentLevel);
        }
    }

    private void SaveLevelAndStars(int amountStars, int amountStarsForCurrentLevel) {
        _starsData.stars += amountStars;
        SaveAndLoadStars.SaveStars(_starsData);

        int _currentLevel = _currentLevelIndex - 1;
        int _nextLevel = _currentLevelIndex;

        _levels[_currentLevel].stars = amountStarsForCurrentLevel;
        _levels[_nextLevel].isUnlock = true;
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
    }
}
