using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tower))]
public class TowerEditor : Editor
{
    Module module;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Tower towerAi = (Tower)target;

        if (GUILayout.Button("Upgrade Tower"))
        {
            towerAi.UpgradeTower();
        }

        if (GUILayout.Button("Degrade Tower"))
        {
            towerAi.DegradeTower();
        }

        GUILayout.Label("Upgrade", EditorStyles.boldLabel);

        module = EditorGUILayout.ObjectField("Module", module, typeof(ScriptableObject), true) as Module;

        if (GUILayout.Button("Submit"))
        {
            towerAi.MountUpgrade(module);
        }
    }
}
