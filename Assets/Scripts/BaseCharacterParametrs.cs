using UnityEngine;

public abstract class BaseCharacterParametrs: MonoBehaviour
{
    public float damage;
    public float maxHealth;
    public float health;
    public float speed;
    public bool isFlipLeft = false;

    public virtual void TakeDamage(float damage) { }
}
