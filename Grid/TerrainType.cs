using System;

/// <summary>
/// 地形类型的位枚举
/// </summary>
[Flags]
public enum TerrainType
{
    None = 0,
    Plains = 1 << 0, // 平原
    Water = 1 << 1, // 水域
    Mountain = 1 << 2, // 山地
    Forest = 1 << 3, // 森林
    Ocean = 1 << 4, // 海洋
    Grassland = 1 << 5, // 草原
}
