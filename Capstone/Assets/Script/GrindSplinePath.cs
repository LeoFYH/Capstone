using UnityEngine;
using UnityEngine.U2D;
using System.Collections.Generic;

public class GrindSplinePath : MonoBehaviour
{
    private SpriteShapeController spriteShapeController;
    private List<Vector3> trackPoints = new List<Vector3>();

    private void Awake()
    {
        spriteShapeController = GetComponent<SpriteShapeController>();
        SampleSplinePoints();
    }

    // 采样轨道上的节点
    private void SampleSplinePoints()
    {
        trackPoints.Clear();

        var spline = spriteShapeController.spline;
        int pointCount = spline.GetPointCount();

        for (int i = 0; i < pointCount; i++)
        {
            // 轨道节点坐标是相对spriteShape对象本地坐标的，转换为世界坐标
            Vector3 localPos = spline.GetPosition(i);
            Vector3 worldPos = spriteShapeController.transform.TransformPoint(localPos);
            trackPoints.Add(worldPos);
        }
    }

    // 通过t(0~1)沿轨道插值获取位置
    public Vector3 GetPositionOnTrack(float t)
    {
        if (trackPoints.Count < 2)
            return transform.position;

        float totalSegments = trackPoints.Count - 1;
        float exactPos = t * totalSegments;

        int indexA = Mathf.FloorToInt(exactPos);
        int indexB = Mathf.Min(indexA + 1, trackPoints.Count - 1);

        float lerpT = exactPos - indexA;

        return Vector3.Lerp(trackPoints[indexA], trackPoints[indexB], lerpT);
    }
}
