using System;
using UnityEngine;

[Flags]
/// <summary>
/// 地形类型的位枚举
/// </summary>
public enum TerrainType
{
    None = 0,
    Water = 1 << 1,
    Mountain = 1 << 2,
    Plane = 1 << 3,

}