using UnityEngine;
using System.Collections;

public class Shadow : MonoBehaviour
{
    public Vector3 InitialScale;
    public float DistanceFromTarget = 1f;
    public float FloorPosition;
    public Vector3 Offset;

    public void SetBaseScale(Vector3 scale)
    {
        InitialScale = scale;
    }

    public void SetFloorPosition(float floor)
    {
        FloorPosition = floor;
    }

    public void SetDistance(float distance)
    {
        DistanceFromTarget = distance;

        if (DistanceFromTarget < 1)
        {
            DistanceFromTarget = 1; 
        }

        // Update scale
        transform.localScale = InitialScale / (DistanceFromTarget);
    }

    public void UpdatePosition(Vector3 position)
    {
        transform.position = position.SetY(FloorPosition) + Offset;
    }

    public void SetOffset(Vector3 vector3)
    {
        Offset = vector3;
    }
}
