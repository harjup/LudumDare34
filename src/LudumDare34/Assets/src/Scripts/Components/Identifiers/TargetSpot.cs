using UnityEngine;
using System.Collections;

public class TargetSpot : MonoBehaviour 
{
    public enum TargetType
    {
        Unknown,
        LeftStart,
        LeftTarget,
        RightStart,
        RightTarget,
        EnemyStart,
        EnemyTarget
    }

    public TargetType Target;
}
