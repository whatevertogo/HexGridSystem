using UnityEngine;

namespace Core.Interfaces
{
    /// <summary>
    /// 可地形化的格子接口
    /// </summary>
    public interface ITerrainCell
    {
        void AddTerrainType(HexCell hexCell,TerrainType terrain);
        void RemoveTerrainType(HexCell hexCell,TerrainType terrain);
        bool HasTerrainType(HexCell hexCell,TerrainType terrain);
    }
}