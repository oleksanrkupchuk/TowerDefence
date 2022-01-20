using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinMenu : MenuBase
{
    private int _indexNextLevel;

    [Header("Buttons Win Menu")]
    [SerializeField]
    private Button _nextLevel;

    [Header("Stars")]
    [SerializeField]
    private Star[] _stars;

    public int countStars = 0;

    void Start()
    {
        _indexNextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        SubscriptionButtons();
    }

    public void StartShowAnimationStars() {
        StartCoroutine(ShowStars());
    }

    public IEnumerator ShowStars() {
        for (int i = 0; i < countStars; i++) {
            _stars[i].StartAnimation();
            yield return new WaitUntil(() => _stars[i].endAnamation);
        }
    }

    private void SubscriptionButtons() {
        _nextLevel.onClick.AddListener(() => { LoadGameLevel(_indexNextLevel); });
    }
}
