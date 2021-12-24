using UnityEngine;

public class Bezier
{
    //(1 - t)^2 * P0 + 2 * t * (1 - t) * P1 + t^2 * P2
    public static Vector2 GetTrajectoryForBullet(Vector2 p0, Vector2 p1, Vector2 p2, float t) {
        t = Mathf.Clamp01(t);
        float _oneMinusT = 1 - t;
        return (_oneMinusT * _oneMinusT * p0) +
            (2 * t * _oneMinusT * p1) + (t * t * p2);

    }
}
