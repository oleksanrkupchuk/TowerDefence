using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class GenerateLevel : EditorWindow
{
    private string _cansavName = "Canvas";
    private string _eventSystemName = "EventSystem";
    private string _managersName = "Managers";
    private string _gridName = "GridMap";
    private string _groundName = "Ground";
    private string _roadName = "Road";
    private string _placeForTowerName = "PlaceForTower";
    private string _wayPointsName = "WayPoints";
    private string _gameMenuName = "GameMenu";
    private string _informationPanelName = "InformationPanel";
    private string _gameManagerName = "GameManager";
    private string _towerManagerName = "TowerManager";
    private string _enemySpawnerName = "EnemySpawner";

    private string _cloneName = "(Clone)";

    private Camera _mainCamera;
    private GameManager _gameManager;
    private TowerManager _towerManager;
    private EnemySpawner _enemySpawner;
    private GameMenu _gameMenu;
    private InformationPanel _informationPanel;

    private GameObject _settingsMenu;

    private string _description = "Generate map with of default managers and objects,\nanother object need add separately";

    [MenuItem("Generate new level/Create Level")]
    public static void CreateLevel() {
        CreateWindow<GenerateLevel>("Create Level");
    }

    private void OnGUI() {
        GUI.Label(new Rect(10, 10, 300, 60), _description);

        GUILayout.Space(70);

        _settingsMenu = (GameObject)EditorGUILayout.ObjectField("SettingsMenu", _settingsMenu, typeof(GameObject), true, new GUILayoutOption[0] { });

        if (GUILayout.Button("Generate")) {
            GenerateElementsOnScene();
        }
    }

    private void GenerateElementsOnScene() {
        if(_settingsMenu == null) {
            Debug.LogError("SettingsMenu is null");
        }

        _mainCamera = FindObjectOfType<Camera>();
        _mainCamera.gameObject.AddComponent<CameraMovement>().settingMenu = _settingsMenu.GetComponent<SettingsMenu>();
        _mainCamera.orthographicSize = 15;

        CreateAndSetupCanvas();
        CreateAndSetupManagers();
        CreateAndSetupTilemap();
        CreatePlaceForTowerAndWay();

        InitGameManager();
        InitTowerManager();
        InitEnemySpawner();
        InitGameMenu();
        InitInformationPanel();

        DestroyExtraObject();
        RemaneSpawnObject();
    }

    private void CreateAndSetupCanvas() {
        GameObject _canvas = Instantiate(new GameObject(_cansavName, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster)));

        Canvas _canvasComponent = _canvas.GetComponent<Canvas>();
        _canvasComponent.renderMode = RenderMode.ScreenSpaceCamera;
        _canvasComponent.worldCamera = _mainCamera;
        _canvasComponent.sortingOrder = 200;

        CanvasScaler _canvasScalerComponent = _canvas.GetComponent<CanvasScaler>();
        _canvasScalerComponent.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        _canvasScalerComponent.referenceResolution = new Vector2(1920, 1080);

        GameObject _eventSystem = Instantiate(new GameObject(_eventSystemName, typeof(EventSystem), typeof(StandaloneInputModule)));

        _gameMenu = Instantiate(Resources.Load("Menu/" + _gameMenuName, typeof(GameMenu))) as GameMenu;

        _informationPanel = Instantiate(Resources.Load("Menu/" + _informationPanelName, typeof(InformationPanel))) as InformationPanel;

        _gameMenu.transform.SetParent(_canvas.transform);
        _informationPanel.transform.SetParent(_canvas.transform);

        _gameMenu.transform.localScale = new Vector3(1f, 1f, 1f);
        ResetRectTransformToDefault(_gameMenu.GetComponent<RectTransform>());

        _informationPanel.transform.localScale = new Vector3(1f, 1f, 1f);
        ResetRectTransformToDefault(_informationPanel.GetComponent<RectTransform>());
    }

    private void ResetRectTransformToDefault(RectTransform rect) {
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);

        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 1);
    }

    private void CreateAndSetupManagers() {
        GameObject _managers = Instantiate(new GameObject(_managersName));

        _gameManager = Instantiate(Resources.Load("Managers/" + _gameManagerName, typeof(GameManager))) as GameManager;
        _towerManager = Instantiate(Resources.Load("Managers/" + _towerManagerName, typeof(TowerManager))) as TowerManager;
        _enemySpawner = Instantiate(Resources.Load("Managers/" + _enemySpawnerName, typeof(EnemySpawner))) as EnemySpawner;

        _gameManager.transform.SetParent(_managers.transform);
        _towerManager.transform.SetParent(_managers.transform);
        _enemySpawner.transform.SetParent(_managers.transform);
    }

    private void CreateAndSetupTilemap() {
        GameObject _grid = Instantiate(new GameObject(_gridName));
        _grid.AddComponent<Grid>();

        GameObject _timeMapGround = Instantiate(new GameObject(_groundName, typeof(Tilemap)));
        _timeMapGround.AddComponent<TilemapRenderer>().sortingOrder = -1000;

        GameObject _timeMapRoad = Instantiate(new GameObject(_roadName, typeof(Tilemap)));
        _timeMapRoad.AddComponent<TilemapRenderer>().sortingOrder = -950;

        _timeMapGround.transform.SetParent(_grid.transform);
        _timeMapRoad.transform.SetParent(_grid.transform);
    }

    private void CreatePlaceForTowerAndWay() {
        GameObject _placeForTower = Instantiate(new GameObject(_placeForTowerName));
        GameObject _ways = Instantiate(new GameObject(_wayPointsName));
    }

    private void InitGameManager() {
        _gameManager.enemySpawner = _enemySpawner;
        _gameManager.informationPanel = _informationPanel;
        _gameManager.gameMenu = _gameMenu;
    }

    private void InitTowerManager() {
        _towerManager.gameManager = _gameManager;
        _towerManager.mainCamera = _mainCamera;
    }

    private void InitEnemySpawner() {
        GameObject _waves = new GameObject("Wawes");
        _enemySpawner.gameManager = _gameManager;
        _enemySpawner.mainCamera = _mainCamera;
        _enemySpawner.waveObject = _waves;
    }

    private void InitGameMenu() {
        _gameMenu.gameManager = _gameManager;
    }

    private void InitInformationPanel() {
        _informationPanel.gameManager = _gameManager;
        _informationPanel.towerManager = _towerManager;
        _informationPanel.enemySpawner = _enemySpawner;
    }

    private void DestroyExtraObject() {
        DestroyImmediate(GameObject.Find(_managersName));
        DestroyImmediate(GameObject.Find(_gridName));
        DestroyImmediate(GameObject.Find(_groundName));
        DestroyImmediate(GameObject.Find(_roadName));
        DestroyImmediate(GameObject.Find(_cansavName));
        DestroyImmediate(GameObject.Find(_eventSystemName));
        DestroyImmediate(GameObject.Find(_placeForTowerName));
        DestroyImmediate(GameObject.Find(_wayPointsName));
    }

    private void RemaneSpawnObject() {
        GameObject.Find(_cansavName + _cloneName).name = _cansavName;
        GameObject.Find(_eventSystemName + _cloneName).name = _eventSystemName;
        GameObject.Find(_managersName + _cloneName).name = _managersName;
        GameObject.Find(_gameMenuName + _cloneName).name = _gameMenuName;
        GameObject.Find(_informationPanelName + _cloneName).name = _informationPanelName;
        GameObject.Find(_gameManagerName + _cloneName).name = _gameManagerName;
        GameObject.Find(_towerManagerName + _cloneName).name = _towerManagerName;
        GameObject.Find(_enemySpawnerName + _cloneName).name = _enemySpawnerName;
        GameObject.Find(_gridName + _cloneName).name = _gridName;
        GameObject.Find(_groundName + _cloneName).name = _groundName;
        GameObject.Find(_roadName + _cloneName).name = _roadName;
        GameObject.Find(_placeForTowerName + _cloneName).name = _placeForTowerName;
        GameObject.Find(_wayPointsName + _cloneName).name = _wayPointsName;
    }
}
