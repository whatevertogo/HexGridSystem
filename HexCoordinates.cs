using UnityEngine;
using System;
using Grid;
using System.Collections.Generic;

[Serializable]
/// <summary>
/// 结构体用来表示六边形网格的坐标系统
/// 作用是提供六边形网格的坐标表示和相关操作
/// 从世界坐标转换为六边形坐标，计算邻居坐标灯
/// </summary>
public struct HexCoordinates : IEquatable<HexCoordinates>
{
    [SerializeField]
    private int x, y;

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
    /// 偏移坐标方向字典
    /// </summary>
    /// <returns></returns>
    private static readonly Dictionary<HexDirection, Vector2Int> directionOffsets = new()
    {
        { HexDirection.NE, new Vector2Int(1, -1) },
        { HexDirection.E, new Vector2Int(1, 0) },
        { HexDirection.SE, new Vector2Int(0, 1) },
        { HexDirection.SW, new Vector2Int(-1, 1) },
        { HexDirection.W, new Vector2Int(-1, 0) },
        { HexDirection.NW, new Vector2Int(0, -1) }
    };

    /// <summary>
    /// 通过字典得到方向得到邻居坐标
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public HexCoordinates GetNeighbor(HexDirection direction)
    {
        Vector2Int offset = directionOffsets[direction];
        return new HexCoordinates(x + offset.x, y + offset.y);
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
    /// 哈希值计算
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
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