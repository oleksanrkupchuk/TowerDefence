using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner _enemySpawner = null;
    public static EnemySpawner Instance { get => _enemySpawner; }

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int _enemyAmountInWave;
    [SerializeField] private int _enemyAmountSpawn;
    [SerializeField] private int _quantityWave;
    [SerializeField] private int _waves;

    private void Awake() {
        if(_enemySpawner == null) {
            _enemySpawner = this;
        }
        else if(_enemySpawner != this) {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartSpawn();
    }

    public void StartSpawn() {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn() {
        if(_waves < _quantityWave) {
            for (int i = 0; i < _enemyAmountInWave; i++) {
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                _enemyAmountSpawn++;

                yield return new WaitForSeconds(2f);
            }
            _waves++;
        }

        _enemyAmountInWave++;
    }

    public void ResetEnemyQuantityInSpawn() {
        _enemyAmountSpawn = 0;
    }
}
