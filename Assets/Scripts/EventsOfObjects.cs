using UnityEngine;

public class EventsOfObjects : MonoBehaviour {
    private static EventsOfObjects _eventOfObjects;

    [SerializeField]
    private Tower[] _towers;
    [SerializeField]
    private Bullet[] _bullet;
    [SerializeField]
    private RockBullet _rockBullet;
    [SerializeField]
    private Enemy[] _enemy;

    private void Awake() {
        if (_eventOfObjects == null) {
            _eventOfObjects = this;
        }
        else if (_eventOfObjects != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        AddEventForTowerAnimation();
        AddEventForBulletAnimation();
        AddEventForEnemyAnimation();
    }

    private void AddEventForTowerAnimation() {
        for (int i = 0; i < _towers.Length; i++) {
            //_towers[i].AddResetShootEventForShootAnimation();
            _towers[i].AddShootEventForShootAnimation();
        }
    }

    private void AddEventForBulletAnimation() {
        for (int i = 0; i < _bullet.Length; i++) {
            _bullet[i].AddDestroyEventForDestroyAnimation();
        }
    }

    private void AddEventForEnemyAnimation() {
        for (int i = 0; i < _enemy.Length; i++) {
            _enemy[i].AddDestroyEventForDeadAnimation();
            _enemy[i].AddEnableEventForAttackAnimation();
            _enemy[i].AddDisableEventForAttackAnimation();
        }
    }
}
