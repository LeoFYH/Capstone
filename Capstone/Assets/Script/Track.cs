using UnityEngine;

public class Track : MonoBehaviour
{
    public Vector3 GetTrackPosition()
    {
        return transform.position;
    }

    public Vector3 GetTrackDirection()
    {
        return transform.right;
    }
}
