using UnityEditor;

[CustomEditor(typeof(Potion))]
public class PotionEditor : Editor
{
    public override void OnInspectorGUI() {
        Potion _target = (Potion)target;
        _target.isSimplePotion = EditorGUILayout.Toggle("isSimplePotion", _target.isSimplePotion);

        if (!_target.isSimplePotion) {
            _target.healthMultiplier = EditorGUILayout.FloatField("healthMultiplier", _target.healthMultiplier);
            _target.damageMultiplier = EditorGUILayout.FloatField("damageMultiplier", _target.damageMultiplier);
        }
    }
}
