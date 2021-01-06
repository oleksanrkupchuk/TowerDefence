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

    /// <summary>
    /// Перевірка розміщення вежі, і встановлення нового тегу для неможливості повторного розміщення вежі на одному місці
    /// </summary>
    /// <param name="hit2D"></param>
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

    /// <summary>
    /// Вибір вежі при натиснені на кнопку
    /// </summary>
    /// <param name="towerButton"></param>
    public void SelectedTower(TowerButton towerButton)
    {
        towerButtonPressed = towerButton;
        EnableSprite(towerButtonPressed.TowerSprite);
        Debug.Log("tower = " + towerButtonPressed.gameObject.name);
    }

    /// <summary>
    /// Включення спрайту вежі
    /// </summary>
    /// <param name="sprite"></param>
    public void EnableSprite(Sprite sprite)
    {
        if(spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = 100;
        }

        else
        {
            Debug.LogError("The variable spriteRenderer of TowerManager has not been assigned.");
        }
    }

    /// <summary>
    /// Відключення спрайту вежі
    /// </summary>
    public void DisbleSprite()
    {
        spriteRenderer.enabled = false;
    }

    //Слідкування спрайта вежі за мишкою. Потрібно вказувати вісь z тому що по стандарту вона має значення -10 і таким спрайт відображатиметься за картою
    public void FollowMouseTowerIcon()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 10);
    }
}
