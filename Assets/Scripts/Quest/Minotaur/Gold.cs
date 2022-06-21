using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Gold : MonoBehaviour {
    private GameManager _gameManager;

    [SerializeField]
    private int _coin;

    public void Init(GameManager gameManager) {
        _gameManager = gameManager;
    }

    private void OnMouseDown() {
        _gameManager.AddCoin(_coin);
        gameObject.SetActive(false);
    }
}
