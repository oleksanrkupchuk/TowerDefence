using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private int speed;
    private Transform enemyNextTransform;
    private GameObject road;

    void Start()
    {
        road = GameObject.FindGameObjectWithTag("Road");
        enemyNextTransform = road.transform.GetChild(0).transform.GetComponent<Transform>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, enemyNextTransform.position, speed * Time.deltaTime);
    }
}
