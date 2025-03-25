using UnityEngine;

public static class HexMetrics
{
    public const float outerRadius = 10f;//
    public const float innerRadius = outerRadius * 0.866025404f;//根号3除以2的近似值，计算机图形学的真好，也就是cos30°

    public static Vector3[] corners = {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius)
    };
}