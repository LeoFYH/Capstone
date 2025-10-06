using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RailNode))]
public class RailNodeEditor : Editor
{

    public override void OnInspectorGUI()
    {
        // 显示原本的Inspector
        base.OnInspectorGUI();

        RailNode node = (RailNode)target;

        // 添加按钮
        if (GUILayout.Button("Reset Handle"))
        {
            node.ResetHandle();
        }
    }
}
