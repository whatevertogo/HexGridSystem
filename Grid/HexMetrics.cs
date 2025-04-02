using UnityEngine;

namespace Core.Grid
{
    public static class HexMetrics
    {
        public const float outerRadius = 1f;
        public const float innerRadius = outerRadius * 0.866025404f;

        public static readonly Vector3[] corners = {
            new Vector3(0f, outerRadius, 0f),          // 北
            new Vector3(innerRadius, 0.5f * outerRadius, 0f),    // 东北
            new Vector3(innerRadius, -0.5f * outerRadius, 0f),   // 东南
            new Vector3(0f, -outerRadius, 0f),         // 南
            new Vector3(-innerRadius, -0.5f * outerRadius, 0f),  // 西南
            new Vector3(-innerRadius, 0.5f * outerRadius, 0f),   // 西北
            new Vector3(0f, outerRadius, 0f)           // 回到北点，闭合
        };
    }
}