using UnityEngine;

public class Track : MonoBehaviour
{
    [Header("轨道设置")]
    public float grindSpeed = 8f; // 滑轨速度
    public bool isActive = true; // 轨道是否激活

    // 获取滑轨速度
    public float GetGrindSpeed()
    {
        return grindSpeed;
    }

    // 检查轨道是否有效
    public bool IsValid()
    {
        return isActive;
    }

    // 获取轨道位置（简化版本，直接使用Transform）
    public Vector3 GetTrackPosition()
    {
        return transform.position;
    }

    // 获取轨道方向
    public Vector3 GetTrackDirection()
    {
        return transform.right; // 使用Transform的右方向作为轨道方向
    }
}
