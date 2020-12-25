using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacing : MonoBehaviour
{
    private Vector2 cursorMousePosition;
    private RaycastHit2D raycastHit;
    [SerializeField] private LayerMask layerMask;
    public bool placeIsEmpty;

    void Start()
    {
        placeIsEmpty = true;
}

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            cursorMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            raycastHit = Physics2D.Raycast(cursorMousePosition, Vector2.zero, 10f, layerMask);

            if (raycastHit.transform != null)
            {
                if (raycastHit.transform.gameObject.CompareTag("PlaceForTower"))
                {
                    Debug.Log("True");
                }
            }

            else
            {
                Debug.LogError("There are no objects on the way");
            }
        }
    }
}
