/// <summary>
/// HexCell的六个方向
/// </summary>
public enum HexDirection
{
    NE,
    E,
    SE,
    SW,
    W,
    NW,
}

/// <summary>
/// 扩展枚举的功能
/// </summary>
public static class HexDirectionExtensions
{
    // 通过扩展方法，来扩展枚举的功能
    // 拿到一条边的对边方向
    public static HexDirection Opposite(this HexDirection direction)
    {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }
}
