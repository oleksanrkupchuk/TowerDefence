using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewWaveIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    private EnemySpawner _enemySpawner;
    private Dictionary<int, List<EnemySpawnRules>> _cartInfoEnemy = new Dictionary<int, List<EnemySpawnRules>>();
    private int _numberSpawn;
    private List<EnemySpawnRules> _currentRules = new List<EnemySpawnRules>();
    private int _numberRules;

    [SerializeField]
    private SpawnInfo _spawnInfo;

    public void Init(EnemySpawner enemySpawner) {
        _enemySpawner = enemySpawner;
        gameObject.SetActive(false);
    }

    private void OnEnable() {
        gameObject.transform.localScale = new Vector3(1f, 1f);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        gameObject.transform.localScale = new Vector3(1.2f, 1.2f);

        if (_spawnInfo.isAccessSpawnInfo) {
            EnableCartInfo();
        }
    }

    private void EnableCartInfo() {
        if (_spawnInfo.newWaveIcon != this) {
            _spawnInfo.EnableCartInfo(_currentRules, this);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        gameObject.transform.localScale = new Vector3(1f, 1f);
    }

    public void OnPointerClick(PointerEventData eventData) {
        SoundManager.Instance.PlaySound(SoundName.StartWave);
        _enemySpawner.EnableWaveEnemy();
        _spawnInfo.Disable();
    }

    public void SetCurrentRules() {
        _currentRules = _cartInfoEnemy[_numberRules];

        if (_numberRules + 1 < _cartInfoEnemy.Count) {
            _numberRules++;
        }
    }

    public void SetListEnemy(List<EnemySpawnRules> enemies) {
        _cartInfoEnemy.Add(_numberSpawn, enemies);
        _numberSpawn++;
    }
}
