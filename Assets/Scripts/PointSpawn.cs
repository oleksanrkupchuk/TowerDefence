using UnityEngine;

public enum Sign {
    Minus,
    Plus
}

public enum Axis {
    X,
    Y
}

public class PointSpawn : MonoBehaviour
{
    [SerializeField]
    private Transform _firstWayPoint;
    [SerializeField]
    private Sign _sign = new Sign();
    [SerializeField]
    private Axis _axis = new Axis();
    [SerializeField]
    private float _step;

    private void Start() {
        SettingsMenu.ChangeScreenResolution += PositionPointSpawn;
        PositionPointSpawn();
    }

    public void PositionPointSpawn() {
        switch (_sign) {
            case Sign.Minus:
                SetMinusPosition();
                break;

            case Sign.Plus:
                SetPlusPosition();
                break;
        }
    }

    public void SetMinusPosition() {
        switch (_axis) {
            case Axis.X:
                transform.position = new Vector2(_firstWayPoint.position.x - _step, _firstWayPoint.position.y);
                break;

            case Axis.Y:
                transform.position = new Vector2(_firstWayPoint.position.x, _firstWayPoint.position.y - _step);
                break;
        }
    }

    public void SetPlusPosition() {
        switch (_axis) {
            case Axis.X:
                transform.position = new Vector2(_firstWayPoint.position.x + _step, _firstWayPoint.position.y);
                break;

            case Axis.Y:
                transform.position = new Vector2(_firstWayPoint.position.x, _firstWayPoint.position.y + _step);
                break;
        }
    }
}
