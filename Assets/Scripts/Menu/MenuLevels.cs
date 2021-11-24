using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuLevels : MenuBase {
    [SerializeField]
    private GameObject _backgroundMenu;
    [SerializeField]
    private GameObject _backgroundInformationPanel;
    [SerializeField]
    private EnemySpawner _enemySpawner;

    public float counter;
    [SerializeField]
    private TextMeshProUGUI _counterText;
    [SerializeField]
    private GameObject _counterTextObject;
    public TextMeshProUGUI CounterText { get => _counterText; }

    void Start() {
        //StopTime();
        //StartLevelAnimationCount();
    }

    private void Update() {
        //StartCoroutine(CounterUntilWave());
        CounterUntilWave();
    }

    private void CounterUntilWave() {
        if (_counterText.enabled == true) {
            if (_enemySpawner.EnemyList.Count <= 0) {
                if (counter > 0) {
                    counter -= Time.deltaTime;
                    _counterText.text = counter.ToString("0.0");

                    if (counter <= 0) {
                        _enemySpawner.StartSpawn();
                        _counterTextObject.SetActive(false);
                    }
                }
            }
        }
    }

    private void GameObjectSetActive(GameObject sceneObject, bool isActive) {
        sceneObject.SetActive(isActive);
    }

    public void EnableCounterObject() {
        _counterTextObject.SetActive(true);
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
            //_countText.text = "" + counter;
            ScaleTextObject(doubleScale, waitOfTime);
            yield return new WaitForSeconds(waitOfTime);
            ScaleTextObject(singleScale, 0f);
            counter++;
        }

        StartCoroutine(CheckAmounterCounterScaleGOTextAndDisableText(doubleScale, maxCounter, counter, waitOfTime));
    }

    private void ScaleTextObject(Vector3 scale, float waitOfTime) {
        //LeanTween.scale(_countText.gameObject, scale, waitOfTime);
    }

    private IEnumerator CheckAmounterCounterScaleGOTextAndDisableText(Vector3 doubleScale, int maxCounter, int counter, float waitOfTime) {
        if (counter >= maxCounter) {
            //_countText.text = "GO!";
            ScaleTextObject(doubleScale, waitOfTime);
            yield return new WaitForSeconds(waitOfTime);
            DisableTextObject();
        }
    }

    private void DisableTextObject() {
        //_countText.gameObject.SetActive(false);
    }
}
