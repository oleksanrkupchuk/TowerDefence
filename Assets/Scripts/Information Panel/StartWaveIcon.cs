using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartWaveIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    private EnemySpawner _enemySpawner;
    private Dictionary<int, List<ChainOfEnemies>> _cartInfoEnemy = new Dictionary<int, List<ChainOfEnemies>>();
    private int _numberSpawn;
    private List<ChainOfEnemies> _currentRules = new List<ChainOfEnemies>();
    private int _numberRules;

    public void Init(EnemySpawner enemySpawner) {
        _enemySpawner = enemySpawner;
    }

    private void OnEnable() {
        gameObject.transform.localScale = new Vector3(1f, 1f);
    }

    private void Awake() {
        gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        gameObject.transform.localScale = new Vector3(1.2f, 1.2f);
    }

    public void OnPointerExit(PointerEventData eventData) {
        gameObject.transform.localScale = new Vector3(1f, 1f);
    }

    public void OnPointerClick(PointerEventData eventData) {
        SoundManager.Instance.PlaySound(SoundName.StartWave);
        _enemySpawner.countWave++;
        _enemySpawner.EnableWaveEnemy();
    }

    public void SetCurrentRules() {
        _currentRules = _cartInfoEnemy[_numberRules];

        if (_numberRules + 1 < _cartInfoEnemy.Count) {
            _numberRules++;
        }
    }

    public void SetListEnemy(List<ChainOfEnemies> enemies) {
        _cartInfoEnemy.Add(_numberSpawn, enemies);
        _numberSpawn++;
    }
}
