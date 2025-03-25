using UnityEngine;

[System.Serializable]
/// <summary>
/// 六边形坐标结构体
/// </summary>
public struct HexCoordinates
{
    public int X { get; private set; }
    public int Z { get; private set; }

    //通过x+y+z这种立方体坐标系或者轴坐标系来表示六边形的位置
    //在六边形坐标系中，X + Y + Z = 0
    public int Y => -X - Z;


    /// <summary>
    /// 构造函数初始化一个六边形坐标实例，x横坐标，y纵坐标
    /// </summary>
    /// <param name="x">横坐标</param>
    /// <param name="z">纵坐标</param>
    public HexCoordinates(int x, int z)
    {
        X = x;
        Z = z;
    }

    /// <summary>
    ///提供 FromOffsetCoordinates(int x, int z) 方法，可以从偏移坐标（offset coordinates）转换为立方体坐标
    ///偏移坐标是一种更接近矩形网格的表示方法，通过这个方法可以从常见的"行列"式表示转为内部使用的立方体坐标
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public static HexCoordinates FromOffsetCoordinates(int x, int z)
    {
        return new HexCoordinates(x - z / 2, z);
    }

    /// <summary>
    /// 返回坐标的字符串表示，形如 "(X, Y, Z)"
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return "(" + X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    /// <summary>
    /// ToStringOnSeparateLines(): 将坐标以多行方式显示，便于调试或在UI上展示
    /// </summary>
    /// <returns></returns>
    public string ToStringOnSeparateLines()
    {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }

    /// <summary>
    /// 六边形方向枚举
    /// </summary>
    public enum Direction
    {
        NE, E, SE, SW, W, NW
    }

    /// <summary>
    /// 获取指定方向的邻居坐标
    /// </summary>
    public HexCoordinates GetNeighbor(Direction direction)
    {
        switch (direction)
        {
            case Direction.NE: return new HexCoordinates(X + 1, Z - 1);
            case Direction.E: return new HexCoordinates(X + 1, Z);
            case Direction.SE: return new HexCoordinates(X, Z + 1);
            case Direction.SW: return new HexCoordinates(X - 1, Z + 1);
            case Direction.W: return new HexCoordinates(X - 1, Z);
            case Direction.NW: return new HexCoordinates(X, Z - 1);
            default: return this;
        }
    }

    /// <summary>
    /// 获取指定索引方向的邻居坐标
    /// </summary>
    public HexCoordinates GetNeighbor(int directionIndex)
    {
        return GetNeighbor((Direction)directionIndex);
    }

    /// <summary>
    /// 从世界坐标位置获取六边形坐标
    /// </summary>
    public static HexCoordinates FromPosition(Vector3 position)
    {
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float y = -x;
        float offset = position.z / (HexMetrics.outerRadius * 3f);
        x -= offset;
        y -= offset;
        
        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x -y);
        
        if (iX + iY + iZ != 0)
        {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x -y - iZ);
            
            if (dX > dY && dX > dZ)
                iX = -iY - iZ;
            else if (dZ > dY)
                iZ = -iX - iY;
        }
        
        return new HexCoordinates(iX, iZ);
    }
}
