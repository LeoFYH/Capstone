using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LogicalRail))]
public class LogicalRailEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 保留原有 Inspector
        DrawDefaultInspector();

        LogicalRail rail = (LogicalRail)target;

        if (GUILayout.Button("Add Fixed Node"))
        {
            rail.AddNodeFromAssetPath();   // 调用写死路径的方法
        }

        if (GUILayout.Button("Refresh Node"))
        {
            rail.RefreshNodesFromChildren();   // 调用写死路径的方法
        }

        if (GUILayout.Button("Clear All Node"))
        {
            rail.ClearAllNodes();   // 调用写死路径的方法
        }
    }
}
