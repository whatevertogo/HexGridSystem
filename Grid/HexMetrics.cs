using UnityEngine;

namespace Grid
{
    /// <summary>
    /// 六边形网格系统的核心工具类，提供了六边形的几何属性和顶点位置属性
    /// </summary>
    public static class HexMetrics
    {
        public static float outerRadius = 1f; //六边形的外接圆半径
        public static float innerRadius => outerRadius * 0.866025404f; //六边形的内接圆半径

        /// <summary>
        /// 一个Vector3数组，表示六边形的6个顶点坐标
        /// </summary>
        /// <value></value>
        public static readonly Vector3[] corners =
        {
            new Vector3(0f, outerRadius, 0f), // 北
            new Vector3(innerRadius, 0.5f * outerRadius, 0f), // 东北
            new Vector3(innerRadius, -0.5f * outerRadius, 0f), // 东南
            new Vector3(0f, -outerRadius, 0f), // 南
            new Vector3(-innerRadius, -0.5f * outerRadius, 0f), // 西南
            new Vector3(-innerRadius, 0.5f * outerRadius, 0f), // 西北
            new Vector3(0f, outerRadius, 0f), // 回到北点，闭合
        };

        // 边的前一个顶点
        public static Vector3 GetFirstCorner(HexDirection direction)
        {
            return corners[(int)direction];
        }

        // 边的后一个顶点
        public static Vector3 GetSecondCorner(HexDirection direction)
        {
            return corners[(int)direction + 1];
        }
    }
}
