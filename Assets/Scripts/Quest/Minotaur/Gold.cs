using UnityEngine;
using UnityEngine.EventSystems;

public class Gold : MonoBehaviour, IPointerDownHandler {
    private GameManager _gameManager;

    [SerializeField]
    private int _coin;

    public void Init(GameManager gameManager) {
        _gameManager = gameManager;
    }

    public void OnPointerDown(PointerEventData eventData) {
        _gameManager.AddCoin(_coin);
        gameObject.SetActive(false);
    }
}
