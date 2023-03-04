using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LaserTower))]
public class LaserTowerEditor : Editor
{
    Module module;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LaserTower towerAi = (LaserTower)target;

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
