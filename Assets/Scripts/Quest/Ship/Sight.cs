using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour {
    private Vector2 _mousePosition;
    private bool _canShoot = false;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private Ship _ship;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (gameObject.activeSelf == true) {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = _mousePosition;

            if (Input.GetKey(KeyCode.Mouse0) && _canShoot) {
                _canShoot = false;
                _ship.positionForExplosions = GetRandomPointsInCrosshair();
                _ship.ShootCannon();
                StartCoroutine(_ship.EnableExplosionCannon());
                _spriteRenderer.sprite = null;
            }

            if (_canShoot) {
                _spriteRenderer.color = Color.green;
            }
            else {
                _spriteRenderer.color = Color.red;
            }
        }
    }

    private List<Vector2> GetRandomPointsInCrosshair() {
        List<Vector2> _randomPoints = new List<Vector2>();
        for (int i = 0; i < 7; i++) {
            Vector2 _randomPoint = (Vector2)transform.position + Random.insideUnitCircle * 2f;
            _randomPoints.Add(_randomPoint);
        }

        return _randomPoints;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(Tags.road)) {
            _canShoot = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(Tags.road)) {
            _canShoot = false;
        }
    }
}
