using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float timeSpawn;
    [SerializeField] private float timer;
    [SerializeField] private int enemyAmountInWave;
    [SerializeField] private int enemyNumberWaves;
    [SerializeField] private int enemyAmountSpawn;

    void Start()
    {
        
    }

    void Update()
    {
        EnemySpawn();
    }

    private void EnemySpawn()
    {
        if(enemyPrefab != null)
        {
            if (enemyAmountSpawn < enemyAmountInWave)
            {
                timer += Time.deltaTime;

                if (timer > timeSpawn)
                {
                    Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                    enemyAmountSpawn++;
                    timer = 0f;
                }
            }
        }

        else
        {
            Debug.LogError("'enemyPrefab' return null");
        }
        
    }
}
