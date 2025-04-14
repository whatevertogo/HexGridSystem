using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "NewHexTileData", menuName = "Game/HexTileData")]
    public class HexTileData : ScriptableObject
    {
        public string terrainType; // 地形类型（如平原、森林等）
        public int resourceValue; // 资源价值
        public int movementCost; // 移动消耗
        public bool isOccupied; // 是否被占用（初始化为false）
    }
}
