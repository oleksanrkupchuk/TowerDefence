using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPoint : MonoBehaviour
{
    private Bullet _bullet;
    
    public void SetBullet(Bullet bullet) {
        _bullet = bullet;
    }

    private void Start() {
        _bullet.DestroyBeizerPoint += DestroyPoint;
    }

    public void DestroyPoint() {
        _bullet.SetBezierPointsNull();
        Destroy(gameObject);
    }
}
