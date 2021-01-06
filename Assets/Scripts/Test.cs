using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    private GameObject road;

    [SerializeField] private int speed;
    private Transform[] enemyNextTransform;
    private Vector3 nexPos;
    private List<Vector3> roadPos = new List<Vector3>();

    [SerializeField] private bool canNextPos = true;

    void Start()
    {
        road = GameObject.FindGameObjectWithTag("Road");
        enemyNextTransform = new Transform[road.transform.childCount];

        nexPos = transform.position;

        for (int i = 0; i < road.transform.childCount; i++)
        {
            enemyNextTransform[i] = road.transform.GetChild(i).GetComponent<Transform>();
        }
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nexPos, speed * Time.deltaTime);
    }

    //private void OntriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("way"))
    //    {
    //        Debug.Log("way");

    //        if (nexPos != collision.gameObject.transform.position)
    //        {
    //            nexPos = collision.gameObject.transform.position;
    //            Debug.Log("nex pos = " + nexPos);
    //        }
    //    }
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!canNextPos)
        {
            if(transform.position == nexPos)
            {
                canNextPos = true;
            }
        }

        else if (canNextPos)
        {
            if (collision.gameObject.CompareTag("Way"))
            {
                if (roadPos.Count == 0)
                {
                    nexPos = collision.gameObject.transform.position;
                    roadPos.Add(nexPos);
                    canNextPos = false;
                    return;
                }

                if(roadPos.Count > 0)
                {
                    if (NextWay(collision) && Position(collision))
                    {
                        nexPos = collision.gameObject.transform.position;
                        roadPos.Add(nexPos);
                        canNextPos = false;
                        return;
                    }
                }
            }
        }
    }

    private bool Position(Collider2D collision)
    {
        if(Mathf.Abs(Vector3.Distance(collision.transform.position, nexPos)) == 2f)
        {
            return true;
        }

        return false;
    }

    private bool NextWay(Collider2D collision)
    {
        for (int i = 0; i < roadPos.Count; i++)
        {
            if (collision.gameObject.transform.position == roadPos[i])
            {
                Debug.Log("nex way = " + collision.transform.position);
                return false;
            }
        }

        return true;
    }
}
