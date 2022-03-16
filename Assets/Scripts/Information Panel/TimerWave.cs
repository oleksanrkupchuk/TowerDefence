using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimerWave : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    private EnemySpawner _enemySpawner;
    public float time;
    public Image iconComponent;

    public void Init(EnemySpawner enemySpawner) {
        _enemySpawner = enemySpawner;
    }

    public IEnumerator StartAnimationTiwerWave() {
        while (time > 0) {
            time -= Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        
    }

    public void OnPointerExit(PointerEventData eventData) {
        
    }

    public void OnPointerClick(PointerEventData eventData) {
        print("startNewWave");
        //_enemySpawner.EnableWaveEnemy();
    }
}
