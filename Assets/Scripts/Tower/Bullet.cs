using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] 
    private float _speed;
    [SerializeField]
    private int _damageBasic;
    [SerializeField]
    private int _damage;
    public int Damage { get => _damage; set => _damage = value; }

    private Vector3 _direction;
    private float _angleDirection;
    private Enemy _target;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if(!_target.isDead) {
            _direction = _target.transform.position - transform.position;
            _angleDirection = Mathf.Atan2(_direction.x, _direction.y) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.AngleAxis(_angleDirection, Vector3.forward);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(_target.transform.position.x, _target.transform.position.y + _target.CenterBoxColliderOnY, _target.transform.position.z), _speed * Time.deltaTime);
        }
        else {
            _direction = _target.diePosition - transform.position;
            _angleDirection = Mathf.Atan2(_direction.x, _direction.y) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.AngleAxis(_angleDirection, Vector3.forward);
            transform.position = Vector3.MoveTowards(transform.position, _target.diePosition, _speed * Time.deltaTime);
            if(transform.position == _target.diePosition) {
                Destroy(gameObject);
            }
        }
    }

    public void SetTarget(Enemy enemy) {
        _target = enemy;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag(Tags.enemy)) {
            _target.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }

    public void SetBasicDamage() {
        _damage = _damageBasic;
    }
}
