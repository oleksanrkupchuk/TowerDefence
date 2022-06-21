using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Potion : MonoBehaviour {
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rigidbody;
    private Vector3 _startPosition;

    public bool isSimplePotion;
    public float healthMultiplier;
    public float damageMultiplier;

    public void Init() {
        _startPosition = transform.position;
    }

    private void Awake() {
        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.isTrigger = true;
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0;
    }

    private void OnMouseDrag() {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    private void OnMouseUp() {
        if (transform.position != _startPosition) {
            transform.position = _startPosition;
        }
    }
}
