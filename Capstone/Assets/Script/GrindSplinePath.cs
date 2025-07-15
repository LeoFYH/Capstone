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

    // ��������ϵĽڵ�
    private void SampleSplinePoints()
    {
        trackPoints.Clear();

        var spline = spriteShapeController.spline;
        int pointCount = spline.GetPointCount();

        for (int i = 0; i < pointCount; i++)
        {
            // ����ڵ����������spriteShape���󱾵�����ģ�ת��Ϊ��������
            Vector3 localPos = spline.GetPosition(i);
            Vector3 worldPos = spriteShapeController.transform.TransformPoint(localPos);
            trackPoints.Add(worldPos);
        }
    }

    // ͨ��t(0~1)�ع����ֵ��ȡλ��
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
