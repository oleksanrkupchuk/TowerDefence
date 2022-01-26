using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinMenu : BaseMenu {
    private int _indexNextLevel;
    private int _starsCount;
    private int _currentLevelStars;

    [Header("Buttons Win Menu")]
    [SerializeField]
    private Button _nextLevel;

    [Header("Stars")]
    [SerializeField]
    private Star[] _star;

    public int starsCurrentLevel = 0;

    void Start() {
        _indexNextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        print("index level = " + (_indexNextLevel - 1));
        LoadStars();
        SubscriptionButtons();
    }

    private void LoadStars() {
        StarsData _starsData = SaveSystemStars.LoadStars();
        _starsCount = _starsData.stars;
        _currentLevelStars = _starsData.level[SceneManager.GetActiveScene().buildIndex];
        print("count stars = " + _starsCount);
        for (int i = 0; i < _starsData.level.Length; i++) {
            print($"level {i} = " + _starsData.level[i]);
        }
    }

    private void SubscriptionButtons() {
        _nextLevel.onClick.AddListener(() => {
            SaveStars();
            LoadGameLevel(_indexNextLevel);
        });
    }

    private void SaveStars() {
        print("count stars = " + starsCurrentLevel);
        print("_index Level = " + (_indexNextLevel - 1));
        if (starsCurrentLevel > _currentLevelStars) {
            _starsCount += (starsCurrentLevel - _currentLevelStars);
            SaveSystemStars.SaveStars(_starsCount, starsCurrentLevel, _indexNextLevel - 1);
        }
    }

    public void StartAnimationStars() {
        StartCoroutine(StarsFillingAndBlinkAnimation());
    }

    private IEnumerator StarsFillingAndBlinkAnimation() {
        for (int i = 0; i < starsCurrentLevel; i++) {
            _star[i].StartFillingAnimation();
            yield return new WaitUntil(() => _star[i].endFillingAnimation);
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < starsCurrentLevel; i++) {
            _star[i].StartBlinkAnimation();
        }
    }
}
