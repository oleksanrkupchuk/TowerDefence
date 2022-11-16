using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Room))]
public class RoomDraw : PropertyDrawer {
    private RoomColors _roomColors;

    private const float FOLDOUT_HEIGHT = 18;
    private int _amountFieldInTheClassRoom = 4;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if (_roomColors == null) {
            _roomColors = GameObject.FindGameObjectWithTag("RoomColors").GetComponent<RoomColors>();
        }

        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        SerializedProperty _colorProperty = property.FindPropertyRelative("color");
        SerializedProperty _amountOfTablesProperty = property.FindPropertyRelative("amountOfTables");
        SerializedProperty _amountOfChairsProperty = property.FindPropertyRelative("amountOfChairs");
        SerializedProperty _subRoomProperty = property.FindPropertyRelative("subRooms");

        var _availableColors = _roomColors.colors.Select(item => new GUIContent(item)).ToArray();
        var popUpHeight = EditorGUI.GetPropertyHeight(_colorProperty);


        var _colorPosition = new Rect(position.x, position.y, position.width, FOLDOUT_HEIGHT);
        var _amountOfTablesPosition = new Rect(position.x, position.y + FOLDOUT_HEIGHT, position.width, FOLDOUT_HEIGHT);
        var _amountOfChairsPosition = new Rect(position.x, position.y + (FOLDOUT_HEIGHT * 2), position.width, FOLDOUT_HEIGHT);
        var _subRoomPosition = new Rect(position.x, position.y + (FOLDOUT_HEIGHT * 3), position.width, FOLDOUT_HEIGHT * 3);

        _colorProperty.intValue = EditorGUI.Popup(_colorPosition, new GUIContent(_colorProperty.displayName), _colorProperty.intValue, _availableColors);
        EditorGUI.PropertyField(_amountOfTablesPosition, _amountOfTablesProperty, new GUIContent(_amountOfTablesProperty.displayName));
        EditorGUI.PropertyField(_amountOfChairsPosition, _amountOfChairsProperty, new GUIContent(_amountOfChairsProperty.displayName));
        EditorGUI.PropertyField(_subRoomPosition, _subRoomProperty, new GUIContent(_subRoomProperty.displayName));

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        SerializedProperty _subRoomProperty = property.FindPropertyRelative("subRooms");

        float height = FOLDOUT_HEIGHT * _amountFieldInTheClassRoom;
        if (_subRoomProperty.isExpanded) {
            if (_subRoomProperty.arraySize == 0) {
                height += FOLDOUT_HEIGHT * 2;
            }

            for (int i = 0; i < _subRoomProperty.arraySize; i++) {
                height += EditorGUI.GetPropertyHeight(_subRoomProperty.GetArrayElementAtIndex(i), true);
            }
            height += FOLDOUT_HEIGHT * 2;
        }
        else {
            height = (FOLDOUT_HEIGHT * _amountFieldInTheClassRoom) + 10;
        }

        return height;
    }
}
