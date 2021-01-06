using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisionEnemy : MonoBehaviour
{
    private GameObject road;

    [Header("Ignore Collision")]
    private BoxCollider2D boxCollider;
    private BoxCollider2D[] roadCollider;

    void Start()
    {
        road = GameObject.FindGameObjectWithTag("Road");

        roadCollider = new BoxCollider2D[road.transform.childCount];
        boxCollider = gameObject.GetComponent<BoxCollider2D>();

        GetRoadCollider();
        IgnoreCollision();
    }

    private void IgnoreCollision()
    {
        for (int i = 0; i < roadCollider.Length; i++)
        {
            Physics2D.IgnoreCollision(boxCollider, roadCollider[i], true);
        }
    }

    private void GetRoadCollider()
    {
        for (int i = 0; i < road.transform.childCount; i++)
        {
            roadCollider[i] = road.transform.GetChild(i).gameObject.GetComponent<BoxCollider2D>();
        }
    }
}
