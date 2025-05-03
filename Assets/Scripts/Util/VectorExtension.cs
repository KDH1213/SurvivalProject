using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtension
{
    public static Vector2 ConvertVector2(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }
    public static Vector2 ToVector2(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }
    public static Vector2 ToVector2XZ(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }
    public static Vector3 ToVector3(this Vector2 vec, float z = 0)
    {
        return new Vector3(vec.x, vec.y, z);
    }
    public static Vector3 ToVector3XZ(this Vector2 vec, float y = 0)
    {
        return new Vector3(vec.x, y, vec.y);
    }
}
