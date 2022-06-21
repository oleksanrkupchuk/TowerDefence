using UnityEngine;

public abstract class BaseCharacterParametrs: MonoBehaviour
{
    public float damage;
    public float maxHealth;
    public float health;
    public float speed;
    public bool isFlipLeft = false;

    public virtual void TakeDamage(float damage) { }

    public void Move(Transform toObject) {
        transform.position = Vector2.MoveTowards(transform.position, toObject.position, speed * Time.deltaTime);
    }

    public void CheckFlipSprite(Transform target) {
        if (transform.position.x - target.position.x < 0 && isFlipLeft) {
            FlipSprite();
        }

        if (transform.position.x - target.position.x > 0 && !isFlipLeft) {
            FlipSprite();
        }
    }

    public void FlipSprite() {
        isFlipLeft = !isFlipLeft;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
    }
}
