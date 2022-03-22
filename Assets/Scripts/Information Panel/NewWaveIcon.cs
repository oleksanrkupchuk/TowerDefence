using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewWaveIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    private EnemySpawner _enemySpawner;

    [SerializeField]
    private Image _icon;

    public void Init(EnemySpawner enemySpawner, Sprite icon) {
        _enemySpawner = enemySpawner;
        _icon.sprite = icon;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        gameObject.transform.localScale = new Vector3(1.2f, 1.2f);
    }

    public void OnPointerExit(PointerEventData eventData) {
        gameObject.transform.localScale = new Vector3(1f, 1f);
    }

    public void OnPointerClick(PointerEventData eventData) {
        _enemySpawner.EnableWaveEnemy();
    }
}
