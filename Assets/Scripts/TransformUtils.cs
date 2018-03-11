using UnityEngine;

public static class TransformUtils
{
    public static Vector2 position2D(this Transform transform)
    {
        return transform.position;
    }
}