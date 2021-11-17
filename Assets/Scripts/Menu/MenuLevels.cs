using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuLevels : MonoBehaviour {
    [SerializeField]
    private Button _startButton;
    [SerializeField]
    private GameObject _backgroundMenu;
    [SerializeField]
    private TextMeshProUGUI _countText;
    [SerializeField]
    private EnemySpawner _enemySpawner;
    [SerializeField]
    private EnemyList _enemyList;

    public float counter;
    [SerializeField]
    private TextMeshProUGUI _counterText;
    void Start() {
        //StopTime();
        SubscribeStartButton();
        //StartLevelAnimationCount();
    }

    private void Update() {
        //StartCoroutine(CounterUntilWave());
        CounterUntilWave();
    }

    private void StopTime() {
        Time.timeScale = 0f;
    }

    private void CounterUntilWave() {
        if (_enemyList.Enemys.Count <= 0) {
            if (counter > 0) {
                counter -= Time.deltaTime;
                _counterText.text = counter.ToString("0.0");

                if (counter <= 0) {
                    _enemySpawner.StartSpawn();
                }
            }
        }
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

    private void StartLevelAnimationCount() {
        Vector3 singleScale = new Vector3(1f, 1f, 1f);
        Vector3 doubleScale = new Vector3(2f, 2f, 2f);
        StartCoroutine(IncreaseCounterAndPlayAnimation(doubleScale, singleScale));
    }

    private IEnumerator IncreaseCounterAndPlayAnimation(Vector3 doubleScale, Vector3 singleScale) {
        int counter = 1;
        int maxCounter = 3;
        float waitOfTime = 1f;
        for (int i = 0; i < maxCounter; i++) {
            _countText.text = "" + counter;
            ScaleTextObject(doubleScale, waitOfTime);
            yield return new WaitForSeconds(waitOfTime);
            ScaleTextObject(singleScale, 0f);
            counter++;
        }

        StartCoroutine(CheckAmounterCounterScaleGOTextAndDisableText(doubleScale, maxCounter, counter, waitOfTime));
    }

    private void ScaleTextObject(Vector3 scale, float waitOfTime) {
        LeanTween.scale(_countText.gameObject, scale, waitOfTime);
    }

    private IEnumerator CheckAmounterCounterScaleGOTextAndDisableText(Vector3 doubleScale, int maxCounter, int counter, float waitOfTime) {
        if (counter >= maxCounter) {
            _countText.text = "GO!";
            ScaleTextObject(doubleScale, waitOfTime);
            yield return new WaitForSeconds(waitOfTime);
            DisableTextObject();
        }
    }

    private void DisableTextObject() {
        _countText.gameObject.SetActive(false);
    }
}
