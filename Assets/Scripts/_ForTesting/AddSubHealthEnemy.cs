using UnityEngine;
using UnityEngine.UI;

public class AddSubHealthEnemy : MonoBehaviour
{
    [SerializeField]
    private Button _addHealth;
    [SerializeField]
    private Button _subHealth;
    [SerializeField]
    private Enemy _enemy;
    [SerializeField]
    private float _percentageOfRecovery;
    [SerializeField]
    private float _takeAwayHealth;

    private void Awake() {
        _addHealth.onClick.AddListener(() => {
            _enemy.AddHealth(_percentageOfRecovery);
        });

        _subHealth.onClick.AddListener(() => {
            _enemy.TakeDamage(_takeAwayHealth);
        });
    }
}
