using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLevels : MonoBehaviour
{
    [SerializeField]
    private Button _startButton;
    [SerializeField]
    private GameObject _backgroundMenu;

    void Start()
    {
        StopTime();
        SubscribeStartButton();
    }
    private void StopTime() {
        Time.timeScale = 0f;
    }

    private void SubscribeStartButton() {
        _startButton.onClick.AddListener(() => { StartTime(); DisableBackgroundMenu(); });
    }

    private void StartTime() {
        Time.timeScale = 1f;
    }

    private void DisableBackgroundMenu() {
        _backgroundMenu.SetActive(false);
    }

    private void EnableBackgroundMenu() {
        _backgroundMenu.SetActive(true);
    }
}
