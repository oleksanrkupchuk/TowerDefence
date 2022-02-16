using UnityEngine;

public class ChangeEnemyLayer : MonoBehaviour
{
    [SerializeField]
    private int _maxEnemyLayer;
    [SerializeField]
    private int _minEnemyLayer;
    [SerializeField]
    private bool _startLargeEnemyLayer;

    private void OnEnable() {
        GameManager.SpawnNewWave += ValueReseEnemyLayer;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.TryGetComponent(out Enemy enemy)) {
            if (_startLargeEnemyLayer) {
                enemy.Setlayer(_maxEnemyLayer);
                _maxEnemyLayer--;
            }
            else {
                enemy.Setlayer(_minEnemyLayer);
                _minEnemyLayer++;
            }
        }
    }

    public void ValueReseEnemyLayer() {
        _maxEnemyLayer = 50;
        _minEnemyLayer = 0;
    }

    private void OnDestroy() {
        GameManager.SpawnNewWave -= ValueReseEnemyLayer;
    }
}
