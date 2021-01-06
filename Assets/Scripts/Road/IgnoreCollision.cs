using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    private BoxCollider2D enemyBoxCollider;

    private BoxCollider2D roadBoxCollider;
    void Start()
    {
        enemyBoxCollider = enemyPrefab.GetComponent<BoxCollider2D>();
        roadBoxCollider = gameObject.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(enemyBoxCollider, roadBoxCollider, true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
