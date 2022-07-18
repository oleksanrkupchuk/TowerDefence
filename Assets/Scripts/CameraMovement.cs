using UnityEngine;

public class CameraMovement : MonoBehaviour {
    private Vector3 _mouseClick;
    private Vector3 _mouseSwipe;
    private bool _isResolution640x480 = false;
    private float _leftBorderCamera = -6.5f;
    private float _rightBorderCamera = 6.5f;

    public SettingsMenu settingMenu;

    private void OnEnable() {
        SettingsMenu.ChangeScreenResolution += SetResolution640x480;
        LoadAndApplyScreenSetting();
    }

    private void LoadAndApplyScreenSetting() {
        //_settingMenu.LoadSettingsAndSetResolution();
    }
   
    private void SetResolution640x480(float weidth) {
        if (weidth == 640) {
            _isResolution640x480 = true;
        }
        else {
            _isResolution640x480 = false;
            transform.position = new Vector3(0f, 0f, transform.position.z);
        }
    }

    private void Update() {
        if (_isResolution640x480) {
            MoveCamera();
        }
    }

    private void MoveCamera() {
        if (Input.GetButtonDown("Fire2")) {
            _mouseClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetButton("Fire2")) {
            _mouseSwipe = Camera.main.ScreenToWorldPoint(Input.mousePosition - _mouseClick);
            float _distance = _mouseClick.x - _mouseSwipe.x;

            if (_distance >= 0) {
                transform.position = new Vector3(transform.position.x + Mathf.Abs(_distance) * Time.deltaTime,
                    transform.position.y, transform.position.z);

                if (transform.position.x > _rightBorderCamera) {
                    transform.position = new Vector3(_rightBorderCamera, transform.position.y, transform.position.z);
                }
            }
            if (_distance < 0) {
                transform.position = new Vector3(transform.position.x - Mathf.Abs(_distance) * Time.deltaTime,
                    transform.position.y, transform.position.z);

                if (transform.position.x < _leftBorderCamera) {
                    transform.position = new Vector3(_leftBorderCamera, transform.position.y, transform.position.z);
                }
            }
        }
    }

    private void OnDestroy() {
        SettingsMenu.ChangeScreenResolution -= SetResolution640x480;
    }
}
