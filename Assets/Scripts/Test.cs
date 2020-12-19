using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private int enemyPointTransform;
    private Transform[] enemyNextTransform;

    private GameObject road;

    void Start()
    {
        enemyPointTransform = 0;
        road = GameObject.FindGameObjectWithTag("Road");

        for (int i = 0; i < road.transform.childCount; i++)
        {
            enemyNextTransform[i] = road.transform.GetChild(i).GetComponent<Transform>();
        }
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, enemyNextTransform[enemyPointTransform].position, speed * Time.deltaTime);

        if (transform.position == enemyNextTransform[enemyPointTransform].position)
        {
            enemyPointTransform++;
        }
    }
}
