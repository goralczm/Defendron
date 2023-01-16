using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TowerAi))]
public class TowerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TowerAi towerAi = (TowerAi)target;

        if (GUILayout.Button("Upgrade Tower"))
        {
            towerAi.UpgradeTower(false);
        }

        if (GUILayout.Button("Degrade Tower"))
        {
            towerAi.DegradeTower();
        }
    }
}
