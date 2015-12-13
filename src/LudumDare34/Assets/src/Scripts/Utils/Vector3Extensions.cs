using System;
using UnityEngine;
using System.Collections;

public static class Vector3Extensions
{
    public static Vector3 SetY(this Vector3 position, float val)
    {
        return new Vector3(
            position.x,
            val,
            position.z);
    }
}

