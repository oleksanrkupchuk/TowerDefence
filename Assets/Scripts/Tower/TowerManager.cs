using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Loader<TowerManager>
{
    private TowerButton towerButtonPressed;
    private GameObject towerButtonObject;

    private Vector2 cursorMousePosition;
    private RaycastHit2D raycastHit;
    [SerializeField] private LayerMask layerMask;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            cursorMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            raycastHit = Physics2D.Raycast(cursorMousePosition, Vector2.zero, 10f, layerMask);
            PlaceTower(raycastHit);
            
        }
    }

    public void PlaceTower(RaycastHit2D hit2D)
    {
        if (hit2D.transform != null)
        {
            if (hit2D.transform.gameObject.CompareTag("PlacePositionForTower"))
            {
                hit2D.transform.gameObject.tag = "PlaceForTowerFull";
                //Instantiate(towerButtonPressed.TowerObject, raycastHit.transform.position, Quaternion.identity);
                Instantiate(towerButtonObject, raycastHit.transform.position, Quaternion.identity);
                Debug.Log("True");
            }

            else
            {
                Debug.LogError("There are no objects on the way with tag PlacePositionForTower");
            }
        }

        else
        {
            Debug.LogError("There are no objects on the way");
        }
    }

    public void SelectedTower(TowerButton towerButton)
    {
        towerButtonPressed = towerButton;
        Debug.Log("tower = " + towerButtonPressed.gameObject.name);
    }

    public void SelectedTowerSecondVarious(GameObject towerButton)
    {
        towerButtonObject = towerButton;
    }
}
