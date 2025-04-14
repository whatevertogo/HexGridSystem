using System;

[Flags]
/// <summary>
/// 地形类型的位枚举
/// </summary>
public enum TerrainType
{
    None = 0,
    Plains = 1 << 0,      // 平原
    Water = 1 << 1,       // 水域
    Mountain = 1 << 2,    // 山地
    Forest = 1 << 3,      // 森林
    Desert = 1 << 4,      // 沙漠
    Grassland = 1 << 6,   // 草地
    Volcano = 1 << 9,     // 火山
}