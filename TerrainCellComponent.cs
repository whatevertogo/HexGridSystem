using UnityEngine;
using Core.Interfaces;

namespace Grid
{
    public class TerrainCellComponent : MonoBehaviour, ITerrainCell
    {
        [SerializeField] private TerrainSettings terrainSettings;
        private TerrainType terrainType;
        private HexCell hexCell;

        private void Awake()
        {
            hexCell = GetComponent<HexCell>();
            if (terrainSettings == null)
            {
                // 如果在Inspector中没有设置，尝试从Resources加载
                terrainSettings = Resources.Load<TerrainSettings>("TerrainSettings");
            }
        }

        public void SetTerrain(TerrainType type)
        {
            terrainType = type;
            UpdateTerrainVisuals();
        }

        public TerrainType GetTerrain()
        {
            return terrainType;
        }

        private void UpdateTerrainVisuals()
        {
            if (terrainSettings != null && hexCell != null)
            {
                Color terrainColor = terrainSettings.GetTerrainColor(terrainType);
                hexCell.SetColor(terrainColor);
            }
        }

        public void AddTerrainType(HexCell hexCell, TerrainType terrain)
        {
            // 添加地形类型的逻辑
            if (hexCell != null && terrain != TerrainType.None)
            {
                SetTerrain(terrain);
            }
        }
        public void RemoveTerrainType(HexCell hexCell, TerrainType terrain)
        {
            // 移除地形类型的逻辑
            if (hexCell != null && terrain == terrainType)
            {
                SetTerrain(TerrainType.None);
            }
        }
        public bool HasTerrainType(HexCell hexCell, TerrainType terrain)
        {
            // 检查是否具有特定地形类型的逻辑
            return hexCell != null && terrain == terrainType;
        }

        public string GetTerrainName()
        {
            return terrainSettings != null ? terrainSettings.GetTerrainName(terrainType) : terrainType.ToString();
        }

        public string GetTerrainDescription()
        {
            return terrainSettings != null ? terrainSettings.GetTerrainDescription(terrainType) : string.Empty;
        }
    }
}