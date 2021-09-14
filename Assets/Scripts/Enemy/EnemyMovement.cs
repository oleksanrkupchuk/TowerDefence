using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private int enemyPointTransform;

    private GameObject _road;
    private Transform[] _enemyNextTransform;

    void Start()
    {
        enemyPointTransform = 0;
        _road = GameObject.FindGameObjectWithTag(Tags.road);
        _enemyNextTransform = new Transform[_road.transform.childCount];

        for (int i = 0; i < _road.transform.childCount; i++)
        {
            _enemyNextTransform[i] = _road.transform.GetChild(i).GetComponent<Transform>();
        }
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _enemyNextTransform[enemyPointTransform].position, speed * Time.deltaTime);

        if (transform.position == _enemyNextTransform[enemyPointTransform].position)
        {
            enemyPointTransform++;
        }
    }
}
