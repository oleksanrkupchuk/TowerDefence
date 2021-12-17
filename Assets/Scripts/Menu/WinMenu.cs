using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinMenu : MenuBase
{
    [Header("Buttons Win Menu")]
    [SerializeField]
    private Button _nextLevel;

    private int _indexNextLevel;

    void Start()
    {
        _indexNextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        SubscriptionButtons();
    }

    private void SubscriptionButtons() {
        _nextLevel.onClick.AddListener(() => { LoadGameLevel(_indexNextLevel); });
    }
}
