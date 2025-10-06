using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LogicalRail : MonoBehaviour
{
    [Tooltip("轨道上的节点（按顺序）")]
    public List<RailNode> nodes = new List<RailNode>();

    [Tooltip("每段曲线的采样点数")]
    [Range(4, 256)] public int samplesPerSegment = 32;

    [Tooltip("是否闭合曲线")]
    public bool loop;

    // —— 节点管理 ————————————————

    public RailNode AddNodeFromResources(string path)
    {
        var go = Instantiate(Resources.Load<GameObject>(path), transform);
        var node = go.GetComponent<RailNode>();
        node.SendMessage("Reset");   // 触发 RailNode.Reset() 自动抓第一个子物体为 handle
        nodes.Add(node);
        return node;
    }

#if UNITY_EDITOR
    public RailNode AddNodeFromAssetPath()
    {
        const string path = "Assets/Prefab/EditableRail/RailNode.prefab";   // 写死路径
        var prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (prefab == null) { Debug.LogError($"Asset 路径不存在：{path}"); return null; }

        var go = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab, transform);
        go.name = $"RailNode_{nodes.Count}";
        var node = go.GetComponent<RailNode>();
        node.SendMessage("Reset");
        node.transform.localPosition = Vector3.zero;
        nodes.Add(node);
        return node;
    }
#endif

    /// <summary>按层级顺序刷新列表（可在需要时调用）。</summary>
    public void RefreshNodesFromChildren()
    {
        nodes.Clear();
        foreach (Transform t in transform)
        {
            var n = t.GetComponent<RailNode>();
            if (n) nodes.Add(n);
        }
    }

    public void ClearAllNodes()
    {
    #if UNITY_EDITOR
        foreach (var n in nodes)
        {
            if (n) UnityEngine.Object.DestroyImmediate(n.gameObject);
        }
    #else
        foreach (var n in nodes)
        {
            if (n) Destroy(n.gameObject);
        }
    #endif

        nodes.Clear();   // 清空列表
    }

    

    // —— 曲线计算 ————————————————

    /// <summary>返回第 i 段（i 到 i+1）的第 k 个采样位置（0..samplesPerSegment-1）。</summary>
    public Vector2 GetPointOnSegment(int i, int k)
    {
        int j = (i + 1) % nodes.Count;
        var n0 = nodes[i];
        var n1 = nodes[j];

        Vector2 p0 = n0.transform.position;                           // 起点
        Vector2 p1 = n0.handle
                    ? (Vector2)n0.handle.position
                    : p0;                                            // 出手柄（已有）

        Vector2 p1Sym = p0 * 2 - p1;   // 计算对称手柄 = 节点中心的镜像

        Vector2 p3 = n1.transform.position;                           // 终点
        Vector2 p2 = n1.handle
                    ? (Vector2)n1.handle.position
                    : p3;                                            // 入手柄（已有）

        Vector2 p2Sym = p3 * 2 - p2;   // 计算终点的对称手柄

        float t = (samplesPerSegment <= 1) ? 0f : (float)k / (samplesPerSegment - 1);
        return Cubic(p0, p1Sym, p2, p3, t);
    }

    static Vector2 Cubic(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
    {
        float u = 1f - t;
        return u * u * u * p0 + 3f * u * u * t * p1 + 3f * u * t * t * p2 + t * t * t * p3;
    }

    // —— 可视化（编辑器模式下始终绘制） ————————————————

    void OnDrawGizmos()
    {
        if (nodes == null || nodes.Count < 2) return;

        Gizmos.color = Color.white;

        int segCount = loop ? nodes.Count : nodes.Count - 1;
        for (int s = 0; s < segCount; s++)
        {
            Vector2 prev = GetPointOnSegment(s, 0);
            for (int k = 1; k < samplesPerSegment; k++)
            {
                Vector2 curr = GetPointOnSegment(s, k);
                Gizmos.DrawLine(prev, curr);
                prev = curr;
            }
        }

        // 画锚点与手柄的辅助
        Gizmos.color = Color.yellow;
        foreach (var n in nodes)
        {
            if (!n) continue;
            Gizmos.DrawSphere(n.transform.position, 0.05f);
            if (n.handle)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(n.transform.position, n.handle.position);
                Gizmos.DrawSphere(n.handle.position, 0.035f);
                Gizmos.color = Color.yellow;
            }
        }
    }
}


public class TrackDirComputeTool
{
    public Vector3 origin;                 // 轨道起点（世界坐标）
    private readonly LogicalRail rail;     // 引用你的曲线对象

    public TrackDirComputeTool(LogicalRail rail)
    {
        this.rail = rail;
        if (rail.nodes != null && rail.nodes.Count > 0 && rail.nodes[0] != null)
            this.origin = rail.nodes[0].transform.position;
        else
            this.origin = Vector3.zero;
    }

    /// <summary>
    /// 输入一个世界坐标点，返回：
    ///  - 最近点在轨道上的世界坐标
    ///  - 该点处的切线方向（已归一化）
    /// </summary>
    public (Vector3 nearest, Vector3 tangent) GetNearestPointAndTangent(Vector3 worldPos)
    {
        Vector3 local = worldPos;       
        float minDist = float.MaxValue;
        Vector3 bestPoint = Vector3.zero;
        Vector3 bestTan = Vector3.right;

        // 在曲线上采样搜索最近点
        int segCount = rail.loop ? rail.nodes.Count : rail.nodes.Count - 1;
        for (int s = 0; s < segCount; s++)
        {
            for (int k = 0; k < rail.samplesPerSegment; k++)
            {
                Vector3 p = rail.GetPointOnSegment(s, k);   // 轨道上的点（世界坐标）
                Debug.Log("计算了点" + p);
                float d = (p - local).sqrMagnitude;
                if (d < minDist)
                {
                    minDist = d;
                    bestPoint = p;
                    // 切线用前后相邻两个点求差近似
                    Vector3 pNext = rail.GetPointOnSegment(s, Mathf.Min(k + 1, rail.samplesPerSegment - 1));
                    bestTan = (pNext - p).normalized;
                }
            }
        }

        return (bestPoint, bestTan);  // 返回最近点换回世界坐标
    }
}
