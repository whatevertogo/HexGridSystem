using System;
using Grid;
using UnityEngine;

[Serializable]
/// <summary>
/// 结构体用来表示六边形网格的坐标系统
/// 作用是提供六边形网格的坐标表示和相关操作
/// 从世界坐标转换为六边形坐标，计算邻居坐标等
/// </summary>
public struct HexCoordinates : IEquatable<HexCoordinates>
{
    [SerializeField]
    private int x,
        y;

    public int X => x;
    public int Y => y;
    public int Z => -X - Y;

    public HexCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// 将偏移坐标转换为六边形坐标
    /// </summary>
    /// <param name="x">左下开始数的横轴</param>
    /// <param name="y">左下开始数的竖轴</param>
    /// <returns></returns>
    public static HexCoordinates FromOffsetCoordinates(int x, int y)
    {
        return new HexCoordinates(x - y / 2, y);
    }

    /// <summary>
    /// 将世界坐标转换为六边形坐标
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static HexCoordinates FromPosition(Vector3 position)
    {
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float y = position.y / (HexMetrics.outerRadius * 1.5f);

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x - y);

        if (iX + iY + iZ != 0)
        {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x - y - iZ);

            if (dX > dY && dX > dZ)
            {
                iX = -iY - iZ;
            }
            else if (dZ > dY)
            {
                iZ = -iX - iY;
            }
        }

        return new HexCoordinates(iX, iY);
    }

    /// <summary>
    /// 方向偏移数组，比字典更高效
    /// </summary>
    private static readonly Vector2Int[] directionVectors =
    {
        new Vector2Int(1, -1), // NE
        new Vector2Int(1, 0), // E
        new Vector2Int(0, 1), // SE
        new Vector2Int(-1, 1), // SW
        new Vector2Int(-1, 0), // W
        new Vector2Int(0, -1), // NW
    };

    /// <summary>
    /// 通过方向得到邻居坐标
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public HexCoordinates GetNeighbor(HexDirection direction)
    {
        Vector2Int offset = directionVectors[(int)direction];
        return new HexCoordinates(x + offset.x, y + offset.y);
    }

    /// <summary>
    /// 计算两个六边形坐标之间的距离
    /// </summary>
    /// <param name="other">目标坐标</param>
    /// <returns>距离</returns>
    public int DistanceTo(HexCoordinates other)
    {
        // 在六边形坐标系统中，距离是三个轴向差值的最大值
        return Mathf.Max(Mathf.Abs(X - other.X), Mathf.Abs(Y - other.Y), Mathf.Abs(Z - other.Z));
    }

    /// <summary>
    /// 两个HexCoordinates坐标位置相等的判断，运算符重载了
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(HexCoordinates other)
    {
        return x == other.x && y == other.y;
    }

    /// <summary>
    /// 修bug用
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"({X}, {Y}, {Z})";
    }

    #region 运算符重载
    /// <summary>
    /// 哈希值计算
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }


    public override bool Equals(object obj)
    {
        return obj is HexCoordinates other && Equals(other);
    }

    public static bool operator ==(HexCoordinates a, HexCoordinates b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(HexCoordinates a, HexCoordinates b)
    {
        return !a.Equals(b);
    }
    #endregion
}
