using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Loader<TowerManager>
{
    private TowerButton towerButtonPressed;

    private Vector2 cursorMousePosition;
    private RaycastHit2D raycastHit;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private SpriteRenderer spriteRenderer;

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

        if(spriteRenderer.sprite == true)
        {
            FollowMouseTowerIcon();
        }
    }

    public void PlaceTower(RaycastHit2D hit2D)
    {
        if (hit2D.transform != null)
        {
            if (hit2D.transform.gameObject.CompareTag("PlacePositionForTower"))
            {
                hit2D.transform.gameObject.tag = "PlaceForTowerFull";
                Instantiate(towerButtonPressed.TowerObject, raycastHit.transform.position, Quaternion.identity);
                DisbleSprite();
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
        EnableSprite(towerButtonPressed.TowerSprite);
        Debug.Log("tower = " + towerButtonPressed.gameObject.name);
    }

    public void EnableSprite(Sprite sprite)
    {
        if(spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = sprite;
        }

        else
        {
            Debug.LogError("The variable spriteRenderer of TowerManager has not been assigned.");
        }
    }

    public void DisbleSprite()
    {
        spriteRenderer.enabled = false;
    }

    public void FollowMouseTowerIcon()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 10);
    }
}
