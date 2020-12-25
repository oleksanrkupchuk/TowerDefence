using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private int enemyPointTransform;

    private Transform enemyNextTransform;
    private GameObject road;

    void Start()
    {
        enemyPointTransform = 0;
        road = GameObject.FindGameObjectWithTag("Road");
        enemyNextTransform = road.transform.GetChild(0).transform.GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, enemyNextTransform.position, speed * Time.deltaTime);

        if (transform.position == enemyNextTransform.position)
        {
            EnemyNextPosition();
        }
    }

    private void EnemyNextPosition()
    {
        enemyPointTransform++;
        enemyNextTransform = road.transform.GetChild(enemyPointTransform).transform.GetComponent<Transform>();
    }
}
